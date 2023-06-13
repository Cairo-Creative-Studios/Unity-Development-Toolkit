using System;
using UnityEngine;

namespace UDT.Core
{
    public class RuntimeSingleton : MonoBehaviour
    {
        [Tooltip("The actual Runtime Instance")]
        public RuntimeBase runtime;
        public static bool runtimeDataInitialized = false;
        
        void Start()
        {
            if(!runtimeDataInitialized) CoreModule.SetRuntimeData();
            
            runtime._singletonObject = this;
            runtime.Start();
        }
        
        void Update()
        {
                runtime.Update();
        }
    }

}