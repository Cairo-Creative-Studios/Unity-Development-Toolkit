using NaughtyAttributes;
using UnityEngine;

namespace UDT.Core
{
    public class StandardComponent : MonoBehaviour, IComponentBase
    {
        [Expandable] public ComponentDataBase Data;
        public StandardObject Object { get; set; }
        
        public virtual void OnInstantiate()
        {
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
            //Ensure Standard Component
            if (Object == null) Object = gameObject.GetComponent<StandardObject>();
            if (Object == null) Object = gameObject.AddComponent<StandardObject>();
            if (!Object.HasComponent(this.GetType()))
            {
                Object.Components.Add(this, Data);
                OnAddComponent();
            }
            ObjectModule.OnComponentAdded?.Invoke(this);
        }
        
        public virtual void OnAddComponent()
        {
        }
    }

    /// <summary>
    /// Use for implementing Standard Components that tie into the Component Data Managaement System
    /// This will create the Component Data for you, and will allow you to access it via the Data property without casting it yourself.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public class StandardComponent<TComponent> : StandardComponent where TComponent : ComponentDataBase
    {
        public new TComponent Data => (TComponent)(object)base.Data;
        public override void OnInstantiate()
        {
            base.OnInstantiate();
        }
    }
    
    /// <summary>
    /// Use for implementing Standard Components that tie into the Component Data Managaement System and add it to a System
    /// This will create the Component Data for you, and will allow you to access it via the Data property without casting it yourself.
    /// This will also add the object to the System with the specified System Type
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <typeparam name="TSystem"></typeparam>
    public class StandardComponent<TComponent, TSystem> : StandardComponent where TSystem : System<TSystem> where TComponent : ComponentDataBase
    {
        public TSystem System = System<TSystem>.GetInstance();
        public new TComponent Data => (TComponent) base.Data;

        public override void OnInstantiate()
        {
            base.OnInstantiate();
            System<TSystem>.AddObject(this.Object);
        }
    }
}