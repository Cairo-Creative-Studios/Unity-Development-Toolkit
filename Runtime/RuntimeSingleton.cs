using UnityEngine;

namespace UDT.Core
{
    public class RuntimeSingleton : MonoBehaviour
    {
        [Tooltip("The actual Runtime Instance")]
        public RuntimeBase runtime;
    }

}