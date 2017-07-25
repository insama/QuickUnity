using CSharpExtensions.Events;
using CSharpExtensions.IO.Ports;
using QuickUnity.IO.Ports;
using System;
using System.Collections.Generic;
using System.IO;
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

        private class TestSerialPortPacketHandler : ISerialPortPacketHandler, IDisposable
        {
            private const int MaxLength = 14;

            private MemoryStream m_buffer = new MemoryStream();
            private BinaryReader m_reader;

            public TestSerialPortPacketHandler()
            {
                m_reader = new BinaryReader(m_buffer);
            }

            public ISerialPortPacket Pack(object data)
            {
                byte[] bytes = Encoding.Default.GetBytes((string)data);
                return new TestSerialPortPacket(bytes);
            }

            public ISerialPortPacket[] Unpack(byte[] bytes)
            {
                List<ISerialPortPacket> packets = new List<ISerialPortPacket>();
                m_buffer.Write(bytes, 0, bytes.Length);

                while (m_buffer != null && m_buffer.Position >= MaxLength)
                {
                    ISerialPortPacket packet = Decode();

                    if (packet != null)
                    {
                        packets.Add(packet);
                    }
                }

                return packets.ToArray();
            }

            private ISerialPortPacket Decode()
            {
                long bytesPosition = m_buffer.Position;

                m_buffer.Position = 0;
                char[] chars = m_reader.ReadChars(MaxLength);
                long currentPosition = m_buffer.Position;

                if (bytesPosition >= currentPosition)
                {
                    m_buffer.Position = 0;
                    m_buffer.Write(m_buffer.GetBuffer(), (int)currentPosition, (int)(bytesPosition - currentPosition));
                }

                return new TestSerialPortPacket(new string(chars));
            }

            public void Dispose()
            {
                if (m_reader != null)
                {
                    m_reader.Close();
                    m_reader = null;
                }

                if (m_buffer != null)
                {
                    m_buffer.Dispose();
                    m_buffer = null;
                }
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

        private void OnDisable()
        {
            if (m_serialPort != null)
            {
                m_serialPort.Close();
                UnityEngine.Debug.Log("close serial port");
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
            m_serialPort.Send("MonoSerialPort.Send");
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