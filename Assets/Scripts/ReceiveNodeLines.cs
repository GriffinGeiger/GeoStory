using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReceiveNodeLines : EventTrigger, IDropHandler {

    public List<BezierCurve4PointRenderer> curves;
    public new void OnDrop(PointerEventData data)
    {
        BezierCurve4PointRenderer currentCurve = ManipulateNodeLines.lastDraggedCurve.GetComponent<BezierCurve4PointRenderer>();
        curves.Add(currentCurve);
        currentCurve.receivingConnector = gameObject;
        currentCurve.snapEndpointsToConnectors();
        //When dropped send reference to this page or element to the origin element
        PrefabInfo.PrefabType prefabType = GetComponent<PrefabInfo>().prefabType;

        //adds a connection to the associated element and gives reference of it to the originConnector
        GameObject associatedElement = currentCurve.originConnector.GetComponentInParent<AssociatedElementReference>().associatedElement;
        currentCurve.originConnector.GetComponent<ManipulateNodeLines>().connectionKey =  associatedElement.GetComponent<PageElementEventTrigger>().
            AddConnection(GetComponentInParent<PageNodeGraphicManager>().page,
            associatedElement, currentCurve.action);
    }
}
