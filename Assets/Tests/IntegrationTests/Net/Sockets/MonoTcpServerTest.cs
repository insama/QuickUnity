using CSharpExtensions.Net.Sockets;
using QuickUnity.Net.Sockets;
using System;
using System.IO;
using System.Net;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("MonoTcpServerTest")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    internal class MonoTcpServerTest : MonoBehaviour
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

        private MonoTcpServer server;

        private void Start()
        {
            server = new MonoTcpServer(IPAddress.Parse("127.0.0.1"), 10000);
            server.SocketPacketHandler = new TestPacketHandler();
            server.AddEventListener(SocketEvent.ServerStart, OnServerStart);
            server.AddEventListener(SocketEvent.ServerStop, OnServerStop);
            server.AddEventListener(SocketEvent.ServerSocketException, OnServerSocketException);
            server.AddEventListener(SocketEvent.ClientConnected, OnClientConnected);
            server.Start();
        }

        private void Update()
        {
            if (server != null)
            {
                server.Update();
            }
        }

        private void OnDisable()
        {
            if (server != null)
            {
                server.Stop();
                server.RemoveEventListener(SocketEvent.ServerStart, OnServerStart);
                server.RemoveEventListener(SocketEvent.ServerStop, OnServerStop);
                server.RemoveEventListener(SocketEvent.ServerSocketException, OnServerSocketException);
                server.RemoveEventListener(SocketEvent.ClientConnected, OnClientConnected);
                server = null;
            }
        }

        private void OnServerStart(Events.Event eventObj)
        {
            Debug.Log("server started!");
        }

        private void OnServerStop(Events.Event eventObj)
        {
            Debug.Log("server stop");
        }

        private void OnServerSocketException(Events.Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            Exception e = socketEvent.Exception;
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }

        private void OnClientConnected(Events.Event eventObj)
        {
            Debug.Log("client connected!");
            IntegrationTest.Pass();
        }
    }
}