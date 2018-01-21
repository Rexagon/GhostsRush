using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : MonoBehaviour
{
    public Camera MainCamera { get; set; }

    protected Building selectedBuilding;


    // General input

    public abstract FieldCell GetSelectedCell();

    public abstract bool AcceptButtonPressed();

    public abstract bool RejectButtonPressed();


    // UI input

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
