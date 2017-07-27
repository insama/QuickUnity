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

namespace QuickUnity.Events
{
    /// <summary>
    /// The Event class is used as the base class for the creation of Event objects, which are passed as parameters to event listeners when an event occurs. 
    /// </summary>
    public class Event
    {
        /// <summary>
        /// The type of event. 
        /// </summary>
        private string eventType;

        /// <summary>
        /// The type of event. 
        /// </summary>
        /// <value> The type of event. </value>
        public string EventType
        {
            get { return eventType; }
        }

        /// <summary>
        /// The context object. 
        /// </summary>
        private object context;

        /// <summary>
        /// Gets or sets the context object. 
        /// </summary>
        /// <value> The context object. </value>
        public object Context
        {
            get { return context; }
            set { context = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class. 
        /// </summary>
        /// <param name="eventType"> The type of event. </param>
        /// <param name="context"> The context object. </param>
        public Event(string eventType, object context = null)
        {
            this.eventType = eventType;
            this.context = context;
        }
    }
}