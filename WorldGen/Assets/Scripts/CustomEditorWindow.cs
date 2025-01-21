using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomEditorWindow : EditorWindow
{
    [MenuItem("Window/World Generator")]
    static void OpenWindow()
    {
        CustomEditorWindow window = (CustomEditorWindow)GetWindow(typeof(CustomEditorWindow));
        window.minSize = new Vector2(720, 480);
        window.Show();
    }
}
