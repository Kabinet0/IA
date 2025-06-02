using System.Collections.Generic;
using UnityEngine;


public interface ISplineSegment
{
	public Vector2 GetP0();
	public Vector2 GetP1();
	public Vector2 GetP2();
	public Vector2 GetP3();


	public void setData(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3);
	public void setData(ISplineSegment segment);

	public List<Vector2> generatePointsOnSegment(int steps, float tValue = 1);
	public List<Vector2> generateDerrivativeVectors(int steps, float tValue = 1);

	public float GetLength(int steps);

	public string GetAsString();
}