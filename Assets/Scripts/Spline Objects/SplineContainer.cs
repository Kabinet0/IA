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

    public List<SplineSegment> GetSegments()
    {
        return splineSegments;
    }
}
