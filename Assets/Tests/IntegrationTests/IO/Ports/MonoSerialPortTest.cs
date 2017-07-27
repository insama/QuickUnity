using CSharpExtensions.IO.Ports;
using QuickUnity.Events;
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
            private byte[] bytes;
            private string text;

            public TestSerialPortPacket(byte[] bytes)
            {
                this.bytes = bytes;
            }

            public TestSerialPortPacket(string text)
            {
                this.text = text;
            }

            public byte[] Bytes
            {
                get { return bytes; }
            }

            public string Text
            {
                get { return text; }
            }
        }

        private class TestSerialPortPacketHandler : ISerialPortPacketHandler, IDisposable
        {
            private const int maxLength = 14;

            private MemoryStream buffer = new MemoryStream();
            private BinaryReader reader;

            public TestSerialPortPacketHandler()
            {
                reader = new BinaryReader(buffer);
            }

            public ISerialPortPacket Pack(object data)
            {
                byte[] bytes = Encoding.Default.GetBytes((string)data);
                return new TestSerialPortPacket(bytes);
            }

            public ISerialPortPacket[] Unpack(byte[] bytes)
            {
                List<ISerialPortPacket> packets = new List<ISerialPortPacket>();
                buffer.Write(bytes, 0, bytes.Length);

                while (buffer != null && buffer.Position >= maxLength)
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
                long bytesPosition = buffer.Position;

                buffer.Position = 0;
                char[] chars = reader.ReadChars(maxLength);
                long currentPosition = buffer.Position;

                if (bytesPosition >= currentPosition)
                {
                    buffer.Position = 0;
                    buffer.Write(buffer.GetBuffer(), (int)currentPosition, (int)(bytesPosition - currentPosition));
                }

                return new TestSerialPortPacket(new string(chars));
            }

            public void Dispose()
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }

                if (buffer != null)
                {
                    buffer = null;
                }
            }
        }

        private MonoSerialPort serialPort;

        private void Start()
        {
            serialPort = new MonoSerialPort("COM2");
            serialPort.PacketHandler = new TestSerialPortPacketHandler();
            serialPort.AddEventListener(SerialPortEvent.SerialPortOpen, OnSerialPortOpen);
            serialPort.AddEventListener(SerialPortEvent.SerialPortDataReceived, OnSerialDataReceived);
            serialPort.AddEventListener(SerialPortEvent.SerialPortException, OnSerialPortException);
            serialPort.AddEventListener(SerialPortEvent.SerialPortClosed, OnSerialPortClosed);
            serialPort.Open();
        }

        private void Update()
        {
            if (serialPort != null)
            {
                serialPort.Update();
            }
        }

        private void OnDisable()
        {
            if (serialPort != null)
            {
                serialPort.Close();
                UnityEngine.Debug.Log("close serial port");
                serialPort.RemoveEventListener(SerialPortEvent.SerialPortOpen, OnSerialPortOpen);
                serialPort.RemoveEventListener(SerialPortEvent.SerialPortDataReceived, OnSerialDataReceived);
                serialPort.RemoveEventListener(SerialPortEvent.SerialPortException, OnSerialPortException);
                serialPort.RemoveEventListener(SerialPortEvent.SerialPortClosed, OnSerialPortClosed);
                serialPort = null;
            }
        }

        private void OnSerialPortOpen(Event e)
        {
            UnityEngine.Debug.Log("Serial port open.");
        }

        private void OnSerialDataReceived(Event e)
        {
            SerialPortEvent serialEvent = (SerialPortEvent)e;
            TestSerialPortPacket packet = (TestSerialPortPacket)serialEvent.SerialPortPacket;
            UnityEngine.Debug.Log("Serial port received data: " + packet.Text);
            serialPort.Send("MonoSerialPort.Send");
        }

        private void OnSerialPortException(Event e)
        {
            SerialPortEvent serialEvent = (SerialPortEvent)e;
            UnityEngine.Debug.LogException(serialEvent.Exception);
        }

        private void OnSerialPortClosed(Event e)
        {
            UnityEngine.Debug.Log("Serial port closed.");
        }
    }
}