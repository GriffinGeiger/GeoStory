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
        GetComponentInParent<PageNodeGraphicManager>().drawElementNodes();
        GameObject.Destroy(selectionConnector);
    }
}
