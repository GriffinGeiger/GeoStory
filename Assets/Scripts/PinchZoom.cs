using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float zoomSpeed = .5f;
    public RectTransform t;
    public float maxZoomIn = 4f;
    public float maxZoomOut = .5f;
    private Camera cam;
    private RectTransform scrollContent;
    private Vector2 rawPivotPoint; //The in scrollContent space before converted to percentage of the scrollcontent space
    private Vector2 pivotPoint; // the actual value for the pivot
    private bool pivotSet;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        scrollContent = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (Input.touchCount < 2)
            pivotSet = false;
        float scrollWheelTest = Input.GetAxis("Mouse ScrollWheel");
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            Vector3 centerOfTouches = Vector3.Lerp(Input.GetTouch(0).position, Input.GetTouch(1).position, .5f);
            //Set the pivot point 
            ///////////////////////////////////////////////////////////////////////////////////////////////
            if (!pivotSet)
            {
                rawPivotPoint = scrollContent.InverseTransformPoint(cam.ScreenToWorldPoint(Vector3.Lerp(Input.GetTouch(0).position, Input.GetTouch(1).position, .5f)));
                pivotPoint = (rawPivotPoint - scrollContent.rect.min) / scrollContent.rect.size;

                if (pivotPoint.x > 1)
                    pivotPoint.x = 1;
                else if (pivotPoint.x < 0)
                    pivotPoint.x = 0;
                if (pivotPoint.y > 1)
                    pivotPoint.y = 1;
                else if (pivotPoint.y < 0)
                    pivotPoint.y = 0;
                Vector2 pos = scrollContent.anchoredPosition;
                scrollContent.pivot = pivotPoint;
                scrollContent.anchoredPosition = pos;
                pivotSet = true;
            }
            alignPivotToScreenPoint(centerOfTouches);
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //Scaling
            ///////////////////////////////////////////////////////////////////////////////////////////////


            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            t.localScale += new Vector3(deltaMagnitudeDiff * zoomSpeed, deltaMagnitudeDiff * zoomSpeed, 0);

            //Make it so that theres constraints to how far you can zoom in or out
            if (t.localScale.y < maxZoomOut)
            {
                t.localScale = new Vector3(maxZoomOut, maxZoomOut, 0);
            }
            else if (t.localScale.x > maxZoomIn)
            {
                t.localScale = new Vector3(maxZoomIn, maxZoomIn, 0);
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////
        }
        else if (!Mathf.Approximately(scrollWheelTest,0.0f))
        {
            t.localScale += new Vector3(zoomSpeed * scrollWheelTest, zoomSpeed * scrollWheelTest, 0);
            if(t.localScale.y < maxZoomOut)
            {
                t.localScale = new Vector3(maxZoomOut, maxZoomOut, 0);
            }
            else if(t.localScale.x > maxZoomIn)
            {
                t.localScale = new Vector3(maxZoomIn, maxZoomIn, 0);
            }
        }
        
    }

    private void alignPivotToScreenPoint(Vector2 screenPoint)
    {
        Vector2 screenPointInWorldSpace =cam.ScreenToWorldPoint(screenPoint);
        Vector2 pivotPointInWorldSpace = scrollContent.TransformPoint((scrollContent.pivot*scrollContent.rect.size) + scrollContent.rect.min);
        //move pivot point to screen point
        Vector2 offset = pivotPointInWorldSpace - screenPointInWorldSpace;
        scrollContent.position -= new Vector3(offset.x,offset.y,0);
    }
}
