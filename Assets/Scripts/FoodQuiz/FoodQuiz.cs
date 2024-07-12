using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FoodQuiz : MonoBehaviour
{
    public Text questionText;
    public Button leftButton;
    public Button rightButton;
    public Button checkButton; 
    public Image rightSelected;
    public Image leftSelected;
    public Sprite[] leftFoodSprite; // Sprite do alimento saudável
    public Sprite[] rightFoodSprite; // Sprite do alimento não saudável
    public string[] mainText;

    private int totalOfQuestions;
    private int currentIndex = 0;
    private bool answer = false;
    private int totalAnswers = 0;

    private void Start()
    {
        if (PlayerPrefs.GetInt("levelAt") < 2)
            PlayerPrefs.SetInt("levelAt", 2);
        PlayerPrefs.SetInt("MaxIndex", 9);
        PlayerPrefs.SetInt("MinIndex", 5);

        totalOfQuestions = leftFoodSprite.Length;


        SetQuestion(currentIndex);

        checkButton.gameObject.SetActive(false);
        RemoveSelection();
    }

    private void SetButtonSprite(Button button, Sprite sprite)
    {
        Image image = button.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite; // Define a sprite do botão como a sprite fornecida
        }
        else
        {
            Debug.LogWarning("O botão não tem um componente Image.");
        }
    }

    public void OnLeftButtonClick()
    {
        answer = true;
        PlayerPrefs.SetInt("FoodAnswer", 1);
        PlayerPrefs.Save();
        //Debug.Log(PlayerPrefs.GetInt("FoodAnswer"));
        leftSelected.gameObject.SetActive(true);
        rightSelected.gameObject.SetActive(false);
        checkButton.gameObject.SetActive(true);
    }

    public void OnRightButtonClick()
    {   
        answer = false;
        PlayerPrefs.SetInt("FoodAnswer", 0);
        PlayerPrefs.Save();
        //Debug.Log(PlayerPrefs.GetInt("FoodAnswer"));
        rightSelected.gameObject.SetActive(true);
        leftSelected.gameObject.SetActive(false);
        checkButton.gameObject.SetActive(true);
    }

    public void OnCheckButtonClick()
    {
        //Debug.Log("Botão de checar foi clicado");
        if (answer)
        {
            // Debug.Log("Acertou");
            ScoreManager.Instance.AddFoodScore(4, 1); ////////////////////////////////////////////////////////////
            totalAnswers++;
        }
        else
        {
            totalAnswers++;
            //Debug.Log("Errou");
        }
        
        PlayerPrefs.SetInt("TotalAnswers", totalAnswers);
        PlayerPrefs.Save();

        NextQuestion();
    }


    private void NextQuestion()
    {

        checkButton.gameObject.SetActive(false);
        //Debug.Log("Entrou para trocar de questão");
        PlayerPrefs.SetInt("FoodQuestionIndex", currentIndex);
        PlayerPrefs.Save();
        //Debug.Log("Salvou index como: " + PlayerPrefs.GetInt("FoodQuestionIndex"));

        if (currentIndex < totalOfQuestions - 1)
        {
            currentIndex++;
            //Debug.Log("index: " + currentIndex);
        }
        else
        {
            Debug.Log("index: " + currentIndex);
            //currentIndex = 0;
            //SceneManager.LoadScene("Bonus1");
        }


        RemoveSelection();
        SetQuestion(currentIndex);
    }

    private void RemoveSelection()
    {
        rightSelected.gameObject.SetActive(false);
        leftSelected.gameObject.SetActive(false);
    }

    private void SetQuestion(int i)
    {
        questionText.text = mainText[i];
        SetButtonSprite(leftButton, leftFoodSprite[i]); // Alimento saudável
        SetButtonSprite(rightButton, rightFoodSprite[i]); // Alimento não saudável
        //Debug.Log("Setou nova sprite");
    }
}
