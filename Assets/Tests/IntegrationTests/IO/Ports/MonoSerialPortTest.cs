using CSharpExtensions.Events;
using CSharpExtensions.IO.Ports;
using QuickUnity.IO.Ports;
using System.Text;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("MonoSerialPortTest")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    internal class MonoSerialPortTest : UnityEngine.MonoBehaviour
    {
        private class TestSerialPortPacket : ISerialPortPacket
        {
            private byte[] m_Bytes;
            private string m_Text;

            public TestSerialPortPacket(byte[] bytes)
            {
                m_Bytes = bytes;
            }

            public TestSerialPortPacket(string text)
            {
                m_Text = text;
            }

            public byte[] Bytes
            {
                get { return m_Bytes; }
            }

            public string Text
            {
                get { return m_Text; }
            }
        }

        private class TestSerialPortPacketHandler : ISerialPortPacketHandler
        {
            public ISerialPortPacket Pack(object data)
            {
                byte[] bytes = Encoding.Default.GetBytes((string)data);
                return new TestSerialPortPacket(bytes);
            }

            public ISerialPortPacket[] Unpack(byte[] bytes)
            {
                string text = Encoding.Default.GetString(bytes);
                return new ISerialPortPacket[] { new TestSerialPortPacket(text) };
            }
        }

        private MonoSerialPort m_serialPort;

        private void Start()
        {
            m_serialPort = new MonoSerialPort("COM2");
            m_serialPort.PacketHandler = new TestSerialPortPacketHandler();
            m_serialPort.AddEventListener(SerialPortEvent.SerialPortOpen, OnSerialPortOpen);
            m_serialPort.AddEventListener(SerialPortEvent.SerialPortDataReceived, OnSerialDataReceived);
            m_serialPort.AddEventListener(SerialPortEvent.SerialPortException, OnSerialPortException);
            m_serialPort.AddEventListener(SerialPortEvent.SerialPortClosed, OnSerialPortClosed);
            m_serialPort.Open();
        }

        private void Update()
        {
            if (m_serialPort != null)
            {
                m_serialPort.Update();
            }
        }

        private void OnDestroy()
        {
            if (m_serialPort != null)
            {
                m_serialPort.Close();
                m_serialPort.RemoveEventListener(SerialPortEvent.SerialPortOpen, OnSerialPortOpen);
                m_serialPort.RemoveEventListener(SerialPortEvent.SerialPortDataReceived, OnSerialDataReceived);
                m_serialPort.RemoveEventListener(SerialPortEvent.SerialPortException, OnSerialPortException);
                m_serialPort.RemoveEventListener(SerialPortEvent.SerialPortClosed, OnSerialPortClosed);
                m_serialPort = null;
            }
        }

        private void OnSerialPortOpen(Event e)
        {
            UnityEngine.Debug.Log("Serial port open.");
        }

        private void OnSerialDataReceived(Event e)
        {
            SerialPortEvent serialEvent = (SerialPortEvent)e;
            TestSerialPortPacket packet = (TestSerialPortPacket)serialEvent.serialPortPacket;
            UnityEngine.Debug.Log("Serial port received data: " + packet.Text);
        }

        private void OnSerialPortException(Event e)
        {
            SerialPortEvent serialEvent = (SerialPortEvent)e;
            UnityEngine.Debug.LogException(serialEvent.exception);
        }

        private void OnSerialPortClosed(Event e)
        {
            UnityEngine.Debug.Log("Serial port closed.");
        }
    }
}