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

using System;

namespace QuickUnity.Timers
{
    /// <summary>
    /// Generates an event after a set interval, with an option to generate recurring events. 
    /// </summary>
    /// <seealso cref="Events.EventDispatcher"/>
    /// <seealso cref="ITimer"/>
    public class Timer : ITimer
    {
        /// <summary>
        /// The minimum delay time. 
        /// </summary>
        private const float minDelayTime = 0.02f;

        #region Event Memebers

        /// <summary>
        /// Indicates that the <see cref="Timer"/> started. 
        /// </summary>
        public TimerStartedEventHandler TimerStarted;

        /// <summary>
        /// Indicates that the <see cref="Timer"/> is ticking. 
        /// </summary>
        public TimerTickingEventHandler TimerTicking;

        /// <summary>
        /// Indicates that the <see cref="Timer"/> completed. 
        /// </summary>
        public TimerCompletedEventHandler TimerCompleted;

        /// <summary>
        /// Indicates that the <see cref="Timer"/> paused. 
        /// </summary>
        public TimerPausedEventHandler TimerPaused;

        /// <summary>
        /// Indicates that the <see cref="Timer"/> resumed. 
        /// </summary>
        public TimerResumedEventHandler TimerResumed;

        /// <summary>
        /// Indicates that the <see cref="Timer"/> stopped. 
        /// </summary>
        public TimerStoppedEventHandler TimerStopped;

        /// <summary>
        /// Indicates that the <see cref="Timer"/> reseted. 
        /// </summary>
        public TimerResetedEventHandler TimerReseted;

        #endregion Event Memebers

        #region Fields

        private float time;

        private uint currentCount = 0;

        private float delay;

        private uint repeatCount;

        private TimerState timerState;

        private bool ignoreTimeScale = true;
        private bool stopOnDisable = true;

        #endregion Fields

        #region ITimer Interface

        /// <summary>
        /// Gets the current count of <see cref="ITimer"/>. 
        /// </summary>
        /// <value> The current count of <see cref="ITimer"/>. </value>
        public uint CurrentCount
        {
            get { return currentCount; }
        }

        /// <summary>
        /// Gets the delay time of <see cref="ITimer"/>. 
        /// </summary>
        /// <value> The delay timer of <see cref="ITimer"/>. </value>
        public float Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        /// <summary>
        /// Gets the repeat count of <see cref="ITimer"/>. 
        /// </summary>
        /// <value> The repeat count of <see cref="ITimer"/>. </value>
        public uint RepeatCount
        {
            get { return repeatCount; }
            set { repeatCount = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ITimer"/> is enabled. 
        /// </summary>
        /// <value> <c> true </c> if enabled Tick function will be invoked; otherwise, <c> false </c>. </value>
        public bool Enabled
        {
            set
            {
                if (!value)
                {
                    // Disable timer object.
                    if (StopOnDisable)
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
                    if (!StopOnDisable && timerState == TimerState.Pause)
                    {
                        Resume();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the state of the <see cref="ITimer"/>. 
        /// </summary>
        /// <value> The state of the <see cref="ITimer"/>. </value>
        public TimerState TimerState
        {
            get { return timerState; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ITimer"/> ignore time scale of Unity. 
        /// </summary>
        /// <value> <c> true </c> if ignore time scale of Unity; otherwise, <c> false </c>. </value>
        public bool IgnoreTimeScale
        {
            get { return ignoreTimeScale; }
            set { ignoreTimeScale = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ITimer"/> stop when the <see cref="ITimer"/> is disabled. 
        /// </summary>
        /// <value> <c> true </c> if the <see cref="ITimer"/> stop whtn the <see cref="ITimer"/> is disabled; otherwise, <c> false </c>. </value>
        public bool StopOnDisable
        {
            get { return stopOnDisable; }
            set { stopOnDisable = value; }
        }

        #endregion ITimer Interface

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class. 
        /// </summary>
        /// <param name="delay"> The delay of the <see cref="ITimer"/>. </param>
        /// <param name="repeatCount"> The repeat count of the <see cref="ITimer"/>. </param>
        /// <param name="ignoreTimeScale"> if set to <c> true </c> the <see cref="ITimer"/> will ignore time scale of Unity. </param>
        /// <param name="stopOnDisable"> if set to <c> true </c> the <see cref="ITimer"/> won't stop when the <see cref="ITimer"/> is disabled. </param>
        /// <param name="autoStart"> if set to <c> true </c> the <see cref="ITimer"/> will start automatically. </param>
        public Timer(float delay, uint repeatCount = 0, bool ignoreTimeScale = true, bool stopOnDisable = true, bool autoStart = true)
        {
            timerState = TimerState.Stop;

            if (delay > minDelayTime)
            {
                this.delay = delay;
            }
            else
            {
                this.delay = minDelayTime;
            }

            this.repeatCount = repeatCount;
            this.ignoreTimeScale = ignoreTimeScale;
            this.stopOnDisable = stopOnDisable;

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
            if (timerState != TimerState.Running)
            {
                timerState = TimerState.Running;
                DispatchTimerStartedEvent();
            }
        }

        /// <summary>
        /// This <see cref="ITimer"/> pause timing. 
        /// </summary>
        public void Pause()
        {
            if (timerState != TimerState.Pause)
            {
                timerState = TimerState.Pause;
                DispatchTimerPausedEvent();
            }
        }

        /// <summary>
        /// This <see cref="ITimer"/> resume timing. 
        /// </summary>
        public void Resume()
        {
            if (timerState == TimerState.Pause)
            {
                timerState = TimerState.Running;
                DispatchTimerResumedEvent();
            }
        }

        /// <summary>
        /// This <see cref="ITimer"/> stop timing. 
        /// </summary>
        public void Stop()
        {
            if (timerState != TimerState.Stop)
            {
                timerState = TimerState.Stop;
                DispatchTimerStoppedEvent();
            }
        }

        /// <summary>
        /// This <see cref="ITimer"/> resets timing. Set CurrentCount to 0. 
        /// </summary>
        public void Reset()
        {
            Stop();

            currentCount = 0;
            time = 0f;

            DispatchTimerResetedEvent();
        }

        /// <summary>
        /// This <see cref="ITimer"/> tick. 
        /// </summary>
        /// <param name="deltaTime"> The delta time. </param>
        public void Tick(float deltaTime)
        {
            if (timerState == TimerState.Running)
            {
                time += deltaTime;

                if (time >= delay)
                {
                    // Dispatch timer event.
                    currentCount++;
                    DispatchTimerTickingEvent();

                    // Dispatch timer complete event.
                    if (repeatCount != 0 && currentCount >= repeatCount)
                    {
                        DispatchTimerCompletedEvent();
                        Reset();
                    }

                    // Reset delay time.
                    time -= delay;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. 
        /// </summary>
        public virtual void Dispose()
        {
            TimerManager.Instance.Remove(this);
        }

        #endregion ITimer Interface

        #region Protected Methods

        /// <summary>
        /// Initializes this instance. 
        /// </summary>
        protected virtual void Initialize()
        {
            TimerManager.Instance.Add(this);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Dispatches the event of this <see cref="Timer"/> started. 
        /// </summary>
        private void DispatchTimerStartedEvent()
        {
            if (TimerStarted != null)
            {
                TimerStarted.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Dispatches the event of this <see cref="Timer"/> ticking. 
        /// </summary>
        private void DispatchTimerTickingEvent()
        {
            if (TimerTicking != null)
            {
                TimerTicking.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Dispatches the event of this <see cref="Timer"/> completed. 
        /// </summary>
        private void DispatchTimerCompletedEvent()
        {
            if (TimerCompleted != null)
            {
                TimerCompleted.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Dispatches the event of this <see cref="Timer"/> paused. 
        /// </summary>
        private void DispatchTimerPausedEvent()
        {
            if (TimerPaused != null)
            {
                TimerPaused.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Dispatches the event of this <see cref="Timer"/> resumed. 
        /// </summary>
        private void DispatchTimerResumedEvent()
        {
            if (TimerResumed != null)
            {
                TimerResumed.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Dispatches the event of this <see cref="Timer"/> stopped. 
        /// </summary>
        private void DispatchTimerStoppedEvent()
        {
            if (TimerStopped != null)
            {
                TimerStopped.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Dispatches the event of this <see cref="Timer"/> reseted. 
        /// </summary>
        private void DispatchTimerResetedEvent()
        {
            if (TimerReseted != null)
            {
                TimerReseted.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion Private Methods
    }
}