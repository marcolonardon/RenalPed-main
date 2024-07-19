using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    public static SceneNavigation Instance { get; private set; }
    private void Awake()
    {

        // If an instance already exists and it's not this one, destroy this instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Set this instance as the singleton instance
            Instance = this;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void InitialPageLoad(string sceneName)
    {
        if (PlayerPrefs.GetInt("Customized") == 1)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene("CharacterPage1");
        }
    }

    public void StartPaintGame()
    {
        PlayerPrefs.SetInt("TotalScore", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("PaintPage1");
    }

    public void StartWashGame()
    {
        PlayerPrefs.SetInt("WashScore", 1000);
        PlayerPrefs.Save();
        SceneManager.LoadScene("SpeechBubblePage6");
    }

    public void StartQuizGame()
    {
        PlayerPrefs.SetInt("QuizScore", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("QuizPage");
    }




}
