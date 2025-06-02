using System.Collections.Generic;
using UnityEngine;

public class SplineOptimizer : MonoBehaviour
{
    [SerializeField] private DrawSpline splineDrawer;
    [SerializeField] private SplineContainer splineVisual;
    [SerializeField] private DerivativeGraph derivativeDrawer;

    [SerializeField] private int poolSize = 10;
    [SerializeField] private int numIterations = 10;
    private int iterations = 0;
    [SerializeField] private int iterationPause = 5;
    private int pauseCounter = 0;


    [SerializeField] private List<Vector2> points;
    [SerializeField] private float mutationRange = 3;
    [SerializeField] private int integrationSamples = 64;
    private List<splineOptimizerRepresentation> pool = new List<splineOptimizerRepresentation>();


    private class splineOptimizerRepresentation
    {
        private readonly List<Vector2> points;
        private List<Vector2> pointParms = new List<Vector2>(); // Velocity vector is parm
        private List<BaseSplineSegment> rawSegments = new List<BaseSplineSegment>();

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
                rawSegments.Add(new BaseSplineSegment());
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

        public List<BaseSplineSegment> getRaw()
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
            string str = "";
            foreach (BaseSplineSegment segment in rawSegments) {
                str += segment + ", ";
            }
            Debug.Log("raw: " + str);
        }

        public float CalculateFitness(int steps)
        {
            fillRawFromParams();

            float total = 0;
            foreach (var segment in rawSegments)
            {
                // // Length based fitness
                //total += segment.GetLength(precision);

                // Second derivative based fitness
                total += segment.GetTotalAcceleration(steps);

                // Time based fitness
                //total += segment.GetTime(steps);
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

                rawSegments[i - 1].setData(pointStart, pointStart + (velStart / 3.0f), pointEnd - (velEnd / 3.0f), pointEnd);
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
            if (pauseCounter >= iterationPause)
            {
                pauseCounter = 0;

                // Run genetic optimization
                int bestFitnessIdx = 0;
                float bestFitness = Mathf.Infinity;
            
                for (int i = 0; i < poolSize; i++)
                {
                    if (i != 0) {
                        pool[i].RandomizeParms(mutationRange);
                    }
                    
                    float fitness = pool[i].CalculateFitness(integrationSamples);

                    if (fitness < bestFitness)
                    {
                        bestFitnessIdx = i;
                        bestFitness = fitness;
                    }
                }

                List<Vector2> bestParms = pool[bestFitnessIdx].getPointParms();
                for (int i = 0; i < poolSize; i++)
                {
                    pool[i].setPointParms(new List<Vector2>(bestParms));
                }

                Debug.Log("BestFitness = " + bestFitness);
                //pool[bestFitnessIdx].PrintRaw();
                splineVisual.SetSegments(pool[bestFitnessIdx].getRaw());
                splineDrawer.RepaintSpline();
                derivativeDrawer.RepaintSpline();


                Debug.Log("Optimization pass " +  (iterations + 1) + " of " + numIterations + " completed.");
                iterations++;
            } else
            {
                pauseCounter++;
            }
        }
    }
}
