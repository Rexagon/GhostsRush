using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera), typeof(AmplifyOcclusion), typeof(FXAA))]
public class OrbitCamera : NetworkBehaviour
{
    public Vector3 origin { get; set; }
    public float distance = 30.0f;
    public float movementSpeed = 50.0f;
    public float movementLimit = 150.0f;
    public Vector2 rotationSpeed = new Vector2(250.0f, 120.0f);
    public Vector2 rotationLimit = new Vector2(1.0f, 80.0f);
    public Vector2 zoomLimit = new Vector2(10.0f, 100.0f);
    public float zoomSpeed = 300.0f;

    private Vector2 angles = new Vector2(0.0f, 0.0f);

    private bool needsUpdate = true;

    void Awake()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100.0f, 1 << 10))
        {
            origin = hit.point;
        }
        else
        {
            origin = new Vector3();
        }

        Vector3 cameraAngles = transform.eulerAngles;
        angles.x = cameraAngles.y;
        angles.y = cameraAngles.x;

        LateUpdate();
    }
   
    void LateUpdate()
    {
        if (!isLocalPlayer) return;

        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        float mouseVerticalAxis = Input.GetAxis("Mouse X");
        float mouseHorizontalAxis = Input.GetAxis("Mouse Y");

        needsUpdate |= scrollAxis != 0.0f || verticalAxis != 0.0f || horizontalAxis != 0.0f ||
            mouseVerticalAxis != 0.0f || mouseHorizontalAxis != 0.0f;

        if (needsUpdate)
        {
            Vector3 velocity = transform.right * horizontalAxis + Vector3.Cross(transform.right, new Vector3(0, 1, 0)) * verticalAxis;
            velocity.Normalize();

            origin += velocity * movementSpeed * Time.deltaTime;
            origin = Vector3.ClampMagnitude(origin, movementLimit);

            if (scrollAxis != 0)
            {
                distance -= scrollAxis * zoomSpeed * Time.deltaTime;
                distance = Mathf.Clamp(distance, zoomLimit.x, zoomLimit.y);
            }

            if (Input.GetMouseButton(2))
            {
                angles.x += mouseVerticalAxis * rotationSpeed.x * Time.deltaTime;
                angles.y -= mouseHorizontalAxis * rotationSpeed.y * Time.deltaTime;

                angles.y = ClampAngle(angles.y, rotationLimit.x, rotationLimit.y);
            }

            Quaternion rotation = Quaternion.Euler(angles.y, angles.x, 0);
            Vector3 position = rotation * (new Vector3(0.0f, 0.0f, -distance)) + origin;

            transform.rotation = rotation;
            transform.position = position;

            needsUpdate = false;
        }
    }
   
    private float ClampAngle(float angle, float min, float max)
    {
        if(angle < -360)
        {
            angle += 360;
        }
        if(angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }
}