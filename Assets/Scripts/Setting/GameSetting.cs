using FishNet.Object;
using Survival.Data;
using Survival.Ingame.Item;
using Survival.Ingame.UI;
using UnityEngine;

namespace Survival.Ingame.Setting
{
    public class GameSetting : NetworkBehaviour
    {
        [SerializeField] private ServerUIController serverUI;
        [SerializeField] private ClientUIController clientUI;
        [SerializeField] private Transform uiCanvas;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            DataContainer.Instance.Init();


            if (IsServerOnly)
            {
                //Instantiate(serverUI, uiCanvas);
                serverUI.startButton.onClick.AddListener(OnStartButtonClicked);
                ItemManager.Instance.SpawnItems(true);
            }

            if (IsClientOnly)
            {
                //Instantiate(clientUI, uiCanvas);
                ItemManager.Instance.SpawnItems(false);
            }
        }

        private void OnStartButtonClicked()
        {
            //ó���� �����Ӱ� ���ΰ� ��������
            //��ŸƮ ��ư ������ ������ �ڸ��� �̵��ϰ� ���� ���� ��ŸƮ
        }

    }
}