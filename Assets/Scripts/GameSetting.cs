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

            //준비 동작 무언가
        }
    }

    private void OnStartButtonClicked()
    {
        //처음엔 자유롭게 놔두고 무적상태
        //스타트 버튼 누르면 배정된 자리로 이동하고 실제 게임 스타트
    }
}
