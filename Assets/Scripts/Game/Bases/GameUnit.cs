using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public abstract class GameUnit : NetworkBehaviour
{
    public OpponentId opponentId;

    [SyncVar] public int cost;
    [SyncVar] public int health;    

    private Player owner;
    private MaterialPropertyBlock materialProperties;

    void Awake()
    {
        materialProperties = new MaterialPropertyBlock();

        SetColor(opponentId);
    }

    public virtual void ApplyDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        if (health <= 0)
        {
            OnDeath();            
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
        return owner;
    }

    public virtual void SetColor(OpponentId opponentId)
    {
        materialProperties.SetFloat("_ColorId", opponentId == OpponentId.First ? 0 : 1);

        ChangeMaterialColor(transform, materialProperties);
        foreach (Transform child in transform)
        {
            ChangeMaterialColor(child, materialProperties);
        }
    }

    public virtual void SetHighlighted(bool highlighted)
    {
        materialProperties.SetFloat("_Highlighted", highlighted ? 1 : 0);

        ChangeMaterialColor(transform, materialProperties);
        foreach (Transform child in transform)
        {
            ChangeMaterialColor(child, materialProperties);
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

    [ClientRpc]
    public void RpcSetOwner(GameObject playerObject)
    {
        Player player = playerObject.GetComponent<Player>();

        if (player == null) return;

        owner = player;
        opponentId = player.opponentId;
        SetColor(player.opponentId);
    }
}
