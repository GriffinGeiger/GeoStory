using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransformResizingHandleEventTrigger : EventTrigger, IBeginDragHandler, IDragHandler, IPointerUpHandler{
    [SerializeField]
    public RectTransform rt;
    [SerializeField]
    public bool isMax; //True if this trigger pairs to the max, false if pairs to min
    private RectTransform canvasRect;
    Camera cam;
    void Awake()
    {
        cam = Camera.main;
        canvasRect = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        rt = GetComponentInParent<EditingHandlesManager>().editedRect;
    }
    public new void OnBeginDrag(PointerEventData data)
    {

    }

    public new void OnDrag(PointerEventData data)
    {
        //Vector3 receivingPointInScrollWindowSpace = curve.transform.InverseTransformPoint(cam.ScreenToWorldPoint(pointerPosition + addedPosition));
        //move anchor with drag, dot will move with anchor since pivot is at (1,1)
        if (isMax)
        {
            Vector2 newMax = ((Vector2)data.position) / canvasRect.sizeDelta;
            if(newMax.x<rt.anchorMin.x)
            {
                newMax.x = rt.anchorMin.x;
            }
            if (newMax.y < rt.anchorMin.y)
            {
                newMax.y = rt.anchorMin.y;
            }

            rt.anchorMax = newMax;
        }
        else
        {
            Vector2 newMin = ((Vector2)data.position) / canvasRect.sizeDelta;
            if (newMin.x > rt.anchorMax.x)
                newMin.x = rt.anchorMax.x;
            if (newMin.y > rt.anchorMax.y)
                newMin.y = rt.anchorMax.y;
            rt.anchorMin = newMin;
        }
    }
    public new void OnPointerUp(PointerEventData data)
    {

    }
}
