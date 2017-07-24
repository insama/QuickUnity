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

using CSharpExtensions.Events;
using CSharpExtensions.Net.Sockets;
using System;

namespace QuickUnity.Net.Sockets
{
    /// <summary>
    /// The <see cref="SocketEvent"/> class represents event objects that are specific to the <see cref="MonoTcpClient"/> object or the <see
    /// cref="MonoTcpServer"/> object.
    /// </summary>
    /// <seealso cref="Event"/>
    public class SocketEvent : Event
    {
        #region Event Constants

        /// <summary>
        /// Occurs when socket connected.
        /// </summary>
        public const string SocketConnected = "SocketConnected";

        /// <summary>
        /// Occurs when socket disconnected.
        /// </summary>
        public const string SocketDisconnected = "SocketDisconnected";

        /// <summary>
        /// Occurs when socket received data.
        /// </summary>
        public const string SocketDataReceived = "SocketDataReceived";

        /// <summary>
        /// Occurs when socket closed.
        /// </summary>
        public const string SocketClosed = "SocketClosed";

        /// <summary>
        /// Occurs when caught an <see cref="Exception"/>.
        /// </summary>
        public const string SocketException = "SocketException";

        /// <summary>
        /// Occurs when socket server start.
        /// </summary>
        public const string ServerStart = "SserverStart";

        /// <summary>
        /// Occurs when socket server stop.
        /// </summary>
        public const string ServerStop = "ServerStop";

        /// <summary>
        /// Occurs when TCP server caught an <see cref="Exception"/>.
        /// </summary>
        public const string ServerSocketException = "ServerSocketException";

        /// <summary>
        /// Occurs when socket client connected.
        /// </summary>
        public const string ClientConnected = "ClientConnected";

        /// <summary>
        /// Occurs when socket client disconnected.
        /// </summary>
        public const string ClientDisconnected = "clientDisconnected";

        /// <summary>
        /// Occurs when socket client received data.
        /// </summary>
        public const string ClientData = "ClientDataReceived";

        /// <summary>
        /// Occurs when socket client closed.
        /// </summary>
        public const string ClientClosed = "ClientClosed";

        /// <summary>
        /// Occurs when socket client caught an <see cref="Exception"/>.
        /// </summary>
        public const string ClientSocketException = "ClientSocketException";

        #endregion Event Constants

        private ISocketPacket m_socketPacket;

        private Exception m_exception;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketEvent"/> class with event type and <see cref="MonoTcpClient"/> instance.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="client">The instance of <see cref="MonoTcpClient"/>.</param>
        /// <param name="packet">The <see cref="ISocketPacket"/> unpacked.</param>
        public SocketEvent(string eventType, MonoTcpClient client, ISocketPacket packet = null)
            : base(eventType, client)
        {
            m_socketPacket = packet;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketEvent"/> class with event type and <see cref="MonoTcpClient"/> instance.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="client">The instance of <see cref="MonoTcpClient"/>.</param>
        /// <param name="exception">The <see cref="Exception"/> caught.</param>
        public SocketEvent(string eventType, MonoTcpClient client, Exception exception)
            : base(eventType, client)
        {
            m_exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketEvent"/> class with event type and <see cref="MonoTcpServer"/> instance.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="server">The instance of <see cref="MonoTcpServer"/>.</param>
        public SocketEvent(string eventType, MonoTcpServer server)
            : base(eventType, server)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketEvent"/> class with event type and <see cref="MonoTcpClient"/> instance.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="server">The instance of <see cref="MonoTcpServer"/>.</param>
        /// <param name="exception">The <see cref="Exception"/> caught.</param>
        public SocketEvent(string eventType, MonoTcpServer server, Exception exception)
            : base(eventType, server)
        {
            m_exception = exception;
        }

        #endregion Constructors

        /// <summary>
        /// Gets an instance of <see cref="MonoTcpClient"/>.
        /// </summary>
        /// <value>An instance of <see cref="MonoTcpClient"/></value>
        public MonoTcpClient tcpClient
        {
            get { return (MonoTcpClient)m_Context; }
        }

        /// <summary>
        /// Gets an instance of <see cref="MonoTcpServer"/>.
        /// </summary>
        /// <value>An instance of <see cref="MonoTcpServer"/></value>
        public MonoTcpServer tcpServer
        {
            get { return (MonoTcpServer)m_Context; }
        }

        /// <summary>
        /// Gets the <see cref="ISocketPacket"/> received from server.
        /// </summary>
        /// <value>The <see cref="ISocketPacket"/> received from server.</value>
        public ISocketPacket socketPacket
        {
            get { return m_socketPacket; }
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> caught.
        /// </summary>
        /// <value>The <see cref="Exception"/> caught.</value>
        public Exception exception
        {
            get { return m_exception; }
        }
    }
}