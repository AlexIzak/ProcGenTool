using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.Collections.Generic;

public class CustomEditorWindow : EditorWindow
{

    string description = string.Empty;
    Object tool;

    List<Object> stages = new List<Object>();   

    Object stage;

    [MenuItem("Window/World Generator")]
    static void OpenWindow()
    {
        //CustomEditorWindow window = (CustomEditorWindow)GetWindow(typeof(CustomEditorWindow));
        //window.minSize = new Vector2(720, 480);
        //window.Show();

        GetWindow<CustomEditorWindow>("2D World Generator");
    }

    void OnGUI()
    {
        //Window Code
        GUILayout.Space(10);
        GUILayout.Label("Attach the world generation script", EditorStyles.boldLabel);
        GUILayout.Space(10);
        tool = EditorGUILayout.ObjectField(tool, typeof(WorldGeneration), true);// as Object;
        GUILayout.Space(10);

        GUILayout.Label("Choose desired generation stages", EditorStyles.boldLabel);
        GUILayout.Space(10);

        //TODO Look at the website tutorial on how to add a list to the custom window
        //stages.Add();

        foreach (Object obj in stages)
        {
            stage = EditorGUILayout.ObjectField(obj, typeof(BaseGeneration), true);
            tool.GetComponent<WorldGeneration>().SetStages(stage.GetComponent<BaseGeneration>());
        }
        //description = EditorGUILayout.TextField("Description", description);

        //Generating and clearing the world
        GUILayout.Label("Generate a 2D tilemap world", EditorStyles.boldLabel);
        GUILayout.Space(10);
        if (GUILayout.Button("Generate") && tool != null)
        {
            Debug.Log("Generating...");

            tool.GetComponent<WorldGeneration>().Generate();
        }

        if (GUILayout.Button("Clear Generation") && tool != null)
        {
            Debug.Log("Cleaning...");

            tool.GetComponent<WorldGeneration>().Clear();
        }
    }
}
