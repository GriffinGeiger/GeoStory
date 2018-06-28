using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve4PointRenderer))]
public class BezierCurve4PointRendererEditor : Editor {
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BezierCurve4PointRenderer curve = (BezierCurve4PointRenderer)target;
        Transform startPoint = curve.point1;
        Transform endPoint = curve.point4;
        if(GUILayout.Button("Update location"))
        {
            curve.setEndpoints(startPoint.position, endPoint.position);
        }
    }
}
