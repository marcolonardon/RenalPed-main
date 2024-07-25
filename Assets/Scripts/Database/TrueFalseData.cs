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
    private const int MAXSCORE = 1000;
    private int ADDSCORE;

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
    public static int numOfQuizQuestions;

    private int questionIndex = 0;
    private string questionAnswer;

    private Color originalColor;

    void Start()
    {
        LoadScore();
        HidePopup();
        originalColor = TrueText.color;
        UIAnimationsDisable();
        getTotalQuestions();
        OnGetQuestion();
        ADDSCORE = MAXSCORE / ((numOfQuizQuestions) + (numOfTFQuestions)); // para que a soma máxima dos acertos sempre fique em 1000 - ignora casas decimais 
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
            IncrementScore(); // Atualiza o score
            Debug.Log("Acertou! A resposta é verdadeira");
        }
        else
        {
            SadFace.SetActive(true);
            TrueText.color = Color.red;
            Debug.Log("Errou:( A resposta é falsa");
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
            IncrementScore(); // Atualiza o score
            Debug.Log("Acertou! A resposta é falsa");
        }
        else
        {
            SadFace.SetActive(true);
            FalseText.color = Color.red;
            Debug.Log("Errou. A resposta é verdadeira");
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

                ShowPopup(); // Carregar prêmio
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
        numOfQuizQuestions = PlayerPrefs.GetInt("TotalQuizQuestions");
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
        Debug.Log("Final Score = " + score);

        if (score > 4900)
        {
            SceneManager.LoadScene("Award02");
        }
        else if (score > 2400)
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

        LoadScore();
        SetStars();
        SetMedals();
    }

    private void SetStars()
    {
        Debug.LogWarning("Score nas estrelas > " + totalScore);
        Debug.Log("LastQuizScore -->--> " + PlayerPrefs.GetInt("LastQuizScore", 0));

        if (totalScore == MAXSCORE)
        {
            Stars[0].gameObject.SetActive(true);
            Stars[2].gameObject.SetActive(true);
            Stars[1].gameObject.SetActive(true);
        }
        else if (totalScore > MAXSCORE / 2)
        {
            Stars[0].gameObject.SetActive(true);
            Stars[2].gameObject.SetActive(true);
        }
        else
        {
            Stars[0].gameObject.SetActive(true);
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

    private void IncrementScore()
    {
        // Atualize a pontuação total adicionando ADDSCORE
        totalScore = PlayerPrefs.GetInt("TotalTrueFalseScore", 0) + ADDSCORE;

        // Limite a pontuação total ao máximo permitido
        totalScore = Mathf.Min(totalScore, MAXSCORE);

        // Salve a pontuação atualizada no PlayerPrefs
        PlayerPrefs.SetInt("TotalTrueFalseScore", totalScore);
        PlayerPrefs.Save();
        Debug.Log("Total Score no increment---> " + totalScore);

        // Atualize o ScoreManager
        ScoreManager.Instance.AddQuizScore(MAXSCORE, totalScore);
    }

    private void LoadScore()
    {
        Debug.LogWarning("Entrou no LoadScore");

        // Carregue a pontuação combinada de TotalTrueFalseScore e TotalQuizScore
        totalScore = (PlayerPrefs.GetInt("TotalTrueFalseScore", 0) + PlayerPrefs.GetInt("LastQuizScore", 0));

        Debug.LogWarning("TotalTrueFalse == " + PlayerPrefs.GetInt("TotalTrueFalseScore", 0) + "LastQuizScore == " + PlayerPrefs.GetInt("LastQuizScore", 0));

        scoreText.text = totalScore.ToString();

        // Atualize o ScoreManager
        ScoreManager.Instance.AddQuizScore(MAXSCORE, totalScore);
    }
}
