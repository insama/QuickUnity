using QuickUnity.Timers;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Integration test of class scaled Timer.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour"/>
    [IntegrationTest.DynamicTest("TimerTests")]
    [IntegrationTest.SucceedWithAssertions]
    public class ScaledTimerTest : MonoBehaviour
    {
        /// <summary>
        /// The test timer.
        /// </summary>
        private ITimer testTimer;

        private void Awake()
        {
            Time.timeScale = 0.5f;
        }

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            testTimer = new Timer(1.0f, 5, false, true, false);
            testTimer.AddEventListener<TimerEvent>(TimerEvent.Timer, OnTimer);
            testTimer.AddEventListener<TimerEvent>(TimerEvent.TimerComplete, OnTimerComplete);
            testTimer.Start();
        }

        private void OnDestroy()
        {
            if (testTimer != null)
            {
                testTimer.RemoveEventListener<TimerEvent>(TimerEvent.Timer, OnTimer);
                testTimer.RemoveEventListener<TimerEvent>(TimerEvent.TimerComplete, OnTimerComplete);
                testTimer.Dispose();
                testTimer = null;
            }
        }

        private void OnTimer(TimerEvent timerEvent)
        {
            ITimer timer = timerEvent.TimerObject;
            Debug.Log(timer.CurrentCount);
        }

        private void OnTimerComplete(TimerEvent timerEvent)
        {
            IntegrationTest.Pass(gameObject);
        }
    }
}