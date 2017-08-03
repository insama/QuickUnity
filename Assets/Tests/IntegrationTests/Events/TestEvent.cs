using QuickUnity.Events;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Test event object.
    /// </summary>
    /// <seealso cref="QuickUnity.Events.Event"/>
    internal class TestEvent : Event
    {
        /// <summary>
        /// The test event type.
        /// </summary>
        public const string Test = "Test";

        /// <summary>
        /// The event type of complete doing something.
        /// </summary>
        public const string Complete = "Complete";

        /// <summary>
        /// Initializes a new instance of the <see cref="MockEvent"/> class.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="context">The context.</param>
        public TestEvent(string eventType, object context = null)
            : base(eventType, context)
        {
        }
    }
}