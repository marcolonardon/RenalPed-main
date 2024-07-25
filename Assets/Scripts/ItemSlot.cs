using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private int MAXSCORE = 500;
    private int ADDSCORE = 71;
    private int totalScore;

    public string[] expectedTags; // Array de tags esperadas dos objetos
    public bool keepActivated;
    private int expectedTotal = 7;

    private void Start()
    {
        Debug.Log("Entrou no start");
        PlayerPrefs.SetInt("TotalDropped", 0);
        LoadScore();

    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragDrop draggableItem = dropped.GetComponent<DragDrop>();

        // Verifica se o objeto tem uma das tags esperadas
        if (draggableItem != null)
        {
            foreach (string tag in expectedTags)
            {
                if (dropped.CompareTag(tag)) // Verifica se a tag é correta e se ainda há espaço no slot
                {

                    draggableItem.parentAfterDrag = transform;
                    // Mantém ou não o objeto ativado conforme a configuração
                    dropped.SetActive(keepActivated);
                    PlayerPrefs.SetInt("TotalDropped", PlayerPrefs.GetInt("TotalDropped") + 1);
                    // Desativa o objeto dropado para fazê-lo desaparecer
                    Debug.Log("total de objetos dropados: " + PlayerPrefs.GetInt("TotalDropped"));
                    Debug.Log("Objeto dropado com a tag correta: " + tag);
                    IncrementScore();
                    CompletedCheck();
                    return; // Sai do loop assim que encontrar uma tag correspondente
                }

            }
        }
    }



    private void CompletedCheck()
    {
        if (PlayerPrefs.GetInt("TotalDropped") == expectedTotal)
        {
            ScoreManager.Instance.AddBedRoomScore(MAXSCORE, 500); ///////////////////////////////////////////////////////////////////////
            SceneManager.LoadScene("SpeechBubblePage2");
        }
    }

    private void LoadScore()
    {
        Debug.LogWarning("Entrou no LoadScore");
        totalScore = (PlayerPrefs.GetInt("TotalItemSlotScore", 0));
    }

    private void IncrementScore()
    {
        if (PlayerPrefs.GetInt("TotalItemSlotScore", 0) < MAXSCORE)
        {
            totalScore += ADDSCORE;
            PlayerPrefs.SetInt("TotalItemSlotScore", totalScore);
            PlayerPrefs.Save();
            Debug.Log("Total Score---> " + totalScore);

            
        }

        Debug.LogWarning("Está com --> " + PlayerPrefs.GetInt("TotalItemSlotScore", 0));
    }

}