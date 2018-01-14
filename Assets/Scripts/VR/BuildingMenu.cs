using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BuildingMenu : MonoBehaviour {
    public Vector2 menuSideSize;

    public GameUnit[] buildings;

    public Material material;
    private Mesh mesh;

    void OnEnable()
    {
        GenerateMesh();
        SpawnChildren();
    }

    private void GenerateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "menu";

        int sideCount = buildings.Length;

        Vector3[] vertices = new Vector3[sideCount * 6];
        //Vector3[] normals = new Vector3[sideCount * 6];
        int[] triangles = new int[sideCount * 12];

        Vector3 origin = new Vector3(0.0f, 0.0f, 0.0f);

        Quaternion sideRotation = Quaternion.Euler(0.0f, 360.0f / sideCount, 0.0f);
        
        float l = menuSideSize.x / (2.0f * Mathf.Sin(Mathf.PI / (2.0f * sideCount)));
        Vector3 currentEdge = new Vector3(0.0f, 0.0f, l);

        int t = 0;
        int index = 0;
        for (int i = 0; i < buildings.Length; ++i)
        {
            Vector3 nextEdge = sideRotation * currentEdge;
            Vector3 up = new Vector3(0.0f, menuSideSize.y, 0.0f);

            vertices[index + 0] = origin + currentEdge;
            vertices[index + 1] = origin + nextEdge;
            vertices[index + 2] = origin + nextEdge + up;
            vertices[index + 3] = origin + currentEdge + up;
            vertices[index + 4] = origin;
            vertices[index + 5] = origin + up;

            /*
             * Vector3 sideDirection = vertices[index + 0] - vertices[index + 1];
            sideDirection = Vector3.Normalize(Vector3.Cross(sideDirection, up));

            for (int j = 0; j < 4; ++j)
            {
                normals[index + j] = sideDirection;
            }
            normals[index + 4] = Vector3.Normalize(up);
            normals[index + 5] = -normals[index + 4];
            */

            int[] localTriangles = new int[]
            {
                0, 1, 2,
                0, 2, 3,
                0, 4, 1,
                3, 2, 5
            };

            for (int k = 0; k < localTriangles.Length; ++k)
            {
                triangles[t++] = index + localTriangles[k];
            }

            currentEdge = nextEdge;
            index += 6;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.normals = normals;

        GetComponent<MeshRenderer>().material = material;
    }

    private void SpawnChildren()
    {
        //TODO: delete children

        int sideCount = buildings.Length;

        Quaternion sideRotation = Quaternion.Euler(0.0f, 360.0f / sideCount, 0.0f);

        float h = menuSideSize.x / (2.0f * Mathf.Tan(180.0f / sideCount));
        Vector3 currentNormal = new Vector3(h, 0.0f, 0.0f);

        for (int i = 0; i < buildings.Length; ++i)
        {
            currentNormal = sideRotation * currentNormal;

            if (buildings[i] != null)
            {
                GameObject building = Instantiate(buildings[i].gameObject);
                building.transform.parent = transform;
                building.transform.position = currentNormal;
                building.transform.rotation = sideRotation;
            }

            sideRotation *= sideRotation;
        }
    }
}
