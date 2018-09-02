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
            //Check if theres already a Change connection since each page element can only have one change
            foreach(ConnectionInfo connection in currentCurve.originConnector.GetComponentInParent<ElementNodeGraphicManager>().
                associatedElement.GetComponent<PageElementEventTrigger>().connections.Values)
            {
                if(connection.action == PageElementEventTrigger.Action.Change)
                {
                    Debug.Log("Already has a change connection, cannot have more than one per page element");
                    currentCurve.breakLink();
                    return;
                }
            }

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

        PageElementEventTrigger originPeet =
            currentCurve.originConnector.GetComponentInParent<ElementNodeGraphicManager>().associatedElement.GetComponent<PageElementEventTrigger>();
        try
        { //If there's no connectedElement (this is a page connector) then exception will be thrown sinc engm will be null
        PageElementEventTrigger thisPeet = GetComponentInParent<ElementNodeGraphicManager>().associatedElement.GetComponent<PageElementEventTrigger>();
            thisPeet.addConnection(originPeet, currentCurve.action); //Event is taken from the original peet and the action is taken from the curve, which got it from the dropdown   
        }
        catch (Exception) {
            PageElementEventTrigger thisPeet =
            thisPeet.addConnection(originPeet, currentCurve.action);
        } //There is no associated element if this is a page connector
    }
}
