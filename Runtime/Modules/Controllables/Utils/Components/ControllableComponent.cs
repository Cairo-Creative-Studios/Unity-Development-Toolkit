using System;
using NaughtyAttributes;
using UDT.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UDT.Core.Controllables
{
    /// <summary>
    /// Interface for components that can be controlled by a controller
    /// Reflection is used to call Methods within classes that implement this interface, based on the name of the input
    /// that is given to it from a Controller.
    /// </summary>
    public class ControllableComponent : StandardComponent
    {
        public Byte inputByte;
        public bool isPossessed = false;
        public Controller Controller;

        [Tooltip("Pairs of input names and method names to call when the input is triggered")]
        public SerializableDictionary<string, string> InputsToMethodsMap = new SerializableDictionary<string, string>();

        public void OnInputAction(InputAction.CallbackContext context)
        {
            this.CallMethod(InputsToMethodsMap[context.action.name], new object[]{context});
        }

        public void OnInputAction(SerializedInput input)
        { 
            if(input.inputType == SerializedInput.InputType.Button)
                this.CallMethod(InputsToMethodsMap[input.action], new object[]{input.boolValue});   
            else if(input.inputType == SerializedInput.InputType.Axis)
                this.CallMethod(InputsToMethodsMap[input.action], new object[]{input.floatValue});  
            else if(input.inputType == SerializedInput.InputType.Vector2 && input.vector2Value != Vector2.zero)
                this.CallMethod(InputsToMethodsMap[input.action], new object[]{input.vector2Value});  
        }

        public bool Possess(Controller controller)
        {
            if(isPossessed)
                return false;
            
            Controller = controller;
            isPossessed = true;
            return true;
        }

        public void UnPossess()
        {
            Controller = null;
            isPossessed = false;
        }
    }
    
    public class ControllableComponent<TComponentData> : ControllableComponent where TComponentData : ComponentDataBase
    {
        [Button("Generate Data")]
        public void GenerateData()
        {
            base.Data = ScriptableObject.CreateInstance<TComponentData>();
        }
        public new TComponentData Data => (TComponentData)base.Data;

        public override void OnReset()
        {
            GenerateData();
        }
    }
    
    public class ControllableComponent<TComponentData, TSystem> : ControllableComponent where TComponentData : ComponentDataBase where TSystem : System<TSystem>
    {
        [Button("Generate Data")]
        public void GenerateData()
        {
            base.Data = ScriptableObject.CreateInstance<TComponentData>();
        }
        public System<TSystem> system;

        public new TComponentData Data => (TComponentData)base.Data;

        public override void OnInstantiate()
        {
            base.OnInstantiate();
            system = System<TSystem>.GetInstance();
            System<TSystem>.AddObject(Object);
        }

        public override void OnReset()
        {
            GenerateData();
        }
    }
}