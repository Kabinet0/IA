using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class BaseSplineSegment : ISplineSegment {
    private Vector2 P0, P1, P2, P3 = Vector2.zero;

    public void setData(ISplineSegment segment)
    {
        setData(segment.GetP0(), segment.GetP1(), segment.GetP2(), segment.GetP3());
    }

    public void setData(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3)
    {
        this.P0 = P0;
        this.P1 = P1;
        this.P2 = P2;
        this.P3 = P3;
    }

    public Vector2 GetP0()
    {
        return P0;
    }

    public Vector2 GetP1()
    {
        return P1;
    }

    public Vector2 GetP2()
    {
        return P2;
    }

    public Vector2 GetP3()
    {
        return P3;
    }

    public List<Vector2> generatePointsOnSegment(int steps, float tValue = 1)
    {
        return generatePointsOnSegment(GetP0(), GetP1(), GetP2(), GetP3(), tValue, steps);
    }

    public static List<Vector2> generatePointsOnSegment(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float tValue, int steps = 32)
    {
        var points = new List<Vector2>();

        for (int i = 0; i < steps; i++)
        {
            float t = ((float)i / (steps - 1)) * tValue;
            points.Add(
                getPointOnCurve(P0, P1, P2, P3, t)
            );
            //Debug.Log(points[i]);
        }

        return points;
    }

    public static Vector2 getPointOnCurve(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float t)
    {
        return (
            (Mathf.Pow((1.0f - t), 3) * P0) +
            (3.0f * t * P1 * (1.0f - t) * (1.0f - t)) +
            (3.0f * (1.0f - t) * t * t * P2) +
            (t * t * t * P3)
        );
    }

    public List<Vector2> generateDerrivativeVectors(int steps, float tValue = 1)
    {
        return generateDerrivativeVectors(GetP0(), GetP1(), GetP2(), GetP3(), tValue, steps);
    }

    public static List<Vector2> generateDerrivativeVectors(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float tValue, int steps = 32)
    {
        var points = new List<Vector2>();

        for (int i = 0; i < steps; i++)
        {
            float t = ((float)i / (steps - 1)) * tValue;
            points.Add(
                (3.0f * (1.0f - t) * (1.0f - t) * (P1 - P0)) +
                (6.0f * (1.0f - t) * t * (P2 - P1)) +
                (3 * t * t * (P3 - P2))
            );
            //Debug.Log(points[i]);
        }

        return points;
    }

    public float GetLength(int steps)
    {
        return GetLength(GetP0(), GetP1(), GetP2(), GetP3(), steps);
    }

    public static float GetLength(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, int steps)
    {
        float sum = 0;
        Vector2 currentPoint = P0;
        for (int i = 0; i < steps; i++)
        {
            float t = Mathf.Clamp01((float)(i + 1) / (steps));
            Vector2 nextPoint = getPointOnCurve(P0, P1, P2, P3, t);

            sum += Vector2.Distance(currentPoint, nextPoint);

            currentPoint = nextPoint;
        }

        return sum;
    }

    public string GetAsString()
    {
        return "(" + P0.ToString() + ", " + P1.ToString() + ", " + P2.ToString() + ", " + P3.ToString() + ")";
    }
}
