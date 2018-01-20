using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputController : InputController
{
    public override Transform GetHeadTransform()
    {
        return transform;
    }

    public override FieldCell GetSelectedCell()
    {
        if (mainCamera == null ||
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return null;
        }

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 200.0f, 1 << 9))
        {
            FieldCell cell = hit.transform.gameObject.GetComponent<FieldCell>();
            if (cell)
            {
                return cell;
            }
        }

        return null;
    }

    public override bool AcceptButtonPressed()
    {
        return Input.GetMouseButtonDown(0);
    }

    public override bool RejectButtonPressed()
    {
        return Input.GetMouseButtonDown(1);
    }
}
