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
        Vector3 connectionPointInScrollWindowSpace = currentCurve.transform.InverseTransformPoint(transform.TransformPoint(transform.position));
        currentCurve.setEndpoints(connectionPointInScrollWindowSpace,1);
        currentCurve.receivingConncector = gameObject;

        //When dropped send reference to this page or element to the origin element
        PrefabInfo.PrefabType prefabType = GetComponent<PrefabInfo>().prefabType;
        if (prefabType == PrefabInfo.PrefabType.NodeConnectorReceiver)
        {
            //Sets the connectedElement in the pageElementEventTrigger of the element where the nodeLine was dragged from to this associated element
            currentCurve.originConnector.GetComponentInParent<AssociatedElementReference>().associatedElement.GetComponent<PageElementEventTrigger>().connectedElement
                = GetComponentInParent<AssociatedElementReference>().associatedElement;
            Debug.Log("Reference to the next element has been given to pageElementEventTrigger");
        }
        else if (prefabType == PrefabInfo.PrefabType.PageNodeConnectorReceiver)
        {
            //Gives reference to the page this receiver represents to the eventTrigger that will change pages
            currentCurve.originConnector.GetComponentInParent<AssociatedElementReference>().associatedElement.GetComponent<PageElementEventTrigger>().connectedPage
                = GetComponentInParent<PageNodeGraphicManager>().page;
            Debug.Log("NextPage reference has been given to pageElementEventTrigger");
        }
    }
}
