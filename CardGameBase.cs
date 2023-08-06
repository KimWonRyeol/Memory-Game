using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CardGameBase : MonoBehaviour
{
    //Components
    protected CardController cardController;
    protected SetCardPanel setCardPanel;
    protected GameUIController gameUIController;
    protected Timer timer;

    

    protected int maxStage;
    protected int _stage;
    protected int cardCount;
    protected float gameLimitTime;
    protected int _point;
    protected int pointPerCorrect;
    protected int countCorrectGuesses;
    private bool isFirstGuess, isSecondGuess;
    private int firstGuessIndex, secondGuessIndex;
    private int firstGuessCard, secondGuessCard;

    public int stage
    {
        get { return _stage; }
        set
        {
            _stage = value;

            //UI 동기화
            gameUIController.UpdateStageText(_stage + 1);
        }
    }

    public int point
    {
        get { return _point; }
        set
        {
            _point = value;

            //UI 동기화
            gameUIController.UpdatePointText(_point);
        }
    }

    protected virtual void Awake()
    {
        //Get Components
        cardController = GetComponent<CardController>();
        setCardPanel= GetComponent<SetCardPanel>();
        timer = GetComponent<Timer>();
        gameUIController = GameObject.FindWithTag("UIController").GetComponent<GameUIController>();

        //Set Default
        stage = 0;
        point = 0;
        countCorrectGuesses = 0;
    }

  
    protected void StartGame()
    {
        //카드들 Instantiate
        cardController.InstantiateCards(cardCount);
        //card 랜덤으로 뽑기
        cardController.SetRandomCards();
        //타이머 설정
        timer.StartTimer(gameLimitTime);
    }
   
    public void PickUpCard(Button clickedButton)
    {
        AudioManager.Instance.PlaySFX("PickCard");
    
        string name = clickedButton.gameObject.name;
        
        if(!isFirstGuess)
        {
            isFirstGuess= true;
            firstGuessIndex = int.Parse(name);
            firstGuessCard = cardController.GetCard(firstGuessIndex);
            cardController.OpenCard(firstGuessIndex);
        }
        else if(!isSecondGuess)
        {
            isSecondGuess= true;
            secondGuessIndex= int.Parse(name);
            secondGuessCard = cardController.GetCard(secondGuessIndex);
            cardController.OpenCard(secondGuessIndex);

            StartCoroutine(CheckMatchCard());
        }
    }

    IEnumerator CheckMatchCard()
    {

        yield return new WaitForSeconds(.5f);

        if (firstGuessCard == secondGuessCard)
        {
            MatchCard();
        }
        else
        {
            MismatchCard();
        }


        isFirstGuess = isSecondGuess = false;
    }

    protected virtual void MatchCard()
    {
        AudioManager.Instance.PlaySFX("Match");

        //맞춘 카드 지우기
        cardController.DisableCard(firstGuessIndex);
        cardController.DisableCard(secondGuessIndex);

        //맞춘 카드 개수
        countCorrectGuesses++;
        point += pointPerCorrect;

        //스테이지 클리어했는지 확인
        CheckClearStage();
    }

    void MismatchCard()
    {
        AudioManager.Instance.PlaySFX("Mismatch");


        //맞추지 못하면 다시 뒤집기
        cardController.CloseCard(firstGuessIndex);
        cardController.CloseCard(secondGuessIndex);
    }


    //Stage 클리어 확인
    void CheckClearStage()
    {
        if(countCorrectGuesses == cardCount/2)
        {
            AudioManager.Instance.PlaySFX("ClearStage");

            StartCoroutine(ChangeNextStage());
        }
    }

    //다음 Stage로 Change
    protected virtual IEnumerator ChangeNextStage()
    {
        //전 스테이지에서 사용한 것들 Clear
        cardController.ClearCards();

        //카드 Destroy()할 때 OnDestroy가 Frame Update 후에
        //호출되어 다음 Frame에 실행
        yield return new WaitForEndOfFrame();

        //모든 스테이지 클리어했는지 확인
        CheckClearGame();
    }

    //모든 Stage 클리어 확인
    protected virtual void CheckClearGame() { }
    

    // 이번 게임 마무리
    protected void FinishAndSaveGame()
    {
        //끝낸 시간 기록
        CultureInfo enUS = new CultureInfo("en-US");
        string finishTime = DateTime.Now.ToString("<<MMMM dd | hh:mm>>", enUS);

        string gameMode = GameModeToString(GameManager.Instance.kindGameMode);

        //싱글톤 객체에 게임 기록 저장
        GameManager.Instance.SaveCurretGame(stage, point, Mathf.FloorToInt(timer.timeLeft), finishTime, gameMode);
        SceneManager.LoadScene("EndScene");
    }

    string GameModeToString(KindGameMode gameMode)
    {
        switch (gameMode)
        {
            case KindGameMode.StageMode:
                return "Standard";
            case KindGameMode.ChallengeMode:
                return "Challenge";
            default:
                return "Error";
        }
    }

}
