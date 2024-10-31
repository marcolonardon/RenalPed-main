using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectObjects : MonoBehaviour
{
    private const int MAXSCORE = 1000;
    private int AddScore;

    [SerializeField] Button checkButton;
    [SerializeField] Image[] selected;
    private int selectedObjects = 0;
    private string[] selectedSequence;

    void Start()
    {
        AddScore = PlayerPrefs.GetInt("TotalWashScore", 1000); 
        SetAllFalse();
        selectedSequence = new string[3];
    }

    void Update()
    {
        if (selectedObjects == 3)
        {
            checkButton.gameObject.SetActive(true);
        }
        else
        {
            checkButton.gameObject.SetActive(false);
        }
    }

    private void SetAllFalse()
    {
        for (int i = 0; i < selected.Length; i++)
        {
            selected[i].gameObject.SetActive(false); // Desativa todas as imagens no início
        }
    }

    public void OnButtonClick()
    {
        if (selectedObjects == 3)
        {
            selectedObjects = 0;
        }

        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        selectedSequence[selectedObjects] = clickedButton.tag;

        Debug.Log("selectedSequence[selectedObjects] = " + selectedSequence[selectedObjects]);

        // Desativa todas as imagens novamente
        foreach (var image in selected)
        {
            image.gameObject.SetActive(false);
        }

        // Ativa as últimas três imagens selecionadas
        for (int i = Mathf.Max(0, selectedObjects - 2); i <= selectedObjects; i++)
        {
            for (int j = 0; j < selected.Length; j++)
            {
                if (selected[j].gameObject.CompareTag(selectedSequence[i]))
                {
                    selected[j].gameObject.SetActive(true); // Ativa apenas as imagens correspondentes
                    break;
                }
            }
        }

        selectedObjects++;
    }

    public void checkObjects()
    {
        Array.Sort(selectedSequence);
        string sequenceString = string.Join("", selectedSequence);

        // Verifica se a sequência Slot4, Slot6, Slot8 está presente na string
        if (sequenceString.Contains("Slot4") && sequenceString.Contains("Slot6") && sequenceString.Contains("Slot8"))
        {
            PlayerPrefs.SetInt("TotalWashScore", AddScore);
            SceneManager.LoadScene("SpeechBubblePage5");
            Debug.Log("Todos os objetos foram selecionados");
        }
        else
        {
            AddScore -= 100;
            if (AddScore < 0)
            {
                AddScore = 0; // Garante que a pontuação não fique negativa
            }
            PlayerPrefs.SetInt("TotalWashScore", AddScore);
            Debug.Log("Errou. Score --> " + AddScore);
            SceneManager.LoadScene("SpeechBubblePage4");
        }
    }
}
