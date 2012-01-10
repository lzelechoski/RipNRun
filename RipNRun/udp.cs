// Author: Toomas Kaljus
// http://www.digigrupp.com

namespace RipNRun
{
	public class UDPPacket // rfc768
	{
		public ushort SourcePort;
		public ushort DestinationPort;
		public ushort Length;
		public ushort Checksum;
		public byte[] PacketData;
		
		public UDPPacket() : base() { }

		public UDPPacket(ref byte[] Packet) : base()
		{
			try {
				SourcePort = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 0));
				DestinationPort = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 2));
				Length = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 4));
				Checksum = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 6));
				PacketData = new byte[Packet.Length - 8];
				System.Buffer.BlockCopy(Packet, 8, PacketData, 0, Packet.Length - 8);
			} catch { }
		}

		public byte[] GetBytes(ref System.Net.IPAddress SourceAddress, ref System.Net.IPAddress DestinationAddress)
		{
			if (PacketData == null) PacketData = new byte[0];
			byte[] Packet = new byte[8 + PacketData.Length];
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)SourcePort)), 0, Packet, 0, 2);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)DestinationPort)), 0, Packet, 2, 2);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)Length)), 0, Packet, 4, 2);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)0), 0, Packet, 6, 2);
			System.Buffer.BlockCopy(PacketData, 0, Packet, 8, PacketData.Length);
			Checksum = GetChecksum(ref Packet, 0, 8 - 1, ref SourceAddress, ref DestinationAddress);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Checksum), 0, Packet, 6, 2);
			return PacketData;
		}

		public ushort GetChecksum(ref byte[] Packet, int start, int end, ref System.Net.IPAddress SourceAddress, ref System.Net.IPAddress DestinationAddress)
		{
			byte[] PseudoPacket;
			PseudoPacket = new byte[12 + Packet.Length];
			System.Buffer.BlockCopy(SourceAddress.GetAddressBytes(), 0, PseudoPacket, 0, 4);
			System.Buffer.BlockCopy(DestinationAddress.GetAddressBytes(), 0, PseudoPacket, 4, 4);
			PseudoPacket[8] = 0;
			PseudoPacket[9] = 17;
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)Packet.Length)), 0, PseudoPacket, 10, 2);
			System.Buffer.BlockCopy(Packet, 0, PseudoPacket, 12, Packet.Length);
			return IPPacket.GetChecksum(ref PseudoPacket, 0, PseudoPacket.Length - 1);
		}
	}
}