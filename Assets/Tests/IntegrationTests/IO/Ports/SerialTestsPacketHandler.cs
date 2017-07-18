using QuickUnity.IO.Ports;
using System.Collections.Generic;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// The serial data packet handler for serial test.
    /// </summary>
    /// <seealso cref="QuickUnity.IO.Ports.ISerialPacketHandler"/>
    public class SerialTestsPacketHandler : ISerialPacketHandler
    {
        /// <summary>
        /// Packs the specified data to serial data packet.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The serial data packet.</returns>
        public ISerialPacket Pack(object data)
        {
            return new SerialTestPacket(data as string);
        }

        /// <summary>
        /// Unpacks the specified bytes to serial data packets.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The serial data packets.</returns>
        public ISerialPacket[] Unpack(byte[] bytes)
        {
            List<ISerialPacket> list = new List<ISerialPacket>();

            for (int i = 0, length = bytes.Length; i < length; ++i)
            {
                byte charByte = bytes[i];
                list.Add(new SerialTestPacket(new byte[1] { charByte }));
            }

            return list.ToArray();
        }
    }
}