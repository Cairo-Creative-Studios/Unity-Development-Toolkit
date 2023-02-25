using System;
using System.Collections.Generic;
using UDT.Data;
using UnityEngine;

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
        public ObjectSelection Objects;
        
        public Tree<IStateNode> states { get; set; }
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
        public void SetState(string path)
        {
            StateMachineModule.SetState(this, path);
        }

        private void Awake()
        {
            stateTree = states;
        }
        
        /// <summary>
        /// Starts the specified system, creating it if it doesn't exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static System<T> StartSystem(ObjectSelection objects = null, Type[] managedComponentTypes = null, string managedObjectType = "")
        {
            var instance = GetInstance();
            SetObjects(objects);
            

            if (managedComponentTypes != null)
            {
                System<T>.managedComponentTypes = managedComponentTypes;
                ObjectModule.OnComponentAdded += instance.OnComponentAdded;
                ObjectModule.OnComponentRemoved += instance.OnComponentRemoved;
            }

            if (managedObjectType != "")
            {
                System<T>.managedObjectType = managedObjectType;
                ObjectModule.OnObjectAdded += instance.OnObjectAdded;
                ObjectModule.OnObjectRemoved += instance.OnObjectRemoved;
            }

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
        /// Adds the specified objects to the System
        /// </summary>
        /// <param name="objects"></param>
        public static void AddObjects(ObjectSelection objects)
        {
            Instance.Objects.Append((List<StandardObject>)objects);
        }
        
        /// <summary>
        /// Removes the specified objects from the System
        /// </summary>
        /// <param name="objects"></param>
        public static void RemoveObjects(ObjectSelection objects)
        {
            Instance.Objects.RemoveAll(objects);
        }
        
        /// <summary>
        /// Adds the specified object to the System
        /// </summary>
        /// <param name="obj"></param>
        public static void AddObject(StandardObject obj)
        {
            Instance.Objects.Add(obj);
        }
        
        /// <summary>
        /// Removes the specified object from the System
        /// </summary>
        /// <param name="obj"></param>
        public static void RemoveObject(StandardObject obj)
        {
            Instance.Objects.Remove(obj);
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
        
        public void OnComponentAdded(StandardComponent component)
        {
            foreach (var type in managedComponentTypes)
            {
                if (component.GetType() == type)
                {
                    AddObject(component.Object);
                }
            }
        }

        public void OnComponentRemoved(StandardComponent component)
        {
            foreach (var type in managedComponentTypes)
            {
                if (component.GetType() == type)
                {
                    RemoveObject(component.Object);
                }
            }
        }

        public void OnObjectAdded(StandardObject instance)
        {
            if (instance.prefab.name == managedObjectType)
            {
                AddObject(instance);
            }
        }
        
        public void OnObjectRemoved(StandardObject instance)
        {
            if (instance.prefab.name == managedObjectType)
            {
                RemoveObject(instance);
            }
        }
        
        /// <summary>
        /// When the System is destroyed, call Stop the System.
        /// </summary>
        ~System(){
            StopSystem();
        }
    }
}