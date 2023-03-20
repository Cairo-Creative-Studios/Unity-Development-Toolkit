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

        public SerializableDictionary<StandardComponent, ComponentDataBase> Components =
            new SerializableDictionary<StandardComponent, ComponentDataBase>();

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
            foreach (var component in Components.Keys)
            {
                component.Object = this;
            }
        }

        private void Awake()
        {
            _standardEvents.onAwake?.Invoke();

            foreach (var component in Components.Keys)
            {
                component.Object = this;
            }
        }

        private void Start()
        {
            _standardEvents.onStart?.Invoke();
        }

        private void Update()
        {
            _standardEvents.onUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            _standardEvents.onLateUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            _standardEvents.onFixedUpdate?.Invoke();
        }

        private void OnEnable()
        {
            _standardEvents.onOnEnable?.Invoke();
        }

        private void OnDisable()
        {
            _standardEvents.onOnDisable?.Invoke();
        }

        private void OnApplicationQuit()
        {
            _applicationEvents.onOnApplicationQuit?.Invoke();
        }

        private void OnApplicationFocus(bool focus)
        {
            _applicationEvents.onOnApplicationFocus?.Invoke(focus);
        }

        private void OnCollisionEnter(Collision collision)
        {
            _collisionEvents.onOnCollisionEnter?.Invoke(collision);
        }
        private void OnCollisionStay(Collision collision)
        {
            _collisionEvents.onOnCollisionStay?.Invoke(collision);
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
                foreach (var c in Controllables)
                {
                    c.OnInputAction(context);
                }
            }
     
            public void OnInputAction(SerializedInput input)
            {
                foreach (var c in Controllables)
                {
                    c.OnInputAction(input);
                }
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
}