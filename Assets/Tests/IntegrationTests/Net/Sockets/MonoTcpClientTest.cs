using CSharpExtensions.Net.Sockets;
using QuickUnity.Net.Sockets;
using System;
using System.IO;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("MonoTcpClientTests")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    internal class MonoTcpClientTest : MonoBehaviour
    {
        private class TestPacket : ISocketPacket
        {
            private byte[] bytes;
            private string text;

            public TestPacket(byte[] bytes)
            {
                this.bytes = bytes;
            }

            public TestPacket(string text)
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

        private class TestPacketHandler : ISocketPacketHandler
        {
            public ISocketPacket Pack(object data)
            {
                byte[] bytes = null;
                MemoryStream stream = new MemoryStream();
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write((string)data);
                    bytes = stream.ToArray();
                    stream.Close();
                    stream = null;
                }

                return new TestPacket(bytes);
            }

            public ISocketPacket[] Unpack(byte[] buffer, int bytesRead)
            {
                MemoryStream stream = new MemoryStream();
                BinaryReader reader = new BinaryReader(stream);
                int data = reader.Read();
                TestPacket packet = new TestPacket(data.ToString());
                return new ISocketPacket[] { packet };
            }
        }

        private MonoTcpClient client;

        private void Start()
        {
            client = new MonoTcpClient();
            client.SocketPacketHandler = new TestPacketHandler();
            client.AddEventListener(SocketEvent.SocketConnected, OnSocketConnected);
            client.AddEventListener(SocketEvent.SocketDisconnected, OnSocketDisconnected);
            client.AddEventListener(SocketEvent.SocketDataReceived, OnSocketData);
            client.AddEventListener(SocketEvent.SocketClosed, OnSocketClosed);
            client.AddEventListener(SocketEvent.ExceptionCaught, OnSocketExceptionCaught);
            client.BeginConnect("127.0.0.1", 10000, true);
        }

        private void Update()
        {
            if (client != null)
            {
                client.Update();
            }
        }

        private void OnDisable()
        {
            if (client != null)
            {
                client.RemoveEventListener(SocketEvent.SocketConnected, OnSocketConnected);
                client.RemoveEventListener(SocketEvent.SocketDisconnected, OnSocketDisconnected);
                client.RemoveEventListener(SocketEvent.SocketDataReceived, OnSocketData);
                client.RemoveEventListener(SocketEvent.SocketClosed, OnSocketClosed);
                client.RemoveEventListener(SocketEvent.ExceptionCaught, OnSocketExceptionCaught);
                client.Close();
                client = null;
            }
        }

        private void OnSocketConnected(Events.Event eventObj)
        {
            Debug.Log("socket is connected");
        }

        private void OnSocketDisconnected(Events.Event eventObj)
        {
            Debug.Log("socket is disconnected");
        }

        private void OnSocketData(Events.Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            TestPacket packet = (TestPacket)socketEvent.SocketPacket;
            Debug.Log(packet.Text);
            IntegrationTest.Pass(gameObject);
        }

        private void OnSocketClosed(Events.Event eventObj)
        {
            Debug.Log("socket is closed");
        }

        private void OnSocketExceptionCaught(Events.Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            Exception e = socketEvent.Exception;
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }
    }
}