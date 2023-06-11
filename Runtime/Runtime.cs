using UDT.Data;
using UnityEditor;
using UnityEngine;

namespace UDT.Core
{
    /// <summary>
    /// A Runtime Object is a Singleton that can be used to manage the state of the game's Runtime.
    /// Extend this class to create custom Runtimes.
    /// </summary>
    /// <typeparam name="T">The inheriting class's Type</typeparam>
    public class Runtime<T> : Singleton<T>, IFSM where T : Runtime<T>
    {        
        public Tree<IStateNode> states { get; set; }
        public Tree<IStateNode> stateTree = new Tree<IStateNode>();
        public Transition[] transitions { get; set; }

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

        
        /// <summary>
        /// This Static Method spawns any Runtime Classes that are exist in Scripts as soon as the Runtime starts.
        /// That said, you should limit yourself to only one Runtime. If you would like to create seperate management
        /// scripts, use Systems instead. 
        /// </summary>
        void Awake()
        {
            StateMachineModule.AddStateMachine(this);
            stateTree = states;
            RuntimeStarted();
        }

        protected virtual void RuntimeStarted()
        {
            
        }
        
        public static void SetState(string path)
        {
            StateMachineModule.SetState(Instance, path);
        }
    }


    /// <summary>
    /// A Runtime Object is a Singleton that can be used to manage the state of the game's Runtime.
    /// Extend this class to create custom Runtimes.
    /// </summary>
    /// <typeparam name="T">The inheriting class's Type</typeparam>
    /// <typeparam name="TData">The Data for the Runtime</typeparam>
    public class Runtime<T, TData> : Runtime<T> where TData : RuntimeData where T : Runtime<T>
    {
        public static TData Data; //= Resources.LoadAll<TData>("")[0];
        
        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void InitializeData()
        {
            if(Data == null)
                Data = Resources.LoadAll<TData>("")[0];
            if (Data == null)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TData>(),"Assets/UDT/Core/Resources/" + typeof(TData).Name + ".asset");
                AssetDatabase.SaveAssets();
                Data = Resources.LoadAll<TData>("")[0];
            }
        }
        #endif
        
        protected override void RuntimeStarted()
        {
            Data = Resources.LoadAll<TData>("")[0];
        }
    }
}