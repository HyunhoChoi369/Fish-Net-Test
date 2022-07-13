using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public List<Transform> Characters = new List<Transform>();
    public CinemachineVirtualCamera PlayerCamera;

    private void Awake()
    {
        PlayerCamera = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
    }
}
