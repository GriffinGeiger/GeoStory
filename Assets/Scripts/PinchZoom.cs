using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float zoomSpeed = .5f;
    public RectTransform t;
    public float maxZoomIn = 4f;
    public float maxZoomOut = .5f;
    void Update()
    {
        float scrollWheelTest = Input.GetAxis("Mouse ScrollWheel");
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchZero.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            t.localScale += new Vector3(deltaMagnitudeDiff * zoomSpeed, deltaMagnitudeDiff * zoomSpeed, 0);
            //Make it so that theres constraints to how far you can zoom in or out
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
}
