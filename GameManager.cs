using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public List<int> gamePoint = new List<int>(); 
    public List<string> finishTime = new List<string>();
    public List<string> gameMode = new List<string>();
    public int bestPoint;
}

public struct GameInfo
{
    public int stage;
    public int point;
    public int timeLeft;
    public string finishTime;
    public string gameMode;

    public GameInfo(int stage, int point, int timeLeft, string finishTime, string gameMode)
    {
        this.stage = stage;
        this.point = point;
        this.timeLeft = timeLeft;
        this.finishTime = finishTime;
        this.gameMode = gameMode;
    }
}

public enum KindGameMode
{
    StageMode,
    ChallengeMode
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    //���ݱ��� �ߴ� ���� ������ ����
    private List<GameInfo> gameInfoList = new List<GameInfo>();
    private int maxGameInfoLimit;

    public KindGameMode kindGameMode { get; set; }
    public int bestPoint { get; set; }
    public int backCardSkin { get; set; }
    public bool isMuted { get; set; }


    public static GameManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        //Default
        backCardSkin = 1;
        maxGameInfoLimit = 10;
        bestPoint= 0;
        isMuted = false;

        // �� ��ȯ�� Destory ����
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //���̺� ������ �޾ƿ���
        SaveData saveData = SaveDataManager.Instance.LoadGameData();
        GameInfo gameInfo = new GameInfo();

        for (int i = 0; i < saveData.gamePoint.Count; i++)
        {
            gameInfo.point= saveData.gamePoint[i];
            gameInfo.finishTime = saveData.finishTime[i];
            gameInfo.gameMode = saveData.gameMode[i];
            gameInfoList.Add(gameInfo);
        }
        bestPoint = saveData.bestPoint; 
    }

    //���� ���� History ����
    public void SaveCurretGame(int stage, int point, int timeLeft, string finishTime, string gameMode)
    {
        GameInfo gameInfo = new(stage, point, timeLeft, finishTime, gameMode);

        //�ֱ� 10���� ���� ������ ����
        if(gameInfoList.Count == maxGameInfoLimit)
        {
            gameInfoList.RemoveAt(0);
        }

        gameInfoList.Add(gameInfo);
    }

    public void ApplyBonusLatestgame(int bonus)
    {
        GameInfo latestGameInfo = gameInfoList[gameInfoList.Count - 1];
        latestGameInfo.point += bonus;
        gameInfoList[gameInfoList.Count - 1] = latestGameInfo;
    }

    public List<GameInfo> GetGameInfoList()
    {
        return gameInfoList;
    }

    public void SaveGameData()
    {
        SaveData saveData = new SaveData();

        foreach(GameInfo gameInfo in gameInfoList)
        {
            saveData.gamePoint.Add(gameInfo.point);
            saveData.finishTime.Add(gameInfo.finishTime);
            saveData.gameMode.Add(gameInfo.gameMode);
        }
        saveData.bestPoint = bestPoint;

        SaveDataManager.Instance.SaveGameData(saveData);
    }

    public void CheckBestPoint()
    {
        int currentGamePoint = gameInfoList[gameInfoList.Count - 1].point;

        if (currentGamePoint > bestPoint)
        {
            bestPoint = currentGamePoint;
        }
    }
}
