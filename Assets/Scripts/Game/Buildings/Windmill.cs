using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Windmill : Building
{
    public float cooldown;
    public int mealPerTick;

    public ParticleSystem mealParticlesSystem;

    private float currentCooldown = 0;

    [Server]
    void Update ()
    {
        if (currentCooldown >= cooldown)
        {
            Player owner = GetOwner();

            if (owner != null)
            {
                owner.resources.AddMeal(mealPerTick);

                RpcPlayParticels();
            }

            currentCooldown = Mathf.Repeat(currentCooldown, cooldown);
        }

        currentCooldown += Time.deltaTime;
	}

    [ClientRpc]
    public void RpcPlayParticels()
    {
        if (mealParticlesSystem != null)
        {
            mealParticlesSystem.Play();
        }
    }
}
