using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : MonoBehaviour
{
    protected Camera mainCamera;

    public abstract Transform GetHeadTransform();

    public abstract FieldCell GetSelectedCell();

    public abstract bool AcceptButtonPressed();

    public abstract bool RejectButtonPressed();

    public void SetCamera(Camera camera)
    {
        mainCamera = camera;
    }
}
