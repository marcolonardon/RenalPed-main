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
    public GameObject Exclamation;
    public Image ScoreTable;
    public GameObject[] Stars;
    public GameObject[] Medals;
    public Text scoreText;
    public Button scoreButton;

    private int questionIndex = 0;
    private int numOfCircleQuestions;
    private string questionAnswer;
    private int correctAdder;
    private int totalQuestions;
    private int UIHelper = 0;
    private List<Image> circleImages; // Lista de imagens dos círculos
    private int[] correctCircles;

    private int totalScore = 0;
    private const int MAXSCORE = 500;
    private int totalErrors = 0;
    private int PenaltyScore = 37;

    void Start()
    {
        if (PlayerPrefs.GetInt("levelAt") < 4)
            PlayerPrefs.SetInt("levelAt", 4);

        PlayerPrefs.SetInt("MaxIndex", 18);
        PlayerPrefs.SetInt("MinIndex", 13);
        totalErrors = PlayerPrefs.GetInt("totalCircleErrors", 0);
        disableUI();
        getTotalQuestions();
        resetSlots();
        OnGetQuestion();
        circleImages = new List<Image>(); // Inicializa a lista
        FindCircleImages(); // Encontra todas as imagens dos círculos
        correctCircles = new int[6]; // Inicializa o array de círculos corretos
        LoadScore();
    }

    private void Update()
    {
        EnableButton();
        checkUserAnswer();
       
    }

    private void disableUI()
    {
        if (ScoreTable != null)
        {
            HidePopup();
        }

        checkButton.gameObject.SetActive(false);
        wrongCongratsButton.gameObject.SetActive(false);
        Exclamation.gameObject.SetActive(false);
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
                //Debug.Log("Index do correctCircles -----> " + correctCircles[correctAdder]);
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
        int adder = 0;

        for (int i = 1; i <= 6; i++)
        {
            string slotAnswer = PlayerPrefs.GetString("CircleSlot" + i);
            string correctAnswer = "Circle" + i;

            if (slotAnswer == correctAnswer)
            {
                PaintCircle(i, Color.green);
                adder++;
                Debug.Log("ACERTOU " + adder);
            }
            else
            {
                PaintCircle(i, Color.red);
            }
        }



        int errors = numOfCircleQuestions - adder;
        totalErrors += errors;
        Debug.LogWarning("errors >>> " + errors + " TotalErrors>>>> " + totalErrors);
        PlayerPrefs.SetInt("totalCircleErrors", totalErrors);
        PlayerPrefs.Save();


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
        LoadScore();

        if (PlayerPrefs.GetInt("levelAt") < 4) // Desbloqueia a próxima missão
            PlayerPrefs.SetInt("levelAt", 4);

        buttonText.text = "Continuar";
        wrongCongratsButton.gameObject.SetActive(true);
        ShowPopup();

        ScoreManager.Instance.AddDragCircleScore(MAXSCORE, totalScore);

        PlayerPrefs.SetInt("totalCircleErrors", 0);

        StartCoroutine(WaitForClick("Bonus3"));
    }

    private void wrongAnswer()
    {
        buttonText.text = "Tentar Novamente";
        wrongCongratsButton.gameObject.SetActive(true);
        Exclamation.gameObject.SetActive(true);
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
        wrongCongratsButton.gameObject.SetActive(false);


        SetStars();
        SetMedals();
        LoadScore();

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
        else if (totalScore > MAXSCORE / 1.5)
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
        else if (totalScore > MAXSCORE / 1.5)
        {
            Medals[1].gameObject.SetActive(true);
        }
        else
        {
            Medals[0].gameObject.SetActive(true);
        }
    }

    private void LoadScore()
    {
        totalScore = MAXSCORE - (totalErrors * PenaltyScore);
        if (totalScore < 0) totalScore = 0;
        scoreText.text = ((totalScore + 500).ToString());
        Debug.LogWarning("LoadScore == " + totalScore + "TotalErrors == " + totalErrors);
    }





}