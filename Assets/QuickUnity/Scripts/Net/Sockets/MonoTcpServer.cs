/*
 *  The MIT License (MIT)
 *
 *  Copyright (c) 2017 Jerry Lee
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */

using System;
using System.Net.Sockets;
using CSharpExtensions.Net.Sockets;
using QuickUnity.Events;
using System.Net;
using CSharpExtensions.Events;
using System.Collections.Generic;

namespace QuickUnity.Net.Sockets
{
    /// <summary>
    /// <see cref="MonoTcpServer"/> class to listen, send and receive for connections from TCP network clients for Unity engine.
    /// </summary>
    /// <seealso cref="TcpServerBase"/>
    /// <seealso cref="IThreadEventDispatcher"/>
    public class MonoTcpServer : TcpServerBase, IThreadEventDispatcher
    {
        protected ThreadEventDispatcher m_eventDispatcher;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoTcpServer"/> class with the specified local endpoint.
        /// </summary>
        /// <param name="localEP">An <see cref="IPEndPoint"/> that represents the local endpoint to which to bind the listener <see cref="Socket"/>.</param>
        /// <exception cref="ArgumentNullException"><c>localEP</c> is <c>null</c>.</exception>
        public MonoTcpServer(IPEndPoint localEP)
            : base(localEP)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoTcpServer"/> class that listens for incoming connection attempts on the specified local IP
        /// address and port number.
        /// </summary>
        /// <param name="localaddr">An <see cref="IPAddress"/> that represents the local IP address.</param>
        /// <param name="port">The port on which to listen for incoming connection attempts.</param>
        /// <exception cref="ArgumentNullException"><c>localaddr</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>port</c> is not between <see cref="IPEndPoint.MinPort"/> and <see cref="IPEndPoint.MaxPort"/>.</exception>
        public MonoTcpServer(IPAddress localaddr, int port)
            : base(localaddr, port)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoTcpServer"/> class that listens for incoming connection attempts on the specified local IP
        /// address string and port number.
        /// </summary>
        /// <param name="localhost">An <see cref="string"/> that represents the local IP address.</param>
        /// <param name="port">The port on which to listen for incoming connection attempts.</param>
        /// <exception cref="ArgumentNullException"><c>localaddr</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>port</c> is not between <see cref="IPEndPoint.MinPort"/> and <see cref="IPEndPoint.MaxPort"/>.</exception>
        public MonoTcpServer(string localhost, int port)
            : base(localhost, port)
        {
        }

        #endregion Constructors

        /// <summary>
        /// Finalizes an instance of the <see cref="MonoTcpServer"/> class.
        /// </summary>
        ~MonoTcpServer()
        {
            m_eventDispatcher = null;
        }

        #region Public Functions

        /// <summary>
        /// Sets the keepalive.
        /// </summary>
        /// <param name="modeOn">if set to <c>true</c> [keepalive of TCP mode on].</param>
        /// <param name="keepaliveTime">The keepalive time in milliseconds.</param>
        /// <param name="keepaliveInterval">The keepalive interval in milliseconds.</param>
        /// <exception cref="NotImplementedException">The <see cref="Socket.IOControl"/> didn't implement in Mono environment.</exception>
        public override void SetKeepalive(bool modeOn = true, int keepaliveTime = 5000, int keepaliveInterval = 75)
        {
            throw new NotImplementedException();
        }

        #endregion Public Functions

        #region IThreadEventDispatcher Interface

        /// <summary>
        /// Update is called every frame.
        /// </summary>
        public void Update()
        {
            if (m_eventDispatcher != null)
            {
                m_eventDispatcher.Update();
            }

            foreach (KeyValuePair<IPEndPoint, TcpClientBase> kvp in clients)
            {
                if (kvp.Value != null)
                {
                    MonoTcpClient tcpClient = (MonoTcpClient)kvp.Value;
                    tcpClient.Update();
                }
            }
        }

        /// <summary>
        /// Registers an event listener object with an EventDispatcher object so that the listener receives notification of an event.
        /// </summary>
        /// <param name="eventType">The type of event.</param>
        /// <param name="listener">The listener function that processes the event.</param>
        public void AddEventListener(string eventType, Action<Event> listener)
        {
            if (m_eventDispatcher != null)
            {
                m_eventDispatcher.AddEventListener(eventType, listener);
            }
        }

        /// <summary>
        /// Dispatches the event.
        /// </summary>
        /// <param name="eventObject">The event object.</param>
        public void DispatchEvent(Event eventObject)
        {
            if (m_eventDispatcher != null)
            {
                m_eventDispatcher.DispatchEvent(eventObject);
            }
        }

        /// <summary>
        /// Checks whether the EventDispatcher object has any listeners registered for a specific type of event.
        /// </summary>
        /// <param name="eventType">The type of event.</param>
        /// <param name="listener">The listener function that processes the event.</param>
        /// <returns>A value of <c>true</c> if a listener of the specified type is registered; <c>false</c> otherwise.</returns>
        public bool HasEventListener(string eventType, Action<Event> listener)
        {
            if (m_eventDispatcher != null)
            {
                return m_eventDispatcher.HasEventListener(eventType, listener);
            }

            return false;
        }

        /// <summary>
        /// Removes a listener from the EventDispatcher object.
        /// </summary>
        /// <param name="eventType">The type of event.</param>
        /// <param name="listener">The listener object to remove.</param>
        public void RemoveEventListener(string eventType, Action<Event> listener)
        {
            if (m_eventDispatcher != null)
            {
                m_eventDispatcher.RemoveEventListener(eventType, listener);
            }
        }

        #endregion IThreadEventDispatcher Interface

        #region Protected Functions

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            m_eventDispatcher = new ThreadEventDispatcher();
        }

        /// <summary>
        /// Dispatches the event of server start.
        /// </summary>
        protected override void DispatchServerStartEvent()
        {
            m_eventDispatcher.DispatchEvent(new SocketEvent(SocketEvent.ServerStart));
        }

        /// <summary>
        /// Dispatches the event of server stop.
        /// </summary>
        protected override void DispatchServerStopEvent()
        {
            m_eventDispatcher.DispatchEvent(new SocketEvent(SocketEvent.ServerStop));
        }

        /// <summary>
        /// Dispatches the event of server socket error.
        /// </summary>
        /// <param name="socketException">The <see cref="SocketException"/> from server socket connection.</param>
        protected override void DispatchSocketErrorEvent(SocketException socketException)
        {
            m_eventDispatcher.DispatchEvent(new SocketEvent(SocketEvent.SocketError, socketException));
        }

        /// <summary>
        /// Dispatches the event of server error.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> from server error.</param>
        protected override void DispathErrorEvent(Exception exception)
        {
            m_eventDispatcher.DispatchEvent(new SocketEvent(SocketEvent.SocketError, exception));
        }

        /// <summary>
        /// Disposes the <see cref="TcpClientBase"/>.
        /// </summary>
        /// <param name="client">The <see cref="TcpClientBase"/> that need to be disposed.</param>
        protected override void DisposeTcpClient(TcpClientBase client)
        {
            if (client != null)
            {
                MonoTcpClient tcpClient = (MonoTcpClient)client;
                tcpClient.RemoveEventListener(SocketEvent.Connected, OnClientConnected);
                tcpClient.RemoveEventListener(SocketEvent.Disconnected, OnClientDisconnected);
                tcpClient.RemoveEventListener(SocketEvent.Data, OnClientData);
                tcpClient.RemoveEventListener(SocketEvent.Closed, OnClientClosed);
                tcpClient.RemoveEventListener(SocketEvent.SocketError, OnClientSocketError);
                tcpClient.RemoveEventListener(SocketEvent.Error, OnClientError);
            }
        }

        /// <summary>
        /// Handles the <see cref="System.Net.Sockets.TcpClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Sockets.TcpClient"/>.</param>
        /// <param name="packetHandler">The <see cref="ISocketPacketHandler"/> for handling socket packets.</param>
        /// <returns>The <see cref="TcpClientBase"/> that was converted.</returns>
        protected override TcpClientBase HandleTcpClient(System.Net.Sockets.TcpClient client, ISocketPacketHandler packetHandler)
        {
            MonoTcpClient tcpClient = new MonoTcpClient(client, packetHandler);
            tcpClient.AddEventListener(SocketEvent.Connected, OnClientConnected);
            tcpClient.AddEventListener(SocketEvent.Disconnected, OnClientDisconnected);
            tcpClient.AddEventListener(SocketEvent.Data, OnClientData);
            tcpClient.AddEventListener(SocketEvent.Closed, OnClientClosed);
            tcpClient.AddEventListener(SocketEvent.SocketError, OnClientSocketError);
            tcpClient.AddEventListener(SocketEvent.Error, OnClientError);
            return tcpClient;
        }

        #endregion Protected Functions

        #region Private Functions

        /// <summary>
        /// Called when [TCP client socket connected].
        /// </summary>
        /// <param name="eventObj">The <see cref="Event"/> object.</param>
        private void OnClientConnected(Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            MonoTcpClient tcpClient = socketEvent.tcpClient;
            DispatchEvent(new SocketEvent(SocketEvent.ClientConnected, tcpClient));
        }

        /// <summary>
        /// Called when [TCP client socket disconnected].
        /// </summary>
        /// <param name="eventObj">The <see cref="Event"/> object.</param>
        private void OnClientDisconnected(Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            MonoTcpClient tcpClient = socketEvent.tcpClient;
            DispatchEvent(new SocketEvent(SocketEvent.ClientDisconnected, tcpClient));
        }

        /// <summary>
        /// Called when [TCP client socket received data].
        /// </summary>
        /// <param name="eventObj">The <see cref="Event"/> object.</param>
        private void OnClientData(Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            MonoTcpClient tcpClient = socketEvent.tcpClient;
            ISocketPacket socketPacket = socketEvent.socketPacket;
            DispatchEvent(new SocketEvent(SocketEvent.ClientData, tcpClient, socketPacket));
        }

        /// <summary>
        /// Called when [TCP client socket closed].
        /// </summary>
        /// <param name="eventObj">The <see cref="Event"/> object.</param>
        private void OnClientClosed(Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            MonoTcpClient tcpClient = socketEvent.tcpClient;
            DispatchEvent(new SocketEvent(SocketEvent.ClientClosed, tcpClient));
        }

        /// <summary>
        /// Called when [TCP client got socket error].
        /// </summary>
        /// <param name="eventObj">The <see cref="Event"/> object.</param>
        private void OnClientSocketError(Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            MonoTcpClient tcpClient = socketEvent.tcpClient;
            SocketException ex = (SocketException)socketEvent.exception;
            DispatchEvent(new SocketEvent(SocketEvent.ClientSocketError, ex, tcpClient));
        }

        /// <summary>
        /// Called when [TCP client got error].
        /// </summary>
        /// <param name="eventObj">The <see cref="Event"/> object.</param>
        private void OnClientError(Event eventObj)
        {
            SocketEvent socketEvent = (SocketEvent)eventObj;
            MonoTcpClient tcpClient = socketEvent.tcpClient;
            Exception ex = socketEvent.exception;
            DispatchEvent(new SocketEvent(SocketEvent.ClientError, ex, tcpClient));
        }

        #endregion Private Functions
    }
}