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
    }
    public new void OnBeginDrag(PointerEventData data)
    {

    }

    public new void OnDrag(PointerEventData data)
    {
        //Vector3 receivingPointInScrollWindowSpace = curve.transform.InverseTransformPoint(cam.ScreenToWorldPoint(pointerPosition + addedPosition));
        //move anchor with drag, dot will move with anchor since pivot is at (1,1)
        Debug.Log("Dragging");
        if (isMax)
        {
            Debug.Log("data.position:" + data.position +
                "\n worldPoint: " + cam.ScreenToWorldPoint(data.position) +
                "\n canvasRectPoint " + (Vector2)canvasRect.InverseTransformPoint(cam.ScreenToWorldPoint(data.position)) +
                "\n Sizedelta:" + canvasRect.sizeDelta);
            rt.anchorMax = ((Vector2)rt.InverseTransformPoint(cam.ScreenToWorldPoint(data.position))) / canvasRect.sizeDelta;
            Debug.Log("AnchorMax" + rt.anchorMax);
        }
        else
        {
            Debug.Log("Moving anchorMin");
            rt.anchorMin = ((Vector2)rt.InverseTransformPoint(cam.ScreenToWorldPoint(data.position))) / canvasRect.sizeDelta;
        }
    }
    public new void OnPointerUp(PointerEventData data)
    {

    }
}
