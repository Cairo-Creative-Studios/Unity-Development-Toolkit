using System;
using UnityEngine;

namespace UDT.Core
{
    public class RuntimeSingleton : MonoBehaviour
    {
        [Tooltip("The actual Runtime Instance")]
        public RuntimeBase runtime;
        
        void Start()
        {
            if(CoreModule.DataGenerated())
                runtime.Start();
        }
        
        void Update()
        {
            if(CoreModule.DataGenerated())
                runtime.Update();
        }
    }

}