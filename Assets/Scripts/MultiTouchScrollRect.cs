using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiTouchScrollRect : ScrollRect, IDragHandler
{
    private bool multitouchDragging;

    public override void OnDrag(PointerEventData eventData)
    {
        if(Input.touchCount ==1)
        {
            base.OnDrag(eventData);
        }
        else if(Input.touchCount> 1)
        {
            eventData.pressPosition = 
                Vector3.Lerp(Input.GetTouch(0).position, Input.GetTouch(1).position, .5f);
            base.OnDrag(eventData);
        }
    }
    
}
