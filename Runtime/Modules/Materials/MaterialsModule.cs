using UDT.Core;
using UnityEngine;

namespace UDT.Materials
{
    public class MaterialsModule : Runtime<MaterialsModule, MaterialData>
    {
        /// <summary>
        /// Finds the material type of the given material.
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public static string FindMaterialType(Material material)
        {
            for(int i = 0; i < Data.MaterialTypes.Count; i++)
            {
                var key = Data.MaterialTypes.KeyAt(i);
                for(int j = 0; j < Data.MaterialTypes[key].Count; j++)
                {
                    var storedMaterial = Data.MaterialTypes[key][j];
                    if(material == storedMaterial)
                    {
                        return key;
                    }
                }
            }
            
            return "";
        }
    }
}