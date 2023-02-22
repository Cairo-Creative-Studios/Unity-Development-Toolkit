using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UDT.Core.Controllables
{
    [Serializable]
    public class SerializedInput
    {
        public enum InputType
        {
            Button,
            Axis,
            Vector2
        }
        
        public string action;
        public InputType inputType;
        
        public bool boolValue;
        public float floatValue;
        public Vector2 vector2Value;
    }

    /// <summary>
    /// A class that can be serialized to store input data, and can be used to set and get input data for AI Controllers.
    /// It also stores a list of performed inputs, which can be used to determine if an input has been performed this frame.
    /// This is used to m
    /// </summary>
    public class SerializedInputMap
    {
        protected List<SerializedInput> inputs = new List<SerializedInput>();
        public List<SerializedInput> performedInputs = new List<SerializedInput>();
        public InputActionAsset inputActionAsset;
        public string actionMapName;

        public SerializedInputMap(InputActionAsset inputActionAsset = null, string actionMapName = "")
        {
            this.inputActionAsset = inputActionAsset;
            this.actionMapName = actionMapName;
            if (inputActionAsset != null)
                SetInputsFromActionMapAsset();
        }
        
        /// <summary>
        /// Clears the performed inputs list
        /// </summary>
        public void Clear()
        {
            performedInputs.Clear();
        }

        /// <summary>
        /// Set the Inputs of the input map from the action map in the input action asset
        /// </summary>
        public void SetInputsFromActionMapAsset()
        {
            var inputMap = Array.Find(inputActionAsset.actionMaps.ToArray(),x => x.name == actionMapName);
            
            if (inputMap == null)
            {
                Debug.LogError("Input Map not found in Input Action Asset");
                return;
            }
            foreach (var action in inputMap.actions)
            {
                SerializedInput input = GetInput(action.name);
                if (input == null)
                {
                    input = new SerializedInput();
                    input.action = action.name;
                    
                    if (action.type == InputActionType.Button)
                        input.inputType = SerializedInput.InputType.Button;
                    else if (action.type == InputActionType.Value)
                        input.inputType = SerializedInput.InputType.Axis;
                    
                    input.inputType = SerializedInput.InputType.Button;
                    inputs.Add(input);
                }
            }
        }

        /// <summary>
        /// Sets the input action asset and action map name, and then sets the inputs from the action map
        /// </summary>
        /// <param name="asset"></param>
        public void SetInputFromActionMapAsset(InputActionAsset asset)
        {
            inputActionAsset = asset;
            actionMapName = asset.actionMaps[0].name;
            SetInputsFromActionMapAsset();
        }

        /// <summary>
        /// Gets the SerializedInput for the given Action Name
        /// </summary>
        public SerializedInput GetInput(string actionName)
        {
            foreach (SerializedInput input in inputs)
            {
                if (input.action == actionName)
                    return input;
            }

            return null;
        }
        
        /// <summary>
        /// Sets the value of the given Input Action
        /// </summary>
        public void SetInput(string actionName, bool value)
        {
            SerializedInput input = GetInput(actionName);
            if (input == null)
            {
                input = new SerializedInput();
                input.action = actionName;
                input.inputType = SerializedInput.InputType.Button;
                inputs.Add(input);
            }
            input.boolValue = value;
            performedInputs.Add(input);
        }
        
        /// <summary>
        /// Sets the value of the given Input Action
        /// </summary>
        public void SetInput(string actionName, float value)
        {
            SerializedInput input = GetInput(actionName);
            if (input == null)
            {
                input = new SerializedInput();
                input.action = actionName;
                input.inputType = SerializedInput.InputType.Axis;
                inputs.Add(input);
            }
            input.floatValue = value;
            performedInputs.Add(input);
        }
        
        /// <summary>
        /// Sets the value of the given Input Action
        /// </summary>
        public void SetInput(string actionName, Vector2 value)
        {
            SerializedInput input = GetInput(actionName);
            if (input == null)
            {
                input = new SerializedInput();
                input.action = actionName;
                input.inputType = SerializedInput.InputType.Vector2;
                inputs.Add(input);
            }
            input.vector2Value = value;
            performedInputs.Add(input);
        }
        
        /// <summary>
        /// Returns the current value of the Input Action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool GetButton(string action)
        {
            SerializedInput input = GetInput(action);
            if (input == null)
                return false;
            return input.boolValue;
        }
        
        /// <summary>
        /// Returns the current value of the Input Action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public float GetAxis(string action)
        {
            SerializedInput input = GetInput(action);
            if (input == null)
                return 0;
            return input.floatValue;
        }
        
        /// <summary>
        /// Returns the current value of the Input Action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Vector2 GetVector2(string action)
        {
            SerializedInput input = GetInput(action);
            if (input == null)
                return Vector2.zero;
            return input.vector2Value;
        }
    }
}