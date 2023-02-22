using System;
using System.Data.SqlTypes;

namespace UDT.Core
{
    /// <summary>
    /// Using the IComponentBase allows MonoBehaviours to be used within the UDT's Standard Object Managed Component
    /// System, providing a simpler interface to the Component and it's relation to the Object it's attached to.
    /// </summary>
    public interface IComponentBase
    {
        /// <summary>
        /// The Data passed to the Component from the Object Module.
        /// Data is passed by reference, so when Changes to the original data is made with the custom StandardObjectData,
        /// the change will be seen within the Component.
        /// </summary>
        public ComponentDataBase Data { get; set; }
        /// <summary>
        /// The parent object of the component
        /// </summary>
        public StandardObject Object { get; set; }
        
        /// <summary>
        /// Called when the Standard Object is Instantiated
        /// </summary>
        public void OnInstantiate();
        /// <summary>
        /// Called when the Standard Object is pooled/Destroyed
        /// </summary>
        public void OnFree();
    }
}