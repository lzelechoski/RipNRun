using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading;

namespace RipNRun
{
    public partial class RipNRun : Form
    {
        #region Vars

        //Program Vars
        Thread Sniffer;
        Socket ipSocket;
        IPEndPoint endp;
        const int PacketBufferSize = 65536;
        byte[] PacketBuffer = new byte[PacketBufferSize];
        public Size big = new Size(361, 225);
        public Size small = new Size(361, 81);

        //Settings Vars
        public string userName = null;
        public string password = null;
        public bool runonstart = false;
        public string compIp = null;
        public int portNum;

        //Data Vars
        public string callData;
        public string rCastData;
        public bool repeatcall;
        public string lastIncidentNum;
        public string logstring = "";
        public string callLogString = "";
        public string lastCallLogString = "";
        public string[] sayings = new string[100];
        public int rotate = 0;
        public int rotatemax;
        public string[] apparatus = new string[10];

        //To be removed/externalized
        public int gmailport = 587;
        public IPAddress ip608;
        public IPAddress ip609;

        #endregion Vars

        public RipNRun() : base()
        {
            InitializeComponent();
        }

        #region Form Startup/Shutdown
        private void RipNRun_Load(object sender, EventArgs e)
        {
            applySettings();
            fetchVars();
            if (runonstart == true)
            {
                startButton.PerformClick();
            }
        }
        private void RipNRun_FormClosing(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save Current Settings?", "Save Settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                saveSettings();
            }
        }
        #endregion

        #region Packet Capture
        private void RunReceiver()
        {
            try
            {
                try
                {
                    ipSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
                    try
                    {
                        ipSocket.Bind(endp);
                        ipSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1);
                        ipSocket.IOControl(unchecked((int)0x98000001), new byte[4] { 1, 0, 0, 0 }, new byte[4]);
                        while (stopButton.Enabled)
                        {
                            IAsyncResult ar = ipSocket.BeginReceive(PacketBuffer, 0, PacketBufferSize, SocketFlags.None, new AsyncCallback(CallReceive), this);
                            while (ipSocket.Available == 0)
                            {
                                Thread.Sleep(1);
                                if (!stopButton.Enabled) break;
                            }
                            Thread.Sleep(1);
                            if (!stopButton.Enabled) break;
                            int Size = ipSocket.EndReceive(ar);
                            if (!looseQueue.Checked) ExtractBuffer();
                        }
                    }
                    finally
                    {
                        if (ipSocket != null)
                        {
                            ipSocket.Shutdown(SocketShutdown.Both);
                            ipSocket.Close();
                        }
                    }
                }
                finally
                {
                    Button e = stopButton;
                    if (e.InvokeRequired)
                    {
                        e.BeginInvoke((MethodInvoker)delegate
                        {
                            stopButton.Enabled = false;
                        });
                    }
                    else
                    {
                        stopButton.Enabled = false;
                    }
                    Button f = startButton;
                    if (f.InvokeRequired)
                    {
                        f.BeginInvoke((MethodInvoker)delegate
                        {
                            startButton.Enabled = true;
                        });
                    }
                    else
                    {
                        startButton.Enabled = true;
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
            Button g = startButton;
            if (g.InvokeRequired)
            {
                g.BeginInvoke((MethodInvoker)delegate
                {
                    startButton.Enabled = true;
                });
            }
            else
            {
                startButton.Enabled = true;
            }
        }
        #endregion

        #region Extract
        protected void ExtractBuffer()
        {
            IPPacket IP = new IPPacket(ref PacketBuffer);

            string SourceAddress = IP.SourceAddress.ToString();
            string DestinationAddress = IP.DestinationAddress.ToString();

            if (IP.TCP != null)
            {
                string Data = Regex.Replace(Encoding.ASCII.GetString(IP.TCP.PacketData), @"[^a-zA-Z_0-9\.\@\- ]", "");
                callData = Encoding.ASCII.GetString(IP.TCP.PacketData);
                if (IP.TCP.DestinationPort == portNum)
                {
                    if (Data.StartsWith("W1"))
                    {
                        parseCallData();
                    }
                }
            }
            if (IP.UDP != null)
            {
                rCastData = Encoding.ASCII.GetString(IP.UDP.PacketData);
                if (IP.UDP.DestinationPort == portNum)
                {
                    if (rCastData.StartsWith("RipCast|"))
                    {
                        parseRCastData();
                    }
                    else if (rCastData.StartsWith("CheckIn|"))
                    {
                        parseRDSCheckIn();
                    }
                }
            }
        }
        public virtual void CallReceive(System.IAsyncResult ar)
        {
            if (looseQueue.Checked) ExtractBuffer();
        }
        #endregion Packet Capture

        #region Parse Call
        public void parseCallData()
        {

            try
            {
                logstring += "|";
                string ems60UnitResponding = null;
                string call = callData;
                //logstring += call;
                //logstring += "|";
                string[] exData = Regex.Split(call, "\r\n");
                string callType = exData[0].Substring(exData[0].IndexOf("W1") + 2, exData[0].Length - (exData[0].IndexOf("W1") + 2));
                string callAddress = exData[2];
                string callDistrict = exData[5].Substring(0, exData[5].IndexOf("BOX"));
                callDistrict = callDistrict.TrimEnd(' ');
                string[] cd = callDistrict.Split(' '); 
                string callMapCo = exData[5].Substring(exData[5].IndexOf("TG:"), exData[5].Length - exData[5].IndexOf("TG:"));
                string[] mc = callMapCo.Split(':');
                string unitsResponding = exData[7];
                logstring += unitsResponding;
                logstring += "|";
                
                string[] units = Regex.Split(unitsResponding, " ");
                foreach (string unit in units)
                {
                    //logstring += unit;
                    //logstring += "_U";
                    foreach (string piece in apparatus)
                    {
                        //logstring += piece;
                        //logstring += "_P";
                        if (unit == piece)
                        {
                            ems60UnitResponding = unit;
                            //logstring += "|Unit Responding: ";
                            //logstring += unit;
                            //logstring += "|";
                        }
                    }
                }
                
                string callNotes = exData[11];
                string callNumber = exData[13].Substring(0, exData[13].IndexOf(' '));
                
                if (callNumber == "")
                {
                    callNumber = "UNK";
                }
                
                logstring += "CN:";
                logstring += callNumber;
                logstring += "LCN:";
                logstring += lastIncidentNum;
                logstring += "|";

                if (callNumber == lastIncidentNum)
                {
                    repeatcall = true;
                    logstring += "Repeat Call|";
                }
                else
                {
                    repeatcall = false;
                    logstring += "Not A Repeat Call|";
                }


                lastIncidentNum = callNumber;
                string callTimeDate = exData[13].Substring(exData[13].IndexOf(":") - 2, exData[13].Length - (exData[13].IndexOf(":") - 2));
                string callTime = callTimeDate.TrimStart(' ');
                string callDate = callTime;
                callTime = callTime.Substring(0, callTime.IndexOf(' '));
                callDate = callDate.Substring(callDate.IndexOf(' '), callDate.Length - callDate.IndexOf(' '));
                callDate = callDate.TrimStart(' ');
                callDate = callDate.Replace('/', '.');
                string sms = callType + "\r\n\r\n" + callAddress + "\r\n\r\n" + callDistrict + "\r\n" + callMapCo + "\r\n\r\n" + callNumber + "\r\n" + callDate + " " + callTime + "\r\n\r\n" + callNotes;
                callLogString = callNumber + "," + callDate + "," + callTime + ",\"" + callType + "\",\"" + callAddress + "\"," + cd[1] + ",\"" + mc[1] + "\",\"" + callNotes + "\"," + unitsResponding;
                string callUpdate = sayings[rotate] + " " + ems60UnitResponding + " is responding" + "\r\n\r\n" + callAddress + "\r\n\r\n" + callNumber + "\r\n" + callDate + " " + callTime + "\r\n\r\n" + callNotes;
                

                if (userName != null && repeatcall != true)
                {
                    sendEmailForSms(sms);
                    logstring += "--First Dispatch|";
                }

                if (userName != null && repeatcall != true && ems60UnitResponding != null)
                {
                    sendUpdateEmail(callUpdate);
                    logstring += "--First Dispatch--Responding|";
                    rotate++;
                }

                if (userName != null && repeatcall == true && ems60UnitResponding != null)
                {
                    sendUpdateEmail(callUpdate);
                    logstring += "--Repeat Dispatch--Responding|";
                    rotate++;
                }

                if (rotate > rotatemax) 
                { 
                    rotate = 0;
                }

            }

            catch
            {
            }

            try
            {
                logstring += "--Call Parsed";
                using (StreamWriter w = File.AppendText(Path.GetDirectoryName(Application.ExecutablePath) + "\\DebugLog.txt"))
                {
                    Log(logstring, w);
                    w.Close();
                    logstring = "";
                }
            }

            catch
            {
            }

            try
            {

                DateTime thisDay = DateTime.Today;
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\CallLog_" + thisDay.ToString("yyyyMMM") + ".csv";

                if (repeatcall != true)
                {
                    if (File.Exists(path))
                    {
                        using (StreamWriter w = File.AppendText(path))
                        {
                            if (lastCallLogString != "")
                            {
                                callLog(lastCallLogString, w);
                            }
                            w.Close();
                            lastCallLogString = callLogString;
                            callLogString = "";
                        }
                    }
                    else
                    {
                        using (FileStream fs = File.Create(path))
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes("Incident Number,Date,Time,Call Type,Address,District,MapCo,Notes,Units Dispatched\r\n");
                            fs.Write(info, 0, info.Length);
                            fs.Close();
                        }

                        using (StreamWriter w = File.AppendText(path))
                        {
                            if (lastCallLogString != "")
                            {
                                callLog(lastCallLogString, w);
                            }
                            w.Close();
                            lastCallLogString = callLogString;
                            callLogString = "";
                        }
                    }
                }

            }

            catch
            {
            }

        }
        #endregion

        #region Parse RCast, RDS
        public void parseRCastData()
        {

            try
            {
                string[] cast = rCastData.Split('|');
                
                string sms = cast[1];
                
                if (sms.Length > 160)
                {
                    sms = sms.Substring(0, 159);
                }

                if (userName != null)
                {
                    sendEmailForRCast(sms);
                }
            }

            catch
            {
            }
        }

        public void parseRDSCheckIn()
        {
            try
            {
                string[] cast = rCastData.Split('|');
                string[] ip = cast[1].Split(':');
                if (ip[0] == "608")
                {
                    ip608 = IPAddress.Parse(ip[1]);
                    sendCheckInPong("608", ip608);
                }
                else if (ip[0] == "609")
                {
                    ip609 = IPAddress.Parse(ip[1]);
                    sendCheckInPong("609", ip609);
                }
            }

            catch
            {
            }
        }

        #endregion

        #region Send

        public void sendEmailForSms(string t)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("sms.dispatch@ccmedicunit.org", "CCMICU");
                message.To.Add(new MailAddress("sms.dispatch@ccmedicunit.org"));
                string line = null;
                StreamReader cn = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath) + "\\Subscribers.csv");
                while (((line = cn.ReadLine()) != null))
                {
                    string[] fields = new string[5];
                    fields = line.Split(',');
                    if (fields[2].ToLower() == "y")
                    {
                        message.Bcc.Add(new MailAddress(fields[1].ToString()));
                    }
                    else
                    {
                    }
                }
                message.Body = t;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", gmailport);
                smtp.Credentials = new NetworkCredential(userName, password);
                smtp.EnableSsl = true;
                smtp.Send(message);
                cn.Close();
            }

            catch
            {
            }
        }

        public void sendUpdateEmail(string t)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("sms.dispatch@ccmedicunit.org", "CCMICU");
                message.To.Add(new MailAddress("sms.dispatch@ccmedicunit.org"));
                string line = null;
                StreamReader cn = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath) + "\\Subscribers.csv");
                while (((line = cn.ReadLine()) != null))
                {
                    string[] fields = new string[5];
                    fields = line.Split(',');
                    if (fields[3].ToLower() == "y")
                    {
                        message.Bcc.Add(new MailAddress(fields[1].ToString()));
                    }
                    else
                    {
                    }
                }
                message.Body = t;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", gmailport);
                smtp.Credentials = new NetworkCredential(userName, password);
                smtp.EnableSsl = true;
                smtp.Send(message);
                cn.Close();
            }

            catch
            {
            }
        }

        public void sendEmailForRCast(string t)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("sms.dispatch@ccmedicunit.org", "CCMICU");
                message.To.Add(new MailAddress("sms.dispatch@ccmedicunit.org"));
                string line = null;
                StreamReader cn = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath) + "\\Subscribers.csv");
                while (((line = cn.ReadLine()) != null))
                {
                    string[] fields = new string[5];
                    fields = line.Split(',');
                    if (fields[4].ToLower() == "y")
                    {
                        message.Bcc.Add(new MailAddress(fields[1].ToString()));
                    }
                    else
                    {
                    }
                }
                message.Body = t;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", gmailport);
                smtp.Credentials = new NetworkCredential(userName, password);
                smtp.EnableSsl = true;
                smtp.Send(message);
                cn.Close();
            }

            catch
            {
            }
        }

        public void sendCheckInPong(string unit, IPAddress ip)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress serverAddr = ip;
            IPEndPoint endPoint = new IPEndPoint(serverAddr, portNum);
            string text = ip.ToString() + "_" + DateTime.Now.ToString();
            byte[] send_buffer = Encoding.ASCII.GetBytes("PONG|" + text);
            try
            {
                sock.SendTo(send_buffer, endPoint);
            }
            catch
            {
            }
        }
        #endregion

        #region Settings
        private void applySettings()
        {
            try
            {
                StreamReader cn = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.ini");
                string[] settings = Regex.Split(cn.ReadToEnd(),"\r\n");
                string[] login = settings[0].Split('=');
                string[] pass = settings[1].Split('=');
                string[] startrun = settings[2].Split('=');
                string[] ip = settings[3].Split('=');
                string[] port = settings[4].Split('=');
                userName = login[1].ToString();
                password = pass[1].ToString();
                runonstart = bool.Parse(startrun[1].ToString());
                compIp = ip[1].ToString();
                portNum = int.Parse(port[1].ToString());
                cn.Close();
            }
            catch
            {
            }
        }
        private void fetchVars()
        {
            try
            {
                int c = 0;
                StreamReader sa = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath) + "\\Sayings.txt");
                string[] saywhat = Regex.Split(sa.ReadToEnd(), "\r\n");
                logstring += "Sayings: ";
                foreach (string say in saywhat)
                {
                    sayings[c] = say;
                    c++;
                    logstring += say;
                    logstring += " ";
                    logstring += c;
                    logstring += "|";
                }
                rotatemax = c-1;
                sa.Close();
            }
            catch
            {
            }

            try
            {
                int i = 0;
                StreamReader ap = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath) + "\\Apparatus.txt");
                string[] apps = Regex.Split(ap.ReadToEnd(), "\r\n");
                logstring += "Apparatus: ";
                foreach (string app in apps)
                {
                    apparatus[i] = app;
                    i++;
                    logstring += app;
                    logstring += " ";
                    logstring += i;
                    logstring += "|";
                }
                ap.Close();
            }
            catch
            {
            }
        }
        private void saveSettings()
        {
            try
            {
                File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.ini");
                StreamWriter settings = new StreamWriter(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.ini");
                settings.WriteLine("login=" + userName);
                settings.WriteLine("pass=" + password);
                settings.WriteLine("runonstart=" + runonstart.ToString());
                settings.WriteLine("ip=" + compIp.ToString());
                settings.WriteLine("port=" + portNum.ToString());
                settings.Close();
            }
            catch
            {
            }
        }
        #endregion Settings

        #region Buttons
        private void stopButton_Click(object sender, EventArgs e)
        {
            stopButton.Enabled = false;
            using (StreamWriter w = File.AppendText(Path.GetDirectoryName(Application.ExecutablePath) + "\\DebugLog.txt"))
            {
                Log("RipNRun Stopped", w);
                w.Close();
                logstring = "";
            }
        }
        private void startButton_Click(object sender, EventArgs e)
        {
            string selectIP = compIp;
            if (selectIP == "")
            {
                MessageBox.Show("IP Not Set!");
                return;
            }
            endp = new IPEndPoint(IPAddress.Parse(selectIP), 0);
            startButton.Enabled = false;
            stopButton.Enabled = true;
            Sniffer = new Thread(new ThreadStart(RunReceiver));
            Sniffer.Start();
            using (StreamWriter w = File.AppendText(Path.GetDirectoryName(Application.ExecutablePath) + "\\DebugLog.txt"))
            {
                Log("RipNRun Started", w);
                Log(logstring, w);
                w.Close();
                logstring = "";
            }
        }
        #endregion Buttons

        #region Menu
        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Size = big;
            emailPanel.Visible = true;
            loginBox.Text = userName;
        }
        private void cancelLogin_Click(object sender, EventArgs e)
        {
            emailPanel.Visible = false;
            this.Size = small;
            passwordBox.Text = "";
        }
        private void updateLogin_Click(object sender, EventArgs e)
        {
            emailPanel.Visible = false;
            this.Size = small;
            userName = loginBox.Text.ToString();
            password = passwordBox.Text.ToString();
            passwordBox.Text = "";
        }
        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save Current Settings?", "Save Settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                saveSettings();
            }
        }
        private void iPSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Size = big;
            ipPanel.Visible = true;
            ipBox.Text = compIp;
            portBox.Text = portNum.ToString();
        }
        private void cancelIp_Click(object sender, EventArgs e)
        {
            ipPanel.Visible = false;
            this.Size = small;
        }
        private void updateIp_Click(object sender, EventArgs e)
        {
            ipPanel.Visible = false;
            this.Size = small;
            compIp = ipBox.Text.ToString();
            portNum = int.Parse(portBox.Text.ToString());
        }
        #endregion Menu

        #region Log

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
            w.Flush();
        }

        public static void callLog(string logMessage, TextWriter w)
        {
            w.WriteLine(logMessage);
            w.Flush();
        }

        #endregion
    }
}