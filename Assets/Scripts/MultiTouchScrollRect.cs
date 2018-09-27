using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiTouchScrollRect : ScrollRect, IDragHandler
{
    private Camera cam;
    private Transform seswTransform;

    protected override void Awake()
    {
        cam = FindObjectOfType<Camera>();
        seswTransform = GetComponentInChildren<RectTransform>(); //get the StoryEditorScrollWindow transform
        base.Awake();
    }

    private Vector2 screenZoomPoint;
    public override void OnDrag(PointerEventData eventData)
    {

        if(Input.touchCount ==1)
        {
            Debug.Log("Press: " + eventData.position);
            base.OnDrag(eventData);

        }
        else if(Input.touchCount> 1)
        {

            //beginning pinching
            screenZoomPoint = seswTransform.InverseTransformPoint(cam.ScreenToWorldPoint(Vector3.Lerp(Input.GetTouch(0).position, Input.GetTouch(1).position, .5f))); //In in the StoryEditorScrollWindow-space


            Vector3 calculatedPress = Vector3.Lerp(Input.GetTouch(0).position, Input.GetTouch(1).position, .5f);
            eventData.pressPosition = calculatedPress;

            Debug.Log("CalculatedPress" + calculatedPress);

            base.OnDrag(eventData);
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("ending drag");
        base.OnEndDrag(eventData);
    }
    
    //pairs the point on the screen where the scaling is happening and the point on the scroll rect
    public void scaleFromPoint(Vector2 point)
    {

    }
}
