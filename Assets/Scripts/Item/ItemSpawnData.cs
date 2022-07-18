using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Survival.Ingame.Item
{
    [CreateAssetMenu(fileName = "ItemSpawnData", menuName = "Data/ItemSpawnData")]
    public class ItemSpawnData : ScriptableObject
    {
        public List<SpawnData> Items;


    }

    [Serializable]
    public class SpawnData
    {
        public ItemType Type;
        public Vector3 Position;
    }
}