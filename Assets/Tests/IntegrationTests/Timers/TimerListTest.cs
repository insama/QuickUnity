using QuickUnity.Timers;
using System;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Integration test of TimerList. 
    /// </summary>
    /// <seealso cref="MonoBehaviour"/>
    [IntegrationTest.DynamicTest("TimerTests")]
    [IntegrationTest.SucceedWithAssertions]
    public class TimerListTest : MonoBehaviour
    {
        private Timer skillACDTimer;

        private Timer skillBCDTimer;

        private TimerList skillCDTimerList;

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time. 
        /// </summary>
        private void Start()
        {
            skillACDTimer = new Timer(1, 3, true, true, false);
            skillACDTimer.TimerTicking += OnSkillACDTimerTicking;
            skillBCDTimer = new Timer(1, 4, true, true, false);
            skillBCDTimer.TimerTicking += OnSkillBCDTimerTicking;
            skillCDTimerList = new TimerList(skillACDTimer, skillBCDTimer);
            skillCDTimerList.TimersReseted += OnSkillTimersReseted;
            Invoke("ResetAllSkills", 2.5f);
        }

        /// <summary>
        /// Called when [destroy]. 
        /// </summary>
        private void OnDestroy()
        {
            if (skillACDTimer != null)
            {
                skillACDTimer.TimerTicking -= OnSkillACDTimerTicking;
                skillACDTimer = null;
            }

            if (skillBCDTimer != null)
            {
                skillBCDTimer.TimerTicking -= OnSkillBCDTimerTicking;
                skillBCDTimer = null;
            }

            if (skillCDTimerList != null)
            {
                skillCDTimerList.TimersReseted -= OnSkillTimersReseted;
                skillCDTimerList = null;
            }
        }

        private void ResetAllSkills()
        {
            skillCDTimerList.ResetAll();
        }

        private void OnSkillACDTimerTicking(object sender, EventArgs e)
        {
            ITimer timer = (ITimer)sender;
            Debug.LogFormat("Skill A is cooling down: {0}!", timer.CurrentCount);
        }

        private void OnSkillBCDTimerTicking(object sender, EventArgs e)
        {
            ITimer timer = (ITimer)sender;
            Debug.LogFormat("Skill B is cooling down: {0}!", timer.CurrentCount);
        }

        private void OnSkillTimersReseted(object sender, EventArgs e)
        {
            Debug.Log("All skills are ready!");
            IntegrationTest.Pass(gameObject);
        }
    }
}