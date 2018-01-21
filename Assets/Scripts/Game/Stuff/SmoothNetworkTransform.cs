using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SmoothNetworkTransform : NetworkBehaviour
{
    [Header("Setup")]
    [Range(0, 10)] public int sendRate = 2;
    [Range(0, 2)] public float movementThreshold = 0.2f;
    [Range(0, 30)] public float angleThreshold = 5;
    [Range(0, 10)] public float distanceBeforeSnap = 4;
    [Range(0, 90)] public float angleBeforeSnap = 40;

    [Header("Interpolation")]
    [Range(0, 1)] public float movementInterpolation = 0.1f;
    [Range(0, 1)] public float rotationInterpolation = 0.1f;
    
    public float thresholdMovementPrediction = 0.7f;
    public float thresholdRotationPrediction = 15;
    
    private Vector3 lastDirectionPerFrame = Vector3.zero;
    private Vector3 lastPositionSent = Vector3.zero;
    private Quaternion lastRotationSent = Quaternion.identity;
    private Quaternion lastRotationDirectionPerFrame = Quaternion.identity;
    private bool send = true;
    private bool sending = false;
    private int count = 0;

    void Start()
    {
        lastPositionSent = transform.position;
        lastRotationSent = transform.rotation;
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            SendInfo();
        }
        else
        {
            Recontiliation();
        }
    }

    private void SendInfo()
    {
        if (send)
        {
            if (count == sendRate)
            {
                count = 0;
                send = false;
                Vector3 position = transform.position;
                Quaternion rotation = transform.rotation;
                CmdSendPosition(position, rotation);
            }
            else
            {
                count++;
            }
        }
        else
        {
            CheckIfSend();
        }
    }

    private void CheckIfSend()
    {
        if (sending)
        {
            send = true;
            sending = false;
            return;
        }

        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        float distance = Vector3.Distance(lastPositionSent, position);
        float angle = Quaternion.Angle(lastRotationSent, rotation);
        if (distance > movementThreshold || angle > angleThreshold)
        {
            send = true;
            sending = true;
        }
    }

    private void Recontiliation()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        float distance = Vector3.Distance(lastPositionSent, position);
        float angle = Vector3.Angle(lastRotationSent.eulerAngles, rotation.eulerAngles);
        if (distance > distanceBeforeSnap)
        {
            transform.position = lastPositionSent;
        }
        if (angle > angleBeforeSnap)
        {
            transform.rotation = lastRotationSent;
        }

        position += lastDirectionPerFrame;
        rotation *= lastRotationDirectionPerFrame;

        Vector3 positionLerp = Vector3.Lerp(position, lastPositionSent, movementInterpolation);
        Quaternion rotationLerp = Quaternion.Lerp(rotation, lastRotationSent, rotationInterpolation);
        transform.position = positionLerp;
        transform.rotation = rotationLerp;
    }
    
    [Command(channel = 1)]
    private void CmdSendPosition(Vector3 newPosition, Quaternion newRotation)
    {
        RpcReceivePosition(newPosition, newRotation);
    }

    [ClientRpc(channel = 1)]
    private void RpcReceivePosition(Vector3 newPosition, Quaternion newRotation)
    {
        int frames = (sendRate + 1);
        lastDirectionPerFrame = newPosition - lastPositionSent;

        lastDirectionPerFrame /= frames;
        if (lastDirectionPerFrame.magnitude > thresholdMovementPrediction)
        {
            lastDirectionPerFrame = Vector3.zero;
        }
        Vector3 lastEuler = lastRotationSent.eulerAngles;
        Vector3 newEuler = newRotation.eulerAngles;
        if (Quaternion.Angle(lastRotationDirectionPerFrame, newRotation) < thresholdRotationPrediction)
        {
            lastRotationDirectionPerFrame = Quaternion.Euler((newEuler - lastEuler) / frames);
        }
        else
        {
            lastRotationDirectionPerFrame = Quaternion.identity;
        }
        lastPositionSent = newPosition;
        lastRotationSent = newRotation;
    }
}