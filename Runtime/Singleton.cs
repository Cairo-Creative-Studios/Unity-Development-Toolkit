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
    }
}