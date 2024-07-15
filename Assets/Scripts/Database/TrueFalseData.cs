using JetBrains.Annotations;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrueFalseData : MonoBehaviour
{
    private const int MAXSCORE = 1250;
    private const int ADDSCORE = 250;

    public Image ScoreTable;
    public GameObject[] Stars;
    public GameObject[] Medals;
    public Text scoreText;
    public Button scoreButton;

    private int totalScore;

    public GameObject BallonAnimation;
    public GameObject SadFace;
    public GameObject FadeIn, FadeOut;

    public Text questionText;
    public Text TrueText;
    public Text FalseText;

    User user = new User();

    public static string playerQuestion;
    public static string playerAnswer;
    public static int numOfTFQuestions;

    private int questionIndex = 0;
    private string questionAnswer;

    private Color originalColor;

    void Start()
    {
        totalScore = PlayerPrefs.GetInt("QuizScore", 0);
        Debug.Log("Loaded Quiz Total Score>>>>>>>>>>: " + totalScore);

        HidePopup();
        originalColor = TrueText.color;
        UIAnimationsDisable();
        getTotalQuestions();
        OnGetQuestion();
    }

    private void Update()
    {
        // Atualiza o valor do totalScore a partir do PlayerPrefs sempre que for chamado
        totalScore = PlayerPrefs.GetInt("QuizScore", 0);
    }

    public void OnGetQuestion()
    {
        questionText.text = PlayerPrefs.GetString("TrueFalseQuestion" + questionIndex);
    }

    private void UIAnimationsDisable()
    {
        BallonAnimation.SetActive(false);
        SadFace.SetActive(false);
        FadeIn.SetActive(false);
        FadeOut.SetActive(false);
    }

    public void CheckTrueButton()
    {
        questionAnswer = PlayerPrefs.GetString("TrueFalseAnswer" + questionIndex);

        if (questionAnswer == "true")
        {
            BallonAnimation.SetActive(true);
            TrueText.color = Color.green;
            ScoreManager.Instance.AddQuizScore(MAXSCORE, ADDSCORE); // Atualiza o score
            Debug.Log("Acertou! A resposta � verdadeira");
        }
        else
        {
            SadFace.SetActive(true);
            TrueText.color = Color.red;
            Debug.Log("Errou:( A resposta � falsa");
        }

        StartCoroutine(WaitAndGoToNextQuestion());
    }

    public void CheckFalseButton()
    {
        questionAnswer = PlayerPrefs.GetString("TrueFalseAnswer" + questionIndex);

        if (questionAnswer == "false")
        {
            BallonAnimation.SetActive(true);
            FalseText.color = Color.green;
            ScoreManager.Instance.AddQuizScore(MAXSCORE, ADDSCORE); // Atualiza o score
            Debug.Log("Acertou! A resposta � falsa");
        }
        else
        {
            SadFace.SetActive(true);
            FalseText.color = Color.red;
            Debug.Log("Errou. A resposta � verdadeira");
        }

        StartCoroutine(WaitAndGoToNextQuestion());
    }

    public void nextTrueFalseQuestion()
    {
        UIAnimationsDisable();
        FadeIn.SetActive(false);
        FadeOut.SetActive(true);
        ResetAnswerColors();

        if (numOfTFQuestions > questionIndex)
        {
            if (questionIndex == numOfTFQuestions - 1)
            {
                PlayerPrefs.SetInt("MaxIndex", 18);
                PlayerPrefs.SetInt("MinIndex", 13);
                if (PlayerPrefs.GetInt("levelAt") < 5)
                    PlayerPrefs.SetInt("levelAt", 5);

                ShowPopup(); // Carregar pr�mio
            }
            else
            {
                questionIndex++;
            }
        }

        OnGetQuestion();
    }

    private void getTotalQuestions()
    {
        numOfTFQuestions = PlayerPrefs.GetInt("TotalTrueFalseQuestions");
    }

    private IEnumerator WaitAndGoToNextQuestion()
    {
        yield return new WaitForSeconds(2f);
        FadeIn.SetActive(true);
        yield return new WaitForSeconds(.5f);
        nextTrueFalseQuestion();
    }

    public void ResetAnswerColors()
    {
        TrueText.color = originalColor;
        FalseText.color = originalColor;
    }

    public void LoadAwardScene()
    {
        int score = ScoreManager.Instance.GetScore();

        if (score > 10)
        {
            SceneManager.LoadScene("Award02");
        }
        else if (score > 5)
        {
            SceneManager.LoadScene("Award01");
        }
        else
        {
            SceneManager.LoadScene("Award00");
        }
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
        totalScore = PlayerPrefs.GetInt("QuizScore", 0); 
        Debug.Log("MAXSCORE --> " + MAXSCORE);
        Debug.Log("TOTALSCORE --> " + totalScore);
        scoreText.text = totalScore.ToString();
    }
}
