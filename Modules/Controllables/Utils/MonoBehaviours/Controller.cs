using System;
using System.Collections.Generic;
using UnityEngine;

namespace UDT.Core.Controllables
{
    public class Controller : MonoBehaviour
    {
        public List<IComponentControllable> Controllables = new List<IComponentControllable>();
        public SerializedInputMap inputMap = new SerializedInputMap();
        public ControllerData data;

        private void Awake()
        {
            Controllables = new List<IComponentControllable>();
        }

        public virtual void InitController()
        {
            // Override this method to initialize the controller
            ControllerModule.AddController(this);
        }
        
        public void Possess(IComponentControllable controllable)
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
        
        public void UnPossess(IComponentControllable controllable)
        {
            Controllables.RemoveAt(Controllables.IndexOf(controllable));
            controllable.UnPossess();
        }

        public bool PossessingControllable(IComponentControllable controllable)
        {
            return Controllables.Contains(controllable);
        }

        private void Update()
        {
            foreach (IComponentControllable controllable in Controllables)
                if(!controllable.isPossessed && ((MonoBehaviour)controllable).enabled)
                    controllable.Possess(this);
        }

        private void OnDestroy()
        {
            ControllerModule.RemoveController(this);
        }
    }
}