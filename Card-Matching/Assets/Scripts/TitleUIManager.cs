using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    public void GoToMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void GoToRanking()
    {
        RankingSystem.record = -1f;
        SceneManager.LoadScene("Ranking");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
