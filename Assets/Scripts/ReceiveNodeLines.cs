using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReceiveNodeLines : EventTrigger, IDropHandler {

    public List<BezierCurve4PointRenderer> curves;
    private BezierCurve4PointRenderer currentCurve;
    public enum ConnectionReceiverType{Page, Element}
    public ConnectionReceiverType connectionReceiverType;
    void Awake()
    {
        if (GetComponentInParent<ElementNodeGraphicManager>() != null) //Page connectors are children of pageGraphic not elementGraphic
            connectionReceiverType = ConnectionReceiverType.Element;
        else
            connectionReceiverType = ConnectionReceiverType.Page;
    }
    public new void OnDrop(PointerEventData data)
    {
        currentCurve = ManipulateNodeLines.lastDraggedCurve.GetComponent<BezierCurve4PointRenderer>();
        Debug.Log("Action: " + currentCurve.action + "Receiver Type" + connectionReceiverType);
        if(currentCurve.action == PageElementEventTrigger.Action.Change)
        {
            if (connectionReceiverType == ConnectionReceiverType.Element)
            {
                Debug.Log("Wrong receiver type, please connect to the page receiver");
                //Give the user some kind of feedback
                currentCurve.breakLink();
            }
            else
                giveConnectionReferences();
        }
        else
        {
            if (connectionReceiverType == ConnectionReceiverType.Page)
            {
                Debug.Log("Wrong Receiver type, please connect to the element receiver");
                //give user feedback
                currentCurve.breakLink();
            }
            else
                giveConnectionReferences();
        }
        
    }

    private void giveConnectionReferences()
    {
        curves.Add(currentCurve);
        currentCurve.receivingConnector = gameObject;
        currentCurve.snapEndpointsToConnectors();
        //When dropped send reference to this page or element to the origin element


        //adds a connection to the associated element and gives reference of it to the originConnector
        GameObject associatedElement = currentCurve.originConnector.GetComponentInParent<ElementNodeGraphicManager>().associatedElement;
        try
        {
            //If there's no connectedElement (this is a page connector) then exception will be thrown sinc engm will be null
            currentCurve.originConnector.GetComponent<ManipulateNodeLines>().connectionKey = associatedElement.GetComponent<PageElementEventTrigger>().
                AddConnection(GetComponentInParent<PageNodeGraphicManager>().page,
                GetComponentInParent<ElementNodeGraphicManager>().associatedElement, currentCurve.action);
        }
        catch (Exception) {
            currentCurve.originConnector.GetComponent<ManipulateNodeLines>().connectionKey = associatedElement.GetComponent<PageElementEventTrigger>().
                AddConnection(GetComponentInParent<PageNodeGraphicManager>().page,
                null, currentCurve.action);
        } //There is no associated element if this is a page connector
    }
}
