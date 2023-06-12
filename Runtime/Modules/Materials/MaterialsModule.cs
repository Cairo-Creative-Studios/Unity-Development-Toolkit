using UDT.Core;
using UnityEngine;

namespace UDT.Materials
{
    public class MaterialsModule : Runtime<MaterialsModule>, IStaticData<MaterialData>
    {
        /// <summary>
        /// Finds the material type of the given material.
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public static string FindMaterialType(Material material)
        {
            for(int i = 0; i < Instance.Data.MaterialTypes.Count; i++)
            {
                var key = Instance.Data.MaterialTypes.KeyAt(i);
                for(int j = 0; j < Instance.Data.MaterialTypes[key].Count; j++)
                {
                    var storedMaterial = Instance.Data.MaterialTypes[key][j];
                    if(material == storedMaterial)
                    {
                        return key;
                    }
                }
            }
            
            return "";
        }

        public bool Initialized { get; set; }
        public MaterialData Data { get; set; }
    }
}