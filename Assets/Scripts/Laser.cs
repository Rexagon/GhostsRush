using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour {
    public Material laserMaterial;
    public float width = 0.1f;
    
    private LineRenderer lineRenderer;

	// Use this for initialization
	void OnEnable() {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.material = laserMaterial;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;

        Vector3 origin = transform.position - 1.0f * transform.up + 0.8f * transform.forward;
        Vector3 direction = -transform.up + 0.6f * transform.forward;

        if (Physics.Raycast(transform.position, direction, out hit))
        {
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, hit.point + hit.normal);
        }
    }

    public void SetVisible(bool visible)
    {
        lineRenderer.enabled = visible;
        enabled = visible;
    }
}
