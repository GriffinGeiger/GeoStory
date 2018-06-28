using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierCurve4PointRenderer : MonoBehaviour {

    public Transform point1;
    public Transform point2;
    public Transform midPoint; //This is the midpoint between the two curves
    public Transform point3;
    public Transform point4;
    public LineRenderer lineRenderer;
    public int vertexCount =12;
    public float pointExtendRatio = .1f;

	void Update()
    {
        midPoint.transform.position = Vector3.Lerp(point2.position, point3.position, .5f);
        var pointList = new List<Vector3>();
        //First curve
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            Gizmos.color = Color.red;
            var tangentLineVertex1 = Vector3.Lerp(point1.position, point2.position, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point2.position, midPoint.position, ratio);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierPoint);  
        }
        //Second curve
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(midPoint.position, point3.position, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point3.position, point4.position, ratio);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierPoint);
        }
            lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());	
	}
    void OnDrawGizmos()
    {
        
        midPoint.transform.position = Vector3.Lerp(point2.position, point3.position, .5f);

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
    public void setEndpoints(Vector3 start, Vector3 end)
    {
        float deltaX = end.x - start.x;
        point1.position = start;
        point2.position = start + new Vector3(deltaX * pointExtendRatio, 0, 0);
        point3.position = end - new Vector3(deltaX * pointExtendRatio, 0, 0);
        point4.position = end;
    }
}
