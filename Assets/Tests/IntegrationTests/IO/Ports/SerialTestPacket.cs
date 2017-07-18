using QuickUnity.IO.Ports;
using System.Text;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// The data packet of serial test.
    /// </summary>
    /// <seealso cref="QuickUnity.IO.Ports.ISerialPacket"/>
    public class SerialTestPacket : ISerialPacket
    {
        /// <summary>
        /// The bytes of data.
        /// </summary>
        private byte[] m_bytes;

        /// <summary>
        /// Gets the bytes of data.
        /// </summary>
        /// <value>The bytes of data.</value>
        public byte[] bytes
        {
            get { return m_bytes; }
        }

        /// <summary>
        /// The data.
        /// </summary>
        private object m_data;

        /// <summary>
        /// Gets the data of packet.
        /// </summary>
        /// <value>The data of packet.</value>
        public object data
        {
            get
            {
                return m_data;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialTestPacket"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public SerialTestPacket(string data)
        {
            m_data = data;
            m_bytes = Encoding.UTF8.GetBytes(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialTestPacket"/> class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public SerialTestPacket(byte[] bytes)
        {
            m_bytes = bytes;
            m_data = Encoding.UTF8.GetString(bytes);
        }
    }
}