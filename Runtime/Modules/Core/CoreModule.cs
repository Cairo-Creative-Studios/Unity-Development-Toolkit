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
        public List<RuntimeSingleton> runtimes = new List<RuntimeSingleton>();
        public List<SingletonBase> singletons = new List<SingletonBase>();

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeLoad()
        {
            SceneManager.CreateScene("UDT");
            Instance.enabled = true;
            SceneManager.MoveGameObjectToScene(Instance.gameObject, SceneManager.GetSceneByName("UDT"));

            //Create the Runtime Types list and add all the Runtime Types to it that exist in the current project
            List<Type> runtimeTypes = new List<Type>();
            runtimeTypes.AddRange(Type.GetType("UDT.Core.Runtime`1").GetInheritedTypes()); 
            runtimeTypes.AddRange(Type.GetType("UDT.Core.Runtime`2").GetInheritedTypes());

            foreach (var type in runtimeTypes)
            {
                if (type.ContainsGenericParameters || type.Name == "Runtime`1")
                {
                    Debug.Log("Skipped "+ type);
                }
                else
                {
                    // Create the Runtime
                    var runtime = (IRuntime)Activator.CreateInstance(type);
                    runtime.RuntimeStarted();
                    runtime._genericInstance = runtime;
                    
                    // Create the Runtime Singleton Game Object
                    var runtimeSingleton = new GameObject("Runtime " + type.Name).AddComponent<RuntimeSingleton>();
                    runtimeSingleton.runtime = (RuntimeBase)runtime;
                    Instance.runtimes.Add(runtimeSingleton);
                }
            }
        }
        
        public static void AddSingleton(SingletonBase singleton)
        {
            if (!Instance.singletons.Contains(singleton))
                Instance.singletons.Add(singleton);
        }

        void Start()
        {
            foreach (var runtimeSingleton in runtimes)
            {
                runtimeSingleton.runtime.Start();
            }
        }
        
        void Update()
        {
            foreach (var singleton in singletons)
            {
                if (singleton.gameObject.scene.name != "UDT")
                    SceneManager.MoveGameObjectToScene(singleton.gameObject, SceneManager.GetSceneByName("UDT"));
            }

            foreach (var runtimeSingleton in runtimes)
            {
                if(runtimeSingleton.gameObject.scene.name != "UDT")
                    SceneManager.MoveGameObjectToScene(runtimeSingleton.gameObject, SceneManager.GetSceneByName("UDT"));
                
                runtimeSingleton.runtime.Update();
            }
        }
    }
}