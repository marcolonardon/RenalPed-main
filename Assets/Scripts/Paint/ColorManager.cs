using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; 

public class ColorManager : MonoBehaviour 
{
    private const int ADDPOINTS = 11;
    private const int MAXSCORE = 1001;

    private string selectedColorHex;
    public List<Button> colorButtons;
    public List<Image> images;
    private RectTransform selectedButtonRectTransform;
    private Vector2 originalPosition;
    private string currentSceneName;
    private HashSet<Image> paintedImages; // HashSet to track painted images

    private int totalScore = 0;


    public Image ScoreTable;
    public GameObject[] Stars;
    public GameObject[] Medals;
    public Text scoreText;
    public Button scoreButton;

    void Start()
    {
        LoadScore();

        if (ScoreTable != null){
            HidePopup();
        }
        
        currentSceneName = SceneManager.GetActiveScene().name;
        paintedImages = new HashSet<Image>(); // Initialize the HashSet

        foreach (Button button in colorButtons)
        {
            button.onClick.AddListener(() => OnColorButtonClick(button));
        }

        foreach (Image image in images)
        {
            AddClickListenerToImage(image);
        }

        
        LoadPainting();
        
    }



    private void OnColorButtonClick(Button clickedButton)
    {
        // Move the previously selected button back to its original position
        if (selectedButtonRectTransform != null)
        {
            selectedButtonRectTransform.anchoredPosition = originalPosition;
        }

        // Store the RectTransform and original position of the clicked button
        selectedButtonRectTransform = clickedButton.GetComponent<RectTransform>();
        originalPosition = selectedButtonRectTransform.anchoredPosition;

        // Move the clicked button to the right
        selectedButtonRectTransform.anchoredPosition += new Vector2(20, 0);

        // Get the color hex from the button's name or a custom attribute
        selectedColorHex = GetColorHexFromButton(clickedButton);

        Debug.Log("Selected color hex: " + selectedColorHex); // Adicione esta linha para depuração
    }

    private string GetColorHexFromButton(Button button)
    {
        return button.name.Replace("Pencil", "").Replace("Eraser", "");
    }

    private void AddClickListenerToImage(Image image)
    {
        EventTrigger trigger = image.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((data) => OnImageClick(image));
        trigger.triggers.Add(entry);
    }

    private void OnImageClick(Image clickedImage)
    {
        if (!string.IsNullOrEmpty(selectedColorHex))
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(selectedColorHex, out color))
            {
                clickedImage.color = color; // Apply the selected color to the clicked image
                SavePainting();

                // Only increment score if the image is painted for the first time
                if (!paintedImages.Contains(clickedImage))
                {
                    paintedImages.Add(clickedImage);
                    IncrementScore();
                }
            }
            else
            {
                Debug.LogError("Invalid color hex: " + selectedColorHex);
            }
        }
    }

    private void IncrementScore()
    {
        if (PlayerPrefs.GetInt("TotalPaintScore", 0) < MAXSCORE)
        {
            totalScore += ADDPOINTS;
            PlayerPrefs.SetInt("TotalPaintScore", totalScore);
            PlayerPrefs.Save();
            Debug.Log("Total Score---> " + totalScore);
        }

        Debug.LogWarning("Está com --> " + PlayerPrefs.GetInt("TotalPaintScore", 0));
    }


    private void SavePainting()
    {
        for (int i = 0; i < images.Count; i++)
        {
            string colorHex = ColorUtility.ToHtmlStringRGBA(images[i].color);
            PlayerPrefs.SetString(currentSceneName + "_ImageColor_" + i, colorHex);
        }
        PlayerPrefs.Save();
    }

    private void LoadPainting()
    {
        for (int i = 0; i < images.Count; i++)
        {
            string colorHex = PlayerPrefs.GetString(currentSceneName + "_ImageColor_" + i, "#FFFFFFFF"); // Default to white if no color is saved
            Color color;
            if (ColorUtility.TryParseHtmlString("#" + colorHex, out color))
            {
                images[i].color = color;
            }
        }
    }

    private void LoadScore()
    {
        Debug.LogWarning("Entrou no LoadScore");
        totalScore = PlayerPrefs.GetInt("TotalPaintScore", 0);
        Debug.Log("Loaded Total Score---> " + totalScore);
    }

    public void UnlockNextLevel()
    {
        ShowPopup();

        if (PlayerPrefs.GetInt("levelAt") < 1)
            PlayerPrefs.SetInt("levelAt", 1);

        PlayerPrefs.SetInt("MaxIndex", 5);
        PlayerPrefs.SetInt("MinIndex", 0);

        PlayerPrefs.Save();
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
        if (totalScore > MAXSCORE / 1.2)
        {
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].gameObject.SetActive(true);
            }
        }
        else if (totalScore > MAXSCORE / 2.2)
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
        if (totalScore > MAXSCORE / 1.2)
        {
            Medals[2].gameObject.SetActive(true);
        }
        else if (totalScore > MAXSCORE / 2.2)
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
        Debug.Log("MAXSCORE --> " + MAXSCORE);
        Debug.Log("TOTALCORE --> " + totalScore);
        scoreText.text = totalScore.ToString();
        ScoreManager.Instance.AddPaintScore(MAXSCORE, totalScore);
    }
}
