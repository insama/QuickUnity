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

using System.Collections.Generic;

namespace QuickUnity.Timers
{
    /// <summary>
    /// Defines methods to manipulate timer collection.
    /// </summary>
    /// <seealso cref="QuickUnity.Core.Collections.Generic.ISimpleCollection{QuickUnity.Timers.ITimer}"/>
    public interface ITimerCollection : ICollection<ITimer>
    {
        /// <summary>
        /// Sets all timers in the <see cref="ITimerCollection"/> to be enabled or not.
        /// </summary>
        /// <param name="value">
        /// Set to <c>true</c> to enable all timers in the <see cref="ITimerCollection"/> control to
        /// trigger their timer event; otherwise, set to <c>false</c>.
        /// </param>
        void SetAllEnabled(bool value = true);

        /// <summary>
        /// Starts all timers in the <see cref="ITimerCollection"/>.
        /// </summary>
        void StartAll();

        /// <summary>
        /// Pauses all timers in the <see cref="ITimerCollection"/>.
        /// </summary>
        void PauseAll();

        /// <summary>
        /// Resumes all timers in <see cref="ITimerCollection"/>.
        /// </summary>
        void ResumeAll();

        /// <summary>
        /// Stops all timers in the <see cref="ITimerCollection"/>.
        /// </summary>
        void StopAll();

        /// <summary>
        /// Resets all timers in the <see cref="ITimerCollection"/>.
        /// </summary>
        void ResetAll();
    }
}