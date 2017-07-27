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

using QuickUnity.Timers;

namespace QuickUnityEditor.Timers
{
    /// <summary>
    /// Class EditorTimer. 
    /// </summary>
    /// <seealso cref="QuickUnity.Timers.Timer"/>
    internal class EditorTimer : Timer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorTimer"/> class. 
        /// </summary>
        /// <param name="delay"> The delay of the <see cref="ITimer"/>. </param>
        /// <param name="repeatCount"> The repeat count of the <see cref="ITimer"/>. </param>
        /// <param name="ignoreTimeScale"> if set to <c> true </c> the <see cref="ITimer"/> will ignore time scale of Unity. </param>
        /// <param name="stopOnDisable"> if set to <c> true </c> the <see cref="ITimer"/> won't stop when the <see cref="ITimer"/> is disabled. </param>
        /// <param name="autoStart"> if set to <c> true </c> the <see cref="ITimer"/> will start automatically. </param>
        public EditorTimer(float delay, uint repeatCount = 0, bool ignoreTimeScale = true, bool stopOnDisable = true, bool autoStart = true)
            : base(delay, repeatCount, ignoreTimeScale, stopOnDisable, autoStart)
        {
        }

        #region ITimer Interface

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. 
        /// </summary>
        public override void Dispose()
        {
            EditorTimerManager.Instance.Remove(this);
        }

        #endregion ITimer Interface

        #region Protected Methods

        /// <summary>
        /// Initializes this instance. 
        /// </summary>
        protected override void Initialize()
        {
            EditorTimerManager.Instance.Add(this);
        }

        #endregion Protected Methods
    }
}