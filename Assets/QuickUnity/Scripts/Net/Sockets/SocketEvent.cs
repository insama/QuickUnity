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
using System.Net.Sockets;

namespace QuickUnity.Net.Sockets
{
    /// <summary>
    /// The <see cref="SocketEvent"/> class represents event objects that are specific to the <see cref="System.Net.Sockets.Socket"/> object.
    /// </summary>
    /// <seealso cref="Event"/>
    public class SocketEvent : Event
    {
        #region Event Constants

        /// <summary>
        /// Occurs when socket connected.
        /// </summary>
        public const string Connected = "connected";

        /// <summary>
        /// Occurs when socket disconnected.
        /// </summary>
        public const string Disconnected = "disconnected";

        /// <summary>
        /// Occurs when socket received data.
        /// </summary>
        public const string Data = "data";

        /// <summary>
        /// Occurs when socket closed.
        /// </summary>
        public const string Closed = "closed";

        /// <summary>
        /// Occurs when catch socket error.
        /// </summary>
        public const string SocketError = "socketError";

        /// <summary>
        /// Occurs when catch error.
        /// </summary>
        public const string Error = "error";

        /// <summary>
        /// Occurs when socket server start.
        /// </summary>
        public const string ServerStart = "serverStart";

        /// <summary>
        /// Occurs when socket server stop.
        /// </summary>
        public const string ServerStop = "serverStop";

        /// <summary>
        /// Occurs when socket client connected.
        /// </summary>
        public const string ClientConnected = "clientConnected";

        /// <summary>
        /// Occurs when socket client disconnected.
        /// </summary>
        public const string ClientDisconnected = "clientDisconnected";

        /// <summary>
        /// Occurs when socket client received data.
        /// </summary>
        public const string ClientData = "clientData";

        /// <summary>
        /// Occurs when socket client closed.
        /// </summary>
        public const string ClientClosed = "clientClosed";

        /// <summary>
        /// Occurs when socket client catched socket error.
        /// </summary>
        public const string ClientSocketError = "clientSocketError";

        /// <summary>
        /// Occurs when socket client catched error.
        /// </summary>
        public const string ClientError = "clientError";

        #endregion Event Constants

        private MonoTcpClient m_tcpClient;

        private ISocketPacket m_socketPacket;

        private Exception m_exception;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketEvent"/> class with event type.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        public SocketEvent(string eventType)
            : base(eventType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketEvent"/> class with event type, <see cref="<see cref="Array"/>"/> instance and the <see
        /// cref="ISocketPacket"/> received from server.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="client">The <see cref="<see cref="Array"/>"/> instance.</param>
        /// <param name="packet">The <see cref="ISocketPacket"/> received from server.</param>
        public SocketEvent(string eventType, MonoTcpClient client, ISocketPacket packet = null)
            : base(eventType, null)
        {
            m_tcpClient = client;
            m_socketPacket = packet;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketEvent"/> class with event type, the <see cref="Exception"/> catched and <see
        /// cref="<see cref="Array"/>"/> instance.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="exception">The <see cref="Exception"/> catched.</param>
        /// <param name="client">The <see cref="<see cref="Array"/>"/> instance.</param>
        public SocketEvent(string eventType, Exception exception, MonoTcpClient client = null)
            : base(eventType, client)
        {
            m_exception = exception;
            m_tcpClient = client;
        }

        #endregion Constructors

        /// <summary>
        /// Gets the <see cref="<see cref="Array"/>"/> instance.
        /// </summary>
        /// <value>The <see cref="<see cref="Array"/>"/> instance.</value>
        public MonoTcpClient tcpClient
        {
            get { return m_tcpClient; }
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
        /// Gets the <see cref="SocketException"/> catched.
        /// </summary>
        /// <value>The <see cref="SocketException"/> catched.</value>
        public SocketException socketException
        {
            get { return (SocketException)m_exception; }
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> catched.
        /// </summary>
        /// <value>The <see cref="Exception"/> catched.</value>
        public Exception exception
        {
            get { return m_exception; }
        }
    }
}