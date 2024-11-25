using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class TextFontChanger : EditorWindow
{
    private Font newFont;

    [MenuItem("Tools/Font Changer")]
    public static void ShowWindow()
    {
        GetWindow<TextFontChanger>("Font Changer");
    }

    private void OnGUI()
    {
        GUILayout.Label("���ĵ�ǰ�����е����� Text ����", EditorStyles.boldLabel);
        
        newFont = (Font)EditorGUILayout.ObjectField("ѡ������", newFont, typeof(Font), false);
        
        if (GUILayout.Button("Ӧ�����嵽���� Text"))
        {
            ChangeAllTextFonts();
        }
    }

    private void ChangeAllTextFonts()
    {
        if (newFont == null)
        {
            return;
        }
        
        Text[] textComponents = FindObjectsOfType<Text>();
        
        foreach (var text in textComponents)
        {
            Undo.RecordObject(text, "Change Font");
            text.font = newFont;
            EditorUtility.SetDirty(text);
        }
    }
}

