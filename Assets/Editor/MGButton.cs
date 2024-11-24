using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MGButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("生成可行走逻辑瓦片地图"))
        {
            FindObjectOfType<MapGenerator>().GetDataAndGenerateWalkableMap();
        }
    }
}
