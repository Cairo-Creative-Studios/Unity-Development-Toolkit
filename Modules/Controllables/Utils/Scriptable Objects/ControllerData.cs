using UnityEngine;
using UnityEngine.InputSystem;

namespace UDT.Core.Controllables
{
    [CreateAssetMenu(fileName = "Controller Data", menuName = "UDT/Controllables/Controller Data", order = 0)]
    public class ControllerData : ScriptableObject
    {
        public enum ControllerType
        {
            Player,
            AI
        }
        [Tooltip("The type of controller this is. Player or AI")]
        public ControllerType controllerType;
        [Header(" - Properties - ")]
        [Tooltip("Whether the cursor displays while by default using this controller")]
        public bool displayCursor = false;
        [Tooltip("The input map to use for this controller")]
        public InputActionAsset inputActionMap;
    }
    
    
}