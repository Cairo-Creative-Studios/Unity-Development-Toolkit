using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.SEIDInfo;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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
        public StandardObjectData data;   
        public GameObject prefab;
        [HideInInspector] public bool instanced;
        public int UID;
        public int IID;
        public List<string> tags = new List<string>();

        public List<StandardComponent> Components =
            new List<StandardComponent>();
        
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

        private void Awake()
        {
            _standardEvents.onAwake?.Invoke();
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
            foreach(var component in Components)
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
            Components.Clear();
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
        public IComponentBase AddIComponent<T>() where T : StandardComponent, IComponentBase
        {
            var component = gameObject.AddComponent<T>();
            component.Object = this;
            Components.Add(component);
            if (instanced)
                component.OnInstantiate();
            return component;
        }

        /// <summary>
        /// Removes the first component of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveIComponent<T>() where T : MonoBehaviour, IComponentBase
        {
            var component = GetIComponent<T>();
            component.OnFree();
            Destroy(gameObject.GetComponent<T>());
            foreach (var c in Components)
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
        public IComponentBase GetIComponent<T>() where T : MonoBehaviour, IComponentBase
        {
            return Components.FirstOrDefault(c => c.GetType() == typeof(T));
        }

        public bool HasComponent<T>(T component = default)
        {
            return Components.Any(c => c.GetType() ==  typeof(T));
        }
    }
}