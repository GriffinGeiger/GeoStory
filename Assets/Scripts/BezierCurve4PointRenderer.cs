using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierCurve4PointRenderer : MonoBehaviour {

    public RectTransform point1;
    public RectTransform point2;
    public RectTransform midPoint; //This is the midpoint between the two curves
    public RectTransform point3;
    public RectTransform point4;
    public LineRenderer lineRenderer;
    public int vertexCount = 12;
    public float pointExtendRatio = .1f;
    public GameObject originConnector;
    public GameObject receivingConnector;
    public PageElementEventTrigger.Action action; //The action this line represents

    void Update()
    {
        midPoint.anchoredPosition = Vector3.Lerp(point2.anchoredPosition, point3.anchoredPosition, .5f);
        var pointList = new List<Vector3>();
        //First curve
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            Gizmos.color = Color.red;
            var tangentLineVertex1 = Vector3.Lerp(point1.anchoredPosition, point2.anchoredPosition, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point2.anchoredPosition, midPoint.anchoredPosition, ratio);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierPoint);  
        }
        //Second curve
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(midPoint.anchoredPosition, point3.anchoredPosition, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point3.anchoredPosition, point4.anchoredPosition, ratio);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierPoint);
        }
            lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());	
	}
    void OnDrawGizmos()
    {
        
        midPoint.position = Vector3.Lerp(point2.position, point3.position, .5f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(point1.position, point2.position);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(point2.position, point3.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(point3.position, point4.position);

        for (float ratio = .5f / vertexCount; ratio < 1; ratio += 1.0f / vertexCount)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.Lerp(point1.position, point2.position, ratio),
                Vector3.Lerp(point2.position, midPoint.position, ratio));
            Gizmos.DrawLine(Vector3.Lerp(midPoint.position, point3.position, ratio),
                Vector3.Lerp(point3.position, point4.position, ratio));

        }
    }

    //Takes the two endpoints and calculates appropriate point 2 and point 3
    public void setEndpoints(Vector2 start, Vector2 end)
    {
        float deltaX = end.x - start.x;
        point1.anchoredPosition = start;
        point2.anchoredPosition = start + new Vector2(deltaX * pointExtendRatio, 0);
        point3.anchoredPosition = end - new Vector2(deltaX * pointExtendRatio, 0);
        point4.anchoredPosition = end;
    }

    public void snapEndpointsToConnectors()
    {
        //Find the position of endpoints in scrollWindowSpace then set the endpoints
        Vector3 originPointInScrollWindowSpace = transform.InverseTransformPoint(originConnector.transform.TransformPoint(originConnector.transform.position));
        Vector3 receivingPointInScrollWindowSpace;
        if (receivingConnector != null)
            receivingPointInScrollWindowSpace = transform.InverseTransformPoint(receivingConnector.transform.TransformPoint(receivingConnector.transform.position));
        else
            receivingPointInScrollWindowSpace = transform.InverseTransformPoint(point4.transform.position);
        setEndpoints(originPointInScrollWindowSpace, receivingPointInScrollWindowSpace);
    }

    public void breakLink()
    {
        int connectionIndex = originConnector.GetComponent<ManipulateNodeLines>().getConnectionIndex();
        Debug.Log("ConnectionIndex " + connectionIndex);
        originConnector.GetComponentInParent<AssociatedElementReference>().associatedElement.GetComponent<PageElementEventTrigger>().removeConnection(connectionIndex);
        try
        {
            receivingConnector.GetComponent<ReceiveNodeLines>().curves.Remove(this);
        }
        catch (Exception) { }
        Debug.Log("Destroying");
        GameObject.Destroy(this.gameObject);
    }
}
