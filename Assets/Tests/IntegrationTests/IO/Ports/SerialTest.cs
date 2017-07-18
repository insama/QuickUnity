using QuickUnity.IO.Ports;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Integration test of Serial.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour"/>
    [IntegrationTest.DynamicTest("SerialTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    internal class SerialTest : MonoBehaviour
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
                m_serial.Update();
            }
        }

        /// <summary>
        /// Called when receive [serial data].
        /// </summary>
        /// <param name="evt">The evt.</param>
        private void OnSerialData(CSharpExtensions.Events.Event evt)
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