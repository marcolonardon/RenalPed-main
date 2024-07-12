using JetBrains.Annotations;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;





public class CircleDragData : MonoBehaviour
{

    public Text[] questionText;
    public Button checkButton;
    public Button wrongCongratsButton;
    public Text buttonText;
    public Text checkButtonText;
    public GameObject exclamationMarkImage;
    public GameObject congratulationImage;

    private int questionIndex = 0;
    private int numOfCircleQuestions;
    private string questionAnswer;
    private int correctAdder;
    private int totalQuestions;
    private int UIHelper = 0;
    void Start()
    {
        if (PlayerPrefs.GetInt("levelAt") < 4)
            PlayerPrefs.SetInt("levelAt", 4);

        PlayerPrefs.SetInt("MaxIndex", 18);
        PlayerPrefs.SetInt("MinIndex", 13);
        disableUI();
        getTotalQuestions();
        resetSlots();
        OnGetQuestion();
    }

    private void Update()
    {
        EnableButton();
        checkUserAnswer();
    }

    private void disableUI()
    {
        checkButton.gameObject.SetActive(false);
        wrongCongratsButton.gameObject.SetActive(false);
        exclamationMarkImage.gameObject.SetActive(false);
        congratulationImage.gameObject.SetActive(false);
    }


    /* public void OnSubmit()
     {
         playerName = nameText.text;
         PostToDatabase();
     }*/

    public void OnGetQuestion()
    {
        for (int i = 0; i < numOfCircleQuestions; i++)
        {
            questionText[i].text = PlayerPrefs.GetString("CircleSlotQuestion" + i);
        }

        PlayerPrefs.Save();

    }



    //private void PostToDatabase()
    //{
    //    User user = new User();
    //    RestClient.Put("https://renal-ped-f3573-default-rtdb.firebaseio.com/" + playerName + ".json", user);
    //}



    private void getTotalQuestions()
    {
        numOfCircleQuestions = PlayerPrefs.GetInt("TotalCircleSlotQuestions");
    }


    private void resetSlots()
    {
        //Debug.Log("Entrou no reset");
        for (int i = 1; i <= numOfCircleQuestions; i++)
        {
            PlayerPrefs.SetString("CircleSlot" + i, "null");
            //Debug.Log("Resetando CircleSlot" + i + " "+ PlayerPrefs.GetString("CircleSlot"));
        }
        
    }


    private void checkUserAnswer()
    {

        // Ordem correta = 4-5-1-3-2-6
        correctAdder = 0;


        // Verifica cada slot para determinar se a resposta está correta
        for (int i = 1; i <= 6; i++)
        {
            string slotAnswer = PlayerPrefs.GetString("CircleSlot" + i);
            string correctAnswer = "Circle" + i;

            if (slotAnswer == correctAnswer)
            {
                correctAdder++;
            }
        }

        // Salva o número de acertos no PlayerPrefs
        PlayerPrefs.SetInt("CircleSlotCorrectAdder", correctAdder);

        totalQuestions = PlayerPrefs.GetInt("TotalCircleSlotQuestions");

    }

    public void onClick()
    {
        checkButton.gameObject.SetActive(false);

        if (correctAdder == totalQuestions)
        {
            rightAnswer();
        }
        else
        {
            wrongAnswer();
            
        }
    }

    private void rightAnswer()
    {
        if (PlayerPrefs.GetInt("levelAt") < 4) // Desbloqueia a proxima missao
            PlayerPrefs.SetInt("levelAt", 4);

        buttonText.text = "Continuar";
        wrongCongratsButton.gameObject.SetActive(true);
        congratulationImage.gameObject.SetActive(true);
        StartCoroutine(WaitForClick("Bonus3"));
    }
    private void wrongAnswer()
    {
        buttonText.text = "Tentar Novamente";
        wrongCongratsButton.gameObject.SetActive(true);
        exclamationMarkImage.gameObject.SetActive(true);
        StartCoroutine(WaitForClick("DragSequencePage"));

    }

    private IEnumerator WaitForClick(string sceneToLoad)
    {
        // Aguarda até que o jogador clique novamente
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        // Carrega a próxima cena
        SceneManager.LoadScene(sceneToLoad);
    }

    private void EnableButton()
    {
        int counter = 0;
        for(int i = 1;i <= totalQuestions; i++)
        {
            if (PlayerPrefs.GetString("CircleSlot" + i) != "null")
            {
                counter++;
                //Debug.Log(counter);
                //Debug.Log(PlayerPrefs.GetString("CircleSlot" + i));
            }
        }

        if(counter == 6 && UIHelper == 0)
        {
            checkButtonText.text = "Verificar";
            Debug.Log("counter == totalquestions");
            checkButton.gameObject.SetActive(true);
            UIHelper++;
        }
        else if(UIHelper == 0) 
        {
            Debug.Log("counter diferente totalquestions");
            checkButton.gameObject.SetActive(false);
            //buttonText.gameObject.SetActive(false);
        }

    }

}



