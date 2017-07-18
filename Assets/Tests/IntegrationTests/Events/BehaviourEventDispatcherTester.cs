using QuickUnity.Events;
using System.Collections;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Tester simulation for integration test of BehaviourEventDispatcher.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour"/>
    public class BehaviourEventDispatcherTester : BehaviourEventDispatcher
    {
        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            Invoke("DelayDispatch", 0.5f);
        }

        /// <summary>
        /// Dispatch event delay.
        /// </summary>
        private void DelayDispatch()
        {
            DispatchEvent(new TestEvent(TestEvent.Test));
        }
    }
}