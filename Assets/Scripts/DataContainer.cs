using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{
    public Dictionary<int, GunData> GunDataDict = new Dictionary<int, GunData>();

    void Start()
    {
        var datas = Resources.LoadAll<GunData>("Data");
        foreach (var data in datas)
        {
            GunDataDict[data.Index] = data;
        }
    }

}
