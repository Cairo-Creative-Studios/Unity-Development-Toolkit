using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace UDT.Core
{
    public class SingletonBase : MonoBehaviour
    {
        [HideInInspector]
        public bool _nameSet = false;
        public virtual void Init()
        {
            
        }
    }
    
    /// <summary>
    /// A Singleton is a MonoBehaviour that will only ever have one instance in the Game.
    /// The Generic type is the new class that is extending this class.
    /// The Instance property is an instance of the new class, and is created when it is first accessed.
    /// </summary>
    /// <typeparam name="T">The inheriting class's Type</typeparam>
    public class Singleton<T> : SingletonBase where T : SingletonBase
    {
        protected static T _instance;

        public static bool instantiated = false;

        public static T Instance
        {
            get
            {
                if (!instantiated)
                {
                    instantiated = true;
                    
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                    
                    _instance.gameObject.name =
                        System.Text.RegularExpressions.Regex.Replace(typeof(T).Name, "[A-Z]", " $0");
                    _instance._nameSet = true;
                
                    _instance.Init();  
                    
                    CoreModule.AddSingleton(_instance);

                    if (_instance is IData && !(_instance as IData).Initialized)
                    {
                        var interfaces = _instance.GetType().GetInterfaces();
                        
                        foreach (var interfaceType in interfaces)
                        {
                            if (interfaceType == typeof(IData))
                            {
                                interfaceType.GetProperty("Data", BindingFlags.Static)
                                    ?.SetValue(null, CoreModule.GetStaticData(interfaceType));
                            }
                        }
                        
                        (_instance as IData).Initialized = true;
                    }
                }

                Instance = _instance;
                return _instance;
            }
            
            protected set 
            {
                _instance = value;
            }
        }
        
        public static T GetInstance()
        {
            return Instance;
        }
        
        /// <summary>
        /// Wait for a number of seconds, then execute an action.
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="action"></param>
        public static void Wait(float seconds, Action action)
        {
            Instance.StartCoroutine(WaitCoroutine(seconds, action));
        }
        
        private static IEnumerator WaitCoroutine(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }
        
        /// <summary>
        /// Repeat an action until the time has elapsed.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="action"></param>
        public static void Repeat(float time, Action action)
        {
            Instance.StartCoroutine(RepeatCoroutine(time, action));
        }
        
        private static IEnumerator RepeatCoroutine(float time, Action action)
        {
            float startTime = Time.time;
            
            while (Time.time - startTime < time)
            {
                action();
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}