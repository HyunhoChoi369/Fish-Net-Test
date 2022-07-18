using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Survival.Ingame.Player;

namespace Survival.Ingame.UI
{
    public class ServerUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text info;
        public Button startButton;

        private void Start()
        {
            PlayerManager.Instance.OnCharacterCountChanged += ChangeWaitingPlayer;
        }

        public void ChangeWaitingPlayer(int cnt)
        {
            info.text = $"Waiting Player : {cnt}";
        }
    }
}