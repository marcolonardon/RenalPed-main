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

        checkButtonText.text = "Avançar";

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
        Exclamation.gameObject.SetActive(false);
    }

    public void OnGetQuestion()
    {
        questionText[0].text = "Lavagem das mãos para desconexão do cateter";
        questionText[1].text = "Desconexão do cateter após término da terapia";
        questionText[2].text = "Limpeza do quarto e dos materiais da diálise";
        questionText[3].text = "Instalação e conexão da terapia";
        questionText[4].text = "Lavagem das mãos para conexão da terapia";
        questionText[5].text = "Desligar máquina e desprezar os materiais em lixo comum";
    }

    private void getTotalQuestions()
    {
        numOfCircleQuestions = 6;
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

        for (int i = 1; i <= numOfCircleQuestions; i++)
        {
            string slotAnswer = PlayerPrefs.GetString("CircleSlot" + i);
            string correctAnswer = "Circle" + i;

            if (slotAnswer == correctAnswer)
            {
                correctCircles[correctAdder] = i;
                correctAdder++;
            }
        }

        // Atualiza o número de respostas corretas
        PlayerPrefs.SetInt("CircleSlotCorrectAdder", correctAdder);
        totalQuestions = numOfCircleQuestions;
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
        int adder = 0;

        for (int i = 1; i <= numOfCircleQuestions; i++)
        {
            string slotAnswer = PlayerPrefs.GetString("CircleSlot" + i);
            string correctAnswer = "Circle" + i;

            if (slotAnswer == correctAnswer)
            {
                PaintCircle(i, Color.green);  // Pinta o círculo de verde se estiver correto
                adder++;
            }
            else
            {
                PaintCircle(i, Color.red);    // Pinta o círculo de vermelho se estiver errado
            }
        }

        Debug.Log("Adder: " + adder);  // Verifique se o número de respostas corretas está correto

        int errors = numOfCircleQuestions - adder;
        totalErrors += errors;
        PlayerPrefs.SetInt("totalCircleErrors", totalErrors);
        PlayerPrefs.Save();

        if (adder == totalQuestions)
        {
            rightAnswer();
        }
        else
        {
            Debug.Log("entrou no else-resposta incorreta");
            wrongAnswer();  // Este método deve ser chamado se houver erros
        }
    }



    private void RestartLevel()
    {
        Debug.Log("Reiniciando o nível");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reinicia a cena atual
    }



    private void rightAnswer()
    {
        LoadScore();

        if (PlayerPrefs.GetInt("levelAt") < 4) // Desbloqueia a próxima missão
            PlayerPrefs.SetInt("levelAt", 4);

        checkButtonText.text = "Avançar";  // Muda o texto do botão
        ShowPopup();

        ScoreManager.Instance.AddDragCircleScore(MAXSCORE, totalScore);

        PlayerPrefs.SetInt("totalCircleErrors", 0);
        StartCoroutine(WaitForClick("Bonus3"));
    }

    private void wrongAnswer()
    {
        //wrongCongratsButton.gameObject.SetActive(true);
        Exclamation.gameObject.SetActive(true);
        checkButton.gameObject.SetActive(true);
        checkButtonText.text = "Tentar Novamente";  // Muda o texto do botão para "Tentar Novamente"

        checkButton.onClick.RemoveAllListeners();  // Remove qualquer outro evento registrado no botão
        checkButton.onClick.AddListener(() => RestartLevel());  // Adiciona um listener para reiniciar a fase
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
        if(PlayerPrefs.GetInt("TotalCirclesDroped", 0) == 6)
        {
            checkButton.gameObject.SetActive(true);
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
        checkButton.gameObject.SetActive(false);
        checkButtonText.gameObject.SetActive(false);
        ScoreTable.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        scoreButton.gameObject.SetActive(true);


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