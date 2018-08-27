using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteHandleEventTrigger : EventTrigger, IPointerClickHandler {

    public new void OnPointerClick(PointerEventData data)
    {
        Debug.Log("Deleting Element");
        PageElementEventTrigger peet = GetComponentInParent<PageElementEventTrigger>();
        Page pageRef = peet.pageRef;
        GameObject element = peet.gameObject;

        pageRef.removePageElement(element);
    }
}
