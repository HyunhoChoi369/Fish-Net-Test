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

    //�Ƹ� ���� ������ �� ������ �����ϰ� �ε����� ����
    //�Ⱦ� �õ�(index) : Ŭ�� => ����
    //�Ⱦ� ����(bool) : ���� => Ŭ��
    // ���� Ÿ�̹� �۾� (���νÿ��� �浹�� �Ű澲������ �̹� ������� �ʾҴ����� �Ǵ�)
    // 1. ����, Ŭ�󿡼� ����������
    // 1. ����, Ŭ�󿡼� ������ ���Ƴ���
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

        //���߿� SL����ó�� ���׸� �ϰ� �ٲ���

    }

    //������ �Դ°Ű� 2������ �и�
    //���ⰰ�� �ٸ� �÷��̾ �ð������� �˾ƾ��ϴ� ��� �۷ι�
    //�Ѿ˰��� �ٸ� �÷��̾�� ���� �� �ʿ� ���� ��� Ÿ��
    //�ƴ� �ٵ� ������ �ٴڿ��� ������°� �����ϸ� �ᱹ�� �۷ι��̳� ȭ����
    //Item�� syncVal�� ó���ϰ� �־ �ϴ� ����
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
