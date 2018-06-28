using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManipulateNodeLines : EventTrigger, IBeginDragHandler ,IDragHandler {

    GameObject curve;
    public new void OnBeginDrag(PointerEventData data)
    {
        curve = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/StoryEditor/CurveRenderer.prefab"), this.transform);
        curve.GetComponent<BezierCurve4PointRenderer>().setEndpoints(this.transform.position, this.transform.position);
    }
    public new void OnDrag(PointerEventData data)
    {
       
        curve.GetComponent<BezierCurve4PointRenderer>().setEndpoints(this.transform.position, this.transform.position);
    }
}
