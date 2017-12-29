using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftController : MonoBehaviour {
    private BuildingMenu buildingMenu;

    void OnEnable()
    {
        buildingMenu = FindObjectOfType<BuildingMenu>();
        if (buildingMenu == null)
        {
            Debug.LogError("There is no building menu's on left controller!");
        }
    }

    // TODO: make rotation
}
