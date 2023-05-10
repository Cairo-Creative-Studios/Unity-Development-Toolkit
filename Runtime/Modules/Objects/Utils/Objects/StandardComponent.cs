using System;
using NaughtyAttributes;
using UDT.Core.Controllables;
using UDT.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UDT.Core
{    
    /// <summary>
    /// Provides a base class for Standard Components that can be added to the Standard Object.
    /// This will allow you to use the Standard Object's Component Management System.
    /// </summary>
    public class StandardComponent : MonoBehaviour, IFSM
    {
        [Expandable] public ComponentDataBase Data;
        public StandardObject Object { get; set; }
        public Type AttachedSystemType;

        public virtual void OnInstantiate()
        {
        }

        public virtual void OnFree()
        {
        }
        
        public virtual void OnEnable()
        {
            Reset();
        }
        
        public virtual void OnDisable()
        {
            OnFree();
        }

        private void Reset()
        {
            OnReset();
            
            //Ensure Standard Object Component
            if (Object == null) Object = gameObject.GetComponentInParent<StandardObject>();
            if (Object == null) Object = gameObject.GetComponent<StandardObject>();
            if (Object == null) Object = gameObject.AddComponent<StandardObject>();
            
            //Ensure correct Component Hierarchy
            var childName = Data.GetAttachedGOPath();
            if (childName != "" && childName != gameObject.name)
            {
                Object.AddComponent(GetType(), Data, childName);
                this.DestroyWithRequiredComponents();
            }
            
            if (!Object.HasComponent(this.GetType()))
            {
                if (Object.Components.ContainsKey(this)) return;
                Object.Components.Add(this, Data);
                OnAddComponent();
            }
        }
        
        public virtual void OnAddComponent()
        {
        }

        public virtual void OnReset()
        {
        }

        public void OnReady()
        {
            Debug.Log(this + " is ready");
            
            // Add the State Machine to the State Machine Module
            StateMachineModule.AddStateMachine(this);
            
            // Using reflection.
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(GetType());  // Reflection.

            Debug.Log(attrs);
            // Displaying output.
            foreach (System.Attribute attr in attrs)
            {
                var requireStandardComponent = attr as RequireStandardComponent;
                if(requireStandardComponent != null && Object.GetStandardComponent(requireStandardComponent.type) == null)
                {
                    Object.AddComponent((attr as RequireStandardComponent).type);
                }
            }
        }
        
        public virtual void OnInputAction(InputAction.CallbackContext context)
        {
        }
     
        public virtual void OnInputAction(SerializedInput input)
        { 
        }

        public virtual void Possess(Controller controller)
        {
        }

        public virtual void UnPossess()
        {
        }

        public Tree<IStateNode> states { get; set; }

        public void InitMachine()
        {
        }

        /// <summary>
        /// Set the State of the Object, which will toggle the Components within it on and off.
        /// </summary>
        /// <param name="stateName"></param>
        public void _SetState(string stateName)
        {
            Object.SetState(stateName);
        }
        
        /// <summary>
        /// Set the State of this Component
        /// </summary>
        /// <param name="stateName"></param>
        public void SetComponentState(string stateName)
        {
            Object.SetComponentState(this, stateName);
        }
        
        /// <summary>
        /// Add a State to the Object, for Component Management
        /// </summary>
        /// <param name="stateName"></param>
        public void AddState(string stateName)
        {
            Object.AddState(stateName);
        }
        
        public void AddComponentToState(string stateName, StandardComponent componentType, bool enabled = true)
        {
            Object.AddComponentToState(stateName, componentType, enabled);
        }
    }

    /// <summary>
    /// Use for implementing Standard Components that tie into the Component Data Managaement System
    /// This will create the Component Data for you, and will allow you to access it via the Data property without casting it yourself.
    /// </summary>
    /// <typeparam name="TComponentData"></typeparam>
    public class StandardComponent<TComponentData> : StandardComponent where TComponentData : ComponentDataBase
    {
        [Button("Generate Data")]
        public void GenerateData()
        {
            base.Data = ScriptableObject.CreateInstance<TComponentData>();
        }

        public new TComponentData Data
        {
            get => (TComponentData)base.Data;
            set => base.Data = value;
        }

        public override void OnReset()
        {
            if (this.Data == null)
            {
                GenerateData();
            }
        }
    }
    
    /// <summary>
    /// Use for implementing Standard Components that tie into the Component Data Managaement System and add it to a System
    /// This will create the Component Data for you, and will allow you to access it via the Data property without casting it yourself.
    /// This will also add the object to the System with the specified System Type
    /// </summary>
    /// <typeparam name="TComponentData"></typeparam>
    /// <typeparam name="TSystem"></typeparam>
    public class StandardComponent<TComponentData, TSystem> : StandardComponent where TSystem : System<TSystem> where TComponentData : ComponentDataBase
    {
        [Button("Generate Data")]
        public void GenerateData()
        {
            base.Data = ScriptableObject.CreateInstance<TComponentData>();
        }
        public System<TSystem> system;
        
        public new TComponentData Data
        {
            get => (TComponentData)base.Data;
            set => base.Data = value;
        }

        public override void OnInstantiate()
        {
            base.OnInstantiate();
            
            AttachedSystemType = typeof(TSystem);
            system = System<TSystem>.StartSystem();
            
            ObjectModule.OnComponentAdded?.Invoke(this, typeof(TSystem));
        }

        public override void OnFree()
        {
            ObjectModule.OnComponentRemoved?.Invoke(this, typeof(TSystem));
        }

        public void DestroyObject()
        {
            Object.Free();
        }

        public override void OnReset()
        {
            if (this.Data == null)
            {
                GenerateData();
            }
        }
    }
}