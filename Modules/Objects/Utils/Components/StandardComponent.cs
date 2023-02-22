using UnityEngine;

namespace UDT.Core
{
    public class StandardComponent : MonoBehaviour, IComponentBase
    {
        public ComponentDataBase Data { get; set; }
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
                Object.Components.Add(this);
                OnAddComponent();
            }
            ObjectModule.OnComponentAdded?.Invoke(this);
        }
        
        public virtual void OnAddComponent()
        {
        }
    }

    public class StandardComponent<T> : StandardComponent where T : System<T>
    {
        public T System = System<T>.GetInstance();

        public override void OnInstantiate()
        {
            base.OnInstantiate();
            System<T>.AddObject(this.Object);
        }
    }
}