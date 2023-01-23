using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class LR_Collider : MonoBehaviour
{
    EdgeCollider2D edgrCollider;
    LineRenderer ropeRenderer;
    // Start is called before the first frame update
    void Start()
    {
        edgrCollider = this.GetComponent<EdgeCollider2D>();
        ropeRenderer = this.GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        SetEdgeCollider(ropeRenderer);
    }

    void SetEdgeCollider(LineRenderer lineRenderer)
    {
        List<Vector2> edges = new List<Vector2>();

        for(int point = 0; point<lineRenderer.positionCount; point++)
        {
            Vector3 linerendrerpoint = lineRenderer.GetPosition(point);
            edges.Add(new Vector2(linerendrerpoint.x, linerendrerpoint.y));
        }

        edgrCollider.SetPoints(edges);
    }
}
