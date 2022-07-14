using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public List<PlayerBase> Characters = new List<PlayerBase>();
    public event Action<int> OnCharacterCountChanged;
    public CinemachineVirtualCamera PlayerCamera;

    private void Awake()
    {
        PlayerCamera = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
    }

    public void AddPlayer(PlayerBase player)
    {
        Characters.Add(player);
        OnCharacterCountChanged?.Invoke(Characters.Count);
    }
}
