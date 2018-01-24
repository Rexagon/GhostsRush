using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public ColorId colorId { get; private set; }
    
    public Castle castle;

    private void Awake()
    {
        colorId = castle.colorId;
    }
}
