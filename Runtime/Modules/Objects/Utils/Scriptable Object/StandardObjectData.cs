using System.Collections.Generic;
using UnityEngine;

namespace UDT.Core
{
    /// <summary>
    /// Extend this Scriptable Object class to define Custom Data for 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [CreateAssetMenu(fileName = "Standard Object Data", menuName = "UDT/Objects/Data", order = 0)]
    public class StandardObjectData : ScriptableObject
    {
        /// <summary>
        /// The Prefab to use for Instantiation, if the Generation Type is set to Prefab. If this is left empty,
        /// the Object Module will create a new Game Object to use as the Prefab for the created Object.
        /// </summary>
        public GameObject prefab;
        /// <summary>
        /// Components that are added to the Standard Object when it is created. Components can either be generated from
        /// this list, or can be attached directly to the given Prefab.
        /// </summary>
        public string[] components = new string[] { };
        /// <summary>
        /// Data that will be passed to the Components of the 
        /// </summary>
        public List<object> data = new List<object>();
    }


}