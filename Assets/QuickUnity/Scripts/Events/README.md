# **Event System**

If you were a Flash developer, you will be familiar with the Event System of ActionScript 3.0(AS3). This Event System API for Unity engine is just the same as AS3.

## **EventDispatcher**

If you want to use class **EventDispatcher** to dispatch event or add listener for specific event in normal C# class. You need to create a custom **Event** class which is inherited from class **Event** under namespace **QuickUnity.Events** first. And also you need to define a event type. For example:

```c#
using QuickUnity.Events;

public class TestEvent : Event
{
    // Test event type.
    public const string Test = "Test";
  
  	public TestEvent(string eventType, object context = null)
      :base(eventType, context)
    {
    }
}
```

When you finished all things previous step, you can add a listener for this event type. For example:

```c#
IEventDispatcher dispatcher = new EventDispatcher();
dispatcher.AddEventListener<TestEvent>(TestEvent.Test, (testEvent) => 
{
  // Your logic codes here.
});
```

Finnally, you can dispatch this event like this:

```c#
dispatcher.DispatchEvent<TestEvent>(new TestEvent(TestEvent.Test));
```

Then you can write your logic codes and see what gonna happen.



## **BehaviourEventDispatcher**

As you knew, the basic class of Unity is **MonoBehaviour**. So if you want to dispatch event or add event listener in class **MonoBehaviour**, you need to use class **BehaviourEventDispatcher**. Because the class **BehaviourEventDispatcher** is inherited from class **MonoBehaviour**.

All you need to do is just like this:

```c#
using QuickUnity.Events;

public class BehaviourEventDispatcherTest : BehaviourEventDispatcher
{
    private void Start()
    {
      Invoke("DelayDispatch", 0.5f);
    }
  
  	private void DelayDispatch() 
    {
      DispatchEvent(new TestEvent(TestEvent.Test));
    }
}
```

Yes, just create a class which is inherited from class **BehaviourEventDispatcher** and add this class to a GameObject in scene. Then you can dispatch event by invoking function **DispatchEvent**. After that, you can add listener any where. For exmaple:

```c#
BehaviourEventDispatcherTest testObj = GameObject.FindObjectOfType<BehaviourEventDispatchTest>();
testObj.AddEventListener<TestEvent>(TestEvent.Test, (testEvent) => 
{
  // Your logic codes here.
});
```

Ok, all done.



## **ThreadEventDispatcher**

Unity didn't allow you to use any Unity API in sub-threads. Is that means multi-threads in Unity doesn't make any sense? The answer is no. Although you can access any Unity API in sub-threads, but a hack way is that you can make main thread fetch data from sub-threads. So class **ThreadEventDispatcher** can help you to pass data from sub-threads to main thread. This is an example for showing how to use class **ThreadEventDispatcher** to build communication between main thread and sub-threads.

Class **ThreadTextReader**

```c#
using QuickUnity.Events;
using System;
using System.IO;
using UnityEngine;

public class ThreadTextReader : ThreadEventDispatcher
{
  private static readonly string k_imagePath = Application.streamingAssetsPath + "/text.txt";
  
  private FileStream m_fileStream;
  
  public void ReadFile()
  {
    byte[] buffer = new byte[204800];
    m_fileStream = new FileStream(k_imagePath, FileMode.Open, FileAccess.Read);
    m_fileStream.BeginRead(buffer, 0, buffer.Length, new System.AsyncCallback(AsyncReadCallback), this);
  }
  
  private void AsyncReadCallback(IAsyncResult asyncResult)
  {
    if (m_fileStream != null)
    {
      m_fileStream.EndRead(asyncResult);
      m_fileStream.Close();
      m_fileStream = null;
      DispatchEvent(new TestEvent(TestEvent.Complete));
    }
  }
}
```

Class **ThreadEventDispatcherTest**

```c#
using UnityEngine;

public class ThreadEventDispatcherTest : MonoBehaviour
{
  private ThreadTextReader m_threadImageReader;
  
  private void Awake()
  {
    m_threadImageReader = new ThreadTextReader();
    m_threadImageReader.AddEventListener(TestEvent.Complete, OnThreadImageReaderComplete);
  }
  
  private void Start()
  {
    if (m_threadImageReader != null)
      m_threadImageReader.ReadFile();
  }
  
  private void Update()
  {
    // This is the most important step. If you can not receive any event in listener, check this step. Maybe becuase you didn't invoke Update function of ThreadEventDispatcher.
    if (m_threadImageReader != null)
      m_threadImageReader.Update();
  }
  
  private void OnThreadImageReaderComplete(Events.Event eventObject)
  {
    // Your logic codes here.
  }
}
```