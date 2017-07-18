# **Timer System**

If you were a **Flash** developer or **Unreal** developer, you must knew both them provide timer system. In **Flash**, there is a class named **Timer** to implement timer system. In **Unreal**, there is a class named **FTimerManager** to do that also. But in **Unity**, there is nothing to satisify timer requirement. So here we are, I desgin a timer system for **Unity**.

## **Simple Example**

### Limited Repeat Count

```c#
ITimer timer = new Timer(1.0f, 3, true, true, false);

// Timer event that when timer reach delay time each time.
timer.AddEventListener<TimerEvent>(TimerEvent.Timer, (timerEvent) => 
{
  // Your logic codes.
});

// Timer complete event that when repeat count reach to the end.
timer.AddEventListener<TimerEvent>(TimerEvent.TimerComplete, (timerEvent) => 
{
  // Your logic codes.
});

// Start timer.
timer.Start();
```



### Infinite Repeat Count

```c#
// Just set repeatCount to 0 to implement infinite repeat count.
ITimer timer = new Timer(1.0f, 0, true, true, false);

// Timer event that when timer reach delay time each time.
timer.AddEventListener<TimerEvent>(TimerEvent.Timer, (timerEvent) => 
{
  // Your logic codes.
});

// Timer complete event that when repeat count reach to the end.
timer.AddEventListener<TimerEvent>(TimerEvent.TimerComplete, (timerEvent) => 
{
  // Your logic codes.
});

// Start timer.
timer.Start();
```



## **Scaled Timer**

By default, the timer frequency will not change with **Time.timeScale**. Just because the constructor function of class **Timer** receive the default value of parameter **ignoreTimeScale** is true. If you want to get a scaled timer, just do like this:

```c#
// Set parameter 'ignoreTimeScale' to false to implement scaled timer.
ITimer timer = new Timer(1.0f, 3, false, true, false);

// Start timer.
timer.Start();
```



## **Pause When Timer Disabled**

By default, the timer will stop when it be disabled (**TimerManager.instance.enabled = false**). Because the constructor function of class **Timer** receive the default value of parameter **stopOnDisable** is true. if you want to timer pause when it is disabled, pass false when you create a **Timer** object.

```c#
// Set parameter 'stopOnDisable' to false to implement that.
ITimer timer = new Timer(1.0f, 3, true, false, false);

// Start timer.
timer.Start();
```


## **TimerList**

**TimerList** is a feature that allow you to gather some timers into a list. For example, skill cooldown timers. To use **TimerList** is simple, example usage:

```c#
// Skill A CD Timer.
ITimer skillACDTimer = new Timer(1, 3, true, true, false);

// Skill B CD Timer.
ITimer skillBCDTimer = new Timer(1, 5, true, true, false);

// Initialize skill CDs timer list.
ITimerList skillCDsTimerList = new TimerList(skillACDTimer, skillBCDTimer);

// Start your timers.
skillACDTimer.Start();
skillBCDTimer.Start();
```