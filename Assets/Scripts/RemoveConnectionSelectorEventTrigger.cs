using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveConnectionSelectorEventTrigger : EventTrigger, IPointerClickHandler
{
    public new void OnPointerClick(PointerEventData data)
    {
        ElementNodeGraphicManager engm = GetComponentInParent<ElementNodeGraphicManager>();
        GameObject selectionConnector = GetComponentInParent<SelectionConnectorManager>().gameObject;
        engm.selectionConnectors.Remove(selectionConnector);
        engm.drawSelectionConnectors();
        GetComponentInParent<PageNodeGraphicManager>().drawElementNodes();
        try
        {
            GetComponentInParent<SelectionConnectorManager>().GetComponentInChildren<ManipulateNodeLines>().curve.breakLink();
        }
        catch (NullReferenceException) { }
        //Distribute new element indexes to connections in peet since ones with greater indexes than the one we are deleting will shift down
        GameObject.Destroy(selectionConnector);
    }
}
