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

namespace QuickUnity.Net.Sockets
{
    /// <summary>
    /// Provides client connections, data send and data receive for TCP network services for Mono.
    /// </summary>
    /// <seealso cref="CSharpExtensions.Net.Sockets.TcpClientBase"/>
    public class MonoTcpClient : TcpClientBase, IThreadEventDispatcher
    {
        protected ThreadEventDispatcher m_eventDispatcher;

        #region Constructors

        /// <summary> Initializes a new instance of the <see cref="<see cref="Array"/>"/> class. </summary>
        public MonoTcpClient()
            : base()
        {
        }

        /// <summary> Initializes a new instance of the <see cref="<see cref="Array"/>"/> class with the specified <see
        /// cref="System.Net.Sockets.TcpClient"/> and the specified <see cref="ISocketPacketHandler"/>. </summary> <param name="client">The <see
        /// cref="System.Net.Sockets.TcpClient"/> instance.</param> <param name="packetHandler">The <see cref="ISocketPacketHandler"/> to handle socket packet.</param>
        public MonoTcpClient(System.Net.Sockets.TcpClient client, ISocketPacketHandler packetHandler)
            : base(client, packetHandler)
        {
        }

        /// <summary> Initializes a new instance of the <see cref="<see cref="Array"/>"/> class and binds it to the specified local endpoint.
        /// </summary> <param name="localEP">The <see cref="IPEndPoint"/> to which you bind the TCP <see cref="Socket"/>.</param> <exception
        /// cref="ArgumentNullException">The <c>localEP</c> parameter is <c>null</c>.</exception>
        public MonoTcpClient(IPEndPoint localEP)
            : base(localEP)
        {
        }

        /// <summary> Initializes a new instance of the <see cref="<see cref="Array"/>"/> class with the specified family. </summary> <param
        /// name="family">The <see cref="AddressFamily"/> of the IP protocol.</param> <exception cref="ArgumentException"> <para>The <c>family</c>
        /// parameter is not equal to AddressFamily.InterNetwork.</para> <para>-or-</para> <para>The <c>family</c> parameter is not equal to
        /// AddressFamily.InterNetworkV6.</para> </exception>
        public MonoTcpClient(AddressFamily family)
            : base(family)
        {
        }

        /// <summary> Initializes a new instance of the <see cref="<see cref="Array"/>"/> class and connects to the specified port on the specified
        /// host. </summary> <param name="hostname">The DNS name of the remote host to which you intend to connect.</param> <param name="port">The port
        /// number of the remote host to which you intend to connect.</param> <exception cref="ArgumentNullException">The <c>hostname</c> parameter is
        /// <c>null</c>.</exception> <exception cref="ArgumentOutOfRangeException"> The <c>port</c> parameter is not between <see
        /// cref="IPEndPoint.MinPort"/> and <see cref="IPEndPoint.MaxPort"/>. </exception> <exception cref="SocketException">An error occurred when
        /// accessing the socket.</exception>
        public MonoTcpClient(string hostname, int port)
            : base(hostname, port)
        {
        }

        #endregion Constructors

        /// <summary>
        /// Finalizes an instance of the <see cref="MonoTcpClient"/> class.
        /// </summary>
        ~MonoTcpClient()
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
        /// Dispatchs the event of socket connected.
        /// </summary>
        protected override void DispatchSocketConnectedEvent()
        {
            DispatchEvent(new SocketEvent(SocketEvent.Connected, this));
        }

        /// <summary>
        /// Dispatches the event of socket disconnected.
        /// </summary>
        protected override void DispatchSocketDisconnecedEvent()
        {
            DispatchEvent(new SocketEvent(SocketEvent.Disconnected, this));
        }

        /// <summary>
        /// Dispatchs the event of socket receiving data.
        /// </summary>
        /// <param name="packet">The <see cref="ISocketPacket"/> received from server.</param>
        protected override void DispatchSocketDataEvent(ISocketPacket packet)
        {
            DispatchEvent(new SocketEvent(SocketEvent.Data, this, packet));
        }

        /// <summary>
        /// Dispatchs the event of socket closed .
        /// </summary>
        protected override void DispatchSocketClosedEvent()
        {
            DispatchEvent(new SocketEvent(SocketEvent.Closed, this));
        }

        /// <summary>
        /// Dispatches the socket error event.
        /// </summary>
        /// <param name="socketException">The <see cref="SocketException"/> object.</param>
        protected override void DispatchSocketErrorEvent(SocketException socketException)
        {
            DispatchEvent(new SocketEvent(SocketEvent.SocketError, socketException, this));
        }

        /// <summary>
        /// Dispatchs the error event.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> catched.</param>
        protected override void DispatchErrorEvent(Exception exception)
        {
            DispatchEvent(new SocketEvent(SocketEvent.Error, exception, this));
        }

        #endregion Protected Functions
    }
}