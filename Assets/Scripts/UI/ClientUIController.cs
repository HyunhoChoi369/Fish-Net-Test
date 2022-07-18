using Survival.Ingame.Player;
using TMPro;
using UnityEngine;

namespace Survival.Ingame.UI
{
    public class ClientUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text info;

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