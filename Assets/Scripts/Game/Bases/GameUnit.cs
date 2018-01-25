using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public abstract class GameUnit : NetworkBehaviour
{
    private Player player;

    [SyncVar]
    public ColorId colorId;
    
    [SyncVar] public int cost;
    [SyncVar] public int health;

    protected virtual void Awake()
    {
        SetColor(colorId);
    }

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

    [ClientRpc]
    public void RpcSetOwner(GameObject playerObject)
    {
        Player player = playerObject.GetComponent<Player>();

        if (player == null) return;

        Debug.Log(player.colorId == ColorId.First);

        DetachFromPlayer();
        this.player = player;
        AttachToPlayer();
        SetColor(player.colorId);
    }

    public virtual void SetColor(ColorId color)
    {
        colorId = color;

        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        propertyBlock.SetFloat("_ColorId", color == ColorId.First ? 0 : 1);

        ChangeMaterialColor(transform, propertyBlock);
        foreach (Transform child in transform)
        {
            ChangeMaterialColor(child, propertyBlock);
        }
    }

    private void ChangeMaterialColor(Transform transform, MaterialPropertyBlock propertyBlock)
    {
        Renderer renderer = transform.gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.SetPropertyBlock(propertyBlock);
        }
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
