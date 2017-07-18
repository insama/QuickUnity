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

using CSharpExtensions.Patterns.State;
using QuickUnity.Events;

namespace QuickUnity.Patterns.State
{
    /// <summary>
    /// State machine to control state transitions, who is inherited from MonoBehaviour.
    /// </summary>
    /// <seealso cref="BehaviourEventDispatcher"/>
    /// <seealso cref="IStateMachine"/>
    public class BehaviourStateMachine : BehaviourEventDispatcher, IStateMachine
    {
        /// <summary>
        /// The state machine object.
        /// </summary>
        protected IStateMachine m_stateMachine;

        #region Messages

        /// <summary>
        /// Called when script receive message Awake.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_stateMachine = new StateMachine();
        }

        /// <summary>
        /// Called when script receive message Update.
        /// </summary>
        protected virtual void Update()
        {
            Run();
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (m_stateMachine != null)
            {
                m_stateMachine = null;
            }
        }

        #endregion Messages

        #region IStateMachine Interface

        /// <summary>
        /// Directly set the active state by state object.
        /// </summary>
        /// <param name="state">The state object.</param>
        public void SetState(IState state)
        {
            if (m_stateMachine != null)
            {
                m_stateMachine.SetState(state);
            }
        }

        /// <summary>
        /// Registers the transition event.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="state">The state.</param>
        public void RegisterTransitionEvent(string eventType, IState state)
        {
            if (m_stateMachine != null)
            {
                m_stateMachine.RegisterTransitionEvent(eventType, state);
            }
        }

        /// <summary>
        /// Changes the state by event type.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        public void ChangeState(string eventType)
        {
            if (m_stateMachine != null)
            {
                m_stateMachine.ChangeState(eventType);
            }
        }

        /// <summary>
        /// Runs this state machine.
        /// </summary>
        public void Run()
        {
            if (m_stateMachine != null)
            {
                m_stateMachine.Run();
            }
        }

        #endregion IStateMachine Interface
    }
}