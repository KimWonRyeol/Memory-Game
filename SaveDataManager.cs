using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    private static SaveDataManager _instance;

    private string saveDataFileName = "SaveData.json";
    public static SaveDataManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SaveDataManager)) as SaveDataManager;

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
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        // 씬 전환시 Destory 막기
        DontDestroyOnLoad(gameObject);
    }

    // 게임 데이터 세이브 파일에 저장
    public void SaveGameData(SaveData saveData)
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveDataFileName);
        string jsonData = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Game data saved to: " + filePath);
    }

    // 세이브 파일에서 게임 데이터 가져오기
    public SaveData LoadGameData()
    {
        SaveData saveData;
        string filePath = Path.Combine(Application.persistentDataPath, saveDataFileName);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            saveData = JsonUtility.FromJson<SaveData>(jsonData);
            Debug.Log("Game data loaded from: " + filePath);
            return saveData;
        }
        else
        {
            Debug.LogError("Cannot find game data file.");
            return null;
        }
    }
}
