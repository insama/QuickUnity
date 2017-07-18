using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Integration test of class BehaviourEventDispatcher.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour"/>
    [IntegrationTest.DynamicTest("EventTests")]
    [IntegrationTest.SucceedWithAssertions]
    public class BehaviourEventDispatcherTest : MonoBehaviour
    {
        /// <summary>
        /// The name of DispatcherMock object.
        /// </summary>
        private const string k_mockObjectName = "DispatcherMock";

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            BehaviourEventDispatcherTester mock = FindObjectOfType<BehaviourEventDispatcherTester>();

            if (mock)
            {
                mock.AddEventListener<TestEvent>(TestEvent.Test, MockEventTestHandler);
            }
            else
            {
                IntegrationTest.Fail(gameObject, "Can not find DispatchMock component!");
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            BehaviourEventDispatcherTester mock = FindObjectOfType<BehaviourEventDispatcherTester>();

            if (mock)
                IntegrationTest.Assert(gameObject, mock.HasEventListener<TestEvent>(TestEvent.Test, MockEventTestHandler));
        }

        private void OnDestroy()
        {
            BehaviourEventDispatcherTester mock = FindObjectOfType<BehaviourEventDispatcherTester>();

            if (mock)
                mock.RemoveEventListener<TestEvent>(TestEvent.Test, MockEventTestHandler);
        }

        /// <summary>
        /// Mocks the event test eventType handler.
        /// </summary>
        /// <param name="mockEvent">The mock event.</param>
        private void MockEventTestHandler(TestEvent mockEvent)
        {
            IntegrationTest.Pass(gameObject);
        }
    }
}