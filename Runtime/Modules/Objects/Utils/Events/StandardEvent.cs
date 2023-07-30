using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

namespace UDT.Core
{
    /// <summary>
    /// A Standard Event is a Serialized form of an Event. 
    /// </summary>
    [Serializable]
    public class StandardEventBase
    {
        [HideInInspector]
        public string Name;
        public object[] Args;
        public bool triggered;
        public int tick;
        public Action baseAction;

        public StandardEventBase(string name = "")
        {
            this.Name = name;
        }

        /// <summary>
        /// Sets the Arguments of the Event and Invokes the Base Parameterless Base Action.
        /// </summary>
        /// <param name="arguments"></param>
        public async Task InvokeBaseAction(params object[] arguments)
        {
            Args = arguments;
            baseAction?.Invoke();

            triggered = true;
            await Task.Yield();
            triggered = false;
        }

        public void AddListenerToBase(Action action)
        {
            baseAction += action;
        }

        public void RemoveListenerFromBase(Action action)
        {
            baseAction -= action;
        }
    }

    /// <summary>
    /// A Standard Event is a stored form of a UnityEvent, that allows the event's trigger to be checked from a condition.
    /// It can be used to check events from conditions, and as a UnityEvent.
    /// </summary>
    [Serializable]
    public class StandardEvent : StandardEventBase
    {
        public UnityEvent UnityEvent;
        public Action Action;
        
        /// <summary>
        /// Access the Unity Event, to add/remove listeners, or trigger the event.
        /// </summary>
        /// <param name="args"></param>
        public UnityEvent this[object[] args]
        {
            set
            {
                this.UnityEvent = value;
            }
        }
        
        /// <summary>
        /// This allows the event to be triggered from a condition.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="value"></param>
        public bool this[bool value]
        {
            get
            {
                return this.triggered;
            }
        }
        
        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="args"></param>
        public async Task Invoke()
        {
            this.tick = Time.frameCount;
            this.UnityEvent?.Invoke();
            this.Action?.Invoke();
            InvokeBaseAction?.Invoke();

            triggered = true;
            await Task.Yield();
            triggered = false;
        }
        
        public StandardEvent(string name = "") : base(name)
        {
        }

        /// <summary>
        /// AddMethod a method to the event.
        /// </summary>
        /// <param name="method"></param>
        public void AddListener(Action method)
        {
            Action += method;
        }
        
        /// <summary>
        /// RemoveMethod a method from the event.
        /// </summary>
        /// <param name="method"></param>
        public void RemoveListener(Action method)
        {
            Action -= method;
        }
        
        /// <summary>
        /// Clear all subscribers.
        /// </summary>
        public void ClearSubscribers()
        {
            Action = null;
        }
    }

    /// <summary>
    /// A Standard Event is a stored form of a UnityEvent, that allows the event's trigger to be checked from a condition.
    /// It can be used to check events from conditions, and as a UnityEvent.
    /// </summary>
    [Serializable]
    public class StandardEvent<T> : StandardEventBase
    {
        public UnityEvent<T> UnityEvent;
        public Action<T> Action;
        
        /// <summary>
        /// Access the Unity Event, to add/remove listeners, or trigger the event.
        /// </summary>
        /// <param name="args"></param>
        public UnityEvent<T> this[object[] args]
        {
            set
            {
                this.UnityEvent = value;
            }
        }
        
        /// <summary>
        /// This allows the event to be triggered from a condition.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="value"></param>
        public bool this[bool value]
        {
            get
            {
                return this.triggered;
            }
        }
        
        /// <summary>
        /// Invoke the event.
        /// </summary>
        public async Task Invoke(T arg)
        {
            this.tick = Time.frameCount;
            this.UnityEvent?.Invoke(arg);
            this.Action?.Invoke(arg);
            this.Args = new object[] { arg };
            InvokeBaseAction?.Invoke();

            triggered = true;
            await Task.Yield();
            triggered = false;
        }

        public StandardEvent(string name = "") : base(name)
        {
            this.Args = new object[] { default(T) };
        }

        /// <summary>
        /// AddMethod a method to the event.
        /// </summary>
        /// <param name="method"></param>
        public void AddListener(Action<T> method)
        {
            Action += method;
        }
        
        /// <summary>
        /// RemoveMethod a method from the event.
        /// </summary>
        /// <param name="method"></param>
        public void RemoveListener(Action<T> method)
        {
            Action -= method;
        }
        
        /// <summary>
        /// Clear all subscribers.
        /// </summary>
        public void ClearSubscribers()
        {
            Action = null;
        }
    }
    
    /// <summary>
    /// A Standard Event is a stored form of a UnityEvent, that allows the event's trigger to be checked from a condition.
    /// It can be used to check events from conditions, and as a UnityEvent.
    /// </summary>
    [Serializable]
    public class StandardEvent<T1, T2> : StandardEventBase
    {
        public UnityEvent<T1, T2> UnityEvent;
        public Action<T1, T2> Action;
        
        /// <summary>
        /// Access the Unity Event, to add/remove listeners, or trigger the event.
        /// </summary>
        /// <param name="args"></param>
        public UnityEvent<T1, T2> this[object[] args]
        {
            set
            {
                this.UnityEvent = value;
            }
        }
        
        /// <summary>
        /// This allows the event to be triggered from a condition.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="value"></param>
        public bool this[bool value]
        {
            get
            {
                return this.triggered;
            }
        }
        
        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="args"></param>
        public async Task Invoke(T1 arg1, T2 arg2)
        {
            this.tick = Time.frameCount;
            this.UnityEvent?.Invoke(arg1, arg2);
            this.Action?.Invoke(arg1, arg2);
            
            this.Args = new object[] { arg1, arg2 };
            InvokeBaseAction?.Invoke();

            triggered = true;
            await Task.Yield();
            triggered = false;
        }

        public StandardEvent(string name = "") : base(name)
        {
            this.Args = new object[] { default(T1), default(T2) };
        }
        
        /// <summary>
        /// AddMethod a method to the event.
        /// </summary>
        /// <param name="method"></param>
        public void AddListener(Action<T1, T2> method)
        {
            Action += method;
        }
        
        /// <summary>
        /// RemoveMethod a method from the event.
        /// </summary>
        /// <param name="method"></param>
        public void RemoveListener(Action<T1, T2> method)
        {
            Action -= method;
        }
        
        /// <summary>
        /// Clear all subscribers.
        /// </summary>
        public void ClearSubscribers()
        {
            Action = null;
        }
    }
    
    
    /// <summary>
    /// A Standard Event is a stored form of a UnityEvent, that allows the event's trigger to be checked from a condition.
    /// It can be used to check events from conditions, and as a UnityEvent.
    /// </summary>
    [Serializable]
    public class StandardEvent<T1, T2, T3> : StandardEventBase
    {
        public UnityEvent<T1, T2, T3> UnityEvent;
        public Action<T1, T2, T3> Action;
        
        /// <summary>
        /// Access the Unity Event, to add/remove listeners, or trigger the event.
        /// </summary>
        /// <param name="args"></param>
        public UnityEvent<T1, T2, T3> this[object[] args]
        {
            set
            {
                this.UnityEvent = value;
            }
        }
        
        /// <summary>
        /// This allows the event to be triggered from a condition.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="value"></param>
        public bool this[bool value]
        {
            get
            {
                return this.triggered;
            }
        }
        
        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="args"></param>
        public async Task Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            this.tick = Time.frameCount;
            this.UnityEvent?.Invoke(arg1, arg2, arg3);
            this.Action?.Invoke(arg1, arg2, arg3);

            this.Args = new object[] { arg1, arg2, arg3 };
            InvokeBaseAction?.Invoke();

            triggered = true;
            await Task.Yield();
            triggered = false;
        }

        public StandardEvent(string name = "") : base(name)
        {
            this.Args = new object[] { default(T1), default(T2) };
        }
        
        /// <summary>
        /// AddMethod a method to the event.
        /// </summary>
        /// <param name="method"></param>
        public void AddListener(Action<T1, T2, T3> method)
        {
            Action += method;
        }
        
        /// <summary>
        /// RemoveMethod a method from the event.
        /// </summary>
        /// <param name="method"></param>
        public void RemoveListener(Action<T1, T2, T3> method)
        {
            Action -= method;
        }
        
        /// <summary>
        /// Clear all subscribers.
        /// </summary>
        public void ClearSubscribers()
        {
            Action = null;
        }
    }

    /// <summary>
    /// A Standard Event is a stored form of a UnityEvent, that allows the event's trigger to be checked from a condition.
    /// It can be used to check events from conditions, and as a UnityEvent.
    /// </summary>
    [Serializable]
    public class StandardEvent<T1, T2, T3, T4> : StandardEventBase
    {
        public UnityEvent<T1, T2, T3, T4> UnityEvent;
        public Action<T1, T2, T3, T4> Action;

        /// <summary>
        /// Access the Unity Event, to add/remove listeners, or trigger the event.
        /// </summary>
        /// <param name="args"></param>
        public UnityEvent<T1, T2, T3, T4> this[object[] args]
        {
            set { this.UnityEvent = value; }
        }

        /// <summary>
        /// This allows the event to be triggered from a condition.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="value"></param>
        public bool this[bool value]
        {
            get { return this.triggered; }
        }

        /// <summary>
        /// Invoke the event.
        /// </summary>
        /// <param name="args"></param>
        public async Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            this.tick = Time.frameCount;
            this.UnityEvent?.Invoke(arg1, arg2, arg3, arg4);
            this.Action?.Invoke(arg1, arg2, arg3, arg4);

            this.Args = new object[] { arg1, arg2, arg3, arg4 };
            InvokeBaseAction?.Invoke();

            triggered = true;
            await Task.Yield();
            triggered = false;
        }

        public StandardEvent(string name = "") : base(name)
        {
            this.Args = new object[] { default(T1), default(T2) };
        }

        /// <summary>
        /// AddMethod a method to the event.
        /// </summary>
        /// <param name="method"></param>
        public void AddListener(Action<T1, T2, T3, T4> method)
        {
            Action += method;
        }

        /// <summary>
        /// RemoveMethod a method from the event.
        /// </summary>
        public void RemoveListener(Action<T1, T2, T3, T4> method)
        {
            Action -= method;
        }
        
        /// <summary>
        /// Clear all subscribers.
        /// </summary>
        public void ClearSubscribers()
        {
            Action = null;
        }
    }
}