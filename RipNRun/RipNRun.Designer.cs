namespace RipNRun
{
    partial class RipNRun
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (stopButton.Enabled) stopButton_Click(null, null);
            while (stopButton.Enabled) System.Threading.Thread.Sleep(1);
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.looseQueue = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iPSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ipPanel = new System.Windows.Forms.Panel();
            this.portLbl = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.TextBox();
            this.cancelIp = new System.Windows.Forms.Button();
            this.updateIp = new System.Windows.Forms.Button();
            this.ipLbl = new System.Windows.Forms.Label();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.loginBox = new System.Windows.Forms.TextBox();
            this.passwordBox = new System.Windows.Forms.MaskedTextBox();
            this.loginLbl = new System.Windows.Forms.Label();
            this.pwLbl = new System.Windows.Forms.Label();
            this.updateLogin = new System.Windows.Forms.Button();
            this.cancelLogin = new System.Windows.Forms.Button();
            this.emailPanel = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.ipPanel.SuspendLayout();
            this.emailPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 27);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(93, 27);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // looseQueue
            // 
            this.looseQueue.AutoSize = true;
            this.looseQueue.Location = new System.Drawing.Point(258, 224);
            this.looseQueue.Name = "looseQueue";
            this.looseQueue.Size = new System.Drawing.Size(90, 17);
            this.looseQueue.TabIndex = 10;
            this.looseQueue.Text = "Loose Queue";
            this.looseQueue.UseVisualStyleBackColor = true;
            this.looseQueue.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(355, 24);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emailToolStripMenuItem,
            this.iPSettingsToolStripMenuItem,
            this.saveSettingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // emailToolStripMenuItem
            // 
            this.emailToolStripMenuItem.Name = "emailToolStripMenuItem";
            this.emailToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.emailToolStripMenuItem.Text = "Email Settings";
            this.emailToolStripMenuItem.Click += new System.EventHandler(this.emailToolStripMenuItem_Click);
            // 
            // iPSettingsToolStripMenuItem
            // 
            this.iPSettingsToolStripMenuItem.Name = "iPSettingsToolStripMenuItem";
            this.iPSettingsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.iPSettingsToolStripMenuItem.Text = "IP Settings";
            this.iPSettingsToolStripMenuItem.Click += new System.EventHandler(this.iPSettingsToolStripMenuItem_Click);
            // 
            // saveSettingsToolStripMenuItem
            // 
            this.saveSettingsToolStripMenuItem.Name = "saveSettingsToolStripMenuItem";
            this.saveSettingsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.saveSettingsToolStripMenuItem.Text = "Save Settings";
            this.saveSettingsToolStripMenuItem.Click += new System.EventHandler(this.saveSettingsToolStripMenuItem_Click);
            // 
            // ipPanel
            // 
            this.ipPanel.AllowDrop = true;
            this.ipPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ipPanel.Controls.Add(this.portLbl);
            this.ipPanel.Controls.Add(this.portBox);
            this.ipPanel.Controls.Add(this.cancelIp);
            this.ipPanel.Controls.Add(this.updateIp);
            this.ipPanel.Controls.Add(this.ipLbl);
            this.ipPanel.Controls.Add(this.ipBox);
            this.ipPanel.Location = new System.Drawing.Point(9, 73);
            this.ipPanel.Name = "ipPanel";
            this.ipPanel.Size = new System.Drawing.Size(333, 99);
            this.ipPanel.TabIndex = 19;
            this.ipPanel.Visible = false;
            // 
            // portLbl
            // 
            this.portLbl.AutoSize = true;
            this.portLbl.Location = new System.Drawing.Point(64, 37);
            this.portLbl.Name = "portLbl";
            this.portLbl.Size = new System.Drawing.Size(42, 13);
            this.portLbl.TabIndex = 7;
            this.portLbl.Text = "Rx Port";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(108, 34);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(215, 20);
            this.portBox.TabIndex = 6;
            // 
            // cancelIp
            // 
            this.cancelIp.Location = new System.Drawing.Point(167, 63);
            this.cancelIp.Name = "cancelIp";
            this.cancelIp.Size = new System.Drawing.Size(75, 23);
            this.cancelIp.TabIndex = 5;
            this.cancelIp.Text = "Cancel";
            this.cancelIp.UseVisualStyleBackColor = true;
            this.cancelIp.Click += new System.EventHandler(this.cancelIp_Click);
            // 
            // updateIp
            // 
            this.updateIp.Location = new System.Drawing.Point(248, 63);
            this.updateIp.Name = "updateIp";
            this.updateIp.Size = new System.Drawing.Size(75, 23);
            this.updateIp.TabIndex = 4;
            this.updateIp.Text = "Update";
            this.updateIp.UseVisualStyleBackColor = true;
            this.updateIp.Click += new System.EventHandler(this.updateIp_Click);
            // 
            // ipLbl
            // 
            this.ipLbl.AutoSize = true;
            this.ipLbl.Location = new System.Drawing.Point(41, 11);
            this.ipLbl.Name = "ipLbl";
            this.ipLbl.Size = new System.Drawing.Size(65, 13);
            this.ipLbl.TabIndex = 2;
            this.ipLbl.Text = "Computer IP";
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(108, 8);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(215, 20);
            this.ipBox.TabIndex = 0;
            // 
            // loginBox
            // 
            this.loginBox.Location = new System.Drawing.Point(60, 10);
            this.loginBox.Name = "loginBox";
            this.loginBox.Size = new System.Drawing.Size(257, 20);
            this.loginBox.TabIndex = 0;
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(66, 37);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '*';
            this.passwordBox.Size = new System.Drawing.Size(100, 20);
            this.passwordBox.TabIndex = 1;
            // 
            // loginLbl
            // 
            this.loginLbl.AutoSize = true;
            this.loginLbl.Location = new System.Drawing.Point(27, 13);
            this.loginLbl.Name = "loginLbl";
            this.loginLbl.Size = new System.Drawing.Size(33, 13);
            this.loginLbl.TabIndex = 2;
            this.loginLbl.Text = "Login";
            // 
            // pwLbl
            // 
            this.pwLbl.AutoSize = true;
            this.pwLbl.Location = new System.Drawing.Point(11, 41);
            this.pwLbl.Name = "pwLbl";
            this.pwLbl.Size = new System.Drawing.Size(53, 13);
            this.pwLbl.TabIndex = 3;
            this.pwLbl.Text = "Password";
            // 
            // updateLogin
            // 
            this.updateLogin.Location = new System.Drawing.Point(248, 69);
            this.updateLogin.Name = "updateLogin";
            this.updateLogin.Size = new System.Drawing.Size(75, 23);
            this.updateLogin.TabIndex = 4;
            this.updateLogin.Text = "Update";
            this.updateLogin.UseVisualStyleBackColor = true;
            this.updateLogin.Click += new System.EventHandler(this.updateLogin_Click);
            // 
            // cancelLogin
            // 
            this.cancelLogin.Location = new System.Drawing.Point(167, 69);
            this.cancelLogin.Name = "cancelLogin";
            this.cancelLogin.Size = new System.Drawing.Size(75, 23);
            this.cancelLogin.TabIndex = 5;
            this.cancelLogin.Text = "Cancel";
            this.cancelLogin.UseVisualStyleBackColor = true;
            this.cancelLogin.Click += new System.EventHandler(this.cancelLogin_Click);
            // 
            // emailPanel
            // 
            this.emailPanel.AllowDrop = true;
            this.emailPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.emailPanel.Controls.Add(this.cancelLogin);
            this.emailPanel.Controls.Add(this.updateLogin);
            this.emailPanel.Controls.Add(this.pwLbl);
            this.emailPanel.Controls.Add(this.loginLbl);
            this.emailPanel.Controls.Add(this.passwordBox);
            this.emailPanel.Controls.Add(this.loginBox);
            this.emailPanel.Location = new System.Drawing.Point(9, 73);
            this.emailPanel.Name = "emailPanel";
            this.emailPanel.Size = new System.Drawing.Size(333, 99);
            this.emailPanel.TabIndex = 18;
            this.emailPanel.Visible = false;
            // 
            // RipNRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 56);
            this.Controls.Add(this.ipPanel);
            this.Controls.Add(this.emailPanel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.looseQueue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RipNRun";
            this.Text = "RipNRun - V2.2.5";
            this.Load += new System.EventHandler(this.RipNRun_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ipPanel.ResumeLayout(false);
            this.ipPanel.PerformLayout();
            this.emailPanel.ResumeLayout(false);
            this.emailPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.CheckBox looseQueue;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSettingsToolStripMenuItem;
        private System.Windows.Forms.Panel ipPanel;
        private System.Windows.Forms.Button cancelIp;
        private System.Windows.Forms.Button updateIp;
        private System.Windows.Forms.Label ipLbl;
        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.ToolStripMenuItem iPSettingsToolStripMenuItem;
        private System.Windows.Forms.Label portLbl;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.TextBox loginBox;
        private System.Windows.Forms.MaskedTextBox passwordBox;
        private System.Windows.Forms.Label loginLbl;
        private System.Windows.Forms.Label pwLbl;
        private System.Windows.Forms.Button updateLogin;
        private System.Windows.Forms.Button cancelLogin;
        private System.Windows.Forms.Panel emailPanel;
    }
}

