using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIController : MonoBehaviour
{
    public BuildingSelectionButton[] buildingSelectionButtons;

    protected Building selectedBuilding;

    void Awake()
    {
        foreach (BuildingSelectionButton selectionButton in buildingSelectionButtons)
        {
            if (selectionButton != null)
            {
                selectionButton.uiController = this;
            }
        }
    }

    public abstract void SetMealAmount(int amount);

    public abstract void SetManaAmount(int amount);

    public virtual void SelectBuilding(Building building)
    {
        selectedBuilding = building;
    }

    public virtual Building GetSelectedBuilding()
    {
        return selectedBuilding;
    }
}
