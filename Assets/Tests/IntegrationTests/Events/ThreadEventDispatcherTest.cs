using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Integration test of class ThreadEventDispatcher.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour"/>
    [IntegrationTest.DynamicTest("EventTests")]
    [IntegrationTest.SucceedWithAssertions]
    public class ThreadEventDispatcherTest : MonoBehaviour
    {
        /// <summary>
        /// The image reader.
        /// </summary>
        private ThreadTextReader m_threadImageReader;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            m_threadImageReader = new ThreadTextReader();
            m_threadImageReader.AddEventListener(TestEvent.Complete, OnThreadImageReaderComplete);
        }

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            if (m_threadImageReader != null)
                m_threadImageReader.BeginRead();
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (m_threadImageReader != null)
            {
                m_threadImageReader.RemoveEventListener(TestEvent.Complete, OnThreadImageReaderComplete);
                m_threadImageReader = null;
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (m_threadImageReader != null)
                m_threadImageReader.Update();
        }

        /// <summary>
        /// Called when [thread image reader complete].
        /// </summary>
        /// <param name="eventObject">The event object.</param>
        private void OnThreadImageReaderComplete(CSharpExtensions.Events.Event eventObject)
        {
            IntegrationTest.Pass();
        }
    }
}