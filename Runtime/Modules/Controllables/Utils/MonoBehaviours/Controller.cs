using System;
using System.Collections.Generic;
using UnityEngine;

namespace UDT.Core.Controllables
{
    /// <summary>
    /// The Controller is the base class for all controllers.
    /// Possession of a controllable is handled by the controller.
    /// </summary>
    public class Controller : MonoBehaviour
    {
        public List<ControllableComponent> Controllables = new List<ControllableComponent>();
        public SerializedInputMap inputMap = new SerializedInputMap();
        public ControllerData data;

        private void Awake()
        {
            Controllables = new List<ControllableComponent>();
        }

        public virtual void InitController()
        {
            // Override this method to initialize the controller
            ControllerModule.AddController(this);
        }
        
        public void Possess(StandardObject standardObject)
        {
            bool possessed = false;
            try
            {
                foreach (ControllableComponent controllable in standardObject.Components.Keys)
                {
                    Possess(controllable);
                    possessed = true;
                }
            }
            catch
            {
                if (standardObject == null)
                {
                    Debug.LogError("StandardObject is null", this);
                    return;
                }
                if (!possessed)
                    Debug.LogError("No controllables found on " + standardObject.name, this);
                //ignore invalid cast exception
            }
        }
        
        public void UnPossess(StandardObject standardObject)
        {
            try
            {
                foreach (ControllableComponent controllable in standardObject.Components.Keys)
                    UnPossess(controllable);
            }
            catch
            {
                //ignore invalid cast exception
            }
        }
        
        public void Possess(ControllableComponent controllable)
        {
            if (controllable == null)
            {
                Debug.LogError("Controllable is null", this);
                return;
            }
            
            if (controllable.Possess(this))
            {
                Debug.LogError("Controllable already possessed", (MonoBehaviour)controllable);
                return;
            }
            
            Controllables.Add(controllable);
        }
        
        public void UnPossess(ControllableComponent controllable)
        {
            Controllables.RemoveAt(Controllables.IndexOf(controllable));
            controllable.UnPossess();
        }

        public bool PossessingControllable(ControllableComponent controllable)
        {
            return Controllables.Contains(controllable);
        }

        private void Update()
        {
            foreach (ControllableComponent controllable in Controllables)
                if(!controllable.isPossessed && ((MonoBehaviour)controllable).enabled)
                    controllable.Possess(this);
        }

        private void OnDestroy()
        {
            ControllerModule.RemoveController(this);
        }
    }
}