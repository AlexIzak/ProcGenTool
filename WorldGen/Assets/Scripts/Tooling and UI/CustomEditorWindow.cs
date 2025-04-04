using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEditorInternal;
using System;

public class CustomEditorWindow : EditorWindow
{
    //Object tool; //The generation script

    //List variables
    const string helpText = "Cannot find 'World Generation Script' component on any GameObject in the scene!";
    static Rect helpRect = new Rect(0f, 0f, 400f, 100f); //Size of above message

    //Size of list
    static Vector2 windowMinSize = Vector2.zero * 500f;
    static Rect listRect = new Rect(Vector2.zero, windowMinSize);

    //Is the world generated or not
    bool isActive;

    SerializedObject objectSO = null; //Script containing the list
    ReorderableList listRE = null; //Editor list

    //Reference to the list class
    WorldGeneration worldGen;

    [MenuItem("Window/World Generator")]
    static void OpenWindow()
    {
        //CustomEditorWindow window = (CustomEditorWindow)GetWindow(typeof(CustomEditorWindow));
        //window.minSize = new Vector2(720, 480);
        //window.Show();

        GetWindow<CustomEditorWindow>("2D World Generator");
    }

    private void OnEnable()
    {
        worldGen = FindFirstObjectByType<WorldGeneration>();

        if (worldGen)
        {
            objectSO = new SerializedObject(worldGen);

            //Initialise list
            listRE = new ReorderableList(objectSO, objectSO.FindProperty("stages"), true, true, true, true);

            //listRE.DoList(listRect);

            //EditorGUILayout.PropertyField(objectSO.FindProperty("stages"));

            //Drawing the list
            
            listRE.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Stages");
            //listRE.displayAdd = true;
            //listRE.displayRemove = true;
            listRE.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += 5f;
                rect.height = EditorGUIUtility.singleLineHeight;

                GUIContent objectLabel = new GUIContent($"Stage {index}");

                //Draw each field and give it a label
                //EditorGUI.PropertyField(rect, listRE.serializedProperty.GetArrayElementAtIndex(index), objectLabel);
                EditorGUILayout.PropertyField(listRE.serializedProperty.GetArrayElementAtIndex(index), objectLabel);
            };
        }
    }

    private void OnInspectorUpdate()
    {
        //Updates the editor window
        Repaint();
    }

    void OnGUI()
    {
        //Window Code
        if(objectSO == null)
        {
            EditorGUI.HelpBox(helpRect, helpText, MessageType.Warning);
            return;
        }
        else if(objectSO != null)
        {
            objectSO.Update();
            //listRE.DoList(listRect);
            listRE.DoLayoutList();
            objectSO.ApplyModifiedProperties(); //Adds to the script list
        }

        GUILayout.Space(10f);
        GUILayout.Label("Choose desired generation stages");
        //GUILayout.Space(listRE.GetHeight() + 10f);

        GUILayout.Space(30f);

        //Generating and clearing the world
        GUILayout.Label("Generate a 2D tilemap world", EditorStyles.boldLabel);
        GUILayout.Space(10);
        if (GUILayout.Button("Generate") && worldGen != null)
        {
            Debug.Log("Generating...");

            Generate();
        }

        GUILayout.Space(10f);

        if (GUILayout.Button("Clear Generation") && worldGen != null)
        {
            Debug.Log("Cleaning...");

            Clear();
        }

        GUILayout.Space(10f);

        GUILayout.Label(isActive ? "World Generated!" : "World cleared!", EditorStyles.boldLabel);

        //GUILayout.Space(10);
        //GUILayout.Label("Attach the world generation script", EditorStyles.boldLabel);
        //GUILayout.Space(10);
        //tool = EditorGUILayout.ObjectField(tool, typeof(WorldGeneration), true);// as Object;
        //GUILayout.Space(10);

        //GUILayout.Label("Choose desired generation stages", EditorStyles.boldLabel);
        //GUILayout.Space(10);
    }

    private void Generate()
    {
        isActive = true;

        //foreach (var obj in worldGen.GetStages())
        //{
        //    //stage = EditorGUILayout.ObjectField(obj, typeof(BaseGeneration), true);

        //    Debug.Log($"Stage : {obj.name}");

        //    //worldGen.SetStages(obj);
        //}

        //if (listRE.list.Count > 0)
            worldGen.Generate();
        //else Debug.Log("Please add at least one stage to the list");
    }

    private void Clear()
    {
        isActive = false;

        worldGen.Clear();
    }
}
