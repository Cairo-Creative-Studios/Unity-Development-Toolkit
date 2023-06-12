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
                runtime.Start();
        }
        
        void Update()
        {
                runtime.Update();
        }
    }

}