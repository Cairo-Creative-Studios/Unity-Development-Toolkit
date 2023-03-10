using NaughtyAttributes;
using UDT.Core.Controllables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UDT.Core
{
    /// <summary>
    /// Provides a base class for Standard Components that can be added to the Standard Object.
    /// This will allow you to use the Standard Object's Component Management System.
    /// </summary>
    public class StandardComponent : MonoBehaviour, IComponentBase
    {
        [Expandable] public ComponentDataBase Data;
        public StandardObject Object { get; set; }
        
        public virtual void OnInstantiate()
        {
        }
        
        void Awake()
        {
            OnInstantiate();
        }

        public virtual void OnFree()
        {
            ObjectModule.OnComponentRemoved?.Invoke(this);
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
            ObjectModule.OnComponentAdded?.Invoke(this);
        }
        
        public virtual void OnAddComponent()
        {
        }

        public virtual void OnReset()
        {
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
        public new TComponentData Data => (TComponentData)base.Data;

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

        public new TComponentData Data => (TComponentData)base.Data;

        public override void OnInstantiate()
        {
            base.OnInstantiate();
            system = System<TSystem>.GetInstance();
            System<TSystem>.AddObject(Object);
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