using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(StoryEditorManager))]
public class StoryEditorManagerEditor : Editor
{
    StoryEditorManager sem;
    private void Awake()
    {
         sem = (StoryEditorManager)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

}
