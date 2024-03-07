using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    const int pairNumber = 9;
    private List<int> typeList;
    private List<int> cardList;
    public Transform cards;
    public Text alertText;

    private bool isTimeCheck = false;
    private float time = 0.0f;
    public Text recordText;

    public static int currentCard = -1;
    private int currentPair = 0;

    IEnumerator GameStartCount()
    {
        alertText.enabled = true;
        alertText.text = "3";
        yield return new WaitForSeconds(0.5f);
        alertText.text = "2";
        yield return new WaitForSeconds(0.7f);
        alertText.text = "1";
        yield return new WaitForSeconds(0.7f);
        alertText.text = "START!";
        yield return new WaitForSeconds(0.5f);
        alertText.enabled = false;
        GameStart();
    }

    private void OpenCard(int idx)
    {
        GameObject card = cards.GetChild(idx).gameObject;
        card.GetComponent<Button>().enabled = false;
        card.GetComponent<Image>().color = Color.white;
        card.transform.GetChild(0).gameObject.SetActive(true);
        if(currentCard != -1)
        {
            if (cardList[currentCard] == cardList[idx])
            {
                card.GetComponent<Button>().interactable = false;
                cards.GetChild(currentCard).GetComponent<Button>().interactable = false;
                currentPair++;
                if(currentPair == pairNumber)
                {
                    isTimeCheck = false;
                    GameEnd();
                }
                currentCard = -1;
            }
            else
            {
                ControlAllCard(false);
                StartCoroutine(CloseCard(idx));
            }
        }
        else
        {
            currentCard = idx;
        }
    }

    IEnumerator CloseCard(int idx)
    {
        yield return new WaitForSeconds(0.7f);
        cards.GetChild(idx).GetChild(0).gameObject.SetActive(false);
        cards.GetChild(idx).GetComponent<Image>().color = Color.cyan;
        cards.GetChild(currentCard).GetChild(0).gameObject.SetActive(false);
        cards.GetChild(currentCard).GetComponent<Image>().color = Color.cyan;
        currentCard = -1;
        ControlAllCard(true);
    }

    private void GameEnd()
    {
        //typeList.Insert
        RankingSystem.playerName = "CHY";
        RankingSystem.record = time;
        SceneManager.LoadScene("Ranking");
    }

    private void GameStart()
    {
        isTimeCheck = true;
        time = 0.0f;
        currentCard = -1;
        currentPair = 0;
        for (int i = 0; i < pairNumber * 2; i++)
        {
            int j = i;
            cards.GetChild(j).GetComponent<Image>().color = Color.cyan;
            cards.GetChild(j).GetChild(0).gameObject.SetActive(false);
        }
        ControlAllCard(true);
    }

    private void ControlAllCard(bool status)
    {
        for (int i = 0; i < pairNumber * 2; i++)
        {
            int j = i;
            cards.GetChild(j).GetComponent<Button>().enabled = status;
        }
    }



    private void Awake()
    {
        ControlAllCard(false);
        typeList = new List<int>();
        cardList = new List<int>();
        for (int i = 1; i <= pairNumber; i++)
        {
            typeList.Add(i);
            typeList.Add(i);
        }
        for(int i = 0; i < pairNumber * 2; i++)
        {
            int j = i;
            int randomNumber = Random.Range(0,typeList.Count);
            cards.GetChild(j).GetComponent<Button>().onClick.AddListener(delegate { OpenCard(j); });
            cards.GetChild(j).GetComponent<Image>().color = Color.white;
            cards.GetChild(j).GetChild(0).GetComponent<Image>().sprite =
                Resources.Load<Sprite>("Image/" + typeList[randomNumber].ToString());
            cardList.Add(typeList[randomNumber]);
            typeList.RemoveAt(randomNumber);
        }
        StartCoroutine(GameStartCount());
    }

    private void Update()
    {
        if(isTimeCheck)
        {
            time += Time.deltaTime;
            recordText.text = time.ToString("0.00");
        }
    }
}
