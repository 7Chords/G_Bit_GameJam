using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class TilemapDebugger : EditorWindow
{
    private bool enableDebugger = false; // 是否启用调试工具
    private int selectedScriptIndex = 0; // 当前选中的脚本索引

    // 使用类型直接定义脚本列表
    private List<Type> availableScripts = new List<Type>
    {
        typeof(InvisibleEnterTile),
        typeof(SlidesTile),
        typeof(SeesawTile),
        typeof(DialogueTile),
        typeof(CatTeleport),
        typeof(FinishLevelTile),
        typeof(SpikeTile),
        typeof(DialogueTile)
    };

    [MenuItem("Tools/Tilemap Editor")]
    public static void ShowWindow()
    {
        GetWindow<TilemapDebugger>("Tilemap Editor");
    }

    private void OnGUI()
    {
        // 工具启用开关
        enableDebugger = EditorGUILayout.Toggle("Enable Edit", enableDebugger);

        // 显示脚本选择列表
        GUILayout.Label("Select a Script to Attach:", EditorStyles.boldLabel);
        if (availableScripts.Count > 0)
        {
            string[] scriptNames = availableScripts.ConvertAll(script => script.Name).ToArray();
            selectedScriptIndex = EditorGUILayout.Popup("Script", selectedScriptIndex, scriptNames);
        }
        else
        {
            EditorGUILayout.HelpBox("No scripts available. Add script types to the list.", MessageType.Warning);
        }

        // 提示信息
        if (enableDebugger)
        {
            EditorGUILayout.HelpBox("Debugger is enabled. Select objects in the Hierarchy to modify.", MessageType.Info);

            if (GUILayout.Button("Apply to Selected Object(s)"))
            {
                ApplyToSelectedObjects();
            }

            if (GUILayout.Button("Remove Special Script from Objects"))
            {
                RemoveScriptFromSelectedObjects();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Debugger is disabled. Enable it to use the tool.", MessageType.Warning);
        }
    }

    private void ApplyToSelectedObjects()
    {
        if (!enableDebugger)
        {
            Debug.LogWarning("Debugger is not enabled.");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No GameObjects selected. Please select GameObjects to modify.");
            return;
        }

        Type selectedScript = availableScripts[selectedScriptIndex];

        foreach (GameObject obj in selectedObjects)
        {
            GameObject targetObject = GetValidParent(obj);
            if (targetObject == null)
            {
                Debug.LogWarning($"The object '{obj.name}' or its parents do not contain a Polygon Collider 2D. Skipping.");
                continue;
            }

            if (targetObject.GetComponent<LogicTile>() == null)
            {
                Debug.LogWarning($"The object '{targetObject.name}' does not contain a LogicTile script. Skipping.");
                continue;
            }

            if (targetObject.GetComponent(selectedScript) != null)
            {
                Debug.Log($"The object '{targetObject.name}' already has the script '{selectedScript.Name}' attached. Skipping.");
                continue;
            }

            targetObject.AddComponent(selectedScript);
            Debug.Log($"Added script '{selectedScript.Name}' to object '{targetObject.name}'.");

            targetObject.name = selectedScript.Name;
            Debug.Log($"Object name changed to '{selectedScript.Name}'.");

            MoveToFirstChildInHierarchy(targetObject);
        }
    }

    private void RemoveScriptFromSelectedObjects()
    {
        if (!enableDebugger)
        {
            Debug.LogWarning("Debugger is not enabled.");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No GameObjects selected. Please select GameObjects to modify.");
            return;
        }

        foreach (GameObject obj in selectedObjects)
        {
            GameObject targetObject = GetValidParent(obj);
            if (targetObject == null)
            {
                Debug.LogWarning($"The object '{obj.name}' or its parents do not contain a Polygon Collider 2D. Skipping.");
                continue;
            }

            if (targetObject.GetComponent<LogicTile>() == null)
            {
                Debug.LogWarning($"The object '{targetObject.name}' does not contain a LogicTile script. Skipping.");
                continue;
            }

            bool scriptRemoved = false;
            foreach (Type scriptType in availableScripts)
            {
                Component component = targetObject.GetComponent(scriptType);
                if (component != null)
                {
                    DestroyImmediate(component);
                    Debug.Log($"Removed script '{scriptType.Name}' from object '{targetObject.name}'.");
                    scriptRemoved = true;
                }
            }

            if (scriptRemoved)
            {
                targetObject.name = "LogicTile(Clone)";
                Debug.Log($"Object name changed to 'LogicTile(Clone)'.");
            }
            else
            {
                Debug.Log($"No matching scripts found on '{targetObject.name}'. Skipping.");
            }
        }
    }

    public static void MoveToFirstChildInHierarchy(GameObject obj)
    {
        if (obj.transform.parent != null)
        {
            Transform parentTransform = obj.transform.parent;

            if (parentTransform.childCount > 0)
            {
                obj.transform.SetSiblingIndex(0);
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

    private GameObject GetValidParent(GameObject obj)
    {
        Transform current = obj.transform;
        while (current != null)
        {
            if (current.GetComponent<LogicTile>() != null)
            {
                return current.gameObject;
            }
            current = current.parent;
        }

        return null;
    }
}
