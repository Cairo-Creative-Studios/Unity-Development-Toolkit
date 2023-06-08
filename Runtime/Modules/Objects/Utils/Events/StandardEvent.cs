using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

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

        public StandardEventBase(string name = "")
        {
            this.Name = name;
        }
    }

    /// <summary>
    /// A Standard Event is a stored form of a UnityEvent, that allows the event's trigger to be checked from a condition.
    /// It can be used to check events from conditions, and as a UnityEvent.
    /// </summary>
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
        public void Invoke()
        {
            this.triggered = true;
            this.tick = Time.frameCount;
            this.UnityEvent.Invoke();
            this.Action?.Invoke();
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
        public void Invoke(T arg)
        {
            this.triggered = true;
            this.tick = Time.frameCount;
            this.UnityEvent.Invoke(arg);
            this.Action?.Invoke(arg);
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
        public void Invoke(T1 arg1, T2 arg2)
        {
            this.triggered = true;
            this.tick = Time.frameCount;
            this.Args = new object[] { arg1, arg2 };
            this.UnityEvent.Invoke(arg1, arg2);
            this.Action?.Invoke(arg1, arg2);
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
        public void Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            this.triggered = true;
            this.tick = Time.frameCount;
            this.Args = new object[] { arg1, arg2, arg3 };
            this.UnityEvent.Invoke(arg1, arg2, arg3);
            this.Action?.Invoke(arg1, arg2, arg3);
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
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            this.triggered = true;
            this.tick = Time.frameCount;
            this.Args = new object[] { arg1, arg2, arg3, arg4 };
            this.UnityEvent.Invoke(arg1, arg2, arg3, arg4);
            this.Action?.Invoke(arg1, arg2, arg3, arg4);
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