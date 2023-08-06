using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;



public class GameModeSelector : MonoBehaviour
{
    private GameObject currentGameMode;
    public IGameMode gameMode { get; set; }

    private void setKindGameMode(KindGameMode kindGameMode)
    {
        //원래 붙어있던 게임 모드 지우기
        ClearPreGameControllerComponent();

        switch (kindGameMode)
        {
            case KindGameMode.ChallengeMode:
                gameMode = gameObject.AddComponent<ChallengeModeGameController>();
                break;

            case KindGameMode.StageMode:
                gameMode = gameObject.AddComponent<StageModeGameController>();
                break;
        }
    }

    void ClearPreGameControllerComponent()
    {
        Component c = gameObject.GetComponent<IGameMode>() as Component;

        if (c != null)
        {
            Destroy(c);
        }
    }

    void Start()
    {
        setKindGameMode(GameManager.Instance.kindGameMode);
        gameMode.StartGame();
    }


}
