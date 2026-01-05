using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using UnityEditor.PackageManager.UI;


public class TestAnomalyWindow : EditorWindow
{
    string idAnomaly;

    [MenuItem("Window/My Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TestAnomalyWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("Id Anomaly", EditorStyles.boldLabel);
        idAnomaly = EditorGUILayout.TextField("Text Field", idAnomaly);

        if (GUILayout.Button("Occur"))
        {
            Singleton.gameSM.OnOccurTest(int.Parse(idAnomaly));
        }
    }
}
