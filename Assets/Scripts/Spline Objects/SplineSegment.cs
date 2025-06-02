using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class SplineSegment : MonoBehaviour, ISplineSegment
{
    [SerializeField] private Transform P0;
    [SerializeField] private Transform P1;
    [SerializeField] private Transform P2;
    [SerializeField] private Transform P3;

    // Why no multiple inheritance :(
    private BaseSplineSegment baseSegment = new BaseSplineSegment();

    void Awake()
    {
        baseSegment.setData(P0.position, P1.position, P2.position, P3.position);
    }

    public List<Vector2> generateDerrivativeVectors(int steps, float tValue = 1)
    {
        throw new System.NotImplementedException();
    }

    public List<Vector2> generatePointsOnSegment(int steps, float tValue = 1)
    {
        throw new System.NotImplementedException();
    }

    public float GetLength(int steps) { return baseSegment.GetLength(steps); }

    public Vector2 GetP0() { return baseSegment.GetP0(); }

    public Vector2 GetP1() { return baseSegment.GetP1(); }

    public Vector2 GetP2() { return baseSegment.GetP2(); }

    public Vector2 GetP3() { return baseSegment.GetP3(); }

    public void setData(ISplineSegment segment) {
        setData(segment.GetP0(), segment.GetP1(), segment.GetP2(), segment.GetP3());
    }

    public void setData(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3) { 
        baseSegment.setData(P0, P1, P2, P3); 

        this.P0.position = P0;
        this.P1.position = P1;
        this.P2.position = P2;
        this.P3.position = P3;
    }

    public string GetAsString()
    {
        return baseSegment.GetAsString();
    }
}
