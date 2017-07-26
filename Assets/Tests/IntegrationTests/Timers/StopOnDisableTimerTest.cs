using QuickUnity.Timers;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Integration test of Timer with stopOnDisable is true.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour"/>
    [IntegrationTest.DynamicTest("TimerTests")]
    [IntegrationTest.SucceedWithAssertions]
    public class StopOnDisableTimerTest : MonoBehaviour
    {
        /// <summary>
        /// The test timer.
        /// </summary>
        private ITimer testTimer;

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            testTimer = new Timer(1.0f, 3, true, false, false);
            testTimer.AddEventListener<TimerEvent>(TimerEvent.Timer, OnTimer);
            testTimer.AddEventListener<TimerEvent>(TimerEvent.TimerComplete, OnTimerComplete);
            testTimer.Start();
            Invoke("DisableTimerManager", 1f);
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

        private void DisableTimerManager()
        {
            TimerManager.Instance.enabled = false;
            Invoke("EnableTimerManager", 2f);
        }

        private void EnableTimerManager()
        {
            TimerManager.Instance.enabled = true;
        }
    }
}