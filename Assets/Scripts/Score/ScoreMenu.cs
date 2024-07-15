using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour
{
    public static ScoreMenu Instance;

    public Image ScoreTable;
    public Image[] Stars;
    public Image[] Medals;
    public Text scoreText;
    public Button button;

    private void Start()
    {
        HidIU();
    }
    private void HidIU()
    {
        ScoreTable.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        button.gameObject.SetActive(false); 

        for(int i = 0; i < Stars.Length; i++)
        {
            Stars[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Medals.Length; i++)
        {
            Medals[i].gameObject.SetActive(false);
        }
    }

    public void ShowUI()
    {
        ScoreTable.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        button.gameObject.SetActive(true);

        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < Medals.Length; i++)
        {
            Medals[i].gameObject.SetActive(true);
        }
    }

}
