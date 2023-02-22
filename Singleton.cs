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
    /// A Singleton is a MonoBehaviour that, when extended, will only ever have one instance in the scene.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : SingletonBase where T : SingletonBase
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                bool init = false;
                
                if (_instance == null)
                {
                    init = true;
                    
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        _instance = go.AddComponent<T>();
                    }
                    CoreModule.AddSingleton(_instance);
                    
                }

                if (!_instance._nameSet)
                {
                    _instance.gameObject.name =
                        System.Text.RegularExpressions.Regex.Replace(typeof(T).Name, "[A-Z]", " $0");
                    _instance._nameSet = true;
                }                
                
                if(init)
                    _instance.Init();
                return _instance;
            }
            protected set
            {
                _instance = value;
                Instance = _instance;
            }
        }
        
        public static T GetInstance()
        {
            return Instance;
        }
    }
}