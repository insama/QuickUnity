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

namespace QuickUnity.Timers
{
    /// <summary>
    /// Defines methods to manipulate timer object. 
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// Gets the current count of <see cref="ITimer"/>. 
        /// </summary>
        /// <value> The current count of <see cref="ITimer"/>. </value>
        uint CurrentCount { get; }

        /// <summary>
        /// Gets the delay time of <see cref="ITimer"/>. 
        /// </summary>
        /// <value> The delay timer of <see cref="ITimer"/>. </value>
        float Delay { get; set; }

        /// <summary>
        /// Gets the repeat count of <see cref="ITimer"/>. 
        /// </summary>
        /// <value> The repeat count of <see cref="ITimer"/>. </value>
        uint RepeatCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ITimer"/> is enabled. 
        /// </summary>
        /// <value> <c> true </c> if enabled Tick function will be invoked; otherwise, <c> false </c>. </value>
        bool Enabled { set; }

        /// <summary>
        /// Gets the state of the <see cref="ITimer"/>. 
        /// </summary>
        /// <value> The state of the <see cref="ITimer"/>. </value>
        TimerState TimerState { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ITimer"/> ignore time scale of Unity. 
        /// </summary>
        /// <value> <c> true </c> if ignore time scale of Unity; otherwise, <c> false </c>. </value>
        bool IgnoreTimeScale { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ITimer"/> stop when the <see cref="ITimer"/> is disabled. 
        /// </summary>
        /// <value> <c> true </c> if the <see cref="ITimer"/> stop whtn the <see cref="ITimer"/> is disabled; otherwise, <c> false </c>. </value>
        bool StopOnDisable { get; set; }

        /// <summary>
        /// This <see cref="ITimer"/> start timing. 
        /// </summary>
        void Start();

        /// <summary>
        /// This <see cref="ITimer"/> pause timing. 
        /// </summary>
        void Pause();

        /// <summary>
        /// This <see cref="ITimer"/> resume timing. 
        /// </summary>
        void Resume();

        /// <summary>
        /// This <see cref="ITimer"/> resets timing. Set CurrentCount to 0. 
        /// </summary>
        void Reset();

        /// <summary>
        /// This <see cref="ITimer"/> stop timing. 
        /// </summary>
        void Stop();

        /// <summary>
        /// This <see cref="ITimer"/> tick. 
        /// </summary>
        /// <param name="deltaTime"> The delta time. </param>
        void Tick(float deltaTime);
    }
}