using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManipulateNodeLines : EventTrigger, IBeginDragHandler ,IDragHandler ,IPointerUpHandler {

    GameObject curve;
    public RectTransform scrollArea;
    Camera cam;
    public float scrollSpeed;
    Vector3 pointerPosition;
    public static bool dragging;

    void Awake()
    {
        cam = Camera.main;
        scrollArea = GetComponentInParent<ScrollRect>().content;
        scrollSpeed = .05f;
        dragging = false;
    }
    void Update()
    {
        Debug.Log("Dragging: " + dragging);

        if (dragging)
        {
            if (pointerPosition.x >= .8f * cam.pixelWidth)
            {
                scrollArea.position = scrollArea.position - new Vector3(scrollSpeed, 0, 0);
            }
            if(pointerPosition.x <= .2f * cam.pixelWidth )
            {
                scrollArea.position = scrollArea.position + new Vector3(scrollSpeed, 0, 0);
            }
            if(pointerPosition.y >= .8f * cam.pixelHeight)
            {
                scrollArea.position = scrollArea.position - new Vector3(0, scrollSpeed, 0);
            }
            if (pointerPosition.y <= .2f * cam.pixelHeight)
            {
                scrollArea.position = scrollArea.position + new Vector3(0, scrollSpeed, 0);
            }
        }
    }
    public new void OnBeginDrag(PointerEventData data)
    {
        curve = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/StoryEditor/CurveRenderer.prefab"), this.transform);
        curve.GetComponent<BezierCurve4PointRenderer>().setEndpoints(this.transform.position, this.transform.position);
        dragging = true;
    }
    public new void OnDrag(PointerEventData data)
    {
        curve.GetComponent<BezierCurve4PointRenderer>().setEndpoints(this.transform.position,cam.ScreenToWorldPoint(data.position));
        pointerPosition = data.position;
    }
    public new void OnPointerUp(PointerEventData data)
    {
        dragging = false;
    }
}
