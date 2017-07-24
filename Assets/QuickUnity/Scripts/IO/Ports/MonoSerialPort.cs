/*
 *	The MIT License (MIT)
 *
 *	Copyright (c) 2017 Jerry Lee
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and associated documentation files (the "Software"), to deal
 *	in the Software without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
 */

using CSharpExtensions.IO.Ports;
using System.IO.Ports;
using System;
using QuickUnity.Events;
using CSharpExtensions.Events;
using System.Threading;

namespace QuickUnity.IO.Ports
{
    /// <summary>
    /// <see cref="SerialPort"/> is class for serial port communication in Mono environment.
    /// </summary>
    /// <seealso cref="SerialPortBase"/>
    /// <seealso cref="IThreadEventDispatcher"/>
    public class MonoSerialPort : SerialPortBase, IThreadEventDispatcher
    {
        private IThreadEventDispatcher m_eventDispatcher;

        private Thread m_receiveDataThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoSerialPort"/> class using the specified
        /// port name, baud rate, parity bit, data bits, and stop bit.
        /// </summary>
        /// <param name="portName">The port to use (for example, COM1).</param>
        /// <param name="baudRate">The baud rate.</param>
        /// <param name="parity">One of the <see cref="Parity"/> values.</param>
        /// <param name="dataBits">The data bits value.</param>
        /// <param name="stopBits">One of the <see cref="StopBits"/> values.</param>
        /// <exception cref="IOException">The specified port could not be found or opened.</exception>
        public MonoSerialPort(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
            : base(portName, baudRate, parity, dataBits, stopBits)
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MonoSerialPort"/> class.
        /// </summary>
        ~MonoSerialPort()
        {
            m_eventDispatcher = null;
        }

        #region IThreadEventDispatcher Interface

        /// <summary>
        /// Update is called every frame.
        /// </summary>
        public void Update()
        {
            if (IsOpen && !m_IsListening && !m_IsClosing && !m_receiveDataThread.IsAlive)
            {
                BeginReceive();
            }

            if (m_eventDispatcher != null)
            {
                m_eventDispatcher.Update();
            }
        }

        /// <summary>
        /// Registers an event listener object with an EventDispatcher object so that the listener
        /// receives notification of an event.
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
        /// Checks whether the EventDispatcher object has any listeners registered for a specific
        /// type of event.
        /// </summary>
        /// <param name="eventType">The type of event.</param>
        /// <param name="listener">The listener function that processes the event.</param>
        /// <returns>
        /// A value of <c>true</c> if a listener of the specified type is registered; <c>false</c> otherwise.
        /// </returns>
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

        #region Protected Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected override void Initialize()
        {
            m_eventDispatcher = new ThreadEventDispatcher();
        }

        /// <summary>
        /// Dispatches the event of serial port open.
        /// </summary>
        protected override void DispatchSerialPortOpenEvent()
        {
            DispatchEvent(new SerialPortEvent(SerialPortEvent.SerialPortOpen, this));
        }

        /// <summary>
        /// Dispatches the event of serial port received data.
        /// </summary>
        /// <param name="packet">The <see cref="ISerialPortPacket"/> unpacked.</param>
        protected override void DispatchSeriaPortDataReceivedEvent(ISerialPortPacket packet)
        {
            DispatchEvent(new SerialPortEvent(SerialPortEvent.SerialPortDataReceived, this, packet));
        }

        /// <summary>
        /// Dispatches the event of the <see cref="Exception"/> caught.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> caught.</param>
        protected override void DispatchSerialPortExceptionEvent(Exception exception)
        {
            DispatchEvent(new SerialPortEvent(SerialPortEvent.SerialPortException, this, exception));
        }

        /// <summary>
        /// Dispatches the event of serial port closed.
        /// </summary>
        protected override void DispatchSerialPortClosedEvent()
        {
            DispatchEvent(new SerialPortEvent(SerialPortEvent.SerialPortClosed, this));
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="MonoSerialPort"/> and optionally
        /// releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        /// unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (m_receiveDataThread != null && m_receiveDataThread.IsAlive)
            {
                m_receiveDataThread.Abort();
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Begins to receive data.
        /// </summary>
        private void BeginReceive()
        {
            try
            {
                m_receiveDataThread = new Thread(new ThreadStart(ReceiveData));
                m_receiveDataThread.Name = "MonoSerialPort.ReceiveData";
                m_receiveDataThread.IsBackground = true;
                m_receiveDataThread.Start();
            }
            catch (Exception ex)
            {
                DispatchSerialPortExceptionEvent(ex);
            }
        }

        #endregion Private Methods
    }
}