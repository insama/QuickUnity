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
    /// The TimerListEvent class represents event objects that are specific to the <see
    /// cref="Timers.TimerList"/> object.
    /// </summary>
    /// <seealso cref="QuickUnity.Events.Event"/>
    public class TimerListEvent : Event
    {
        /// <summary>
        /// Dispatched whenever all timers of the <see cref="Timers.TimerList"/> object start to run.
        /// </summary>
        public const string AllStart = "AllStart";

        /// <summary>
        /// Dispatched whenever all timers of the <see cref="Timers.TimerList"/> object pause.
        /// </summary>
        public const string AllPause = "AllPause";

        /// <summary>
        /// Dispatched whenever all timers of the <see cref="Timers.TimerList"/> object resume to run.
        /// </summary>
        public const string AllResume = "AllResume";

        /// <summary>
        /// Dispatched whenever all timers of the <see cref="Timers.TimerList"/> object stop.
        /// </summary>
        public const string AllStop = "AllStop";

        /// <summary>
        /// Dispatched whenever all timers of the <see cref="Timers.TimerList"/> object reset.
        /// </summary>
        public const string AllReset = "AllReset";

        /// <summary>
        /// Gets the <see cref="ITimerList"/> object.
        /// </summary>
        /// <value>The <see cref="ITimerList"/> object.</value>
        public ITimerList TimerList
        {
            get { return (ITimerList)Context; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerListEvent"/> class.
        /// </summary>
        /// <param name="eventType">The type of the event.</param>
        /// <param name="timerList">The <see cref="ITimerList"/> object.</param>
        public TimerListEvent(string eventType, ITimerList timerList)
            : base(eventType, timerList)
        {
        }
    }
}