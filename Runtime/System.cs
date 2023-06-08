using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NaughtyAttributes;
using UDT.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace UDT.Core
{
    /// <summary>
    /// A System is a Singleton that is used to manage a specific part of the game, such as a Character or Camera System
    /// Custom Systems can be constructed by extending this class.
    /// Systems are meant to be used as a way to manage the objects/Components they're given,
    /// and can be used in place of or in conjunction with MonoBehaviours.
    /// Pairing IComponents with Systems allows for more control and flexibility over the lifecycle of objects.
    /// </summary>
    /// <typeparam name="T">The inheriting class's Type</typeparam>
    public class System<T> : Singleton<T>, IFSM where T : System<T>
    {
        /// <summary>
        /// The Objects that are currently being managed by this system
        /// </summary>
        public List<StandardObject> Objects = new List<StandardObject>();
        /// <summary>
        /// The Components that are currently being managed by this system
        /// </summary>
        public List<StandardComponent> Components = new List<StandardComponent>();
        
        public Tree<IStateNode> states { get; set; }
        public Transition[] transitions { get; set; }
        public Tree<IStateNode> stateTree = new Tree<IStateNode>();
        
        public T BaseInstance => Instance;

        /// <summary>
        /// The Type of Objects that this System manages.
        /// Setting this will automatically add all objects with the specified type to the System.
        /// </summary>
        public static string managedObjectType = "";
        /// <summary>
        /// The Types of Components that this System manages.
        /// Setting this will automatically add all objects with the specified components to the System.
        /// </summary>
        public static Type[] managedComponentTypes;

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

        private void Awake()
        {
            stateTree = states;
            InitData();
        }

        protected virtual void InitData()
        {
            return;
        }
        
        /// <summary>
        /// Starts the specified system, creating it if it doesn't exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T StartSystem(ObjectSelection objects = null, Type[] managedComponentTypes = null, string managedObjectType = "", [CallerMemberName] string caller = "", [CallerFilePath] string file = "")
        {
            if (instantiated)
            {
                return GetInstance();
            }
            
            Debug.Log($"Starting System {typeof(T).Name} from {caller} in {file}");
            
            var instance = GetInstance();
            if (objects != null) SetObjects(objects);

            ObjectModule.OnComponentAdded += Instance.OnComponentAdded;
            ObjectModule.OnComponentRemoved += Instance.OnComponentRemoved;

            return instance;
        }

        /// <summary>
        /// Stops the specified system, destroying it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void StopSystem()
        {
            if (Instance != null)
            {
                GameObject.Destroy(Instance);
            }
        }
        
        /// <summary>
        /// Sets the Objects that the System is managing
        /// </summary>
        /// <param name="objects"></param>
        public static void SetObjects(ObjectSelection objects)
        {
            Instance.Objects = objects;
        }
        
        /// <summary>
        /// Adds the specified object to the System
        /// </summary>
        /// <param name="obj"></param>
        public static void AddObject(StandardObject obj)
        {
            Instance.Objects.Add(obj);
        }
        
        public static void AddComponent(StandardComponent component)
        {
            Instance.OnComponentAdded(component);
        }
        
        /// <summary>
        /// Removes the specified object from the System
        /// </summary>
        /// <param name="obj"></param>
        public static void RemoveObject(StandardObject obj)
        {
            Instance.Objects.Remove(obj);
        }

        public static StandardObject GetObjectFromPrefab(string name)
        {
            return Instance.Objects.Find(x => x.prefab.name == name);
        }
        
        public static StandardObject GetObjectFromData(string name)
        {
            return Instance.Objects.Find(x => x.definition.name == name);
        }
        
        public static StandardObject GetObjectFromName(string name)
        {
            return Instance.Objects.Find(x => x.name == name);
        }
        
        public static StandardObject[] GetObjectsFromPrefab(string name)
        {
            return Instance.Objects.FindAll(x => x.prefab.name == name).ToArray();
        }
        
        public static StandardObject[] GetObjectsFromData(string name)
        {
            return Instance.Objects.FindAll(x => x.definition.name == name).ToArray();
        }
        
        public static StandardObject[] GetObjectsFromName(string name)
        {
            return Instance.Objects.FindAll(x => x.name == name).ToArray();
        }

        public static StandardComponent GetComponentFromPrefab(string name)
        {
            return Instance.Components.Find(x => x.Object.prefab.name == name);
        }
        
        /// <summary>
        /// Gets all the Components of the specified type that are currently being managed by the System
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public new static StandardComponent[] GetComponents<TComponent>() where TComponent : StandardComponent
        {
            return Instance.Components.FindAll(x => x is TComponent).ToArray();
        }

        /// <summary>
        /// Gets the specified system if it exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static System<SystemType> GetSystem<SystemType>() where SystemType : System<SystemType>
        {
            return (SystemType)(object)Instance;
        }
        
        public virtual void OnComponentAdded(StandardComponent standardComponent, Type type = null)
        {
            if (type == GetType())
            {
                AddObject(standardComponent.Object);
                Components.Add(standardComponent);
            }
        }

        public virtual void OnComponentRemoved(StandardComponent standardComponent, Type type = null)
        {
            if (type == GetType())
            {
                RemoveObject(standardComponent.Object);
                Components.Add(standardComponent);
            }
        }

        public static void SetState(string statePath)
        {
            Instance._SetState(statePath);
        }

        public void _Transition<TPreviousState, TNextState>()
        {
            ((IFSM)this).Transition<TPreviousState, TNextState>();
        }
        
        public static void Transition<TPreviousState, TNextState>()
        {
            Instance._Transition<TPreviousState, TNextState>();
        }
        
        public static TState GetState<TState>() where TState : IStateNode
        {
            var stateList = Instance.stateTree.Nodes();
            for(int i = 0; i < stateList.Count; i++)
            {
                if (stateList[i] is TState)
                {
                    return (TState)(object)stateList[i];
                }
            }
            return default;
        }

        /// <summary>
        /// When the System is destroyed, call Stop the System.
        /// </summary>
        ~System(){
            StopSystem();
        }
    }

    public class System<TSystem, TSystemData> : System<TSystem> where TSystem : System<TSystem, TSystemData>
        where TSystemData : SystemData
    {
        [Expandable]
        public static new TSystemData Data;

        protected override void InitData()
        {
            Data = (TSystemData)SystemData.GetSystemData<TSystemData>();
        }
    }
}