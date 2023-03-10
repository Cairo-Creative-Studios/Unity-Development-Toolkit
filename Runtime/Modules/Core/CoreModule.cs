using System;
using System.Collections.Generic;
using System.Reflection;
using UDT.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UDT.Core
{
    public class CoreModule : Singleton<CoreModule>
    {
        public List<MonoBehaviour> runtimes = new List<MonoBehaviour>();
        public List<SingletonBase> singletons = new List<SingletonBase>();

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeLoad()
        {
            SceneManager.CreateScene("UDT");
            Instance.enabled = true;
            SceneManager.MoveGameObjectToScene(Instance.gameObject, SceneManager.GetSceneByName("UDT"));
            
            Type[] runtimeTypes = Type.GetType("UDT.Core.Runtime`1").GetInheritedTypes();
            foreach (var type in runtimeTypes)
            {
                if(type.ContainsGenericParameters)
                    continue;

                object runtimeInstance = type.GetProperty("Instance", BindingFlags.Static |  BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).GetValue(null);
                runtimeInstance.CallMethod("StartRuntime");
                Instance.runtimes.Add(runtimeInstance as MonoBehaviour);
            }
        }
        
        public static void AddSingleton(SingletonBase singleton)
        {
            if (!Instance.singletons.Contains(singleton))
                Instance.singletons.Add(singleton);
        }
        
        void Update()
        {
            foreach (var singleton in singletons)
            {
                if (singleton.gameObject.scene.name != "UDT")
                    SceneManager.MoveGameObjectToScene(singleton.gameObject, SceneManager.GetSceneByName("UDT"));
            }
        }
    }
}