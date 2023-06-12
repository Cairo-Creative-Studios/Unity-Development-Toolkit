using UDT.Core;
using UnityEngine;

namespace UDT.Materials
{
    public class MaterialsModule : Runtime<MaterialsModule>, IData<MaterialData>
    {
        /// <summary>
        /// Finds the material type of the given material.
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public static string FindMaterialType(Material material)
        {
            for(int i = 0; i < Instance._Data.MaterialTypes.Count; i++)
            {
                var key = Instance._Data.MaterialTypes.KeyAt(i);
                for(int j = 0; j < Instance._Data.MaterialTypes[key].Count; j++)
                {
                    var storedMaterial = Instance._Data.MaterialTypes[key][j];
                    if(material == storedMaterial)
                    {
                        return key;
                    }
                }
            }
            
            return "";
        }

        public bool Initialized { get; set; }
        public MaterialData _Data { get; set; }
    }
}