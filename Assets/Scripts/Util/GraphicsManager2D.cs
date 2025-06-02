using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class GraphicsManager2D : MonoBehaviour
{
    [SerializeField] private GameObject LinePrefab;

    [SerializeField] private float lineWidth = 0.1f;

    private List<GameObject> LineSegments = new List<GameObject>();
    private GameObject objectContainer;

    public void Awake()
    {
        objectContainer = new GameObject("DrawnObjects");
        objectContainer.transform.parent = transform;
    }

    public void Clear()
    {
        foreach (var item in LineSegments)
        {
            Destroy(item);
        }
    }

    public GameObject DrawLine(Vector2 positionA, Vector2 positionB, Color color)
    {
        return DrawLine(new List<Vector2> { positionA, positionB }, color, color);
    }

    public GameObject DrawLine(Vector2 positionA, Vector2 positionB, Color color, Color endColor)
    {
        return DrawLine(new List<Vector2>{positionA, positionB}, color, endColor);
    }

    public GameObject DrawLine(List<Vector2> positions, Color color)
    {
        return DrawLine(positions, color, color);
    }

    public GameObject DrawLine(List<Vector2> positions, Color startColor, Color endColor)
    {
        var obj = Instantiate(LinePrefab, objectContainer.transform);
        var lineRenderer = obj.GetComponent<LineRenderer>();
       

        Vector3[] vec3pos = new Vector3[positions.Count];
        int i = 0;
        foreach (var item in positions)
        {
            vec3pos[i] = ((Vector3)item) + transform.position;
            i++;
        }

        // Set position
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(vec3pos);

        // Set width
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // Set color
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.colorGradient.mode = GradientMode.PerceptualBlend;
        LineSegments.Add(obj);

        return obj;
    }

    public GameObject DrawCircle(Vector2 position, Color color)
    {
        return DrawCircle(position, color, 0.3f);
    }

    public GameObject DrawCircle(Vector2 position, Color color, float radius)
    {
        var obj = Instantiate(LinePrefab, objectContainer.transform);
        var lineRenderer = obj.GetComponent<LineRenderer>();

        // Set color
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        // Create circle
        float computedRadius = radius - (lineWidth / 2);

        int segments = 32;
        Vector3[] verticies = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float theta = ((float)i / segments) * Mathf.PI * 2;
            verticies[i] = (new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * computedRadius) + (Vector3)position + transform.position;
        }

        lineRenderer.positionCount = segments;
        lineRenderer.SetPositions(verticies);

        // Set width
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.loop = true;

        LineSegments.Add(obj);


        return obj;
    }
}
