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

namespace QuickUnity.Timers
{
    /// <summary>
    /// The TimerEvent class represents event objects that are specific to the Timer object.
    /// </summary>
    /// <seealso cref="Event"/>
    public class TimerEvent : Event
    {
        /// <summary>
        /// Dispatched whenever a Timer start to run.
        /// </summary>
        public const string TimerStart = "TimerStart";

        /// <summary>
        /// Dispatched whenever a Timer object reaches an interval specified according to the
        /// Timer.delay property.
        /// </summary>
        public const string Timer = "Timer";

        /// <summary>
        /// Dispatched whenever a Timer pause.
        /// </summary>
        public const string TimerPause = "TimerPause";

        /// <summary>
        /// Dispatched whenever a Timer resume to run.
        /// </summary>
        public const string TimerResume = "TimerResume";

        /// <summary>
        /// Dispatched whenever a Timer stop.
        /// </summary>
        public const string TimerStop = "TimerStop";

        /// <summary>
        /// Dispatched whenever a Timer reset.
        /// </summary>
        public const string TimerReset = "TimerReset";

        /// <summary>
        /// Dispatched whenever it has completed the number of requests set by Timer.repeatCount.
        /// </summary>
        public const string TimerComplete = "TimerComplete";

        /// <summary>
        /// Gets or sets the timer object.
        /// </summary>
        /// <value>The timer object.</value>
        public ITimer TimerObject
        {
            get { return (ITimer)Context; }
            set { Context = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerEvent"/> class.
        /// </summary>
        /// <param name="type">The event type.</param>
        /// <param name="context">The timer object.</param>
        public TimerEvent(string eventType, ITimer timer = null)
            : base(eventType, timer)
        {
        }
    }
}