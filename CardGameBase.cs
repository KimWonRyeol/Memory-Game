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

            //UI ����ȭ
            gameUIController.UpdateStageText(_stage + 1);
        }
    }

    public int point
    {
        get { return _point; }
        set
        {
            _point = value;

            //UI ����ȭ
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
        //ī��� Instantiate
        cardController.InstantiateCards(cardCount);
        //card �������� �̱�
        cardController.SetRandomCards();
        //Ÿ�̸� ����
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

        //���� ī�� �����
        cardController.DisableCard(firstGuessIndex);
        cardController.DisableCard(secondGuessIndex);

        //���� ī�� ����
        countCorrectGuesses++;
        point += pointPerCorrect;

        //�������� Ŭ�����ߴ��� Ȯ��
        CheckClearStage();
    }

    void MismatchCard()
    {
        AudioManager.Instance.PlaySFX("Mismatch");


        //������ ���ϸ� �ٽ� ������
        cardController.CloseCard(firstGuessIndex);
        cardController.CloseCard(secondGuessIndex);
    }


    //Stage Ŭ���� Ȯ��
    void CheckClearStage()
    {
        if(countCorrectGuesses == cardCount/2)
        {
            AudioManager.Instance.PlaySFX("ClearStage");

            StartCoroutine(ChangeNextStage());
        }
    }

    //���� Stage�� Change
    protected virtual IEnumerator ChangeNextStage()
    {
        //�� ������������ ����� �͵� Clear
        cardController.ClearCards();

        //ī�� Destroy()�� �� OnDestroy�� Frame Update �Ŀ�
        //ȣ��Ǿ� ���� Frame�� ����
        yield return new WaitForEndOfFrame();

        //��� �������� Ŭ�����ߴ��� Ȯ��
        CheckClearGame();
    }

    //��� Stage Ŭ���� Ȯ��
    protected virtual void CheckClearGame() { }
    

    // �̹� ���� ������
    protected void FinishAndSaveGame()
    {
        //���� �ð� ���
        CultureInfo enUS = new CultureInfo("en-US");
        string finishTime = DateTime.Now.ToString("<<MMMM dd | hh:mm>>", enUS);

        string gameMode = GameModeToString(GameManager.Instance.kindGameMode);

        //�̱��� ��ü�� ���� ��� ����
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
