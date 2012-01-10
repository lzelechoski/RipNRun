// Author: Toomas Kaljus
// http://www.digigrupp.com

namespace RipNRun
{
	public class IPPacket // RFC791
	{
		public byte Version;
		public byte HeaderLength;
		public byte TypeOfService;
		public ushort TotalLength;
		public ushort Identification;
		public byte Flags;
		public ushort FragmentOffset;
		public byte TimeToLive;
		public byte Protocol;
		public ushort HeaderChecksum;
		public System.Net.IPAddress SourceAddress;
		public System.Net.IPAddress DestinationAddress;
		public byte[] PacketData;

		public ICMPPacket ICMP;
		public TCPPacket TCP;
		public UDPPacket UDP;

		public IPPacket() : base() { }
		
		public IPPacket(ref byte[] Packet) : base()
		{
			try {
				Version = (byte)(Packet[0] >> 4);
				HeaderLength = (byte)((Packet[0] & 0x0F) * 4);
				TypeOfService = Packet[1];
				TotalLength = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 2));
				Identification = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 4));
				Flags = (byte)((Packet[6] & 0xE0) >> 5);
				FragmentOffset = (ushort)(System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 6)) & 0x1FFF);
				TimeToLive = Packet[8];
				Protocol = Packet[9];
				HeaderChecksum = (ushort)(System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 10)));
				SourceAddress = new System.Net.IPAddress(System.BitConverter.ToInt32(Packet, 12) & 0x00000000FFFFFFFF);
				DestinationAddress = new System.Net.IPAddress(System.BitConverter.ToInt32(Packet, 16) & 0x00000000FFFFFFFF);
				PacketData = new byte[TotalLength - HeaderLength];
				System.Buffer.BlockCopy(Packet, HeaderLength, PacketData, 0, PacketData.Length);
			} catch { }
			
			switch (Protocol) {
				case 1: ICMP = new ICMPPacket(ref PacketData); break;
				case 6: TCP = new TCPPacket(ref PacketData); break;
				case 17: UDP = new UDPPacket(ref PacketData); break;
			}
		}

		public byte[] GetBytes()
		{
			if (ICMP != null) { Protocol = 1; PacketData = ICMP.GetBytes(); }
			if (TCP != null) { Protocol = 6; PacketData = TCP.GetBytes(ref SourceAddress, ref DestinationAddress); }
			if (UDP != null) { Protocol = 17; PacketData = UDP.GetBytes(ref SourceAddress, ref DestinationAddress); }

			if (PacketData == null) PacketData = new byte[0];
			if (Version == 0) Version = 4;
			if (HeaderLength == 0) HeaderLength = 20;
			TotalLength = (ushort)(HeaderLength + PacketData.Length);
			byte[] Packet = new byte[TotalLength];
			if (TimeToLive == 0) TimeToLive = 128;
			Packet[0] = (byte)(((Version & 0x0F) << 4) | ((HeaderLength / 4) & 0x0F));
			Packet[1] = TypeOfService;
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)TotalLength)), 0, Packet, 2, 2);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)Identification)), 0, Packet, 4, 2);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)((FragmentOffset & 0x1F) | ((Flags & 0x03) << 13)))), 0, Packet, 6, 2);
			Packet[8] = TimeToLive;
			Packet[9] = Protocol;
			System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)0), 0, Packet, 10, 2);
			System.Buffer.BlockCopy(SourceAddress.GetAddressBytes(), 0, Packet, 12, 4);
			System.Buffer.BlockCopy(DestinationAddress.GetAddressBytes(), 0, Packet, 16, 4);
			System.Buffer.BlockCopy(PacketData, 0, Packet, HeaderLength, PacketData.Length);
			HeaderChecksum = GetChecksum(ref Packet, 0, HeaderLength - 1);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)HeaderChecksum)), 0, Packet, 10, 2);
			return Packet;
		}

		public static ushort GetChecksum(ref byte[] Packet, int start, int end)
		{
			uint CheckSum = 0;
			int i;
			for (i=start; i<end; i+=2) CheckSum += (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, i));
			if (i == end) CheckSum += (ushort)System.Net.IPAddress.NetworkToHostOrder((ushort)Packet[end]);
			while (CheckSum >> 16 != 0) CheckSum = (CheckSum & 0xFFFF) + (CheckSum >> 16);
			return (ushort)~CheckSum;
		}
	}
}