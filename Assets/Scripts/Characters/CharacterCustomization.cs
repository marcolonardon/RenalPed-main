using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCustomization : MonoBehaviour
{
    // Refer�ncias aos GameObjects que representam as partes do personagem
    public Image bodyImage;
    public Image faceImage;
    public Image hairImage;
    public Image clothesImage;
    public Image accessoryImage;

    // Arrays para armazenar as op��es de sprites
    public Sprite[] bodyOptions;
    public Sprite[] faceOptions;
    public Sprite[] hairOptions;
    public Sprite[] clothesOptions;
    public Sprite[] accessoryOptions;

    // Array para armazenar as op��es de cores do corpo em formato hexadecimal
    public string[] bodyColorHex;

    public InputField characterNameInputField;
    public Text imputFieldNameText;

    // �ndices para rastrear a op��o atual selecionada para cada parte
    private int currentBodyIndex = 0;
    private int currentFaceIndex = 0;
    private int currentHairIndex = 0;
    private int currentBodyColorIndex = 0;
    private int currentClothesIndex = 0;
    private int currentAccessoryIndex = 0;

    private void Start()
    {
        LoadCharacterCustomization();
        UpdateCharacterAppearance();
    }

    // M�todos para alterar as op��es diretamente
    public void SetBodyOption(int bodyIndex)
    {
        currentBodyIndex = bodyIndex;
        UpdateBody(currentBodyIndex);
        SaveCharacterCustomization();
    }

    public void SetFaceOption(int faceIndex)
    {
        currentFaceIndex = faceIndex;
        UpdateFace(currentFaceIndex);
        SaveCharacterCustomization();
    }

    public void SetBodyColorOption(int bodyColorIndex)
    {
        currentBodyColorIndex = bodyColorIndex;
        UpdateBodyColor();
        SaveCharacterCustomization();
    }

    public void SetClothesOption(int clothesIndex)
    {
        currentClothesIndex = clothesIndex;
        UpdateClothes(currentClothesIndex);
        SaveCharacterCustomization();
    }

    public void SetHairOption(int hairIndex)
    {
        currentHairIndex = hairIndex;
        UpdateHair(currentHairIndex);
        SaveCharacterCustomization();
    }

    public void SetAccessoryOption(int accessoryIndex)
    {
        currentAccessoryIndex = accessoryIndex;
        UpdateAccessory(currentAccessoryIndex);
        if (currentHairIndex == 0 && currentAccessoryIndex == 0)
            UpdateAccessory(accessoryOptions.Length - 1);
        SaveCharacterCustomization();
    }

    // M�todos para remover a sele��o
    public void RemoveBodySelection()
    {
        currentBodyIndex = -1;
        bodyImage.sprite = null;
        bodyImage.color = new Color(0, 0, 0, 0); // Torna a imagem transparente
        SaveCharacterCustomization();
    }

    public void RemoveFaceSelection()
    {
        currentFaceIndex = -1;
        faceImage.sprite = null;
        faceImage.color = new Color(0, 0, 0, 0); // Torna a imagem transparente
        SaveCharacterCustomization();
    }

    public void RemoveHairSelection()
    {
        currentHairIndex = -1;
        hairImage.sprite = null;
        hairImage.color = new Color(0, 0, 0, 0); // Torna a imagem transparente
        SaveCharacterCustomization();
    }

    public void RemoveClothesSelection()
    {
        currentClothesIndex = -1;
        clothesImage.sprite = null;
        clothesImage.color = new Color(0, 0, 0, 0); // Torna a imagem transparente
        SaveCharacterCustomization();
    }

    public void RemoveAccessorySelection()
    {
        currentAccessoryIndex = -1;
        accessoryImage.sprite = null;
        accessoryImage.color = new Color(0, 0, 0, 0); // Torna a imagem transparente
        SaveCharacterCustomization();
    }

    // M�todos para atualizar a imagem com a sprite atual
    private void UpdateBody(int index)
    {
        if (index >= 0 && bodyOptions[index] != null)
        {
            bodyImage.sprite = bodyOptions[index];
            bodyImage.color = Color.white; // Restaura a cor ao definir uma nova sprite
        }
    }

    private void UpdateFace(int index)
    {
        if (index >= 0 && faceOptions[index] != null)
        {
            faceImage.sprite = faceOptions[index];
            faceImage.color = Color.white; // Restaura a cor ao definir uma nova sprite
        }
    }

    private void UpdateHair(int index)
    {
        if (index >= 0 && hairOptions[index] != null)
        {
            hairImage.sprite = hairOptions[index];
            hairImage.color = Color.white; // Restaura a cor ao definir uma nova sprite
        }
    }

    private void UpdateClothes(int index)
    {
        if (index >= 0 && clothesOptions[index] != null)
        {
            clothesImage.sprite = clothesOptions[index];
            clothesImage.color = Color.white; // Restaura a cor ao definir uma nova sprite
        }
    }

    private void UpdateAccessory(int index)
    {
        Debug.Log("Index do update acessory: " + index);
        if (index >= 0 && accessoryOptions[index] != null)
        {
            Debug.Log("Entrou no if do update acessory: " + index);
            accessoryImage.sprite = accessoryOptions[index];
            accessoryImage.color = Color.white; // Restaura a cor ao definir uma nova sprite
        }
    }

    private void UpdateBodyColor()
    {
        if (currentBodyColorIndex >= 0)
        {
            Color newColor;
            if (ColorUtility.TryParseHtmlString(bodyColorHex[currentBodyColorIndex], out newColor))
            {
                bodyImage.color = newColor;
            }
            else
            {
                Debug.LogWarning("Invalid color hex string: " + bodyColorHex[currentBodyColorIndex]);
            }
        }
    }

    // Fun��o para salvar as configura��es no PlayerPrefs
    private void SaveCharacterCustomization()
    {
        PlayerPrefs.SetInt("BodyIndex", currentBodyIndex);
        PlayerPrefs.SetInt("FaceIndex", currentFaceIndex);
        PlayerPrefs.SetInt("HairIndex", currentHairIndex);
        PlayerPrefs.SetInt("BodyColorIndex", currentBodyColorIndex);
        PlayerPrefs.SetInt("ClothesIndex", currentClothesIndex);
        PlayerPrefs.SetInt("AccessoryIndex", currentAccessoryIndex);
        PlayerPrefs.Save();
    }

    // Fun��o para carregar as configura��es do PlayerPrefs
    private void LoadCharacterCustomization()
    {
        currentBodyIndex = PlayerPrefs.GetInt("BodyIndex", 0);
        currentFaceIndex = PlayerPrefs.GetInt("FaceIndex", 0);
        currentHairIndex = PlayerPrefs.GetInt("HairIndex", 0);
        currentBodyColorIndex = PlayerPrefs.GetInt("BodyColorIndex", 0);
        currentClothesIndex = PlayerPrefs.GetInt("ClothesIndex", 0);
        currentAccessoryIndex = PlayerPrefs.GetInt("AccessoryIndex", 0);
    }

    // Fun��o para atualizar a apar�ncia do personagem com base nas configura��es salvas
    private void UpdateCharacterAppearance()
    {
        // Verifica se o bodyImage n�o � nulo e se o �ndice � v�lido
        if (bodyImage != null && currentBodyIndex >= 0)
            UpdateBody(currentBodyIndex);
        else if (bodyImage != null)
            bodyImage.color = new Color(0, 0, 0, 0);

        // Verifica se o faceImage n�o � nulo e se o �ndice � v�lido
        if (faceImage != null && currentFaceIndex >= 0)
            UpdateFace(currentFaceIndex);
        else if (faceImage != null)
            faceImage.color = new Color(0, 0, 0, 0);

        // Verifica se o hairImage n�o � nulo e se o �ndice � v�lido
        if (hairImage != null && currentHairIndex >= 0)
            UpdateHair(currentHairIndex);
        else if (hairImage != null)
            hairImage.color = new Color(0, 0, 0, 0);

        // Verifica se o bodyImage n�o � nulo e se o �ndice de cor � v�lido
        if (bodyImage != null && currentBodyColorIndex >= 0)
            UpdateBodyColor();
        else if (bodyImage != null)
            bodyImage.color = new Color(0, 0, 0, 0);

        // Verifica se o clothesImage n�o � nulo e se o �ndice � v�lido
        if (clothesImage != null && currentClothesIndex >= 0)
            UpdateClothes(currentClothesIndex);
        else if (clothesImage != null)
            clothesImage.color = new Color(0, 0, 0, 0);

        // Verifica se o accessoryImage n�o � nulo e se o �ndice � v�lido
        if (accessoryImage != null && currentAccessoryIndex >= 0)
            UpdateAccessory(currentAccessoryIndex);
        else if (accessoryImage != null)
            accessoryImage.color = new Color(0, 0, 0, 0);

        // Verifica se a cena ativa � "CharacterPage4" e se o campo de entrada de nome n�o � nulo
        if (SceneManager.GetActiveScene().name == "CharacterPage4" && characterNameInputField != null)
        {
            characterNameInputField.text = PlayerPrefs.GetString("CharacterName", "");
        }
    }


    public void SetCustomizationAsDone()
    {
        PlayerPrefs.SetInt("Customized", 1);
        PlayerPrefs.Save();
    }

    public void SaveCharacterName()
    {
        string characterName = characterNameInputField.text;

        if (!string.IsNullOrEmpty(characterName))
        {
            PlayerPrefs.SetString("CharacterName", characterName);
            PlayerPrefs.Save();
            Debug.Log("Character Name saved: " + characterName);
        }
        else
        {
            Debug.LogWarning("Character Name is empty. Please enter a name.");
        }
    }

    public void LoadPlayerName()
    {
        characterNameInputField.text = PlayerPrefs.GetString("CharacterName");
    }
}
