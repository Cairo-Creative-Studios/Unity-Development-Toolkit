using System;
using UDT.DataTypes;
using UnityEditor;
using UnityEngine;

namespace UDT.Core
{
    public interface IRuntime
    {
        public void RuntimeStarted();
        public Type type { get; set; }
        public object _genericInstance { get; set; }
        public static object GenericInstance { get; set; }
    }
    
    [Serializable]
    public class RuntimeBase
    {
        public virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            
        }
    }
    
    /// <summary>
    /// A Runtime Object is a Singleton that can be used to manage the state of the game's Runtime.
    /// Extend this class to create custom Runtimes.
    /// </summary>
    /// <typeparam name="T">The inheriting class's Type</typeparam>
    [Serializable]
    public class Runtime<T> : RuntimeBase, IFSM, IRuntime where T : Runtime<T>
    {        
        public Tree<IStateNode> states { get; set; }
        public Tree<IStateNode> stateTree = new Tree<IStateNode>();
        public Transition[] transitions { get; set; }

        public T _instance { get; set; }
        public static T Instance;

        public object _genericInstance
        {
            get
            {
                return GenericInstance;
            }
            set
            {
                GenericInstance = (T)value;
            }
        }
        
        public static object GenericInstance
        {
            get
            {
                return Instance;
            }
            set
            {
                Instance = (T)value;
            }
        }

        public void InitMachine()
        {
            stateTree = states;
        }

        /// <summary>
        /// Set the State of the Runtime
        /// </summary>
        /// <param name="path">The Path to the desired State</param>
        public void _SetState(string path)
        {
            StateMachineModule.SetState(this, path);
        }
        
        
        public void _Transition<TPreviousState, TNextState>()
        {
            ((IFSM)this).Transition<TPreviousState, TNextState>();
        }
        
        public static void Transition<TPreviousState, TNextState>()
        {
            Instance._Transition<TPreviousState, TNextState>();
        }
        
        public static void SetState(string path)
        {
            Instance._SetState(path);
        }

        void IRuntime.RuntimeStarted()
        {
            StateMachineModule.AddStateMachine(this);
            stateTree = states;
        }

        public Type type { get; set; }
    }
}