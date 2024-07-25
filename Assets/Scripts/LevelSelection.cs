using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//
public class LevelSelection : MonoBehaviour
{
    public Button[] lvlButtons;
    public GameObject lockPrefab; // Prefab da imagem do cadeado
    public GameObject CheckPrefab; // Prefab da imagem de check
    public Button[] bonusButtons;

    // Start is called before the first frame update
    void Start()
    {
        // Desativar todos os botões inicialmente
        DisableBonusButtons();

        int levelAt = PlayerPrefs.GetInt("levelAt", 0); 

        for (int i = 0; i < lvlButtons.Length; i++)
        {
            GameObject lockImage = Instantiate(lockPrefab, lvlButtons[i].transform); // Instancia a imagem do cadeado como filha do botão
            PositionLockBonusImage(lockImage.GetComponent<RectTransform>()); // Posiciona a imagem do cadeado na parte inferior direita
            lockImage.SetActive(false); // Desativa a imagem do cadeado inicialmente

            GameObject checkImage = Instantiate(CheckPrefab, lvlButtons[i].transform);
            PositionCheckImage(checkImage.GetComponent<RectTransform>());
            checkImage.SetActive(false); // Desativa a imagem de check inicialmente

            if (i > levelAt)
            {
                //Debug.Log("Index do Cadeado --> "+i);
                //Debug.Log("Index do levelAt --> " + levelAt);
                lvlButtons[i].interactable = false;
                SetButtonToDisabledState(lvlButtons[i]); // Ajusta a aparência para o estado desativado
                lockImage.SetActive(true); // Ativa a imagem do cadeado
            }
            else
            {
                // Ativa a imagem de check para os níveis completados
                if (i < levelAt)
                {
                    checkImage.SetActive(true);
                }
            }
        }

        ActivateBonusButton(levelAt);
    }

    void DisableBonusButtons()
    {
        foreach (Button button in bonusButtons)
        {
            int index = System.Array.IndexOf(bonusButtons, button); // Obtém o índice do botão no array

            if (!IsButtonActivated(index))
            {
                button.interactable = false;
                Color buttonColor = button.image.color; // Obtém a cor atual do botão
                buttonColor.a = 0f; // Define o canal alpha para zero (totalmente transparente)
                button.image.color = buttonColor; // Aplica a nova cor ao botão
            }
        }
    }

    void ActivateBonusButton(int index)
    {
        Debug.Log("Index do bonus " + index);
        index -= 2; // Ajusta o índice conforme necessário

        if (index >= 0 && index < bonusButtons.Length)
        {
            Button buttonToActivate = bonusButtons[index];
            buttonToActivate.interactable = true;

            ColorBlock colors = buttonToActivate.colors;
            colors.normalColor = Color.white; // Supondo que a cor original seja branca, ajuste conforme necessário
            buttonToActivate.colors = colors;

            // Além disso, você pode ajustar a transparência para garantir que o botão esteja totalmente visível
            Color buttonColor = buttonToActivate.image.color;
            buttonColor.a = 1f; // Garante que o botão seja totalmente visível
            buttonToActivate.image.color = buttonColor;

            // Registrar o botão como ativado usando PlayerPrefs
            PlayerPrefs.SetInt("ButtonActivated_" + index, 1);
            PlayerPrefs.Save(); // Salva os PlayerPrefs imediatamente
        }
    }

    bool IsButtonActivated(int index)
    {
        int activated = PlayerPrefs.GetInt("ButtonActivated_" + index, 0);
        return (activated == 1);
    }

    // Método para ajustar a aparência do botão para o estado desativado
    void SetButtonToDisabledState(Button button)
    {
        ColorBlock cb = button.colors;
        cb.disabledColor = new Color(cb.normalColor.r, cb.normalColor.g, cb.normalColor.b, 1f);
        button.colors = cb;
    }

    // Método para posicionar a imagem do cadeado na parte inferior direita
    void PositionLockBonusImage(RectTransform lockRect)
    {
        lockRect.anchorMin = new Vector2(1, 0); // Âncora inferior direita
        lockRect.anchorMax = new Vector2(1, 0); // Âncora inferior direita
        lockRect.pivot = new Vector2(1, 0); // Pivô inferior direita
        lockRect.anchoredPosition = new Vector2(10, -15); // Ajuste para posicionar o cadeado no botão
    }

    // Método para posicionar a imagem de check na parte inferior esquerda
    void PositionCheckImage(RectTransform checkRect)
    {
        checkRect.anchorMin = new Vector2(1, 0); // Âncora inferior esquerda
        checkRect.anchorMax = new Vector2(1, 0); // Âncora inferior esquerda
        checkRect.pivot = new Vector2(1, 0); // Pivô inferior esquerda
        checkRect.anchoredPosition = new Vector2(12, -15); // Ajuste para posicionar o check no botão
    }

    public void PlayButton()
    {
        int levelIndex = PlayerPrefs.GetInt("levelAt", 0);
        string nextLevel;

        switch (levelIndex)
        {
            case 0:
                nextLevel = "SpeechBubblePage8";
                break;
            case 1:
                nextLevel = "SpeechBubblePage7";
                break;
            case 2:
                nextLevel = "SpeechBubblePage6";
                break;
            case 3:
                nextLevel = "SpeechBubblePage";
                break;
            default:
                nextLevel = "QuizPage";
                break;
        }

        SceneManager.LoadScene(nextLevel);
    }


    public void RstScore()
    {
        Debug.Log("Entrou no reset");

        PlayerPrefs.SetInt("PaintScore", 0);
        PlayerPrefs.SetInt("QuizScore", 0);
        PlayerPrefs.SetInt("TrueFalseScore", 0);
        PlayerPrefs.SetInt("WashScore", 0);
        PlayerPrefs.SetInt("FoodScore", 0);
        PlayerPrefs.SetInt("BedRoomScore", 0);
        PlayerPrefs.SetInt("DragCircleScore", 0);

        PlayerPrefs.SetInt("TotalPaintScore", 0);
        PlayerPrefs.SetInt("TotalQuizScore", 0);
        PlayerPrefs.SetInt("TotalTrueFalseScore", 0);
        PlayerPrefs.SetInt("TotalItemSlotScore", 0);
        PlayerPrefs.SetInt("TotalCircleScore", 0);
        PlayerPrefs.SetInt("TotalWashScore", 0);

        PlayerPrefs.SetInt("Customized", 0);

        PlayerPrefs.Save();

        Debug.Log("Todo score resetado");

        SceneManager.LoadScene("Menu");
    }

    public void RstLvl()
    {
        PlayerPrefs.SetInt("levelAt", 0);

        // Remover todos os registros de botões e check images ativados
        for (int i = 0; i < lvlButtons.Length; i++)
        {
            PlayerPrefs.DeleteKey("ButtonActivated_" + i);
            PlayerPrefs.DeleteKey("CheckActivated_" + i);
        }
        PlayerPrefs.Save(); // Salvar as alterações nos PlayerPrefs
    }

    public void UnlockAll()
    {
        PlayerPrefs.SetInt("levelAt", lvlButtons.Length);

        // Marcar todos os botões e check images como ativados
        for (int i = 0; i < lvlButtons.Length; i++)
        {
            PlayerPrefs.SetInt("ButtonActivated_" + i, 1);
            PlayerPrefs.SetInt("CheckActivated_" + i, 1);
        }
        PlayerPrefs.Save(); // Salvar as alterações nos PlayerPrefs
    }

 
}
