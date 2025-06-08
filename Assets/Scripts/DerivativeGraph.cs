using UnityEngine;
using System.Collections.Generic;

public class DerivativeGraph : MonoBehaviour
{
    [SerializeField] private GraphicsManager2D draw;
    [SerializeField] private SplineConstants projectConstants;

    [SerializeField] private SplineContainer spline;
    [SerializeField] private bool drawOnStart = true;

    [SerializeField] private int steps = 50;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (drawOnStart)
        {
            RepaintSpline();
        }

        //draw.DrawLine(finalList, projectConstants.LightPurple, projectConstants.LightRed);
    }

    public void RepaintSpline()
    {
        draw.Clear();

        // Draw grid
        draw.DrawLine(new Vector2(-4, 0), new Vector2(4, 0), Color.black);
        draw.DrawLine(new Vector2(0, 4), new Vector2(0, -4), Color.black);

        //JANKY AS ALL HELL
        var finalList = new List<Vector2>();
        int k = 0;
        foreach (var segment in spline.GetSegments())
        {
            var dxdy = segment.getBaseSegment().generateDerivativeVectors(steps);
            var scaledList = new List<Vector2>();

            // ignore first point?
            for (int i = (k == 0 ? 0 : 1); i < dxdy.Count; i++)
            {
                scaledList.Add(dxdy[i] * 0.3f);
            }

            finalList.AddRange(scaledList);

            var line = draw.DrawLine(scaledList, projectConstants.LightPurple);
            line.GetComponent<LineRenderer>().sortingOrder = 2;
            k++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
