using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{
    public Dictionary<int, GunData> GunDataDict = new Dictionary<int, GunData>();
    public Dictionary<int, SpawnData> MapItemDict = new Dictionary<int, SpawnData>();

    public void Init()
    {
        SetGunData();
        SetItemSpawnData();
    }

    public void SetGunData()
    {
        var datas = Resources.LoadAll<GunData>("Data");
        foreach (var data in datas)
        {
            GunDataDict[data.Index] = data;
        }
    }

    public void SetItemSpawnData()
    {
        var data = Resources.Load<ItemSpawnData>("Data/ItemSpawnData");

        for (int i = 0; i < data.Items.Count; i++)
        {
            MapItemDict[i] = data.Items[i];
        }
    }
}
