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
        private ITimer m_testTimer;

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            m_testTimer = new Timer(1.0f, 3, true, false, false);
            m_testTimer.AddEventListener<TimerEvent>(TimerEvent.Timer, OnTimer);
            m_testTimer.AddEventListener<TimerEvent>(TimerEvent.TimerComplete, OnTimerComplete);
            m_testTimer.Start();
            Invoke("DisableTimerManager", 1f);
        }

        private void OnDestroy()
        {
            if (m_testTimer != null)
            {
                m_testTimer.RemoveEventListener<TimerEvent>(TimerEvent.Timer, OnTimer);
                m_testTimer.RemoveEventListener<TimerEvent>(TimerEvent.TimerComplete, OnTimerComplete);
                m_testTimer.Dispose();
                m_testTimer = null;
            }
        }

        private void OnTimer(TimerEvent timerEvent)
        {
            ITimer timer = timerEvent.timer;
            Debug.Log(timer.currentCount);
        }

        private void OnTimerComplete(TimerEvent timerEvent)
        {
            IntegrationTest.Pass(gameObject);
        }

        private void DisableTimerManager()
        {
            TimerManager.instance.enabled = false;
            Invoke("EnableTimerManager", 2f);
        }

        private void EnableTimerManager()
        {
            TimerManager.instance.enabled = true;
        }
    }
}