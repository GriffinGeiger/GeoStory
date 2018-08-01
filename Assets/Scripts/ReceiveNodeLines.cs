using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReceiveNodeLines : EventTrigger, IDropHandler {

    public List<BezierCurve4PointRenderer> curves;
    public new void OnDrop(PointerEventData data)
    {
        Debug.Log("Dropped");
        BezierCurve4PointRenderer currentCurve = ManipulateNodeLines.lastDraggedCurve.GetComponent<BezierCurve4PointRenderer>();
        curves.Add(currentCurve);
        currentCurve.receivingConnector = gameObject;
        currentCurve.snapEndpointsToConnectors();
        //When dropped send reference to this page or element to the origin element
        PrefabInfo.PrefabType prefabType = GetComponent<PrefabInfo>().prefabType;

        //Sets the connectedElement and page in the pageElementEventTrigger of the element where the nodeLine was dragged from to this associated element at the correct index
        GameObject associatedElement = currentCurve.originConnector.GetComponentInParent<AssociatedElementReference>().associatedElement;
            associatedElement.GetComponent<PageElementEventTrigger>().
            AddConnections(GetComponentInParent<PageNodeGraphicManager>().page,
            associatedElement, currentCurve.action, 
            currentCurve.originConnector.GetComponent<ManipulateNodeLines>().dropdownIndex);

        Debug.Log("Reference to the next element has been given to pageElementEventTrigger");
        
    }
}
