using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

public class AttachCartToPrefabs : EditorWindow
{
    public GameObject cartPrefab; // 拖入你的 CartPrefab
    public string anchorName = "CartAnchor"; // 矿车挂点名称
    public float padding = 0.1f;   // 矿车围绕人物比包围盒更大的余量

    [MenuItem("Tools/批量套矿车到人物Prefabs")]
    static void OpenWindow() => GetWindow<AttachCartToPrefabs>("批量套矿车");

    void OnGUI()
    {
        EditorGUILayout.LabelField("选择 Cart Prefab 并点击开始", EditorStyles.boldLabel);
        cartPrefab = (GameObject)EditorGUILayout.ObjectField("矿车Prefab", cartPrefab, typeof(GameObject), false);
        anchorName = EditorGUILayout.TextField("挂点名称", anchorName);
        padding = EditorGUILayout.FloatField("Padding", padding);

        if (GUILayout.Button("开始批量套用"))
        {
            if (cartPrefab == null)
            {
                EditorUtility.DisplayDialog("错误", "请先选择 CartPrefab！", "OK");
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

            // 进入 Prefab 编辑模式
            GameObject root = PrefabUtility.LoadPrefabContents(path);

            // 查找或创建挂点
            Transform anchor = root.transform.Find(anchorName);
            if (anchor == null)
            {
                GameObject anchorGO = new GameObject(anchorName);
                anchorGO.transform.SetParent(root.transform, false);
                anchor = anchorGO.transform;
            }

            // 如果 prefab 内部已有旧矿车，先删除
            foreach (Transform child in anchor)
                Object.DestroyImmediate(child.gameObject);

            // 实例化新的Cart
            GameObject cartInstance = (GameObject)PrefabUtility.InstantiatePrefab(cartPrefab, root.transform);
            cartInstance.transform.SetParent(anchor, false);
            cartInstance.transform.localPosition = Vector3.zero;
            cartInstance.transform.localRotation = Quaternion.identity;

            // 保存
            PrefabUtility.SaveAsPrefabAsset(root, path);
            PrefabUtility.UnloadPrefabContents(root);

            count++;
        }

        EditorUtility.DisplayDialog("完成", $"成功为 {count} 个 Prefab 添加矿车！", "OK");
    }
}
