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
        originalColor = TrueText.color;
        UIAnimationsDisable();
        getTotalQuestions();
        OnGetQuestion();
    }

   /* public void OnSubmit()
    {
        playerName = nameText.text;
        PostToDatabase();
    }*/

    public void OnGetQuestion()
    {
        questionText.text = PlayerPrefs.GetString("TrueFalseQuestion" + questionIndex);
    }


    //private void PostToDatabase()
    //{
    //    User user = new User();
    //    RestClient.Put("https://renal-ped-f3573-default-rtdb.firebaseio.com/" + playerName + ".json", user);
    //}




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
            ScoreManager.Instance.AddTrueFalseScore(1, 1);/////////////////////////////////////////////
            Debug.Log("Acertou! A resposta é verdadeira");
        }
        else
        {
            SadFace.SetActive(true);
            TrueText.color= Color.red;
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
            ScoreManager.Instance.AddTrueFalseScore(1, 1);////////////////////////////////////////
            Debug.Log("Acertou! A resposta é falsa");
        }
        else
        {
            SadFace.SetActive(true);
            FalseText.color= Color.red;
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
            if(questionIndex == numOfTFQuestions - 1)
            {
                //Debug.Log("Reiniciou");
                PlayerPrefs.SetInt("MaxIndex", 18);
                PlayerPrefs.SetInt("MinIndex", 13);
                if (PlayerPrefs.GetInt("levelAt") < 5)
                    PlayerPrefs.SetInt("levelAt", 5);

                LoadAwardScene();////////////////////carregar premio


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


    private void LoadAwardScene()
    {
        int score = ScoreManager.Instance.GetScore();
        
        
        if (score > 10)
        {
            SceneManager.LoadScene("Award02");
        }else if(score > 5)
        {
            SceneManager.LoadScene("Award01");
        }
        else
        {
            SceneManager.LoadScene("Award00");
        }
    }
}
