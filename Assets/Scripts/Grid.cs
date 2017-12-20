using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {
    public int width = 7;
    public int length = 9;
    public float cellSize = 1.0f;
    public float inflate = 0.05f;

    private Mesh mesh;

    void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "grid";

        int cellCount = width * length;

        Vector3[] vertices = new Vector3[cellCount * 8];
        int[] triangles = new int[(cellCount + 2) * 8 * 3];

        int t = 0;
        int index = 0;
        for (int xi = 0; xi < width; ++xi)
        {
            for (int yi = 0; yi < length; ++yi)
            {
                float x = cellSize * xi;
                float y = cellSize * yi;

                vertices[index + 0] = new Vector3(x, 0, y);
                vertices[index + 1] = new Vector3(x + cellSize, 0, y);
                vertices[index + 2] = new Vector3(x + cellSize, 0, y + cellSize);
                vertices[index + 3] = new Vector3(x, 0, y + cellSize);

                vertices[index + 4] = new Vector3(x + inflate, 0, y + inflate);
                vertices[index + 5] = new Vector3(x + cellSize - inflate, 0, y + inflate);
                vertices[index + 6] = new Vector3(x + cellSize - inflate, 0, y + cellSize - inflate);
                vertices[index + 7] = new Vector3(x + inflate, 0, y + cellSize - inflate);

                int[] localTriangles = new int[]
                {
                    1, 0, 4,
                    1, 4, 5,
                    2, 1, 5,
                    2, 5, 6,
                    3, 2, 6,
                    3, 6, 7,
                    0, 3, 7,
                    0, 7, 4
                };

                for (int i = 0; i < localTriangles.Length; ++i)
                {
                    triangles[t++] = index + localTriangles[i];
                }

                index += 8;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
