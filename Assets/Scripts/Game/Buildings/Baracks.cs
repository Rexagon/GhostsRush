using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baracks : Building
{
    [System.Serializable]
    public class Slot
    {
        [HideInInspector]
        public float currentCooldown = 0;

        public Pawn pawn;
        public float cooldown;

        public bool IsSpawnable
        {
            get
            {
                return currentCooldown <= 0;
            }
        }
    };

    public Slot[] slots = new Slot[2];
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
		foreach (Slot slot in slots)
        {
            if (slot.currentCooldown > 0)
            {
                slot.currentCooldown = Mathf.Max(slot.currentCooldown - Time.deltaTime, 0);
            }
        }
	}
}
