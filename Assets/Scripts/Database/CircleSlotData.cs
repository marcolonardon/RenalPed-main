using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

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
    private List<Image> circleImages; // Lista de imagens dos círculos
    private int[] correctCircles;

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
        circleImages = new List<Image>(); // Inicializa a lista
        FindCircleImages(); // Encontra todas as imagens dos círculos
        correctCircles = new int[6]; // Inicializa o array de círculos corretos
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

    public void OnGetQuestion()
    {
        for (int i = 0; i < numOfCircleQuestions; i++)
        {
            questionText[i].text = PlayerPrefs.GetString("CircleSlotQuestion" + i);
        }

        PlayerPrefs.Save();
    }

    private void getTotalQuestions()
    {
        numOfCircleQuestions = PlayerPrefs.GetInt("TotalCircleSlotQuestions");
    }

    private void resetSlots()
    {
        for (int i = 1; i <= numOfCircleQuestions; i++)
        {
            PlayerPrefs.SetString("CircleSlot" + i, "null");
        }
    }

    private void checkUserAnswer()
    {
        correctAdder = 0;

        for (int i = 1; i <= 6; i++)
        {
            string slotAnswer = PlayerPrefs.GetString("CircleSlot" + i);
            string correctAnswer = "Circle" + i;

            if (slotAnswer == correctAnswer)
            {
                correctCircles[correctAdder] = i;
                Debug.Log("Index do correctCircles -----> " + correctCircles[correctAdder]);
                correctAdder++;
            }
        }

        PlayerPrefs.SetInt("CircleSlotCorrectAdder", correctAdder);
        totalQuestions = PlayerPrefs.GetInt("TotalCircleSlotQuestions");
    }

    private void PaintCircle(int circleIndex, Color color)
    {
        foreach (Image image in circleImages)
        {
            if (image.gameObject.tag == "Circle" + circleIndex)
            {
                image.color = color; // Pinta o círculo da cor especificada
                break;
            }
        }
    }

    public void onClick()
    {
        checkButton.gameObject.SetActive(false);

        for (int i = 1; i <= 6; i++)
        {
            string slotAnswer = PlayerPrefs.GetString("CircleSlot" + i);
            string correctAnswer = "Circle" + i;

            if (slotAnswer == correctAnswer)
            {
                PaintCircle(i, Color.green);
            }
            else
            {
                PaintCircle(i, Color.red);
            }
        }

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
        if (PlayerPrefs.GetInt("levelAt") < 4) // Desbloqueia a próxima missão
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
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    private void EnableButton()
    {
        int counter = 0;
        for (int i = 1; i <= totalQuestions; i++)
        {
            if (PlayerPrefs.GetString("CircleSlot" + i) != "null")
            {
                counter++;
            }
        }

        if (counter == 6 && UIHelper == 0)
        {
            checkButtonText.text = "Verificar";
            checkButton.gameObject.SetActive(true);
            UIHelper++;
        }
        else if (UIHelper == 0)
        {
            checkButton.gameObject.SetActive(false);
        }
    }

    private void FindCircleImages()
    {
        GameObject[] circleGameObjects;

        // Procura por todos os GameObjects com as tags "Circle1" a "Circle6"
        List<GameObject> foundCircleGameObjects = new List<GameObject>();
        for (int i = 1; i <= 6; i++)
        {
            GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Circle" + i);
            foundCircleGameObjects.AddRange(foundObjects);
        }

        circleGameObjects = foundCircleGameObjects.ToArray();

        foreach (GameObject circleGO in circleGameObjects)
        {
            Image circleImage = circleGO.GetComponent<Image>();
            if (circleImage != null)
            {
                circleImages.Add(circleImage); // Adiciona à lista de imagens dos círculos
            }
        }
    }
}