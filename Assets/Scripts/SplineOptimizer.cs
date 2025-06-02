using System.Collections.Generic;
using UnityEngine;

public class SplineOptimizer : MonoBehaviour
{
    [SerializeField] private DrawSpline splineDrawer;
    [SerializeField] private SplineContainer splineVisual;

    [SerializeField] private int poolSize = 10;
    [SerializeField] private int numIterations = 10;
    private int iterations = 0;


    public class SplineSegmentRaw
    {
        public Vector2 P0;
        public Vector2 P1;
        public Vector2 P2;
        public Vector2 P3;

        public override string ToString()
        {
            return "(" + P0.ToString() + ", " + P1.ToString() + ", " + P2.ToString() + ", " + P3.ToString() + ")";
        }
    }

    [SerializeField] private List<Vector2> points;
    [SerializeField] private float mutationRange = 3;
    private List<splineOptimizerRepresentation> pool = new List<splineOptimizerRepresentation>();


    private class splineOptimizerRepresentation
    {
        private readonly List<Vector2> points;
        private List<Vector2> pointParms = new List<Vector2>(); // Velocity vector is parm
        private List<SplineSegmentRaw> rawSegments = new List<SplineSegmentRaw>();

        public splineOptimizerRepresentation(List<Vector2> _points, float mutationRange)
        {
            points = _points;

            foreach (var item in points)
            {
                float randVelX = Random.Range(-mutationRange, mutationRange);
                float randVelY = Random.Range(-mutationRange, mutationRange);
                pointParms.Add(new Vector2(randVelX, randVelY));
            }

            for (int i = 1; i < pointParms.Count; i++)
            {
                rawSegments.Add(new SplineSegmentRaw());
            }

            Debug.Log("parms: " + string.Join(", ", pointParms));

            fillRawFromParams();

            Debug.Log("raw: " + string.Join(", ", rawSegments));
        }

        public void setPointParms(List<Vector2> _pointParms)
        {
            pointParms = _pointParms;
        }

        public List<Vector2> getPointParms()
        {
            return pointParms;
        }

        public List<SplineSegmentRaw> getRaw()
        {
            return rawSegments;
        }

        public void RandomizeParms(float mutationRange)
        {
            for (int i = 1; i < pointParms.Count; i++)
            {
                float randVelX = Random.Range(-mutationRange, mutationRange);
                float randVelY = Random.Range(-mutationRange, mutationRange);

                pointParms[i] += new Vector2(randVelX, randVelY);
            }
        }

        public void PrintRaw()
        {
            Debug.Log("raw: " + string.Join(", ", rawSegments));
        }

        public float CalculateFitness()
        {
            fillRawFromParams();

            float total = 0;
            foreach (var segment in rawSegments)
            {
                total += SplineSegment.GetLength(segment.P0, segment.P1, segment.P2, segment.P3, 32);
            }
            return total;
        }

        private void fillRawFromParams()
        {
            for (int i = 1; i < pointParms.Count; i++)
            {
                Vector2 velStart = pointParms[i - 1];
                Vector2 velEnd = pointParms[i];

                Vector2 pointStart = points[i - 1];
                Vector2 pointEnd = points[i];

                rawSegments[i - 1].P0 = pointStart;
                rawSegments[i - 1].P3 = pointEnd;

                rawSegments[i - 1].P1 = pointStart + (velStart / 3.0f);
                rawSegments[i - 1].P2 = pointEnd - (velEnd / 3.0f);
            }
        }
    }

    void Start()
    {
        //Debug.Log("Len " + spline.GetSplineLength());

        for (int i = 0; i < poolSize; i++) {
            pool.Add(new splineOptimizerRepresentation(points, mutationRange));
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (iterations < numIterations)
        {
            // Run genetic optimization
            int bestFitnessIdx = 0;
            float bestFitness = Mathf.Infinity;
            
            for (int i = 0; i < poolSize; i++)
            {
                pool[i].RandomizeParms(mutationRange);
                float fitness = pool[i].CalculateFitness();

                if (fitness < bestFitness)
                {
                    bestFitnessIdx = i;
                    bestFitness = fitness;
                }
            }

            List<Vector2> bestParms = pool[bestFitnessIdx].getPointParms();
            for (int i = 0; i < poolSize; i++)
            {
                pool[i].setPointParms(bestParms);
            }

            Debug.Log("RAAAAA");
            pool[bestFitnessIdx].PrintRaw();
            splineVisual.SetSegments(pool[bestFitnessIdx].getRaw());
            splineDrawer.RepaintSpline();


            Debug.Log("Optimization pass " +  (iterations + 1) + " of " + numIterations + " completed.");
            iterations++;
        }
    }
}
