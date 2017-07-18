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

using CSharpExtensions.Events;

namespace QuickUnity.IO.Ports
{
    /// <summary>
    /// The SerialEvent class represents event objects that are specific to the Serial object.
    /// </summary>
    /// <seealso cref="Event"/>
    public class SerialEvent : Event
    {
        /// <summary>
        /// Dispatched whenever a serial port opened.
        /// </summary>
        public const string Open = "Open";

        /// <summary>
        /// Dispatched whenever a serial port received data.
        /// </summary>
        public const string Data = "Data";

        /// <summary>
        /// Dispatched whenever a serial port closed.
        /// </summary>
        public const string Close = "Close";

        /// <summary>
        /// The data packets.
        /// </summary>
        private ISerialPacket[] m_dataPackets;

        /// <summary>
        /// Gets or sets the data packets.
        /// </summary>
        /// <value>The data packets.</value>
        public ISerialPacket[] dataPackets
        {
            get { return m_dataPackets; }
            set { m_dataPackets = value; }
        }

        /// <summary>
        /// Gets the serial object.
        /// </summary>
        /// <value>The serial object.</value>
        public Serial serial
        {
            get { return (Serial)m_Context; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialEvent"/> class.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="serial">The serial.</param>
        /// <param name="dataPackets">The data packets.</param>
        public SerialEvent(string eventType, Serial serial = null, ISerialPacket[] dataPackets = null)
            : base(eventType, serial)
        {
            m_dataPackets = dataPackets;
        }
    }
}