using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public abstract class GameUnit : NetworkBehaviour
{
    private Player player;

    [SyncVar] public int cost;
    [SyncVar] public int health;

    public virtual void ApplyDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        if (health <= 0)
        {
            OnDeath();

            DetachFromPlayer();
            Destroy(this);
        }
    }

    public virtual void ApplyHeal(int healing)
    {
        ApplyDamage(-healing);
    }

    protected virtual void OnDeath()
    {
    }

    public Player GetOwner()
    {
        return player;
    }

    public void SetOwner(Player player)
    {
        DetachFromPlayer();
        this.player = player;
        AttachToPlayer();
    }

    private void AttachToPlayer()
    {
        if (player != null)
        {
            player.units.Add(this);
        }
    }

    private void DetachFromPlayer()
    {
        if (player != null)
        {
            player.units.Remove(this);
        }
    }
}
