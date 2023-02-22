using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UDT.Core
{
    /// <summary>
    /// A Streamed Object is a GameObject that is loaded and unloaded at runtime. It can be used to create a level with a lot of objects without having to load them all at once.
    /// Streamed Objects are not managed by the Standard Object system, so they can't be accessed by UID/IID.
    /// Standard Objects added to StreamedObject will managed by the Standard Object system.
    /// </summary>
    [ExecuteAlways]
    public class StreamedObject : MonoBehaviour
    {
        public string ID;
        private GameObject _instance;
        [SerializeField] private GameObject prefab;
        [SerializeField] private string path;
        private GameObject _usedPrefab;
        private string _oldPath;
        private string _oldName;
        private bool _isprefabNull;

        private void Start()
        {
            _isprefabNull = prefab == null;
        }

        private void Awake()
        {
            if (Application.isPlaying)
                Unload();
        }

        public void Load()
        {
            if (prefab == null) return;
            if (_instance != null) return;
            _instance = GameObject.Instantiate(prefab, transform);
        }

        public void Unload()
        {
            foreach (Transform child in transform.GetChildren())
                if (child != transform)
                    Destroy(child.gameObject);
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                return;
            bool generate = _isprefabNull;

            if (_oldName != name || _oldPath != path)
            {
                _isprefabNull = true;
                _oldName = name;
                _oldPath = path;

                //Destroy the old Prefab if it exists
                if (!_isprefabNull)
                {
                    try
                    {
                        DestroyImmediate(prefab, true);
                    }
                    catch
                    {
                        _isprefabNull = false;
                    }
                }
            }

            string fullPath = "Assets/" + path + name + ".prefab";
            if (_isprefabNull)
            {
                //Generate
                GameObject generated = new GameObject();
                prefab = PrefabUtility.SaveAsPrefabAsset(generated, fullPath);
                _isprefabNull = false;
                DestroyImmediate(generated);

                //Instantiate the Instance of the Prefab
                _instance = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(fullPath);
            }

            if (_instance == null)
                _instance = transform.GetChild(0).gameObject;
            else
            {
                prefab = PrefabUtility.SaveAsPrefabAsset(_instance, fullPath);
                _isprefabNull = false;
            }
#endif
        }
    }
}
