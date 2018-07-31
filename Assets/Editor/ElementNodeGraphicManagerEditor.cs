using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ElementNodeGraphicManager))]
public class ElementNodeGraphicManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ElementNodeGraphicManager engm = (ElementNodeGraphicManager)target;
        if(GUILayout.Button("Add selection connector"))
        {
            engm.addSelectionConnectors(1);
        }
    }
}
