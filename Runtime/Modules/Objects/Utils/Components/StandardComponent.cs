using NaughtyAttributes;
using UnityEngine;

namespace UDT.Core
{
    /// <summary>
    /// Provides a base class for Standard Compoennts that can be added to the Standard Object.
    /// This will allow you to use the Standard Object's Component Management System.
    /// </summary>
    public class StandardComponent : MonoBehaviour, IComponentBase
    {
        [Expandable] public ComponentDataBase Data;
        /// <summary>
        /// The Path in the Object's Hierarchy to add to Component. If no Object exists at the given path, it will be added.
        /// </summary>
        public static string ComponentPath = "";
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
                if (Object.Components.ContainsKey(this)) return;
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
    /// <typeparam name="TComponentData"></typeparam>
    public class StandardComponent<TComponentData> : StandardComponent where TComponentData : ComponentDataBase
    {
        [Button("Generate Data")]
        public void GenerateData()
        {
            base.Data = ScriptableObject.CreateInstance<TComponentData>();
        }
        public new TComponentData Data => (TComponentData)base.Data;
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
    /// <typeparam name="TComponentData"></typeparam>
    /// <typeparam name="TSystem"></typeparam>
    public class StandardComponent<TComponentData, TSystem> : StandardComponent where TSystem : System<TSystem> where TComponentData : ComponentDataBase
    {
        public TSystem System = System<TSystem>.GetInstance();
        public new TComponentData Data => (TComponentData)base.Data;

        public override void OnInstantiate()
        {
            base.OnInstantiate();
            System<TSystem>.AddObject(this.Object);
        }
    }
}