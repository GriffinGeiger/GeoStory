using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiTouchScrollRect : ScrollRect, IDragHandler
{
    private Camera cam;
    private RectTransform seswTransform;
    private RectTransform contentTransform;
    private Vector2 screenZoomPoint; //In sesw-space
    private Vector2 windowContentZoomPoint_scrollContent; //in scroll content space
    private Vector2 windowContentZoomPoint_sesw;// in sesw-space
    private bool alreadyPinching;

    protected override void Awake()
    {
        cam = FindObjectOfType<Camera>();
        seswTransform = GetComponent<RectTransform>(); //get the StoryEditorScrollWindow transform
        contentTransform = GetComponentsInChildren<RectTransform>()[1];//get the story editor content window transform
        Debug.Log("seswTransform: " + seswTransform + " contentTransform: " + contentTransform);
        base.Awake();
    }

    
    public override void OnDrag(PointerEventData eventData)
    {

        if(Input.touchCount ==1)
        {
            Debug.Log("Press: " + eventData.position);
            base.OnDrag(eventData);
            alreadyPinching = false;
        }
        else if(Input.touchCount> 1)
        {
            //When beginning to drag the point on the content window will be synced to the calculated pinch point (halfway between the fingers). This sets the content point
           // if(!alreadyPinching)
            { 
                Debug.Log("Calculating windowContentZoomPoint");
                windowContentZoomPoint_scrollContent = contentTransform.InverseTransformPoint(cam.ScreenToWorldPoint(Vector3.Lerp(Input.GetTouch(0).position, Input.GetTouch(1).position, .5f))); //calculated point in the scroll Content space
                windowContentZoomPoint_scrollContent -= contentTransform.rect.min; //get point based on a system with (0,0) in the bottom left corner rather than in the center
                Debug.Log("WindowPoint " + windowContentZoomPoint_scrollContent + "size : " + new Vector2(contentTransform.rect.width, contentTransform.rect.height));
                contentTransform.pivot = windowContentZoomPoint_scrollContent / new Vector2(contentTransform.rect.width,contentTransform.rect.width);
                Debug.Log("contentTransform.pivot: " + contentTransform.pivot);
                
            }

          /*  //Every frame during dragging the scroll content will be moved by the offset between screenZoomPoint and windowContentZoomPoint so any scaling that shifts where the window is relative to the caluculated pinch point is corrected
            screenZoomPoint = seswTransform.InverseTransformPoint(cam.ScreenToWorldPoint(Vector3.Lerp(Input.GetTouch(0).position, Input.GetTouch(1).position, .5f))); //In in the StoryEditorScrollWindow-space
            windowContentZoomPoint_sesw = seswTransform.InverseTransformPoint(contentTransform.TransformPoint(windowContentZoomPoint_scrollContent));
            Vector2 dragPointOffset = windowContentZoomPoint_sesw - screenZoomPoint;
            Debug.Log("Offset: " +seswTransform.TransformPoint(dragPointOffset) + " screenZoomPoint: " + screenZoomPoint + " windowContentZoomPoint " + windowContentZoomPoint_sesw);

            dragPointOffset = seswTransform.TransformPoint(dragPointOffset);
            contentTransform.position += (Vector3) dragPointOffset;

            Vector3 calculatedPress = Vector3.Lerp(Input.GetTouch(0).position, Input.GetTouch(1).position, .5f);
            eventData.pressPosition = calculatedPress;

            */

            base.OnDrag(eventData);
            alreadyPinching = true;
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
