using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

public class AttachCartToPrefabs : EditorWindow
{
    public GameObject cartPrefab; // Drag in your CartPrefab
    public string anchorName = "CartAnchor"; 
    public float padding = 0.1f;   

    [MenuItem("Tools/Batching tubes onto character Prefabs")]
    static void OpenWindow() => GetWindow<AttachCartToPrefabs>("Batch tube deployment");

    void OnGUI()
    {
        EditorGUILayout.LabelField("select Cart Prefab and start", EditorStyles.boldLabel);
        cartPrefab = (GameObject)EditorGUILayout.ObjectField("tubePrefab", cartPrefab, typeof(GameObject), false);
        anchorName = EditorGUILayout.TextField("Anchor point name", anchorName);
        padding = EditorGUILayout.FloatField("Padding", padding);

        if (GUILayout.Button("begin"))
        {
            if (cartPrefab == null)
            {
                EditorUtility.DisplayDialog("erro", "please select CartPrefab first！", "OK");
                return;
            }
            AttachToSelectedPrefabs();
        }
    }

    void AttachToSelectedPrefabs()
    {
        var selections = Selection.objects;
        int count = 0;

        foreach (var obj in selections)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            // edit Prefab 
            GameObject root = PrefabUtility.LoadPrefabContents(path);

            // find or create Anchor point
            Transform anchor = root.transform.Find(anchorName);
            if (anchor == null)
            {
                GameObject anchorGO = new GameObject(anchorName);
                anchorGO.transform.SetParent(root.transform, false);
                anchor = anchorGO.transform;
            }

            // delete if existing old mining carts inside the prefab
            foreach (Transform child in anchor)
                Object.DestroyImmediate(child.gameObject);

            // Instantiation
            GameObject cartInstance = (GameObject)PrefabUtility.InstantiatePrefab(cartPrefab, root.transform);
            cartInstance.transform.SetParent(anchor, false);
            cartInstance.transform.localPosition = Vector3.zero;
            cartInstance.transform.localRotation = Quaternion.identity;

            // save
            PrefabUtility.SaveAsPrefabAsset(root, path);
            PrefabUtility.UnloadPrefabContents(root);

            count++;
        }

        EditorUtility.DisplayDialog("finish", $"add tube to {count} Prefab successfully！", "OK");
    }
}
