using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingSystem : MonoBehaviour
{
    public static float record = -1f;
    public static string playerName;

    public Transform rankingBlocks;

    [SerializeField]
    struct Ranking
    {
        public List<string> names;
        public List<float> records;
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void ResetRanking()
    {

    }

    private void Awake()
    {
        string path = Path.Combine(Application.persistentDataPath, "Ranking.json");
        Ranking ranking = new Ranking();
        if(!File.Exists(path))
        {
            ranking.names = new List<string>();
            ranking.records = new List<float>();
            string newjson = JsonUtility.ToJson(ranking, true);
            File.WriteAllText(path, newjson);
        }
        string json = File.ReadAllText(path);
        ranking = JsonUtility.FromJson<Ranking>(json);

        int limit = ranking.records.Count;
        int currentPlayerIndex = 0;
        if (record >= 0f)
        {
            currentPlayerIndex = limit;
            for (int i = 0; i < limit; i++)
            {
                if(record < ranking.records[i])
                {
                    currentPlayerIndex = i;
                    break;
                }
            }
            ranking.records.Insert(currentPlayerIndex, record);
            ranking.names.Insert(currentPlayerIndex, playerName);
            limit++;
        }

        for (int i = 0; i < Mathf.Min(limit, 10); i++)
        {
            int j = i;
            rankingBlocks.GetChild(j).GetChild(0).GetComponent<Text>().text = (j + 1).ToString();
            rankingBlocks.GetChild(j).GetChild(1).GetComponent<Text>().text = ranking.names[j];
            rankingBlocks.GetChild(j).GetChild(2).GetComponent<Text>().text = ranking.records[j].ToString("0.00");
        }
        for (int i = limit; i < 10; i++)
        {
            int j = i;
            rankingBlocks.GetChild(j).GetChild(0).GetComponent<Text>().text = "";
            rankingBlocks.GetChild(j).GetChild(1).GetComponent<Text>().text = "";
            rankingBlocks.GetChild(j).GetChild(2).GetComponent<Text>().text = "";
        }
       
        if (record >= 0f)
        {
            rankingBlocks.GetChild(10).GetChild(0).GetComponent<Text>().text = (currentPlayerIndex + 1).ToString();
            rankingBlocks.GetChild(10).GetChild(1).GetComponent<Text>().text = ranking.names[currentPlayerIndex];
            rankingBlocks.GetChild(10).GetChild(2).GetComponent<Text>().text = ranking.records[currentPlayerIndex].ToString("0.00");
            rankingBlocks.GetChild(10).gameObject.SetActive(true);
        }


        json = JsonUtility.ToJson(ranking, true);
        File.WriteAllText(path, json);
    }
}
