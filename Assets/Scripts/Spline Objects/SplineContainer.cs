using System.Collections.Generic;
using UnityEngine;

public class SplineContainer : MonoBehaviour
{
    [SerializeField] private List<SplineSegment> splineSegments;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSegments(List<BaseSplineSegment> segments)
    {
        if (segments.Count != splineSegments.Count)
        {
            Debug.LogError("AAAAAAAAAAAAA");
            return;
        }

        for (int i = 0; i < splineSegments.Count; i++)
        {
            splineSegments[i].setData(segments[i]);
        }
    }

    public List<SplineSegment> GetSegments()
    {
        return splineSegments;
    }

    public float GetSplineLength()
    {
        float total = 0;
        foreach (var segment in splineSegments)
        {
            total += segment.getBaseSegment().GetLength(32);
        }
        return total;
    }
}
