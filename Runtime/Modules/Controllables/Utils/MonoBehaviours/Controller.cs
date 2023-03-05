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
        public List<StandardObject> Controllables = new List<StandardObject>();
        public SerializedInputMap inputMap = new SerializedInputMap();
        public ControllerData data;
        public virtual void InitController()
        {
            // Override this method to initialize the controller
            ControllerModule.AddController(this);
        }
        
        public void Possess(StandardObject standardObject)
        {
            if(standardObject.controllerValues.Possess(this))
                Controllables.Add(standardObject);
        }
        
        public void UnPossess(StandardObject standardObject)
        {
            standardObject.controllerValues.UnPossess();
            Controllables.Remove(standardObject);
        }

        private void Awake()
        {
            foreach (StandardObject controllable in Controllables)
                if(!controllable.controllerValues.IsPossessed)
                    controllable.controllerValues.Possess(this);
        }

        private void OnDestroy()
        {
            foreach (var controllable in Controllables)
            {
                controllable.controllerValues.UnPossess();
            }
            ControllerModule.RemoveController(this);
        }
    }
}