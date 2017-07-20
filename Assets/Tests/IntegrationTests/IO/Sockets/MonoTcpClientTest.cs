using CSharpExtensions.Net.Sockets;
using QuickUnity.Net.Sockets;
using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    [IntegrationTest.DynamicTest("MonoTcpClientTest")]
    [IntegrationTest.SucceedWithAssertions]
    [IntegrationTest.Timeout(10)]
    internal class MonoTcpClientTest : MonoBehaviour
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

        private MonoTcpClient m_client;

        private void Start()
        {
            m_client = new MonoTcpClient();
            m_client.SocketPacketHandler = new TestPacketHandler();
            m_client.AddEventListener(SocketEvent.SocketConnected, OnSocketConnected);
            m_client.AddEventListener(SocketEvent.SocketDisconnected, OnSocketDisconnected);
            m_client.AddEventListener(SocketEvent.SocketDataReceived, OnSocketData);
            m_client.AddEventListener(SocketEvent.SocketClosed, OnSocketClosed);
            m_client.AddEventListener(SocketEvent.SocketException, OnSocketException);
            m_client.BeginConnect("127.0.0.1", 10000, true);
        }

        private void Update()
        {
            if (m_client != null)
            {
                m_client.Update();
            }
        }

        private void OnDestroy()
        {
            if (m_client != null)
            {
                m_client.RemoveEventListener(SocketEvent.SocketConnected, OnSocketConnected);
                m_client.RemoveEventListener(SocketEvent.SocketDisconnected, OnSocketDisconnected);
                m_client.RemoveEventListener(SocketEvent.SocketDataReceived, OnSocketData);
                m_client.RemoveEventListener(SocketEvent.SocketClosed, OnSocketClosed);
                m_client.RemoveEventListener(SocketEvent.SocketException, OnSocketException);
                m_client.Close();
                m_client = null;
            }
        }

        private void OnSocketConnected(CSharpExtensions.Events.Event eventObj)
        {
            Debug.Log("socket is connected");
            IntegrationTest.Pass(gameObject);
        }

        private void OnSocketDisconnected(CSharpExtensions.Events.Event eventObj)
        {
            Debug.Log("socket is disconnected");
        }

        private void OnSocketData(CSharpExtensions.Events.Event eventObj)
        {
        }

        private void OnSocketClosed(CSharpExtensions.Events.Event eventObj)
        {
            Debug.Log("socket is closed");
        }

        private void OnSocketException(CSharpExtensions.Events.Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            Exception e = socketEvent.exception;
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }
    }
}