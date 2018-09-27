using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManipulateNodeLines : EventTrigger, IBeginDragHandler ,IDragHandler ,IPointerUpHandler {

    public static GameObject lastDraggedCurve;
    public BezierCurve4PointRenderer curve;
    public RectTransform scrollArea;
    Camera cam;
    public float scrollSpeed;
    public float fastScrollSpeed;
    Vector3 pointerPosition;
    public bool dragging;
    public int connectionKey;

    public Transform contentWindow;

    void Awake()
    {
        cam = Camera.main;
        scrollArea = GetComponentInParent<ScrollRect>().content;
        scrollSpeed = .02f;
        fastScrollSpeed = .05f;
        contentWindow = GetComponentInParent<PinchZoom>().t;  //The pinch zoom script holds reference to the rectTransform
        dragging = false;
        connectionKey = -1; //default value so that if no connection is made then 
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

            //The curve's anchoredPositions are in the local space of the scroll content window so these points must be translated to world space then scroll content space
            Vector3 connectionPointInScrollWindowSpace = curve.transform.InverseTransformPoint(transform.TransformPoint(transform.position));
            Vector3 receivingPointInScrollWindowSpace = curve.transform.InverseTransformPoint(cam.ScreenToWorldPoint(pointerPosition + addedPosition));
            curve.setEndpoints(connectionPointInScrollWindowSpace, receivingPointInScrollWindowSpace ); 
        }
    }
    public new void OnBeginDrag(PointerEventData data)
    {
        //clear any references to next page or next element since previous curve is replaced so the link has been broken
        PageElementEventTrigger peet = GetComponentInParent<ElementNodeGraphicManager>().associatedElement.GetComponent<PageElementEventTrigger>();

        //peet.AddConnection(null, null, PageElementEventTrigger.Action.None); //shouldn't need this with current implementation
        if(curve != null)
            curve.breakLink();
        curve = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/StoryEditor/CurveRenderer.prefab")
            , contentWindow).GetComponent<BezierCurve4PointRenderer>();
        
        curve.setAction(GetComponentInParent<SelectionConnectorManager>().getDropdownSelection());

        curve.originConnector = gameObject; //Give reference to this connector's game object to the curve
        lastDraggedCurve = curve.gameObject;
        dragging = true;
    }
    public new void OnDrag(PointerEventData data)
    {
        pointerPosition = data.position;
    }
    public new void OnPointerUp(PointerEventData data)
    {
        dragging = false;
    }
}
