using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PageNodeGraphicManager))]
public class PageNodeGraphicManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PageNodeGraphicManager pngm = (PageNodeGraphicManager)target;

        GUILayout.Label("Rect height/width (" + pngm.GetComponent<RectTransform>().rect.height + "," + pngm.GetComponent<RectTransform>().rect.width + ")");
    }
}
