using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeGraphicEventTrigger : EventTrigger, IBeginDragHandler, IDragHandler, IPointerUpHandler {

    private bool dragging;
    Vector2 pointerPosition;
    Camera cam;
    public float fastScrollSpeed;
    public float scrollSpeed;
    public RectTransform scrollArea;
    public Vector2 prevPointerPosition;
    public new RectTransform transform;

    void Awake()
    {
        transform = GetComponent<RectTransform>();
        cam = Camera.main;
        fastScrollSpeed = .05f;
        scrollSpeed = .02f;
        scrollArea = GetComponentInParent<ScrollRect>().content;
        dragging = false;
    }
    void Update()
    {
        if (dragging)
        {
            Vector3 addedPosition = new Vector3(0,0,0);
            Debug.Log("AddedPosition y " + scrollArea.TransformDirection(addedPosition).y);
            if (pointerPosition.x >= .8f * cam.pixelWidth)
            {
                if (pointerPosition.x >= .9f * cam.pixelWidth)
                    addedPosition += new Vector3(-fastScrollSpeed, 0, 0);
                else
                    addedPosition += new Vector3(-scrollSpeed, 0, 0);
            }
            if (pointerPosition.x <= .2f * cam.pixelWidth)
            {
                if (pointerPosition.x <= .1f * cam.pixelWidth)
                    addedPosition += new Vector3(fastScrollSpeed, 0, 0);
                else
                    addedPosition += new Vector3(scrollSpeed, 0, 0);
            }
            if (pointerPosition.y >= .8f * cam.pixelHeight)
            {
                if (pointerPosition.y >= .9f * cam.pixelHeight)
                    addedPosition += new Vector3(0, -fastScrollSpeed, 0);
                else
                    addedPosition += new Vector3(0, -scrollSpeed, 0);
            }
            if (pointerPosition.y <= .2f * cam.pixelHeight)
            {
                if (pointerPosition.y <= .1f * cam.pixelHeight)
                    addedPosition += new Vector3(0, fastScrollSpeed, 0);
                else
                    addedPosition += new Vector3(0, scrollSpeed, 0);
            }
            scrollArea.position += addedPosition;
            Debug.Log("AddedPosition y " + addedPosition);
            transform.position -=  addedPosition;
            moveLinesWithNodeGraphic(-scrollArea.InverseTransformVector(addedPosition));
        }
    }

    public new void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("Dragging the nodeGraphic");
        dragging = true;
        pointerPosition = data.position;
    }
    public new void OnDrag(PointerEventData data)
    {
        Vector2 deltaPosition =data.position - pointerPosition;
        Vector2 offset = deltaPosition / scrollArea.transform.localScale;
        transform.anchoredPosition += offset;
        moveLinesWithNodeGraphic(offset);
        pointerPosition = data.position;
    }
    public new void OnPointerUp(PointerEventData data)
    {
        dragging = false;
    }

    public void moveLinesWithNodeGraphic(Vector3 offset)
    {
        ManipulateNodeLines[] nodeLines = GetComponentsInChildren<ManipulateNodeLines>();
        foreach (ManipulateNodeLines mnl in nodeLines)
        {
            if(mnl.curve != null)
                mnl.curve.setEndpoints(mnl.curve.point1.anchoredPosition + (Vector2) offset, 0);
        }

        ReceiveNodeLines[] receivedNodeLines = GetComponentsInChildren<ReceiveNodeLines>();

        foreach (ReceiveNodeLines rnl in receivedNodeLines)
        {
            if(rnl.curve != null)
                rnl.curve.setEndpoints(rnl.curve.point4.anchoredPosition + (Vector2) offset,1);
        }
    }
}
