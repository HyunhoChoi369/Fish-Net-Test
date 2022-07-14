using FishNet.Object;
using UnityEngine;

public class GameSetting : NetworkBehaviour
{
    [SerializeField] private ServerUIController serverUI;
    [SerializeField] private ClientUIController clientUI;
    [SerializeField] private Transform uiCanvas;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        if (IsServerOnly)
        {
            Instantiate(serverUI, uiCanvas);
            serverUI.startButton.onClick.AddListener(OnStartButtonClicked);
        }

        if (IsClientOnly)
        {
            Instantiate(clientUI, uiCanvas);

            //�غ� ���� ����
        }
    }

    private void OnStartButtonClicked()
    {
        //ó���� �����Ӱ� ���ΰ� ��������
        //��ŸƮ ��ư ������ ������ �ڸ��� �̵��ϰ� ���� ���� ��ŸƮ
    }
}
