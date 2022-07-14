using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    AutoRifle,
    Rifle
}

public class ItemBase : NetworkBehaviour
{
    private void Awake()
    {
        if (IsServerOnly)
            GetComponent<Collider>().enabled = false;
    }

    [Client]
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerBase>();
        player.GetItem(0, ItemType.AutoRifle);
    }

}
