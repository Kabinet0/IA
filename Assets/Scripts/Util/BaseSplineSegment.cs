using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class BaseSplineSegment {
    private Vector2 P0, P1, P2, P3 = Vector2.zero;

    public void setData(BaseSplineSegment segment)
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

    // Curve sampling functions

    public static Vector2 getPointOnCurve(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float t)
    {
        return (
            (Mathf.Pow((1.0f - t), 3) * P0) +
            (3.0f * t * P1 * (1.0f - t) * (1.0f - t)) +
            (3.0f * (1.0f - t) * t * t * P2) +
            (t * t * t * P3)
        );
    }

    public static Vector2 getFirstDerivative(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float t)
    {
        return (
            (3.0f * (1.0f - t) * (1.0f - t) * (P1 - P0)) +
            (6.0f * (1.0f - t) * t * (P2 - P1)) +
            (3 * t * t * (P3 - P2))
        );
    }

    public static Vector2 getSecondDerivative(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float t)
    {
        return (
            6.0f * (1.0f - t) *
            (P2 - (2 * P1) + P0) +
            6 * t * (P3 - (2 * P2) + P1)
        );
    }

    // Generation functions

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
        }

        return points;
    }

    public List<Vector2> generateDerivativeVectors(int steps, float tValue = 1)
    {
        return generateDerivativeVectors(GetP0(), GetP1(), GetP2(), GetP3(), tValue, steps);
    }

    public static List<Vector2> generateDerivativeVectors(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float tValue, int steps = 32)
    {
        var points = new List<Vector2>();

        for (int i = 0; i < steps; i++)
        {
            float t = ((float)i / (steps - 1)) * tValue;
            points.Add(
                getFirstDerivative(P0, P1, P2, P3, t)
            );
        }

        return points;
    }

    public List<Vector2> generateSecondDerivativeVectors(int steps, float tValue = 1)
    {
        return generateSecondDerivativeVectors(GetP0(), GetP1(), GetP2(), GetP3(), tValue, steps);
    }

    public static List<Vector2> generateSecondDerivativeVectors(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float tValue, int steps = 32)
    {
        var points = new List<Vector2>();

        for (int i = 0; i < steps; i++)
        {
            float t = ((float)i / (steps - 1)) * tValue;
            points.Add(
                getSecondDerivative(P0, P1, P2, P3, t)
            );
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

    public float GetTotalAcceleration(int steps)
    {
        return GetTotalAcceleration(GetP0(), GetP1(), GetP2(), GetP3(), steps);
    }

    public static float GetTotalAcceleration(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, int steps)
    {
        float sum = 0;
        Vector2 currentPoint = getSecondDerivative(P0, P1, P2, P3, 0);

        float dT = 1.0f / steps;
        for (int i = 0; i < steps; i++)
        {
            float t = Mathf.Clamp01((float)(i + 1) / (steps));
            Vector2 nextPoint = getSecondDerivative(P0, P1, P2, P3, t);
            
            sum += ((currentPoint.magnitude + nextPoint.magnitude) / 2.0f) * dT;

            currentPoint = nextPoint;
        }

        return sum;
    }

    public float GetTime(int steps)
    {
        return GetTime(GetP0(), GetP1(), GetP2(), GetP3(), steps);
    }

    public static float GetTime(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, int steps)
    {
        float sum = 0;
        Vector2 currentPoint = P0;
        float currentVelScalar = getFirstDerivative(P0, P1, P2, P3, 0).magnitude; // can use magnitude since it's always tangential

        for (int i = 0; i < steps; i++)
        {
            float t = Mathf.Clamp01((float)(i + 1) / (steps));
            Vector2 nextPoint = getPointOnCurve(P0, P1, P2, P3, t);
            float nextVelScalar = getFirstDerivative(P0, P1, P2, P3, t).magnitude;

            float averageVel = (nextVelScalar + currentVelScalar) / 2;
            sum += Vector2.Distance(currentPoint, nextPoint) / averageVel;

            currentPoint = nextPoint;
            currentVelScalar = nextVelScalar;
        }

        return sum;
    }

    public float GetAverageAccel(int steps)
    {
        return GetAverageAccel(GetP0(), GetP1(), GetP2(), GetP3(), steps);
    }

    public static float GetAverageAccel(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, int steps)
    {
        float sum = 0;

        for (int i = 0; i < steps; i++)
        {
            float t = Mathf.Clamp01((float)(i + 1) / (steps));
            float accel = getSecondDerivative(P0, P1, P2, P3, t).magnitude;
            sum += accel;
        }

        return sum / steps;
    }

    public override string ToString()
    {
        return "(" + P0.ToString() + ", " + P1.ToString() + ", " + P2.ToString() + ", " + P3.ToString() + ")";
    }
}
