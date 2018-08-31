using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteHandleEventTrigger : EventTrigger, IPointerClickHandler {

    public new void OnPointerClick(PointerEventData data)
    {
        if (GetComponentInParent<PrefabInfo>() != null) //This is an element
        {
            Debug.Log("Deleting Element");
            PageElementEventTrigger peet = GetComponentInParent<PageElementEventTrigger>();
            Page pageRef = peet.pageRef;
            GameObject element = peet.gameObject;
            pageRef.removePageElement(element);
        }
        else //This is a page
        {
            PageNodeGraphicManager pngm = GetComponentInParent<PageNodeGraphicManager>();
            pngm.page.storyRef.removePage(pngm.page.name);
            GameObject.Destroy(pngm.gameObject);
        }
    }
}
