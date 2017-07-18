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
            m_client.AddEventListener(SocketEvent.Connected, OnSocketConnected);
            m_client.AddEventListener(SocketEvent.Disconnected, OnSocketDisconnected);
            m_client.AddEventListener(SocketEvent.Data, OnSocketData);
            m_client.AddEventListener(SocketEvent.Closed, OnSocketClosed);
            m_client.AddEventListener(SocketEvent.SocketError, OnSocketError);
            m_client.AddEventListener(SocketEvent.Error, OnError);
            m_client.BeginConnect("192.168.0.131", 10000, true);
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
                m_client.RemoveEventListener(SocketEvent.Connected, OnSocketConnected);
                m_client.RemoveEventListener(SocketEvent.Disconnected, OnSocketDisconnected);
                m_client.RemoveEventListener(SocketEvent.Data, OnSocketData);
                m_client.RemoveEventListener(SocketEvent.Closed, OnSocketClosed);
                m_client.RemoveEventListener(SocketEvent.SocketError, OnSocketError);
                m_client.RemoveEventListener(SocketEvent.Error, OnError);
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

        private void OnSocketError(CSharpExtensions.Events.Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            SocketException e = socketEvent.socketException;
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }

        private void OnError(CSharpExtensions.Events.Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            Exception e = socketEvent.exception;
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }
    }
}