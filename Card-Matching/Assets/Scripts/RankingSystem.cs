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
    private int pivotPage = 0;
    private int maxPage = 0;
    Ranking ranking;

    public Transform rankingBlocks;

    [SerializeField] GameObject controller;
    [SerializeField] Text pageText;

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

    public void PrevPage()
    {
        if (pivotPage > 0 )
        {
            pivotPage--;
            pageText.text = (pivotPage + 1) + " / " + (maxPage + 1);
            UpdateRankPage(pivotPage);
        }
    }

    public void NextPage()
    {
        if (pivotPage < maxPage)
        {
            pivotPage++;
            pageText.text = (pivotPage + 1) + " / " + (maxPage + 1);
            UpdateRankPage(pivotPage);
        }
    }


    private void UpdateRankPage(int pivot)
    {
        int limit = ranking.records.Count;
        for (int i = 0; i < Mathf.Min(limit - (pivot*10), 10); i++)
        {
            int j = i;
            rankingBlocks.GetChild(j).GetChild(0).GetComponent<Text>().text = (j + (pivot * 10) + 1).ToString();
            rankingBlocks.GetChild(j).GetChild(1).GetComponent<Text>().text = ranking.names[j + (pivot * 10)];
            rankingBlocks.GetChild(j).GetChild(2).GetComponent<Text>().text = ranking.records[j + (pivot * 10)].ToString("0.00");
        }
        for (int i = Mathf.Min(limit - (pivot * 10), 10); i < 10; i++)
        {
            int j = i;
            rankingBlocks.GetChild(j).GetChild(0).GetComponent<Text>().text = "";
            rankingBlocks.GetChild(j).GetChild(1).GetComponent<Text>().text = "";
            rankingBlocks.GetChild(j).GetChild(2).GetComponent<Text>().text = "";
        }
    }

    private void Awake()
    {
        string path = Path.Combine(Application.persistentDataPath, "Ranking.json");
        Debug.Log(path);
        ranking = new Ranking();
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
        maxPage = limit / 10;
        
        if (record >= 0f)
        {
            int currentPlayerIndex = limit;
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

            rankingBlocks.GetChild(10).GetChild(0).GetComponent<Text>().text = (currentPlayerIndex + 1).ToString();
            rankingBlocks.GetChild(10).GetChild(1).GetComponent<Text>().text = ranking.names[currentPlayerIndex];
            rankingBlocks.GetChild(10).GetChild(2).GetComponent<Text>().text = ranking.records[currentPlayerIndex].ToString("0.00");
            rankingBlocks.GetChild(10).gameObject.SetActive(true);
            record = -1f;
        }

        else
        {
            pageText.text = "1 / " + (maxPage + 1);
            controller.SetActive(true);

        }

        UpdateRankPage(0);
       
      


        json = JsonUtility.ToJson(ranking, true);
        File.WriteAllText(path, json);
    }
}
