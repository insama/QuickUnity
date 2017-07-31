using CSharpExtensions.IO.Ports;
using QuickUnity.Events;
using System;

namespace QuickUnity.IO.Ports
{
    /// <summary>
    /// The <see cref="SerialPortEvent"/> class represents event objects that are specific to the <see cref="MonoSerialPort"/> object.
    /// </summary>
    /// <seealso cref="Event"/>
    public class SerialPortEvent : Event
    {
        #region Event Constants

        /// <summary>
        /// Occurs when serial port open.
        /// </summary>
        public const string SerialPortOpen = "SerialPortOpen";

        /// <summary>
        /// Occurs when serial port received data.
        /// </summary>
        public const string SerialPortDataReceived = "SerialPortDataReceived";

        /// <summary>
        /// Occurs when serial port caught <see cref="System.Exception"/>.
        /// </summary>
        public const string SerialPortExceptionCaught = "SerialPortExceptionCaught";

        /// <summary>
        /// Occurs when serial port closed.
        /// </summary>
        public const string SerialPortClosed = "SerialPortClosed";

        #endregion Event Constants

        private ISerialPortPacket serialPortPacket;

        private Exception exception;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialPortEvent"/> class with event type, <see cref="SerialPort"/> instance and <see
        /// cref="ISerialPortPacket"/> unpacked.
        /// </summary>
        /// <param name="eventType">The type of event.</param>
        /// <param name="serialPort">The <see cref="MonoSerialPort"/> instance.</param>
        /// <param name="packet">The <see cref="ISerialPortPacket"/> unpacked.</param>
        public SerialPortEvent(string eventType, MonoSerialPort serialPort, ISerialPortPacket packet = null)
            : base(eventType, serialPort)
        {
            serialPortPacket = packet;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialPortEvent"/> class with event type, <see cref="SerialPort"/> instance and <see
        /// cref="System.Exception"/> caught.
        /// </summary>
        /// <param name="eventType">The type of event.</param>
        /// <param name="serialPort">The <see cref="MonoSerialPort"/> instance.</param>
        /// <param name="exception">The <see cref="System.Exception"/> caught.</param>
        public SerialPortEvent(string eventType, MonoSerialPort serialPort, Exception exception)
            : base(eventType, serialPort)
        {
            this.exception = exception;
        }

        #endregion Constructors

        /// <summary>
        /// Gets the serial port packet.
        /// </summary>
        /// <value>The <see cref="ISerialPortPacket"/> unpacked.</value>
        public ISerialPortPacket SerialPortPacket
        {
            get { return serialPortPacket; }
        }

        /// <summary>
        /// Gets the <see cref="System.Exception"/> caught.
        /// </summary>
        /// <value>The <see cref="System.Exception"/> caught.</value>
        public Exception Exception
        {
            get { return exception; }
        }
    }
}