﻿using System.Collections;
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
        GUILayout.Label("[Key|connectedPageName|connectedElementName] \n" + "----------------------------");
        foreach(KeyValuePair<int,ConnectionInfo> connection in peet.connections)
        {
            GUILayout.Label("[" + connection.Key + " | " + connection.Value.connectedPageName + "|" + connection.Value.connectedElementName + 
                "\n | " + connection.Value.connectedPage.getName() + " | " + connection.Value.connectedElement + " | " + connection.Value.action + "]");
        }
    }
}


