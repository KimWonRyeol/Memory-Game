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

        //Challenge Mode�� �������� Ŭ����� ���ʽ� �ð� ����
        timer.AddTime(stageClearBonusTime);

        //���� ���� �籸��
        setCardPanel.UpdateCardPanel(3);
        cardController.InstantiateCards(cardCount);
        cardController.SetRandomCards();
    }

    protected override void MatchCard()
    {
        //Challenge Mode�� ī�� ���� �� ���ʽ� �ð� ����
        timer.AddTime(cardMatchBonusTime);
        base.MatchCard();
    }


    void IGameMode.StartGame()
    {
        //Challenge Mode�� ī�� ���� 3�ܰ� ũ��� ����
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
