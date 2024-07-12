using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ColorManager : MonoBehaviour
{
    private string selectedColorHex;
    public List<Button> colorButtons;
    public List<Image> images;
    private RectTransform selectedButtonRectTransform;
    private Vector2 originalPosition;
    private string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;

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
            }
            else
            {
                Debug.LogError("Invalid color hex: " + selectedColorHex);
            }
        }
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

    public void UnlockNextLevel()
    {
        if (PlayerPrefs.GetInt("levelAt") < 1)
            PlayerPrefs.SetInt("levelAt", 1);

        PlayerPrefs.SetInt("MaxIndex", 5);
        PlayerPrefs.SetInt("MinIndex", 0);

        PlayerPrefs.Save();
    }
}
