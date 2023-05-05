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

        public StandardEventBase(string name)
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
        }
        
        public StandardEvent(string name) : base(name)
        {
        }
    }

    /// <summary>
    /// A Standard Event is a stored form of a UnityEvent, that allows the event's trigger to be checked from a condition.
    /// It can be used to check events from conditions, and as a UnityEvent.
    /// </summary>
    public class StandardEvent<T> : StandardEventBase
    {
        public UnityEvent UnityEvent;
        public Func<T> CsEvent;
        
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
        public void Invoke()
        {
            this.triggered = true;
            this.tick = Time.frameCount;
            this.UnityEvent.Invoke();
        }

        public StandardEvent(string name) : base(name)
        {
            this.Args = new object[] { default(T) };
        }

        /// <summary>
        /// AddMethod a method to the event.
        /// </summary>
        /// <param name="method"></param>
        public void AddMethod(Func<T> method)
        {
            CsEvent += method;
        }
        
        /// <summary>
        /// RemoveMethod a method from the event.
        /// </summary>
        /// <param name="method"></param>
        public void RemoveMethod(Func<T> method)
        {
            CsEvent -= method;
        }
    }
    
    /// <summary>
    /// A Standard Event is a stored form of a UnityEvent, that allows the event's trigger to be checked from a condition.
    /// It can be used to check events from conditions, and as a UnityEvent.
    /// </summary>
    public class StandardEvent<T1, T2> : StandardEventBase
    {
        public UnityEvent<T1> UnityEvent;
        public Func<T1, T2> CsEvent;
        
        /// <summary>
        /// Access the Unity Event, to add/remove listeners, or trigger the event.
        /// </summary>
        /// <param name="args"></param>
        public UnityEvent<T1> this[object[] args]
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
        public void Invoke(T1 arg1)
        {
            this.triggered = true;
            this.tick = Time.frameCount;
            this.Args = new object[] { arg1 };
            this.UnityEvent.Invoke(arg1);
            CsEvent?.Invoke(arg1);
        }

        public StandardEvent(string name) : base(name)
        {
            this.Args = new object[] { default(T1), default(T2) };
        }
        
        /// <summary>
        /// AddMethod a method to the event.
        /// </summary>
        /// <param name="method"></param>
        public void AddMethod(Func<T1, T2> method)
        {
            CsEvent += method;
        }
        
        /// <summary>
        /// RemoveMethod a method from the event.
        /// </summary>
        /// <param name="method"></param>
        public void RemoveMethod(Func<T1, T2> method)
        {
            CsEvent -= method;
        }
    }
    
    /// <summary>
    /// A Standard Event is a stored form of a UnityEvent, that allows the event's trigger to be checked from a condition.
    /// It can be used to check events from conditions, and as a UnityEvent.
    /// </summary>
    public class StandardEvent<T1, T2, T3> : StandardEventBase
    {
        public UnityEvent<T1, T2> UnityEvent;
        public Func<T1, T2, T3> CsEvent;
        
        
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
            this.Args = new object[] { arg1, arg2};
            this.UnityEvent.Invoke(arg1, arg2);
            CsEvent?.Invoke(arg1, arg2);
        }

        public StandardEvent(string name) : base(name)
        {
            this.Args = new object[] { default(T1), default(T2), default(T3)};
        }
        
        /// <summary>
        /// AddMethod a method to the event.
        /// </summary>
        /// <param name="method"></param>
        public void AddMethod(Func<T1, T2, T3> method)
        {
            CsEvent += method;
        }
        
        /// <summary>
        /// RemoveMethod a method from the event.
        /// </summary>
        /// <param name="method"></param>
        public void RemoveMethod(Func<T1, T2, T3> method)
        {
            CsEvent -= method;
        }
    }
    
    /// <summary>
    /// A Standard Event is a stored form of a UnityEvent, that allows the event's trigger to be checked from a condition.
    /// It can be used to check events from conditions, and as a UnityEvent.
    /// </summary>
    public class StandardEvent<T1, T2, T3, T4> : StandardEventBase
    {
        public UnityEvent<T1, T2, T3> UnityEvent;
        public Func<T1, T2, T3, T4> CsEvent;
        
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
            this.Args = new object[] { arg1, arg2, arg3,};
            this.UnityEvent.Invoke(arg1, arg2, arg3);
            CsEvent?.Invoke(arg1, arg2, arg3);
        }

        public StandardEvent(string name) : base(name)
        {
            this.Args = new object[] { default(T1), default(T2), default(T3), default(T4) };
        }
        
        /// <summary>
        /// Add a method to the event.
        /// </summary>
        /// <param name="method"></param>
        public void AddMethod(Func<T1, T2, T3, T4> method)
        {
            CsEvent += method;
        }
        
        /// <summary>
        /// Remove a method from the event.
        /// </summary>
        /// <param name="method"></param>
        public void RemoveMethod(Func<T1, T2, T3, T4> method)
        {
            CsEvent -= method;
        }
    }
}