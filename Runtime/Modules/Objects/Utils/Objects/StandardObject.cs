using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UDT.Core.Controllables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace UDT.Core
{
    /// <summary>
    /// The base class for all objects in UDT
    /// Standard Objects are pooled and can be accessed by UID/IID, and their components.
    /// Object Pools are created automatically when a Standard Object is created, and they must be managed manually.
    /// </summary>
    [AddComponentMenu("UDT/Standard Object")]
    public class StandardObject : MonoBehaviour
    {
        public ObjectDefinition definition;   
        public GameObject prefab;
        [HideInInspector] public bool instanced;
        public int UID;
        public int IID;
        public List<string> tags = new List<string>();
        
        /// <summary>
        /// 
        /// </summary>
        public SerializableDictionary<StandardComponent, ComponentDataBase> Components =
            new SerializableDictionary<StandardComponent, ComponentDataBase>();

        /// <summary>
        /// The Current State of the Object.
        /// </summary>
        [Dropdown("states")]
        public string State;
        private DropdownList<string> GetStates()
        {
            var list = new DropdownList<string>();
            foreach (var state in states)
            {
                list.Add(state.stateName, state.stateName);
            }
            return list;    
        }
        
        /// <summary>
        /// The Object States, which toggle the Components within them on and off depending on the State of the Object.
        /// </summary>
        public List<ObjectState> states = new List<ObjectState>();
        /// <summary>
        /// The History of the last States that each Component was in before the State was changed.
        /// </summary>
        private Dictionary<ComponentDataBase, bool> previousState; 

        struct StandardEvents
        {
            //Unity Events
            public StandardEvent onAwake;
            public StandardEvent onStart;
            public StandardEvent onUpdate;
            public StandardEvent onFixedUpdate;
            public StandardEvent onLateUpdate;
            public StandardEvent onOnEnable;
            public StandardEvent onOnDisable;
            public StandardEvent onOnDestroy;
        }

        struct CollisionEvents
        {
            //Collision Events
            public StandardEvent<Collision> onOnCollisionEnter;
            public StandardEvent<Collision> onOnCollisionStay;
            public StandardEvent<Collision> onOnCollisionExit;
            public StandardEvent<Collider> onOnTriggerEnter;
            public StandardEvent<Collider> onOnTriggerStay;
            public StandardEvent<Collider> onOnTriggerExit;
        }

        struct ApplicationEvents
        {
            //Application Events
            public StandardEvent onOnApplicationQuit;
            public StandardEvent<bool> onOnApplicationFocus;
            public StandardEvent<bool> onOnApplicationPause;
        }
        
        /// <summary>
        /// A Dictionary of Custom Events, that can be accessed and used by Name.
        /// </summary>
        public SerializableDictionary<string, StandardEvent> CustomEvents = new SerializableDictionary<string, StandardEvent>();

        [SerializeField] private StandardEvents _standardEvents = new StandardEvents();
        [SerializeField] private CollisionEvents _collisionEvents = new CollisionEvents();
        [SerializeField] private ApplicationEvents _applicationEvents = new ApplicationEvents();

        private void Reset()
        {
            if(Application.isPlaying)
            {
                foreach (var component in Components.Keys)
                {
                    component.Object = this;
                }
            }
        }

        private void Awake()
        {
            if(Application.isPlaying)
            {
                _standardEvents.onAwake?.Invoke();

                foreach (var component in Components.Keys)
                {
                    component.Object = this;
                    component.OnInstantiate();
                }
            }
        }

        private void Start()
        {
            if(Application.isPlaying)
                _standardEvents.onStart?.Invoke();
        }

        [ExecuteAlways]
        private void Update()
        {
            if(Application.isPlaying)
                _standardEvents.onUpdate?.Invoke();
            else
            {
                foreach (var component in Components.Keys)
                {
                    component.Object = this;
                }
            }
        }

        private void LateUpdate()
        {
            if(Application.isPlaying)
                _standardEvents.onLateUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            if(Application.isPlaying)
                _standardEvents.onFixedUpdate?.Invoke();
        }

        private void OnEnable()
        {
            if(Application.isPlaying)
                _standardEvents.onOnEnable?.Invoke();
        }

        private void OnDisable()
        {
            if(Application.isPlaying)
                _standardEvents.onOnDisable?.Invoke();
        }

        private void OnApplicationQuit()
        {
            if(Application.isPlaying)
                _applicationEvents.onOnApplicationQuit?.Invoke();
        }

        private void OnApplicationFocus(bool focus)
        {
            if(Application.isPlaying)
                _applicationEvents.onOnApplicationFocus?.Invoke(focus);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(Application.isPlaying)
                _collisionEvents.onOnCollisionEnter?.Invoke(collision);
        }
        private void OnCollisionStay(Collision collision)
        {
            if(Application.isPlaying)
                _collisionEvents.onOnCollisionStay?.Invoke(collision);
        }

        /// <summary>
        /// Set the State of the Object, which will toggle the Components within it on and off.
        /// </summary>
        /// <param name="stateName"></param>
        public void SetState(string stateName)
        {
            foreach(ObjectState state in states)
            {
                if(state.stateName == stateName)
                {
                    foreach(var component in state.enableComponents)
                    {
                        component.enabled = true;
                    }
                    foreach(var component in state.disableComponents)
                    {
                        component.enabled = false;
                    }
                }
            }
        }
        
        /// <summary>
        /// Set the State of a Component
        /// </summary>
        /// <param name="component"></param>
        /// <param name="stateName"></param>
        public void SetComponentState(StandardComponent component, string stateName)
        {
            StateMachineModule.SetState(component, stateName);
        }
        
        /// <summary>
        /// Add a new State to the Object, which will toggle the Components within it on and off.
        /// </summary>
        /// <param name="stateName"></param>
        public void AddState(string stateName)
        {
            states.Add(new ObjectState(stateName));
        }
        
        /// <summary>
        /// Add a Component to a State, which will toggle it on and off when the State is set.
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="component"></param>
        /// <param name="enabled"></param>
        public void AddComponentToState(string stateName, StandardComponent component, bool enabled = true)
        {
            foreach(ObjectState state in states)
            {
                if(state.stateName == stateName)
                {
                    if(enabled)
                        state.enableComponents.Add(component);
                    else
                        state.disableComponents.Add(component);
                }
            }
        }
        
        public void Free()
        {
            foreach(var component in Components.Keys)
            {
                component.OnFree();
            }

            _standardEvents.onOnDestroy?.Invoke();

            ObjectModule.HandleDestroyedObject(this);
        }

        [Button("Instantiate")]
        public StandardObject Instantiate()
        {
            return ObjectModule.Instantiate(prefab.name);
        }

        public void OnCreate()
        {
            if(Application.isPlaying)
                ObjectModule.HandleInstantiatedObject(this);
        }

        /// <summary>
        /// Destroys the object and frees all resources, this should be called instead of Destroy
        /// </summary>
        /// <param name="standardObject"></param>
        public static void Free(StandardObject standardObject)
        {
            standardObject.Free();
        }

        /// <summary>
        /// Instantiates the object
        /// </summary>
        /// <param name="standardObject"></param>
        public static StandardObject Instantiate(StandardObject standardObject)
        {
            return standardObject.Instantiate();
        }

        /// <summary>
        /// Adds the specified Component to the object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public StandardComponent AddComponent<T>(ComponentDataBase data = null) where T : StandardComponent
        {
            return AddComponent(typeof(T), data);
        }

        public StandardComponent AddComponent(Type componentType, ComponentDataBase data = null, string childName = "")
        {
            StandardComponent standardComponent;
            if (childName == "")
                standardComponent = (StandardComponent)gameObject.AddComponent(componentType);
            else
            {
                Transform child;
                try
                {
                    child = transform.GetChildren().First(x => x.name == childName);
                }
                catch
                {
                    child = new GameObject(childName).transform;
                }
                child.transform.SetParent(transform);
                standardComponent = (StandardComponent)child.gameObject.AddComponent(componentType);
            }
            
            if (data != null)
            {
                standardComponent.Data = data;
                Components[standardComponent] = data;
            }
            if (instanced)
                standardComponent.OnInstantiate();
            
            standardComponent.OnInstantiate();

            foreach (var c in Components.Keys)
            {
                c?.OnReady();
            }
            
            return standardComponent;
        }

        /// <summary>
        /// Removes the first component of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveStandardComponent<T>() where T : StandardComponent
        {
            var component = GetStandardComponent<T>();
            component.OnFree();
            Destroy(gameObject.GetComponent<T>());
            foreach (var c in Components.Keys)
            {
                if (c.GetType() == typeof(T))
                {
                    Components.Remove(c);
                    break;
                }
            }
        }

        /// <summary>
        /// Returns a reference to the first component of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public StandardComponent GetStandardComponent(Type T)
        {
            return Components.Keys.FirstOrDefault(c => c.GetType() == T);
        }
        
        /// <summary>
        /// Returns a reference to the first component of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public StandardComponent GetStandardComponent<T>() where T : StandardComponent
        {
            return Components.Keys.FirstOrDefault(c => c.GetType() == typeof(T));
        }

        public bool HasComponent<T>(T component = default)
        {
            return Components.Any(c => c.GetType() ==  typeof(T));
        }
        
        [Serializable]
        public class ControllerValues
        {
            public Byte InputByte;
            public bool IsPossessed;
            public Controller Controller;
            public List<StandardComponent> Controllables;
            
            public ControllerValues(Byte inputByte = default, bool isPossessed = default, Controller controller = null)
            {
                InputByte = inputByte;
                IsPossessed = isPossessed;
                Controller = controller;
                Controllables = new List<StandardComponent>();
            }
            
            public void OnInputAction(InputAction.CallbackContext context)
            {
                var newControllables = Controllables;
                
                foreach (var c in Controllables)
                {
                    if (c != null)
                        c.OnInputAction(context);
                    else
                        newControllables.Remove(c);
                }

                Controllables = newControllables;
            }
     
            public void OnInputAction(SerializedInput input)
            {
                var newControllables = Controllables;
                
                foreach (var c in Controllables)
                {
                    if (c != null)
                        c.OnInputAction(input);
                    else
                        newControllables.Remove(c);
                }

                Controllables = newControllables;
            }

            public bool Possess(Controller controller)
            {
                if(IsPossessed)
                    return false;
            
                Controller = controller;
                IsPossessed = true;
                return true;
            }

            public void UnPossess()
            {
                Controller = null;
                IsPossessed = false;
            }
        }
        /// <summary>
        /// The Controller Values of the Object
        /// </summary>
        [Tooltip("The Controller Values of the Object")]
        [ShowIf("IsControllable")]
        public ControllerValues controllerValues;
        
        public bool IsControllable()
        {
            bool isControllable = false;
            foreach (var component in Components.Keys)
            {
                if (component is IControllable)
                {
                    isControllable = true;
                    
                    if(!controllerValues.Controllables.Contains(component))
                        controllerValues.Controllables.Add(component);
                }
            }

            return isControllable;
        }
    }

    [Serializable]
    public class ObjectState
    {
        public string stateName;
        public List<StandardComponent> enableComponents = new List<StandardComponent>();
        public List<StandardComponent> disableComponents = new List<StandardComponent>();
        
        [HideInInspector]
        public StandardObject standardObject;
        
        public ObjectState(string stateName)
        {
            this.stateName = stateName;
        }
    }
}