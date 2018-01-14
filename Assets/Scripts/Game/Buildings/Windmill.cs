using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : Building {
    public float cooldown;
    public int mealPerTick;

    private float currentCooldown = 0;
    	
	void Update ()
    {
        if (currentCooldown >= cooldown)
        {
            OnTick();
            currentCooldown = Mathf.Repeat(currentCooldown, cooldown);
        }

        currentCooldown += Time.deltaTime;
	}

    private void OnTick()
    {
        Player owner = GetOwner();

        if (owner != null)
        {
            owner.AddResource(ResourceType.Meal, mealPerTick);
            Debug.Log(mealPerTick);
        }
    }
}
