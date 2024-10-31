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
    private const int MAXSCORE = 1000;
    private int ADDSCORE;
    private int totalScore = 0;

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
    public static int numOfTFQuestions;
    public static string playerAAnswer;
    public static string playerBAnswer;
    public static string playerCAnswer;

    private Color originalColorA;
    private Color originalColorB;
    private Color originalColorC;

    void Start()
    {
        LoadScore();
        UIAnimationsDisable();
        originalColorA = AAnswerText.color;
        originalColorB = BAnswerText.color;
        originalColorC = CAnswerText.color;

        getTotalQuestions();
        OnGetQuestion();

        ADDSCORE = MAXSCORE / ((numOfQuizQuestions) + (numOfTFQuestions)); // para que a soma máxima dos acertos sempre fique em 1000 - ignora casas decimais 

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
        switch (myQuizIndex)
        {
            case 0:
                questionText.text = "O que é peritonite";
                break;
            case 1:
                questionText.text = "Quais são os principais sintomas da Peritonite";
                break;
            case 2:
                questionText.text = "O que fazer em casos de sintomas de peritonite";
                break;
            case 3:
                questionText.text = "O que fazer em casos de desconexão do cateter ou da tampa protetora";
                break;
        }

        //questionText.text = PlayerPrefs.GetString("QuizQuestion" + myQuizIndex);

    }

    private void UpdateAnswer()
    {
        switch(myQuizIndex)
        {
            case 0:
                AAnswerText.text = "Nome dado à máquina de diálise.";
                BAnswerText.text = "Infecção no peritônio (barriga).";
                CAnswerText.text = "Infecção no sangue.";
                break;
            case 1:
                AAnswerText.text = "Febre, dor de cabeça e líquido claro.";
                BAnswerText.text = "Febre, dor abdominal e líquido transparente.";
                CAnswerText.text = "Febre, dor abdominal e líquido turvo.";
                break;
            case 2:
                AAnswerText.text = "Continuar normalmente a diálise, não é grave.";
                BAnswerText.text = "Procurar imediatamente o seu serviço de diálise.";
                CAnswerText.text = "Encerrar a diálise e tomar remédio para dor.";
                break;
            case 3:
                AAnswerText.text = "Encaixar novamente o cateter e a tampa.";
                BAnswerText.text = "Apenas colocar um tampa (prep kit) nova.";
                CAnswerText.text = "Procurar o serviço de diálise imediatamente para troca do sistema com técnica estéril.";
                break;
        }


        PlayerPrefs.SetInt("QuizAudioIndex", myQuizIndex);

        //AAnswerText.text = PlayerPrefs.GetString("QuizAnswerA" + myQuizIndex);
        //BAnswerText.text = PlayerPrefs.GetString("QuizAnswerB" + myQuizIndex);
        //CAnswerText.text = PlayerPrefs.GetString("QuizAnswerC" + myQuizIndex);
    }

    //private void PostToDatabase()
    //{
    //    User user = new User();
    //    RestClient.Put("https://renal-ped-f3573-default-rtdb.firebaseio.com/" + playerName + ".json", user);
    //}

    private void RetriveFromDatabase()
    {

        switch (myQuizIndex)
        {
            case 0:
                playerCorrectAnswer = "b";
                break;
            case 1:
                playerCorrectAnswer = "c";
                break;
            case 2:
                playerCorrectAnswer = "b";
                break;
            case 3:
                playerCorrectAnswer = "c";
                break;
        }
   
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
        numOfQuizQuestions = 4;
        numOfTFQuestions = 1;
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


        IncrementScore(); ///////////////////////////////////////////////////////////////////////
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


    private void IncrementScore()
    {
        if (PlayerPrefs.GetInt("TotalQuizScore", 0) < MAXSCORE)
        {
            totalScore += ADDSCORE;
            PlayerPrefs.SetInt("TotalQuizScore", totalScore);
            Debug.Log("Total Score---> " + totalScore);
        }

        PlayerPrefs.SetInt("LastQuizScore", totalScore);
        PlayerPrefs.Save();
        //Debug.LogWarning("Está com --> " + PlayerPrefs.GetInt("TotalQuizScore", 0));
    }


    private void LoadScore()
    {
        //Debug.LogWarning("Entrou no LoadScore");
        totalScore = PlayerPrefs.GetInt("TotalQuizScore", 0);
        ScoreManager.Instance.AddQuizScore(MAXSCORE, totalScore);
    }





}
