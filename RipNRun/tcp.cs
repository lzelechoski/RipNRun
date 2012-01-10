// Author: Toomas Kaljus
// http://www.digigrupp.com

namespace RipNRun
{
	public class TCPPacket //rfc793
	{
		public ushort SourcePort;
		public ushort DestinationPort;
		public uint SequenceNumber;
		public uint AcknowledgmentNumber;
		public byte DataOffset;
		public byte ControlBits;
		public ushort Window;
		public ushort Checksum;
		public ushort UrgentPointer;
		public byte[] Options;
		public byte[] PacketData;

		public TCPPacket() : base() { }

		public TCPPacket(ref byte[] Packet) : base()
		{
			try {
				SourcePort = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 0));
				DestinationPort = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 2));
				SequenceNumber = (uint)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 4));
				AcknowledgmentNumber = (uint)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 8));
				DataOffset = (byte)((Packet[12] >> 4) * 4);
				ControlBits = (byte)((Packet[13] & 0x3F));
				Window = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 14));
				Checksum = (ushort)(System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 16)));
				UrgentPointer = (ushort)System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt16(Packet, 18));
				Options = new byte[DataOffset - 20];
				System.Buffer.BlockCopy(Packet, 20, Options, 0, Options.Length);
				PacketData = new byte[Packet.Length - DataOffset];
				System.Buffer.BlockCopy(Packet, DataOffset, PacketData, 0, Packet.Length - DataOffset);
			} catch { }
		}

		public byte[] GetBytes(ref System.Net.IPAddress SourceAddress, ref System.Net.IPAddress DestinationAddress)
		{
			if (PacketData == null) PacketData = new byte[0];
			if (Options == null) Options = new byte[0];
			int OptionsLength = ((int)((Options.Length + 3) / 4)) * 4;
			DataOffset = (byte)(20 + OptionsLength);
			byte[] Packet = new byte[20 + OptionsLength + PacketData.Length];
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)SourcePort)), 0, Packet, 0, 2);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)DestinationPort)), 0, Packet, 2, 2);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((int)SequenceNumber)), 0, Packet, 4, 4);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((int)AcknowledgmentNumber)), 0, Packet, 8, 4);
			Packet[12] = (byte)(((Packet[12] & 0x0F) | ((DataOffset & 0x0F) << 4)) / 4);
			Packet[13] = (byte)(((Packet[13] & 0xC0) | (ControlBits & 0x3F)));
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)Window)), 0, Packet, 14, 2);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)0), 0, Packet, 16, 2);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)UrgentPointer)), 0, Packet, 18, 2);
			System.Buffer.BlockCopy(Options, 0, Packet, 20, Options.Length);
			if (OptionsLength > Options.Length) System.Buffer.BlockCopy(System.BitConverter.GetBytes((long)0), 0, Packet, 20 + Options.Length, OptionsLength - Options.Length);
			System.Buffer.BlockCopy(PacketData, 0, Packet, DataOffset, PacketData.Length);
			Checksum = GetChecksum(ref Packet, 0, DataOffset - 1, ref SourceAddress, ref DestinationAddress);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Checksum), 0, Packet, 16, 2);
			return PacketData;
		}

		public ushort GetChecksum(ref byte[] Packet, int start, int end, ref System.Net.IPAddress SourceAddress, ref System.Net.IPAddress DestinationAddress)
		{
			byte[] PseudoPacket;
			PseudoPacket = new byte[12 + Packet.Length];
			System.Buffer.BlockCopy(SourceAddress.GetAddressBytes(), 0, PseudoPacket, 0, 4);
			System.Buffer.BlockCopy(DestinationAddress.GetAddressBytes(), 0, PseudoPacket, 4, 4);
			PseudoPacket[8] = 0;
			PseudoPacket[9] = 6;
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((short)Packet.Length)), 0, PseudoPacket, 10, 2);
			System.Buffer.BlockCopy(Packet, 0, PseudoPacket, 12, Packet.Length);
			return IPPacket.GetChecksum(ref PseudoPacket, 0, PseudoPacket.Length - 1);
		}
	}
}