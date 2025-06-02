using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SplineSegment : MonoBehaviour
{
    [SerializeField] private Transform P0;
    [SerializeField] private Transform P1;
    [SerializeField] private Transform P2;
    [SerializeField] private Transform P3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setData(SplineOptimizer.SplineSegmentRaw segment)
    {
        P0.position = segment.P0;
        P1.position = segment.P1;
        P2.position = segment.P2;
        P3.position = segment.P3;
    }

    public Vector2 GetP0() {
        return P0.position;
    }

    public Vector2 GetP1() {
        return P1.position;
    }

    public Vector2 GetP2() {
        return P2.position;
    }

    public Vector2 GetP3() {
        return P3.position;
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
}
