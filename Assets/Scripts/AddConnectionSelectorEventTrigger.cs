using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddConnectionSelectorEventTrigger : EventTrigger, IPointerClickHandler
{
    public new void OnPointerClick(PointerEventData data)
    {
        GetComponentInParent<ElementNodeGraphicManager>().addSelectionConnectors(1);
        GetComponentInParent<PageNodeGraphicManager>().drawElementNodes();
    }
}
