using System;
using System.Collections.Generic;
using System.Linq;
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

            //Create the Runtime Types list and add all the Runtime Types to it that exist in the current project
            List<Type> runtimeTypes = new();
            runtimeTypes.AddRange(Type.GetType("UDT.Core.Runtime`1").GetInheritedTypes()); 
            runtimeTypes.AddRange(Type.GetType("UDT.Core.Runtime`2").GetInheritedTypes());

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