using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival.Ingame.Item
{
    public enum ItemType
    {
        Rifle = 100,
        AutoRifle,
    }

    public class ItemBase : MonoBehaviour
    {
        public ItemType type;
        [HideInInspector] public int index;

        public void Init(int index, bool isServer)
        {
            this.index = index;
            if (isServer)
            {
                var renderers = GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    r.enabled = false;
                }

                GetComponent<Collider>().enabled = false;
            }
        }

        private void PickItem()
        {
            //player.GetItem(0, ItemType.AutoRifle);
        }
    }
}