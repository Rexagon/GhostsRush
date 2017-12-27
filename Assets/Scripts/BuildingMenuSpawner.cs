using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenuSpawner : MonoBehaviour {
    public BuildingMenu buildingMenuPrefab;

    private BuildingMenu buildingMenu;

	// Use this for initialization
	void Start () {
        buildingMenu = Instantiate(buildingMenuPrefab);
        buildingMenu.transform.parent = transform;
        buildingMenu.transform.position = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
