using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManipulateNodeLines : EventTrigger, IBeginDragHandler ,IDragHandler ,IPointerUpHandler {

    public static GameObject lastDraggedCurve;
    public GameObject curve;
    public RectTransform scrollArea;
    Camera cam;
    public float scrollSpeed;
    public float fastScrollSpeed;
    Vector3 pointerPosition;
    public bool dragging;

    void Awake()
    {
        cam = Camera.main;
        scrollArea = GetComponentInParent<ScrollRect>().content;
        scrollSpeed = .02f;
        fastScrollSpeed = .05f;
        dragging = false;
    }
    void Update()
    {

        if (dragging)
        {
            Vector3 addedPosition = new Vector3();
            if (pointerPosition.x >= .8f * cam.pixelWidth)
            {
                if(pointerPosition.x>= .9f * cam.pixelWidth)
                    addedPosition +=  new Vector3(-fastScrollSpeed, 0, 0);
                else
                    addedPosition +=  new Vector3(-scrollSpeed, 0, 0);
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
                    addedPosition += new Vector3(0,-fastScrollSpeed, 0);
                else
                    addedPosition += new Vector3(0, -scrollSpeed, 0);
            }
            if (pointerPosition.y <= .2f * cam.pixelHeight)
            {
                if (pointerPosition.y <= .1f * cam.pixelHeight)
                    addedPosition +=  new Vector3(0, fastScrollSpeed, 0);
                else
                    addedPosition += new Vector3(0, scrollSpeed, 0);
            }
            scrollArea.position += addedPosition;
            curve.GetComponent<BezierCurve4PointRenderer>().setEndpoints(this.transform.position, cam.ScreenToWorldPoint(pointerPosition + addedPosition) );
        }
    }
    public new void OnBeginDrag(PointerEventData data)
    {
        if(curve == null)
            curve = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/StoryEditor/CurveRenderer.prefab"), this.transform);
        curve.GetComponent<BezierCurve4PointRenderer>().setEndpoints(this.transform.position, this.transform.position);
        lastDraggedCurve = curve;
        dragging = true;
    }
    public new void OnDrag(PointerEventData data)
    {
        curve.GetComponent<BezierCurve4PointRenderer>().setEndpoints(this.transform.position,cam.ScreenToWorldPoint(data.position));
        pointerPosition = data.position;
    }
    public new void OnPointerUp(PointerEventData data)
    {
        Debug.Log("pointerUp");
        dragging = false;
    }
}
