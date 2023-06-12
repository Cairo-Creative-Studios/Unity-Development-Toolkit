using System;
using System.Collections.Generic;
using UDT.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace UDT.Core
{
    public class CoreModule : Singleton<CoreModule>
    {
        public List<RuntimeSingleton> runtimes = new List<RuntimeSingleton>();
        public List<SingletonBase> singletons = new List<SingletonBase>();
        public List<Data> staticData = new List<Data>();

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeLoad()
        {
            // Create the Core Module and UDT Scene
            SceneManager.CreateScene("UDT");
            Instance.enabled = true;
            SceneManager.MoveGameObjectToScene(Instance.gameObject, SceneManager.GetSceneByName("UDT"));
            
            // Create the Runtime Types list and add all the Runtime Types to it that exist in the current project
            Type[] runtimeTypes = Type.GetType("UDT.Core.Runtime`1").GetInheritedTypes(); 

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
                    var runtimeSingleton = new GameObject("Runtime " + System.Text.RegularExpressions.Regex.Replace(
                        type.Name, "[A-Z]", " $0")).AddComponent<RuntimeSingleton>();
                    
                    runtimeSingleton.runtime = (RuntimeBase)runtime;
                    Instance.runtimes.Add(runtimeSingleton);
                }
            }

            // Generate or load all the Static Data for created classes that implement IStaticData
            List<Type> staticDataTypes = new List<Type>();
            staticDataTypes.AddRange(typeof(Data).GetInheritedTypes());

            foreach (var dataType in staticDataTypes)
            {
                var staticData = Resources.LoadAll(dataType.Name, dataType);
                if (staticData.Length > 0)
                {
                    Instance.staticData.Add((Data)staticData[0]);
                }
                else
                {
                    var createdInstance = ScriptableObject.CreateInstance(dataType);
#if UNITY_EDITOR
                    UnityEditor.AssetDatabase.CreateAsset(createdInstance,
                        "Assets/Resources/" + dataType.Name + ".asset");
                    UnityEditor.AssetDatabase.SaveAssets();
#endif
                    Instance.staticData.Add((Data)createdInstance);
                }
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

            foreach (var runtimeSingleton in runtimes)
            {
                if(runtimeSingleton.gameObject.scene.name != "UDT")
                    SceneManager.MoveGameObjectToScene(runtimeSingleton.gameObject, SceneManager.GetSceneByName("UDT"));
            }
        }
        
        public static Data GetStaticData(Type type)
        {
            foreach (var data in Instance.staticData)
            {
                if (data.GetType() == type)
                    return data;
            }

            return null;
        }
    }
}