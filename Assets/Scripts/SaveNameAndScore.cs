using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveNameAndScore : MonoBehaviour
{
    public static SaveNameAndScore Instance;

    public string playerName;
    Text nameField;
    Text bestText;
    Text bestTextInGame;
    MainManager mainManager;
    public string bestPlayerName;
    public int bestLoadedScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //File.Delete(Application.persistentDataPath + "/savefile.json");
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (nameField == null)
            {
                nameField = GameObject.Find("Canvas/PlayerName/Text").GetComponent<Text>();
            }
            playerName = nameField.text;
            if (bestText == null)
            {
                bestText = GameObject.Find("Canvas/BestPlayer").GetComponent<Text>();
            }
            if (LoadBest())
            {
                bestText.text = "BEST SCORE: " + bestPlayerName + " : " + bestLoadedScore;
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (bestTextInGame == null)
            {
                bestTextInGame = GameObject.Find("Canvas/BestScoreText").GetComponent<Text>();
            }
            if (LoadBest())
            {
                bestTextInGame.text = "Best Score: " + bestPlayerName + " : " + bestLoadedScore;
            }
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string bestName;
        public int bestScore;
    }

    public void SaveBest()
    {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        SaveData data = new SaveData();
        data.bestName = playerName;
        data.bestScore = mainManager.m_Points;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public bool LoadBest()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayerName = data.bestName;
            bestLoadedScore = data.bestScore;
            return true;
        }
        return false;
    }
}
