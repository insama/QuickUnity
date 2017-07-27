using QuickUnity.Timers;
using System;
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
        private Timer testTimer;

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time. 
        /// </summary>
        private void Start()
        {
            testTimer = new Timer(1.0f, 3, true, false, false);
            testTimer.TimerTicking += OnTimerTicking;
            testTimer.TimerCompleted += OnTimerCompleted;
            testTimer.Start();
            Invoke("DisableTimerManager", 1f);
        }

        private void OnDestroy()
        {
            if (testTimer != null)
            {
                testTimer.TimerTicking -= OnTimerTicking;
                testTimer.TimerCompleted -= OnTimerCompleted;
                testTimer.Dispose();
                testTimer = null;
            }
        }

        private void OnTimerTicking(object sender, EventArgs e)
        {
            ITimer timer = (ITimer)sender;
            Debug.Log(timer.CurrentCount);
        }

        private void OnTimerCompleted(object sender, EventArgs e)
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