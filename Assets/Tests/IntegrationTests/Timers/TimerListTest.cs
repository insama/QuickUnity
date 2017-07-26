using QuickUnity.Timers;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Integration test of TimerList.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour"/>
    [IntegrationTest.DynamicTest("TimerTests")]
    [IntegrationTest.SucceedWithAssertions]
    public class TimerListTest : MonoBehaviour
    {
        private ITimer skillACDTimer;

        private ITimer skillBCDTimer;

        private ITimerList skillCDTimerList;

        /// <summary>
        /// Start is called just before any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            skillACDTimer = new Timer(1, 3, true, true, false);
            skillACDTimer.AddEventListener<TimerEvent>(TimerEvent.Timer, OnSkillACDTimer);
            skillBCDTimer = new Timer(1, 4, true, true, false);
            skillBCDTimer.AddEventListener<TimerEvent>(TimerEvent.Timer, OnSkillBCDTimer);
            skillCDTimerList = new TimerList(skillACDTimer, skillBCDTimer);
            skillCDTimerList.AddEventListener<TimerListEvent>(TimerListEvent.AllReset, OnSkillTimersReset);
            Invoke("ResetAllSkills", 2.5f);
        }

        /// <summary>
        /// Called when [destroy].
        /// </summary>
        private void OnDestroy()
        {
            if (skillACDTimer != null)
            {
                skillACDTimer.RemoveEventListener<TimerEvent>(TimerEvent.Timer, OnSkillACDTimer);
                skillACDTimer = null;
            }

            if (skillBCDTimer != null)
            {
                skillBCDTimer.RemoveEventListener<TimerEvent>(TimerEvent.Timer, OnSkillBCDTimer);
                skillBCDTimer = null;
            }

            if (skillCDTimerList != null)
            {
                skillCDTimerList.RemoveEventListener<TimerListEvent>(TimerListEvent.AllReset, OnSkillTimersReset);
                skillCDTimerList = null;
            }
        }

        /// <summary>
        /// Resets all skills.
        /// </summary>
        private void ResetAllSkills()
        {
            skillCDTimerList.ResetAll();
        }

        private void OnSkillACDTimer(TimerEvent timerEvent)
        {
            Debug.LogFormat("Skill A is cooling down: {0}!", skillACDTimer.CurrentCount);
        }

        private void OnSkillBCDTimer(TimerEvent timerEvent)
        {
            Debug.LogFormat("Skill B is cooling down: {0}!", skillBCDTimer.CurrentCount);
        }

        /// <summary>
        /// Called when [skill timers reset].
        /// </summary>
        /// <param name="timerListEvent">The <see cref="TimerListEvent"/> object.</param>
        private void OnSkillTimersReset(TimerListEvent timerListEvent)
        {
            Debug.Log("All skills are ready!");
            IntegrationTest.Pass(gameObject);
        }
    }
}