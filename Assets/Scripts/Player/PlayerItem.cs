using FishNet.Connection;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : NetworkBehaviour
{
    private int itemLayer;
    private List<ItemBase> curOverlabItems = new List<ItemBase>();

    private PlayerGun playerGun;

    private void Awake()
    {
        itemLayer = LayerMask.NameToLayer("Item");
        playerGun = GetComponent<PlayerGun>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (curOverlabItems.Count != 0)
            {
                TryPickItem(curOverlabItems[0].index);
            }
        }
    }

    //아마 맵이 정해질 때 아이템 동일하게 인덱스로 정리
    //픽업 시도(index) : 클라 => 서버
    //픽업 승인(bool) : 서버 => 클라
    // 승인 타이밍 작업 (승인시에는 충돌은 신경쓰지말고 이미 사라지진 않았는지만 판단)
    // 1. 서버, 클라에서 아이템제거
    // 1. 서버, 클라에서 데이터 갈아끼움
    //
    [ServerRpc]
    private void TryPickItem(int index)
    {
        if (ItemManager.Instance.SpawnedItemDict.ContainsKey(index))
        {
            PickItem(ItemManager.Instance.SpawnedItemDict[index].type);
            PickItemCompleteGlobal(index, base.OwnerId);
            ItemManager.Instance.SpawnedItemDict[index].gameObject.SetActive(false);
        }

        //나중엔 SL버프처럼 제네릭 하게 바꾸자

    }

    //아이템 먹는거가 2종류로 분리
    //무기같이 다른 플레이어도 시각적으로 알아야하는 경우 글로벌
    //총알같이 다른 플레이어는 굳이 알 필요 없는 경우 타겟
    //아니 근데 아이템 바닥에서 사라지는거 생각하면 결국은 글로벌이네 화난다
    //Item을 syncVal로 처리하고 있어서 일단 보류
    [ObserversRpc]
    private void PickItemCompleteGlobal(int itemIndex, int ownerId)
    {
        //PickItem(ItemManager.Instance.SpawnedItemDict[ItemIndex].type);
        ItemManager.Instance.SpawnedItemDict[itemIndex].gameObject.SetActive(false);
    }

    [TargetRpc]
    private void PickItemCompleteTarget(NetworkConnection connection, int ItemIndex)
    {
        //PickItem(ItemManager.Instance.SpawnedItemDict[ItemIndex].type);
    }

    private void PickItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.AutoRifle:
            case ItemType.Rifle:
                playerGun.curGunData = DataContainer.Instance.GunDataDict[(int)type];
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == itemLayer)
        {
            var item = other.GetComponent<ItemBase>();
            curOverlabItems.Add(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == itemLayer)
        {
            var item = other.GetComponent<ItemBase>();
            curOverlabItems.Remove(item);
        }
    }
}
