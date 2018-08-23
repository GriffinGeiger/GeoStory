using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransformResizingHandleEventTrigger))]
public class TransformResizingHandleEventTriggerEditor : Editor{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
