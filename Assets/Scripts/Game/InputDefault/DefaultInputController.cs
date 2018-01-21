using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultInputController : InputController
{
    public Text mealCounterText;
    public Text manaCounterText;

    public BuildingSelectionButton[] buildingSelectionButtons;

    void Awake()
    {
        foreach (BuildingSelectionButton selectionButton in buildingSelectionButtons)
        {
            if (selectionButton != null)
            {
                selectionButton.inputController = this;
            }
        }
    }


    // General input

    public override FieldCell GetSelectedCell()
    {
        if (MainCamera == null ||
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return null;
        }

        RaycastHit hit;
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

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


    // UI input

    public override void SetMealAmount(int amount)
    {
        mealCounterText.text = amount.ToString();
    }

    public override void SetManaAmount(int amount)
    {
        manaCounterText.text = amount.ToString();
    }

    public override void SelectBuilding(Building building)
    {
        if (building == null)
        {
            foreach (BuildingSelectionButton selectionButton in buildingSelectionButtons)
            {
                selectionButton.Deselect();
            }
        }

        base.SelectBuilding(building);
    }
}
