using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditingHandleEventTrigger))]
public class TransformResizingHandleEventTriggerEditor : Editor{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
