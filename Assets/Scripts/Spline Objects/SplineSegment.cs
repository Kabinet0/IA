using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class SplineSegment : MonoBehaviour
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

    public BaseSplineSegment getBaseSegment()
    {
        return baseSegment;
    }

    public void setData(BaseSplineSegment segment) {
        setData(segment.GetP0(), segment.GetP1(), segment.GetP2(), segment.GetP3());
    }

    public void setData(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3) { 
        baseSegment.setData(P0, P1, P2, P3); 

        this.P0.position = P0;
        this.P1.position = P1;
        this.P2.position = P2;
        this.P3.position = P3;
    }

    public override string ToString()
    {
        return baseSegment.ToString();
    }
}
