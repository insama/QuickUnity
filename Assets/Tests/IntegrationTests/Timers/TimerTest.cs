using QuickUnity.Timers;
using System;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Integration test of scaled Timer. 
    /// </summary>
    /// <seealso cref="MonoBehaviour"/>
    [IntegrationTest.DynamicTest("TimerTests")]
    [IntegrationTest.SucceedWithAssertions]
    public class TimerTest : MonoBehaviour
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
            testTimer = new Timer(1.0f, 3, true, true, false);
            testTimer.TimerTicking += OnTimerTicking;
            testTimer.TimerCompleted += OnTimerCompleted;
            testTimer.Start();
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
    }
}