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
    /// Generates an event after a set interval, with an option to generate recurring events.
    /// </summary>
    /// <seealso cref="QuickUnity.Events.EventDispatcher"/>
    /// <seealso cref="QuickUnity.Timers.ITimer"/>
    public class Timer : EventDispatcher, ITimer
    {
        /// <summary>
        /// The minimum delay time.
        /// </summary>
        private const float MinDelayTime = 0.02f;

        /// <summary>
        /// The time of timing.
        /// </summary>
        protected float m_time;

        /// <summary>
        /// The current count of <see cref="ITimer"/>.
        /// </summary>
        protected uint m_currentCount = 0;

        /// <summary>
        /// The delay time of <see cref="ITimer"/>.
        /// </summary>
        protected float m_delay;

        /// <summary>
        /// The repeat count of <see cref="ITimer"/>.
        /// </summary>
        protected uint m_repeatCount;

        /// <summary>
        /// The state of <see cref="ITimer"/>.
        /// </summary>
        protected TimerState m_timerState;

        /// <summary>
        /// The value indicating whether the <see cref="ITimer"/> ignore time scale of Unity.
        /// </summary>
        protected bool m_ignoreTimeScale = true;

        /// <summary>
        /// The value indicating whether the <see cref="ITimer"/> stop when the <see cref="ITimer"/> is disabled.
        /// </summary>
        protected bool m_stopOnDisable = true;

        #region ITimer Interface

        /// <summary>
        /// Gets the current count of <see cref="ITimer"/>.
        /// </summary>
        /// <value>The current count of <see cref="ITimer"/>.</value>
        public uint currentCount
        {
            get { return m_currentCount; }
        }

        /// <summary>
        /// Gets the delay time of <see cref="ITimer"/>.
        /// </summary>
        /// <value>The delay timer of <see cref="ITimer"/>.</value>
        public float delay
        {
            get { return m_delay; }
            set { m_delay = value; }
        }

        /// <summary>
        /// Gets the repeat count of <see cref="ITimer"/>.
        /// </summary>
        /// <value>The repeat count of <see cref="ITimer"/>.</value>
        public uint repeatCount
        {
            get { return m_repeatCount; }
            set { m_repeatCount = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ITimer"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled Tick function will be invoked; otherwise, <c>false</c>.</value>
        public bool enabled
        {
            set
            {
                if (!value)
                {
                    // Disable timer object.
                    if (stopOnDisable)
                    {
                        Reset();
                    }
                    else
                    {
                        Pause();
                    }
                }
                else
                {
                    // Enable timer object.
                    if (!stopOnDisable && m_timerState == TimerState.Pause)
                    {
                        Resume();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the state of the <see cref="ITimer"/>.
        /// </summary>
        /// <value>The state of the <see cref="ITimer"/>.</value>
        public TimerState timerState
        {
            get { return m_timerState; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ITimer"/> ignore time scale of Unity.
        /// </summary>
        /// <value><c>true</c> if ignore time scale of Unity; otherwise, <c>false</c>.</value>
        public bool ignoreTimeScale
        {
            get { return m_ignoreTimeScale; }
            set { m_ignoreTimeScale = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ITimer"/> stop when the <see cref="ITimer"/> is disabled.
        /// </summary>
        /// <value><c>true</c> if the <see cref="ITimer"/> stop whtn the <see cref="ITimer"/> is disabled; otherwise, <c>false</c>.</value>
        public bool stopOnDisable
        {
            get { return m_stopOnDisable; }
            set { m_stopOnDisable = value; }
        }

        #endregion ITimer Interface

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="delay">The delay of the <see cref="ITimer"/>.</param>
        /// <param name="repeatCount">The repeat count of the <see cref="ITimer"/>.</param>
        /// <param name="ignoreTimeScale">if set to <c>true</c> the <see cref="ITimer"/> will ignore time scale of Unity.</param>
        /// <param name="stopOnDisable">if set to <c>true</c> the <see cref="ITimer"/> won't stop when the <see cref="ITimer"/> is disabled.</param>
        /// <param name="autoStart">if set to <c>true</c> the <see cref="ITimer"/> will start automatically.</param>
        public Timer(float delay, uint repeatCount = 0, bool ignoreTimeScale = true, bool stopOnDisable = true, bool autoStart = true)
        {
            m_timerState = TimerState.Stop;

            if (delay > MinDelayTime)
            {
                m_delay = delay;
            }
            else
            {
                m_delay = MinDelayTime;
            }

            m_repeatCount = repeatCount;
            m_ignoreTimeScale = ignoreTimeScale;
            m_stopOnDisable = stopOnDisable;

            Initialize();

            if (autoStart)
            {
                Start();
            }
        }

        #region ITimer Interface

        /// <summary>
        /// This <see cref="ITimer"/> start timing.
        /// </summary>
        public void Start()
        {
            if (m_timerState != TimerState.Running)
            {
                m_timerState = TimerState.Running;
                DispatchEvent(new TimerEvent(TimerEvent.TimerStart, this));
            }
        }

        /// <summary>
        /// This <see cref="ITimer"/> pause timing.
        /// </summary>
        public void Pause()
        {
            if (m_timerState != TimerState.Pause)
            {
                m_timerState = TimerState.Pause;
                DispatchEvent(new TimerEvent(TimerEvent.TimerPause, this));
            }
        }

        /// <summary>
        /// This <see cref="ITimer"/> resume timing.
        /// </summary>
        public void Resume()
        {
            if (m_timerState == TimerState.Pause)
            {
                m_timerState = TimerState.Running;
                DispatchEvent(new TimerEvent(TimerEvent.TimerResume, this));
            }
        }

        /// <summary>
        /// This <see cref="ITimer"/> stop timing.
        /// </summary>
        public void Stop()
        {
            if (m_timerState != TimerState.Stop)
            {
                m_timerState = TimerState.Stop;
                DispatchEvent(new TimerEvent(TimerEvent.TimerStop, this));
            }
        }

        /// <summary>
        /// This <see cref="ITimer"/> resets timing. Set currentCount to 0.
        /// </summary>
        public void Reset()
        {
            Stop();

            m_currentCount = 0;
            m_time = 0f;

            DispatchEvent(new TimerEvent(TimerEvent.TimerReset, this));
        }

        /// <summary>
        /// This <see cref="ITimer"/> tick.
        /// </summary>
        /// <param name="deltaTime">The delta time.</param>
        public void Tick(float deltaTime)
        {
            if (m_timerState == TimerState.Running)
            {
                m_time += deltaTime;

                if (m_time >= m_delay)
                {
                    // Dispatch timer event.
                    m_currentCount++;
                    DispatchEvent(new TimerEvent(TimerEvent.Timer, this));

                    // Dispatch timer complete event.
                    if (m_repeatCount != 0 && m_currentCount >= m_repeatCount)
                    {
                        Reset();
                        DispatchEvent(new TimerEvent(TimerEvent.TimerComplete, this));
                    }

                    // Reset delay time.
                    m_time -= m_delay;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            TimerManager.instance.Remove(this);
        }

        #endregion ITimer Interface

        #region Protected Functions

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected virtual void Initialize()
        {
            TimerManager.instance.Add(this);
        }

        #endregion Protected Functions
    }
}