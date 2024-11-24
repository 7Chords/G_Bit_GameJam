using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MGButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("���ɿ������߼���Ƭ��ͼ"))
        {
            FindObjectOfType<MapGenerator>().GetDataAndGenerateWalkableMap();
        }
    }
}
