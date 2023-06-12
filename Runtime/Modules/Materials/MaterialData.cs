using System.Collections.Generic;
using UDT.Core;
using UnityEngine;

namespace UDT.Materials
{
    public class MaterialData : Data
    {
        /// <summary>
        /// A dictionary of material types and their corresponding materials.
        /// </summary>
        [Tooltip("A dictionary of material types and their corresponding materials.")]
        public SerializableDictionary<string, List<Material>> MaterialTypes = new SerializableDictionary<string, List<Material>>();
    }
}