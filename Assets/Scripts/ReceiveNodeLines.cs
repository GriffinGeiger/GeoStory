using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReceiveNodeLines : EventTrigger, IDropHandler {

    BezierCurve4PointRenderer curve;
    public new void OnDrop(PointerEventData data)
    {
        Debug.Log("Dropped");
        curve = ManipulateNodeLines.lastDraggedCurve.GetComponent<BezierCurve4PointRenderer>();
        curve.setEndpoints(curve.point1.transform.position, this.GetComponent<RectTransform>().position);
    }
}
