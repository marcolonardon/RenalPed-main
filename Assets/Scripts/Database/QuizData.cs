using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class QuizData : MonoBehaviour
{
    private const int MAXSCORE = 1250;
    private const int ADDSCORE = 250;

    public GameObject BallonAnimation;
    public GameObject SadFace;
    public GameObject FadeIn, FadeOut;
    public Text questionText;
    public Text AAnswerText;
    public Text BAnswerText;
    public Text CAnswerText;
    public int myQuizIndex = 0;

    User user = new User();

    public static string playerQuestion;
    public static string playerCorrectAnswer;
    public static int numOfQuizQuestions;
    public static string playerAAnswer;
    public static string playerBAnswer;
    public static string playerCAnswer;

    private Color originalColorA;
    private Color originalColorB;
    private Color originalColorC;

    void Start()
    {
        UIAnimationsDisable();
        originalColorA = AAnswerText.color;
        originalColorB = BAnswerText.color;
        originalColorC = CAnswerText.color;

        getTotalQuestions();
        OnGetQuestion();

    }

    private void UIAnimationsDisable()
    {
        BallonAnimation.SetActive(false);
        SadFace.SetActive(false);
        FadeIn.SetActive(false);
        FadeOut.SetActive(false);
    }

    /* public void OnSubmit()
     {
         playerName = nameText.text;
         PostToDatabase();
     }*/

    public void OnGetQuestion()
    {
        RetriveFromDatabase();
    }

    private void UpdateQuestion()
    {
        questionText.text = PlayerPrefs.GetString("QuizQuestion" + myQuizIndex);

    }

    private void UpdateAnswer()
    {
        AAnswerText.text = PlayerPrefs.GetString("QuizAnswerA" + myQuizIndex);
        BAnswerText.text = PlayerPrefs.GetString("QuizAnswerB" + myQuizIndex);
        CAnswerText.text = PlayerPrefs.GetString("QuizAnswerC" + myQuizIndex);
    }

    //private void PostToDatabase()
    //{
    //    User user = new User();
    //    RestClient.Put("https://renal-ped-f3573-default-rtdb.firebaseio.com/" + playerName + ".json", user);
    //}

    private void RetriveFromDatabase()
    {
        playerCorrectAnswer = PlayerPrefs.GetString("QuizCorrectAnswer" + myQuizIndex);
        UpdateQuestion();
        UpdateAnswer();
    }
  

    public void nextQuizQuestion()
    {
        UIAnimationsDisable();
        ResetAnswerColors();
        FadeIn.SetActive(false);
        FadeOut.SetActive(true);
        if (numOfQuizQuestions > myQuizIndex)
        {
            if (myQuizIndex == numOfQuizQuestions - 1)
            {
                //Debug.Log("Reiniciou");
                SceneNavigation.Instance.LoadScene("TrueFalsePage");

            }
            else
            {
                myQuizIndex++;
            }
        }

        OnGetQuestion();

    }

    private void getTotalQuestions()
    {
        numOfQuizQuestions = PlayerPrefs.GetInt("TotalQuizQuestions");
    }

    public void CheckAButton(string op)
    {
        StartCoroutine(WaitAndGoToNextQuestion());
        Debug.Log("Entrou No checkAButton. Resp - " + playerCorrectAnswer);
        Debug.Log("Primeiro get. Resp - " + playerCorrectAnswer);
        if (playerCorrectAnswer == op)
        {
            printCorrect(op);
        }
        else
        {
            printIncorrect(op);
        }
    }

    private void printCorrect(string op)
    {
        BallonAnimation.SetActive(true);
        switch (op.ToLower())
        {
            case "a":
                AAnswerText.color = Color.green;
                BAnswerText.color = Color.red; 
                CAnswerText.color = Color.red; 
                break;
            case "b":
                AAnswerText.color = Color.red; 
                BAnswerText.color = Color.green; 
                CAnswerText.color = Color.red; 
                break;
            case "c":
                AAnswerText.color = Color.red; 
                BAnswerText.color = Color.red; 
                CAnswerText.color = Color.green; 
                break;
            default:
                Debug.LogWarning("Opção inválida: " + op);
                break;
        }

        ScoreManager.Instance.AddQuizScore(MAXSCORE, ADDSCORE); ///////////////////////////////////////////////////////////////////////
    }

    private void printIncorrect(string op)
    {
        SadFace.SetActive(true);
        switch (playerCorrectAnswer.ToLower())
        {
            case "a":
                AAnswerText.color = Color.green;
                BAnswerText.color = Color.red;
                CAnswerText.color = Color.red;
                break;
            case "b":
                AAnswerText.color = Color.red;
                BAnswerText.color = Color.green;
                CAnswerText.color = Color.red;
                break;
            case "c":
                AAnswerText.color = Color.red;
                BAnswerText.color = Color.red;
                CAnswerText.color = Color.green;
                break;
            default:
                Debug.LogWarning("Opção inválida: " + op);
                break;
        }
    }


    public void ResetAnswerColors()
    {
        AAnswerText.color = originalColorA;
        BAnswerText.color = originalColorB;
        CAnswerText.color = originalColorC;
    }


    private IEnumerator WaitAndGoToNextQuestion()
    {
        yield return new WaitForSeconds(2f);
        FadeIn.SetActive(true);
        yield return new WaitForSeconds(.5f);
        nextQuizQuestion();
    }











}
