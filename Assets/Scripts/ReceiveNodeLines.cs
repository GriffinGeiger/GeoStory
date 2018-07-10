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

        //When dropped send reference to this page or element to the origin element
        string prefabType = GetComponent<PrefabInfo>().prefabType;
        if (prefabType == "NodeConnectorReceiver")
        {
            //Sets the connectedElement in the pageElementEventTrigger of the element where the nodeLine was dragged from to this associated element
            curve.GetComponentInParent<AssociatedElementReference>().associatedElement.GetComponent<PageElementEventTrigger>().connectedElement
                = GetComponentInParent<AssociatedElementReference>().associatedElement;
            Debug.Log("Reference to the next element has been given to pageElementEventTrigger");
        }
        else if (prefabType == "PageNodeConnectorReceiver")
        {
            //Gives reference to the page this receiver represents to the eventTrigger that will change pages
            curve.GetComponentInParent<AssociatedElementReference>().associatedElement.GetComponent<PageElementEventTrigger>().connectedPage
                = GetComponentInParent<PageNodeGraphicManager>().page;
            Debug.Log("NextPage reference has been given to pageElementEventTrigger");
        }
        else
            Debug.Log("PrefabType is expected to be NodeConnectorReceiver or PageNodeConnectorReceiver but got: " + prefabType);
    }
}
