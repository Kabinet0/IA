using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.HableCurve;

public class DrawSpline : MonoBehaviour
{
    [SerializeField] private GraphicsManager2D draw;
    [SerializeField] private SplineConstants projectConstants;

    [SerializeField] private SplineContainer spline;


    void Start()
    {
        foreach (var segment in spline.GetSegments())
        {
            DrawSegment(segment, 1);
            DrawAnnotations(segment);
        }
    }

    private void DrawSegment(SplineSegment segment, float tValue)
    {
        var line = draw.DrawLine(segment.generatePointsOnSegment(32, tValue), Color.black, Color.black);
        line.GetComponent<LineRenderer>().sortingOrder = 2;
    }

    private void DrawAnnotations(SplineSegment segment)
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
