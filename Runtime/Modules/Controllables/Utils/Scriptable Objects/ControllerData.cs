using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        [ShowIf("ShowPlayerProperties")]
        [Tooltip("Whether the cursor displays while by default using this controller")]
        public bool displayCursor = false;
        
        [ShowIf("ShowPlayerProperties")]
        [Tooltip("The input map to use for this controller")]
        public InputActionAsset inputActionMap;
        
        [ShowIf("ShowInputMapName")]
        [Dropdown("GetInputMapNames")]
        [Tooltip("The name of the input map to use for this controller")]
        public string inputMapName;

#if UNITY_EDITOR
        [Button("Create New Input Map")]
        public void CreateNewInputMap()
        {
            InputActionAsset asset = ScriptableObject.CreateInstance<InputActionAsset>();
            asset.name = name + " Input Map";
            AssetDatabase.CreateAsset(asset, "Assets/UDT/Core/Resources/" + asset.name + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            inputActionMap = asset;
        }
#endif

        public bool ShowPlayerProperties()
        {
            return controllerType == ControllerType.Player;
        }
        
        public bool ShowInputMapName()
        {
            return inputActionMap != null;
        }
        
        public DropdownList<string> GetInputMapNames()
        {
            DropdownList<string> names = new DropdownList<string>();
            for (int i = 0; i < inputActionMap.actionMaps.Count; i++)
            {
                names.Add(inputActionMap.actionMaps[i].name, inputActionMap.actionMaps[i].name);
            }
            return names;
        }
    }
    
    
}