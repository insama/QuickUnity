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

using QuickUnity.Core.Miscs;
using QuickUnity.Events;
using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using UnityEngine;

namespace QuickUnity.IO.Ports
{
    /// <summary>
    /// Serial is used for communication between serial ports.
    /// </summary>
    /// <seealso cref="QuickUnity.Events.ThreadEventDispatcher"/>
    public class Serial : ThreadEventDispatcher
    {
        /// <summary>
        /// Used for locking the instance calls.
        /// </summary>
        private static readonly object s_syncRoot = new object();

        /// <summary>
        /// Whether the data is received.
        /// </summary>
        private bool m_isDataReceived;

        /// <summary>
        /// Whether the port is closing.
        /// </summary>
        private bool m_isClosingPort;

        /// <summary>
        /// The serial port object.
        /// </summary>
        private SerialPort m_serialPort;

        /// <summary>
        /// The thread for receiving data.
        /// </summary>
        private Thread m_receiveDataThread;

        /// <summary>
        /// The serial data packet handler.
        /// </summary>
        private ISerialPacketHandler m_packetHandler;

        /// <summary>
        /// Gets or sets the serial data packet handler.
        /// </summary>
        /// <value>The serial data packet handler.</value>
        public ISerialPacketHandler packetHandler
        {
            get { return m_packetHandler; }
            set { m_packetHandler = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the serial port is open.
        /// </summary>
        /// <value><c>true</c> if the serial port is open; otherwise, <c>false</c>.</value>
        public bool isOpen
        {
            get { return m_serialPort != null && m_serialPort.IsOpen; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Serial"/> class.
        /// </summary>
        /// <param name="portName">The port to use (for example, COM1).</param>
        /// <param name="baudRate">The baud rate.</param>
        /// <param name="parity">One of the Parity values.</param>
        /// <param name="dataBits">The data bits value.</param>
        /// <param name="stopBits">One of the StopBits values.</param>
        public Serial(string portName, int baudRate, Parity parity = Parity.None,
            int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            string[] ports = SerialPort.GetPortNames();

            for (int i = 0, length = ports.Length; i < length; ++i)
            {
                if (portName == ports[i])
                {
                    m_serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
                    break;
                }
            }
        }

        #region Public Functions

        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        public void Open()
        {
            try
            {
                if (m_serialPort != null && !isOpen)
                {
                    m_serialPort.Open();
                    DispatchEvent(new SerialEvent(SerialEvent.Open, this));
                    m_isDataReceived = true;
                }
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception, this);
            }
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Send(object data)
        {
            if (data == null)
            {
                return;
            }

            if (m_packetHandler != null)
            {
                ISerialPacket packet = m_packetHandler.Pack(data);

                if (packet != null && isOpen)
                {
                    try
                    {
                        m_serialPort.Write(packet.bytes, 0, packet.bytes.Length);
                    }
                    catch (Exception exception)
                    {
                        DebugLogger.LogException(exception);
                    }
                }
            }
        }

        /// <summary>
        /// Update is called every frame.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (Time.frameCount % 5 == 0 && m_serialPort != null)
            {
                if (m_isDataReceived)
                {
                    lock (s_syncRoot)
                    {
                        if (m_isDataReceived)
                        {
                            if (m_receiveDataThread == null || !m_receiveDataThread.IsAlive)
                            {
                                BeginReceive();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Closes the port connection.
        /// </summary>
        public void Close()
        {
            if (m_serialPort == null)
            {
                return;
            }

            m_isClosingPort = true;

            while (!m_isDataReceived)
            {
                // Wait for the last data is received.
            }

            try
            {
                if (m_receiveDataThread != null && m_receiveDataThread.IsAlive)
                {
                    m_receiveDataThread.Abort();
                }

                if (isOpen)
                {
                    m_serialPort.Close();
                    DispatchEvent(new SerialEvent(SerialEvent.Close, this));
                }
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }

            m_isClosingPort = false;
        }

        #endregion Public Functions

        /// <summary>
        /// Begins to receive data from serial port.
        /// </summary>
        private void BeginReceive()
        {
            try
            {
                // if has old thread, then abort it.
                if (m_receiveDataThread != null && m_receiveDataThread.IsAlive)
                {
                    m_receiveDataThread.Abort();
                }

                m_receiveDataThread = new Thread(ReceiveData);
                m_receiveDataThread.Start();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Receives the serial port data.
        /// </summary>
        private void ReceiveData()
        {
            m_isDataReceived = false;

            try
            {
                if (isOpen && !m_isClosingPort)
                {
                    string data = Convert.ToChar(m_serialPort.ReadChar()).ToString();
                    byte[] receivedBytes = Encoding.UTF8.GetBytes(data);

                    // Disptach data event.
                    if (m_packetHandler != null)
                    {
                        ISerialPacket[] dataPackets = m_packetHandler.Unpack(receivedBytes);

                        if (dataPackets != null && dataPackets.Length > 0)
                        {
                            DispatchEvent(new SerialEvent(SerialEvent.Data, this, dataPackets));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }

            m_isDataReceived = true;
        }
    }
}