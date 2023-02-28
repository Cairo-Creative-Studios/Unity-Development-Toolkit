using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UDT.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace UDT.Core
{
    /// <summary>
    /// Extend this Scriptable Object class to define Custom Data for 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [CreateAssetMenu(fileName = "Standard Object Data", menuName = "UDT/Objects/Data", order = 0)]
    public class ObjectDefinition : ScriptableObject
    {
        /// <summary>
        /// The Prefab to use for Instantiation, if the Generation Type is set to Prefab. If this is left empty,
        /// the Object Module will create a new Game Object to use as the Prefab for the created Object.
        /// </summary>
        public GameObject prefab;
        /// <summary>
        /// Component Data that is passed to the created Object. If the Objects don't contain the Components correlated
        /// with these Data files, a new Component will be added to the Object automatically.
        /// </summary>
        public List<ComponentDataBase> ComponentData = new List<ComponentDataBase>();

        /// <summary>
        /// Subscribed Systems are systems that will be notified when an Object is created using this Object Definition.
        /// </summary>
        [Tooltip("Subscribed Systems are systems that will be notified when an Object is created using this Object Definition.")]
        [Header("Systems")]
        public List<string> SubscribedSystems = new List<string>();

        [Tooltip("Select a System to Subscribe to.")]
        [Dropdown("GetSystemsInProject")] 
        public string SelectedSystem;

        [Button("Subscribe To System")]
        public void Subscribe()
        {
            SubscribedSystems.Add(SelectedSystem);
            
            ValidateSystems();
        }

        private DropdownList<string> systemsInProject;
        
        public DropdownList<string> GetSystemsInProject()
        {
            if(systemsInProject != null) return systemsInProject;
            
            systemsInProject = new DropdownList<string>();
            Type[] systemTypes = Type.GetType("UDT.Core.System`1").GetInheritedTypes();

            foreach (var s in systemTypes)
            {
                systemsInProject.Add(s.Name, s.AssemblyQualifiedName);
            }

            return systemsInProject;
        }

        private void OnValidate()
        {
            ValidateSystems();
        }

        void ValidateSystems()
        {
            
            
            var SubSystems = new List<string>();
            
            foreach (var system in SubscribedSystems)
            {
                if (!SubSystems.Contains(system))
                    SubSystems.Add(system);
            }

            SubscribedSystems = SubSystems;
        }
    }
}