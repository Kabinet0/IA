using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.HableCurve;
using Unity.VisualScripting;

public class DrawSpline : MonoBehaviour
{
    [SerializeField] private GraphicsManager2D draw;
    [SerializeField] private SplineConstants projectConstants;

    [SerializeField] private SplineContainer spline;
    [SerializeField] private bool drawOnStart = true;

    [SerializeField] private int steps = 5;

    void Start()
    {
        if (drawOnStart)
        {
            RepaintSpline();
        }
    }

    public void RepaintSpline()
    {
        draw.Clear();

        foreach (var segment in spline.GetSegments())
        {
            DrawSegment(segment.getBaseSegment(), 1);
            DrawAnnotations(segment.getBaseSegment());

            // Curve length visualization
            //Vector2 currentPoint = segment.getBaseSegment().GetP0();
            //for (int i = 0; i < steps; i++)
            //{
            //    float t = Mathf.Clamp01((float)(i + 1) / (steps));
            //    Vector2 nextPoint = segment.getBaseSegment().getPointOnCurve(t);

            //    var line = draw.DrawLine(currentPoint, nextPoint, projectConstants.Red);
            //    line.GetComponent<LineRenderer>().sortingOrder = 3;

            //    currentPoint = nextPoint;
            //}
        }
    }

    private void DrawSegment(BaseSplineSegment segment, float tValue)
    {
        var line = draw.DrawLine(segment.generatePointsOnSegment(32, tValue), Color.black, Color.black);
        line.GetComponent<LineRenderer>().sortingOrder = 2;
    }

    private void DrawAnnotations(BaseSplineSegment segment)
    {
        draw.DrawLine(segment.GetP0(), segment.GetP1(), projectConstants.LightPurple, projectConstants.LightRed);
        draw.DrawLine(segment.GetP1(), segment.GetP2(), projectConstants.LightRed, projectConstants.LightRed);
        draw.DrawLine(segment.GetP2(), segment.GetP3(), projectConstants.LightRed, projectConstants.LightPurple);

        draw.DrawCircle(segment.GetP0(), projectConstants.Purple);
        draw.DrawCircle(segment.GetP1(), projectConstants.Red);
        draw.DrawCircle(segment.GetP2(), projectConstants.Red);
        draw.DrawCircle(segment.GetP3(), projectConstants.Purple);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
