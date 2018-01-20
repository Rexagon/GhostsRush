using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Field : MonoBehaviour
{
    public static int width = 7;
    public static int height = 9;
    public static float cellSize = 10.0f;
    public float inflate = 0.1f;

    public FieldCell fieldCell;

    public Color color = new Color(0.823f, 0.803f, 0.109f, 0.89f);
    
    private Mesh mesh;
    private Material material;

    void Reset()
    {
        GenerateMesh();
    }

    void Start()
    {
        GenerateCells();
    }

    private void GenerateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "grid";

        int cellCount = width * height + 1;
        int verticesPerQuad = 8;
        int indicesPerQuad = verticesPerQuad * 3;

        Vector3[] vertices = new Vector3[cellCount * verticesPerQuad];
        int[] indices = new int[cellCount * indicesPerQuad];

        float topLeftX = -(width / 2.0f) * cellSize;
        float topLeftY = -(height / 2.0f) * cellSize;

        // temp quad data
        Vector3[] quadVertices;
        int[] quadIndices;

        // Generate all grid
        int quadNumber = 0;
        for (int xi = 0; xi < width; ++xi)
        {
            for (int yi = 0; yi < height; ++yi)
            {
                float x = topLeftX + cellSize * xi;
                float y = topLeftY + cellSize * yi;
                
                GenerateQuad(new Vector3(x, 0, y), new Vector2(cellSize, cellSize), out quadVertices, out quadIndices);

                quadVertices.CopyTo(vertices, quadNumber * verticesPerQuad);
                for (int t = 0; t < indicesPerQuad; ++t)
                {
                    indices[t + quadNumber * indicesPerQuad] = quadNumber * verticesPerQuad + quadIndices[t];
                }

                ++quadNumber;
            }
        }

        // Generate outer quad of whole grid to make outer border 
        // the same width of every inner border
        GenerateQuad(new Vector3(topLeftX - inflate, 0, topLeftY - inflate), new Vector2(cellSize * width, cellSize * height), out quadVertices, out quadIndices);

        quadVertices.CopyTo(vertices, quadNumber * verticesPerQuad);
        for (int t = 0; t < indicesPerQuad; ++t)
        {
            indices[t + quadNumber * indicesPerQuad] = quadNumber * verticesPerQuad + quadIndices[t];
        }

        // Assign generated values to mesh
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();

        // Generate material
        material = new Material(Shader.Find("Transparent/Diffuse"));
        material.color = color;

        GetComponent<MeshRenderer>().material = material;
    }

    private void GenerateQuad(Vector3 position, Vector2 size, out Vector3[] vertices, out int[] indices)
    {
        Vector3[] quadVertices = new Vector3[]
        {
            // outer rectangle
            position,
            position + new Vector3(size.x,  0, 0     ),
            position + new Vector3(size.x,  0, size.y),
            position + new Vector3(0,       0, size.y),

            // inner rectangle
            position + new Vector3(  inflate, 0,  inflate),
            position + new Vector3( -inflate, 0,  inflate) + new Vector3(size.x, 0, 0     ),
            position + new Vector3( -inflate, 0, -inflate) + new Vector3(size.x, 0, size.y),
            position + new Vector3(  inflate, 0, -inflate) + new Vector3(0,      0, size.y),
        };

        int[] quadIndices = new int[]
        {
            // outer rectangle
            1, 0, 4,
            1, 4, 5,
            2, 1, 5,
            2, 5, 6,

            // inner rectangle
            3, 2, 6,
            3, 6, 7,
            0, 3, 7,
            0, 7, 4
        };

        vertices = quadVertices;
        indices = quadIndices;
    }

    private void GenerateCells()
    {
        GameObject[] oldCells = GameObject.FindGameObjectsWithTag("Field Cell");
        foreach (GameObject cell in oldCells)
        {
            DestroyImmediate(cell);
        }

        float topLeftX = -(width / 2.0f - 0.5f) * cellSize;
        float topLeftY = -(height / 2.0f - 0.5f) * cellSize;

        for (int xi = 0; xi < width; ++xi)
        {
            for (int yi = 0; yi < height; ++yi)
            {
                FieldCell cell = Instantiate(fieldCell);
                cell.transform.parent = transform;
                cell.transform.position = new Vector3(topLeftX + xi * cellSize, 0.0f, topLeftY + yi * cellSize);
            }
        }
    }
}
