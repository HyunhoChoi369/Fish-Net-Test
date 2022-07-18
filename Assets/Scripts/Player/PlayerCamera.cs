using Cinemachine;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;

namespace Survival.Ingame.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] NetworkManager networkManager;

        CinemachineVirtualCamera camera;

        private void Start()
        {
            networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
        }

        private void OnDestroy()
        {
            networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
        }

        private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
        {
            if (obj.ConnectionState == LocalConnectionState.Started)
            {

            }
        }
    }
}