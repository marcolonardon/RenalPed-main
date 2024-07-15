using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WashScore : MonoBehaviour
{
    private const int MAXSCORE = 1000;

    public Image ScoreTable;
    public GameObject[] Stars;
    public GameObject[] Medals;
    public Text scoreText;
    public Button scoreButton;

    private int totalScore;

    private void Start()
    {
        totalScore = PlayerPrefs.GetInt("WashScore", 0);
        Debug.Log("Loaded Total Score: " + totalScore);
       
        HidePopup();
        
    }
    public void UnlockNextLevel()
    {
        ShowPopup();

        if (PlayerPrefs.GetInt("levelAt") < 1)
            PlayerPrefs.SetInt("levelAt", 1);

        PlayerPrefs.SetInt("MaxIndex", 5);
        PlayerPrefs.SetInt("MinIndex", 0);

        PlayerPrefs.Save();
    }

    private void HidePopup()
    {
        ScoreTable.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        scoreButton.gameObject.SetActive(false);

        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Medals.Length; i++)
        {
            Medals[i].gameObject.SetActive(false);
        }
    }

    public void ShowPopup()
    {
        ScoreTable.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        scoreButton.gameObject.SetActive(true);


        SetStars();
        SetMedals();
        SetScore();

    }

    private void SetStars()
    {
        if (totalScore == MAXSCORE)
        {
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].gameObject.SetActive(true);
            }
        }
        else if (totalScore > MAXSCORE / 2)
        {
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].gameObject.SetActive(true);
                i++;
            }
        }
        else
        {
            for (int i = 0; i < Stars.Length - 2; i++)
            {
                Stars[i].gameObject.SetActive(true);
            }
        }
    }

    private void SetMedals()
    {
        if (totalScore == MAXSCORE)
        {
            Medals[2].gameObject.SetActive(true);
        }
        else if (totalScore > MAXSCORE / 2)
        {
            Medals[1].gameObject.SetActive(true);
        }
        else
        {
            Medals[0].gameObject.SetActive(true);
        }
    }

    private void SetScore()
    {
        Debug.Log("MAXSCORE --> " + MAXSCORE);
        Debug.Log("TOTALCORE --> " + totalScore);
        scoreText.text = totalScore.ToString();
        ScoreManager.Instance.AddWashHandsScore(MAXSCORE, totalScore);
    }

}
