using System;
using NaughtyAttributes;
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
        public virtual void OnInputAction(InputAction.CallbackContext context)
        {
        }

        public void OnInputAction(SerializedInput input)
        { 
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