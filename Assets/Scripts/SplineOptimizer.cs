using UnityEngine;

public class SplineOptimizer : MonoBehaviour
{
    [SerializeField] private DrawSpline splineDrawer;
    [SerializeField] private SplineContainer spline;

    [SerializeField] private int numIterations = 10;
    private int iterations = 0;

    void Start()
    {
        Debug.Log("Len " + spline.GetSplineLength());
    }

    // Update is called once per frame
    void Update()
    {
        if (iterations < numIterations)
        {
            // Run genetic optimization


            Debug.Log("Optimization pass " +  iterations + " of " + numIterations + " completed.");
            iterations++;
        }
    }
}
