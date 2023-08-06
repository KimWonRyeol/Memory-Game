using System.Collections;
using UnityEngine.UI;

public class ChallengeModeGameController : CardGameBase, IGameMode
{

    private int challengeModeStageCardCount = 18;
    private float cardMatchBonusTime = 1f;
    private float stageClearBonusTime = 5f;
    protected override void Awake()
    {
        base.Awake();

        gameLimitTime = 101f;
        cardCount = challengeModeStageCardCount;
        pointPerCorrect = 5;
    }

    protected override IEnumerator ChangeNextStage()
    {
        yield return StartCoroutine(base.ChangeNextStage());

        stage++;
        cardCount = challengeModeStageCardCount;
        countCorrectGuesses = 0;

        //Challenge Mode는 스테이지 클리어시 보너스 시간 제공
        timer.AddTime(stageClearBonusTime);

        //게임 보드 재구성
        setCardPanel.UpdateCardPanel(3);
        cardController.InstantiateCards(cardCount);
        cardController.SetRandomCards();
    }

    protected override void MatchCard()
    {
        //Challenge Mode는 카드 맞출 시 보너스 시간 제공
        timer.AddTime(cardMatchBonusTime);
        base.MatchCard();
    }


    void IGameMode.StartGame()
    {
        //Challenge Mode는 카드 보드 3단계 크기로 시작
        setCardPanel.UpdateCardPanel(3);
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
