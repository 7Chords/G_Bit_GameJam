using UnityEngine;
using UnityEditor;

public class MoveToFirstChildInHierarchyEditor : EditorWindow
{
    [MenuItem("Tools/Move Selected To First Child In Hierarchy")]
    public static void MoveSelectedToFirstChildInHierarchy()
    {
        // 获取所有选中的物体
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            // 确保该物体有父物体
            if (obj.transform.parent != null)
            {
                Transform parentTransform = obj.transform.parent;

                // 检查父物体是否有子物体
                if (parentTransform.childCount > 0)
                {
                    // 将选定的对象放置到第一个子物体的位置
                    Transform firstChild = parentTransform.GetChild(0);
                    obj.transform.SetSiblingIndex(0); // 将对象设置为第一个子物体
                }
                else
                {
                    Debug.LogWarning($"{parentTransform.name} has no child objects.");
                }
            }
            else
            {
                Debug.LogWarning($"{obj.name} does not have a parent object.");
            }
        }
    }
}
