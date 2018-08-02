using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PageElementEventTrigger))]
public class PageElementEventTriggerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PageElementEventTrigger peet = (PageElementEventTrigger)target;
        GUILayout.Label("Connection size: " + peet.connections.Count);
    }
}


