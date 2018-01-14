using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {
    public float grabRadius = 0.1f;

    private SteamVR_TrackedController controller;
    
    private bool grabbing = false;
    private bool building = false;

    private Laser laserBeam;
    private GameObject grabbedObject;

    // Use this for initialization
    void OnEnable()
    {
        laserBeam = GetComponent<Laser>();
        laserBeam.enabled = false;

        controller = GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += HandleTriggerClicked;
        controller.PadTouched += HandlePadTouched;
        controller.PadUntouched += HandlePadUntouched;
    }

    void OnDisable()
    {
        controller.TriggerClicked -= HandleTriggerClicked;
        controller.PadTouched -= HandlePadTouched;
        controller.PadUntouched -= HandlePadUntouched;
    }
	
	void Update ()
    {
	}

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (building)
        {
            PlaceObject();
        }
        else
        {
            GrabObject();
        }
    }

    private void HandlePadTouched(object sender, ClickedEventArgs e)
    {
        if (grabbing)
        {
            building = true;
            laserBeam.SetVisible(true);
        }
    }

    private void HandlePadUntouched(object sender, ClickedEventArgs e)
    {
        building = false;
        laserBeam.SetVisible(false);
    }

    private void GrabObject()
    {
        RaycastHit[] hits;

        hits = Physics.SphereCastAll(transform.position, grabRadius, transform.forward, 100.0f, 5);

        if (hits.Length > 0)
        {
            int closestHit = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].distance < hits[closestHit].distance)
                {
                    closestHit = i;
                }
            }

            if (hits[closestHit].transform.gameObject.GetComponent<GrabbableObject>().held == false)
            {
                if (grabbedObject != null)
                {
                    Destroy(grabbedObject);
                }

                grabbedObject = Instantiate(hits[closestHit].transform.gameObject);
                grabbedObject.transform.position = transform.position;
                grabbedObject.transform.parent = transform;
                grabbedObject.transform.localPosition = new Vector3(0.0f, -0.0284f, 0.0294f);
                grabbedObject.transform.localRotation = Quaternion.Euler(60.0f, 0.0f, 0.0f);
                grabbedObject.transform.localScale = new Vector3(1f, 1.0f, 1.0f);
                grabbedObject.GetComponent<GrabbableObject>().held = true;

                grabbing = true;
            }
        }
    }

    private void PlaceObject()
    {

    }
}
