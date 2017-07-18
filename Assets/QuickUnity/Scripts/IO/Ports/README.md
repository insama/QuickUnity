# **Serial**

The class **Serial** can help you do the thing of serial port communication easily. The usage of this class also is easy and flexible. This is a example to show how to use it.

First of all, you need to create a class to implement the interface **ISerialPacket**, just like this:

```c#
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
```



After that, you should create a class to implement the interface **ISerialPacketHandler**, for example:

```c#
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
```



Finally, we can use them to implement serial port communication. Example codes:

```c#
using QuickUnity.IO.Ports;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Integration test of Serial.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour"/>
    [IntegrationTest.DynamicTestAttribute("SerialTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    public class SerialTest : MonoBehaviour
    {
        private Serial m_serial;

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            m_serial = new Serial("COM3", 9600);
            m_serial.packetHandler = new SerialTestsPacketHandler();
            m_serial.AddEventListener(SerialEvent.Data, OnSerialData);
            m_serial.Open();
            m_serial.Send("a");
            Invoke("Pass", 9f);
        }

        private void Update()
        {
            if (m_serial != null)
            {
              	// This is the most important, make sure the function Update of class Serial should be invoked here.
                m_serial.Update();
            }
        }

        /// <summary>
        /// Called when receive [serial data].
        /// </summary>
        /// <param name="evt">The evt.</param>
        private void OnSerialData(Events.Event evt)
        {
            SerialEvent serialEvent = evt as SerialEvent;
            ISerialPacket[] dataPackets = serialEvent.dataPackets;

            for (int i = 0, length = dataPackets.Length; i < length; ++i)
            {
                SerialTestPacket packet = dataPackets[i] as SerialTestPacket;
                Debug.Log(packet.data);
            }
        }

        /// <summary>
        /// Passes the test.
        /// </summary>
        private void Pass()
        {
            if (m_serial != null)
            {
                m_serial.RemoveEventListener(SerialEvent.Data, OnSerialData);
                m_serial.Close();
            }

            IntegrationTest.Pass(gameObject);
        }
    }
}
```