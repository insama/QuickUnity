using CSharpExtensions.Net.Sockets;
using QuickUnity.Net.Sockets;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
            private byte[] m_Bytes;
            private string m_Text;

            public TestPacket(byte[] bytes)
            {
                m_Bytes = bytes;
            }

            public TestPacket(string text)
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

        private MonoTcpServer m_server;

        private void Start()
        {
            m_server = new MonoTcpServer(IPAddress.Parse("127.0.0.1"), 10000);
            m_server.SocketPacketHandler = new TestPacketHandler();
            m_server.AddEventListener(SocketEvent.ServerStart, OnServerStart);
            m_server.AddEventListener(SocketEvent.ServerStop, OnServerStop);
            m_server.AddEventListener(SocketEvent.ServerSocketException, OnServerSocketException);
            m_server.AddEventListener(SocketEvent.ClientConnected, OnClientConnected);
            m_server.Start();
        }

        private void Update()
        {
            if (m_server != null)
            {
                m_server.Update();
            }
        }

        private void OnDestroy()
        {
            if (m_server != null)
            {
                m_server.Stop();
                m_server.RemoveEventListener(SocketEvent.ServerStart, OnServerStart);
                m_server.RemoveEventListener(SocketEvent.ServerStop, OnServerStop);
                m_server.RemoveEventListener(SocketEvent.ServerSocketException, OnServerSocketException);
                m_server.RemoveEventListener(SocketEvent.ClientConnected, OnClientConnected);
                m_server = null;
            }
        }

        private void OnServerStart(CSharpExtensions.Events.Event eventObj)
        {
            Debug.Log("server started!");
        }

        private void OnServerStop(CSharpExtensions.Events.Event eventObj)
        {
            Debug.Log("server stop");
        }

        private void OnServerSocketException(CSharpExtensions.Events.Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            Exception e = socketEvent.exception;
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }

        private void OnClientConnected(CSharpExtensions.Events.Event eventObj)
        {
            Debug.Log("client connected!");
            IntegrationTest.Pass();
        }
    }
}