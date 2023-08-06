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
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
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
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        // �� ��ȯ�� Destory ����
        DontDestroyOnLoad(gameObject);
    }

    // ���� ������ ���̺� ���Ͽ� ����
    public void SaveGameData(SaveData saveData)
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveDataFileName);
        string jsonData = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Game data saved to: " + filePath);
    }

    // ���̺� ���Ͽ��� ���� ������ ��������
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
