using UnityEngine;
using UnityEditor;

public class MoveToFirstChildInHierarchyEditor : EditorWindow
{
    [MenuItem("Tools/Move Selected To First Child In Hierarchy")]
    public static void MoveSelectedToFirstChildInHierarchy()
    {
        // ��ȡ����ѡ�е�����
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            // ȷ���������и�����
            if (obj.transform.parent != null)
            {
                Transform parentTransform = obj.transform.parent;

                // ��鸸�����Ƿ���������
                if (parentTransform.childCount > 0)
                {
                    // ��ѡ���Ķ�����õ���һ���������λ��
                    Transform firstChild = parentTransform.GetChild(0);
                    obj.transform.SetSiblingIndex(0); // ����������Ϊ��һ��������
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
