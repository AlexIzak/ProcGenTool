using UnityEngine;
using UnityEditor;
//using Unity.VisualScripting;
//using System.Collections.Generic;
using UnityEditorInternal;
//using System;
//using System.Reflection;

public class CustomEditorWindow : EditorWindow
{
    //Object tool; //The generation script

    //List variables
    string helpText = "Cannot find 'World Generation Script' component on any GameObject in the scene!";
    static Rect helpRect = new Rect(0f, 0f, 400f, 100f); //Size of above message

    //Size of list
    static Vector2 windowMinSize = Vector2.zero * 500f;
    static Rect listRect = new Rect(Vector2.zero, windowMinSize);

    //Is the world generated or not
    bool isActive;

    SerializedObject serializedGen = null; //Script containing the list
    ReorderableList listRE = null; //Editor list

    //Reference to the list class
    WorldGeneration worldGen;

    SerializedObject serializedTiles = null;
    MyTilemap tiles;

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
        tiles = FindFirstObjectByType<MyTilemap>();

        if (worldGen)
        {
            serializedGen = new SerializedObject(worldGen);

            //Initialise list
            listRE = new ReorderableList(serializedGen, serializedGen.FindProperty("stages"), true, true, true, true);

            //Drawing the list
            listRE.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Stages", EditorStyles.boldLabel);

            listRE.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                //rect.y += 5f;
                rect.height = EditorGUIUtility.singleLineHeight;

                GUIContent objectLabel = new GUIContent($"Stage {index}");

                //Draw each field and give it a label
                EditorGUI.PropertyField(rect, listRE.serializedProperty.GetArrayElementAtIndex(index), objectLabel);
            };
        }

        if (tiles)
            serializedTiles = new SerializedObject(tiles);

    }

    private void OnInspectorUpdate()
    {
        //Updates the editor window
        Repaint();
    }

    void OnGUI()
    {
        //List of generation stages
        if(serializedGen == null)
        {
            helpText = "Cannot find 'World Generation Script' component on any GameObject in the scene!";
            EditorGUI.HelpBox(helpRect, helpText, MessageType.Warning);
            return;
        }
        else if(serializedGen != null)
        {
            serializedGen.Update();
            listRE.DoLayoutList();
            serializedGen.ApplyModifiedProperties(); //Adds to the script list
        }

        //GUILayout.Space(10f);
        GUILayout.Label("Choose desired generation stages");

        //Tilemap properties
        if (serializedTiles == null)
        {
            helpText = "Cannot find 'MyTilemap Script' component on any GameObject in the scene!";
            Rect tipRect = new Rect(0f, 100f, 400f, 100f);
            EditorGUI.HelpBox(tipRect, helpText, MessageType.Warning);
            return;
        }
        else if (serializedTiles != null)
        {
            serializedTiles.Update();

            GUILayout.Space(10f);
            GUILayout.Label("World properties", EditorStyles.boldLabel);
            GUIContent widthLabel = new GUIContent("Width");
            EditorGUILayout.PropertyField(serializedTiles.FindProperty("width"), widthLabel);

            GUIContent heightLabel = new GUIContent("Height");
            EditorGUILayout.PropertyField(serializedTiles.FindProperty("height"), heightLabel);

            GUIContent tilemapLabel = new GUIContent("Tilemap");
            EditorGUILayout.PropertyField(serializedTiles.FindProperty("tileGrid"), tilemapLabel);

            serializedTiles.ApplyModifiedProperties();
        }

        GUILayout.Space(30f);

        //Generating and clearing the world
        GUILayout.Label("Generate a 2D tilemap world", EditorStyles.boldLabel);
        GUILayout.Space(10);
        if (GUILayout.Button("Generate") && worldGen != null)
        {
            Generate();
        }

        GUILayout.Space(10f);

        if (GUILayout.Button("Clear Generation") && worldGen != null)
        {
            Clear();
        }

        GUILayout.Space(10f);
        //Tooltip showing if the world is generated or not
        GUILayout.Label(isActive ? "World Generated!" : "World cleared!", EditorStyles.boldLabel);
    }

    private void Generate()
    {
        isActive = true;

        if (serializedGen.FindProperty("stages").arraySize > 0)
        {
            Debug.Log("Generating...");
            worldGen.Generate();
        }
        else 
        {
            Debug.LogWarning("Please add at least one stage to the list");
            return;
        }
    }

    private void Clear()
    {
        isActive = false;
        Debug.Log("Cleaning...");
        worldGen.Clear();
    }
}
