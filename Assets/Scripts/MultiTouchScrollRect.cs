using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiTouchScrollRect : ScrollRect, IDragHandler
{
    void OnDrawGizmos()
    {
        if (Input.touchCount > 1)
        {
            Gizmos.DrawLine(Input.GetTouch(0).position, Input.GetTouch(1).position);
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {

        if(Input.touchCount ==1)
        {
            base.OnDrag(eventData);
        }
        else if(Input.touchCount> 1)
        {
            Vector3 calculatedPress = Vector3.Lerp(Input.GetTouch(0).position, Input.GetTouch(1).position, .5f);
            eventData.pressPosition = calculatedPress;
            base.OnDrag(eventData);
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("ending drag");
        base.OnEndDrag(eventData);
    }

}
