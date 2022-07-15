using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : Singleton<ItemManager>
{
    public Dictionary<int, ItemBase> SpawnedItemDict = new Dictionary<int, ItemBase>();

    public void SpawnItems(bool isServer)
    {
        var datas = DataContainer.Instance.MapItemDict;
        for (int i = 0; i < datas.Count; i++)
        {
            var item = Resources.Load<ItemBase>($"Items/{datas[i].Type}");
            var obj = Instantiate(item, datas[i].Position, Quaternion.identity);
            obj.Init(i, isServer);
            SpawnedItemDict[i] = obj;
        }
    }
}
