using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EndSceneController : MonoBehaviour
{
    [SerializeField]
    private EndGameSceneUIController gameSceneUIController;
    [SerializeField]
    private GameObject gameHistoryPrefab;
    [SerializeField]
    private Transform historyContent;

    private int stagePerBonus = 10;
    private int timePerBonus = 3;
    private int stageBonus;
    private int timeBonus;
    private int totalBonus;
    private int currentPoint;

    private GameInfo currentGameInfo;
    private List<GameInfo> gameInfos = new List<GameInfo>();
    private List<Button> historyBtns = new List<Button>();



    // Start is called before the first frame update
    void Start()
    {
        gameInfos = GameManager.Instance.GetGameInfoList();
        //�̹� ���� ���� ��������
        currentGameInfo = gameInfos[gameInfos.Count - 1];

        //Bonus ��� �� ����
        CalculateBonus();
        GameManager.Instance.ApplyBonusLatestgame(totalBonus);
        GameManager.Instance.CheckBestPoint();

        //SaveData�� ����
        GameManager.Instance.SaveGameData();

        //���� ������ UI Event
        StartCoroutine(UIProcess());

        //HistoryPanel
        InstanciateGameHistory();
    }

    //Bonus ���
    void CalculateBonus()
    {
        stageBonus = currentGameInfo.stage * stagePerBonus;
        timeBonus = currentGameInfo.timeLeft * timePerBonus;
        totalBonus = stageBonus + timeBonus;
    }

    //UI Event ������� ����
    IEnumerator UIProcess()
    {
        //0 ~ ī�� ���� ���� Animation
        StartCoroutine(AnimatePoint(0, currentGameInfo.point, 2f));
        yield return new WaitForSeconds(2.5f);

        //ī�� ���� ���� +stageBonus Animation
        //stageBonus Text ����ְ�
        StartCoroutine(AnimatePoint(currentGameInfo.point, currentGameInfo.point + stageBonus, 1f));
        gameSceneUIController.UpdateStageBonusText(currentGameInfo.stage, stagePerBonus, stageBonus);
        yield return new WaitForSeconds(1.5f);

        //ī�� ���� ���� +timeBonus Animation
        //������ Bonus Text ����ְ�
        StartCoroutine(AnimatePoint(currentGameInfo.point, currentGameInfo.point + totalBonus, 1f));
        gameSceneUIController.UpdateTimeBonusText(currentGameInfo.timeLeft, timePerBonus, timeBonus);
        gameSceneUIController.UpdateTotalBonusText(stageBonus, timeBonus, totalBonus);
        yield return new WaitForSeconds(1.5f);

        //�ְ� ���� Text ǥ��
        gameSceneUIController.UpdateBestScoreText(GameManager.Instance.bestPoint);
    }

    //�������� ���ڰ� �ö󰡴� �ִϸ��̼� ����
    IEnumerator AnimatePoint(int startPoint, int endPoint, float duration)
    {
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float rate = currentTime / duration;
            currentPoint = (int)Mathf.Lerp(startPoint, endPoint, rate);

            gameSceneUIController.UpdatePointText(currentPoint);
            yield return null;
        }

        gameSceneUIController.UpdatePointText(endPoint);
    }

    private void InstanciateGameHistory()
    {
        foreach (GameInfo gameInfo in gameInfos)
        {
            GameObject history = Instantiate(gameHistoryPrefab);
            SetHistoryDescription(history, gameInfo);
            history.transform.SetParent(historyContent, false);
        }
    }



    //History �� ���� ����
    void SetHistoryDescription(GameObject history, GameInfo gameInfo)
    {
        TMP_Text historyTime = history.transform.Find("HistoryTime").GetComponent<TMP_Text>();
        historyTime.text = gameInfo.finishTime;

        TMP_Text historyDes = history.transform.Find("HistoryDescription").GetComponent<TMP_Text>();
        historyDes.text = $"Point: {gameInfo.point}";

        TMP_Text historyGameMode = history.transform.Find("HistoryGameMode").GetComponent<TMP_Text>();
        historyGameMode.text = $"[{gameInfo.gameMode}]";

        if (gameInfo.gameMode == "Standard") historyGameMode.color = new Color(0, 125, 0, 255);
        else if (gameInfo.gameMode == "Challenge") historyGameMode.color = new Color(0, 0, 255, 255);
    }
}
