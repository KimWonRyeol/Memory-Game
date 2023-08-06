using System.Collections;
using UnityEngine.UI;

public class StageModeGameController : CardGameBase, IGameMode
{
    //�� stage�� �� ī�� �� 
    private int[] StageModeStageCardCount = { 4, 8, 18, 18, 18 };
    //�� stage�� �ð� ����
    private float[] StageModeStageTimeLimit = { 30f, 30f, 50f, 40f, 30f };
    

    protected override void Awake()
    {
        base.Awake();

        gameLimitTime = StageModeStageTimeLimit[stage];
        maxStage = StageModeStageCardCount.Length - 1;
        cardCount = StageModeStageCardCount[stage];
        pointPerCorrect = stage + 1;
    }


    protected override IEnumerator ChangeNextStage()
    {
        yield return StartCoroutine(base.ChangeNextStage());

        stage++;
        cardCount = StageModeStageCardCount[stage];
        countCorrectGuesses = 0;
        pointPerCorrect= stage + 1;

        timer.StopTimer();
        timer.StartTimer(StageModeStageTimeLimit[stage]);


        //���� ���� �籸��
        setCardPanel.UpdateCardPanel(stage);
        cardController.InstantiateCards(cardCount);
        cardController.SetRandomCards();
    }

    protected override void CheckClearGame()
    {
        if (stage == maxStage)
        {
            FinishAndSaveGame();
        }
    }
    void IGameMode.StartGame()
    {
        StartGame();
    }

    void IGameMode.FinishGame()
    {
        FinishAndSaveGame();
    }
    void IGameMode.PickUpCard(Button btn)
    {
        PickUpCard(btn);
    }


}
