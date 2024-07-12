using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Singleton instance
    public static ScoreManager Instance { get; private set; }

    public Text TotalScoreText;
    public Text PlayerNameText;

    public int QuizScore { get; private set; }
    public int TrueFalseScore { get; private set; }
    public int WashHandsScore { get; private set; }
    public int FoodScore { get; private set; }
    public int BedRoomScore { get; private set; }
    public int DragCircleScore { get; private set; }

    private void Awake()
    {
        // Check if instance already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        GetScore();
    }


    public void UpdateScore()
    {
        QuizScore = PlayerPrefs.GetInt("QuizScore", 0);
        TrueFalseScore = PlayerPrefs.GetInt("TrueFalseScore", 0);
        WashHandsScore = PlayerPrefs.GetInt("WashHandsScore", 0);
        FoodScore = PlayerPrefs.GetInt("FoodScore", 0);
        BedRoomScore = PlayerPrefs.GetInt("BedRoomScore", 0);
        DragCircleScore = PlayerPrefs.GetInt("DragCircleScore", 0);
    }


    public int GetScore()
    {
        UpdateScore();
        int total = QuizScore +
                    TrueFalseScore +
                    WashHandsScore +
                    FoodScore +
                    BedRoomScore +
                    DragCircleScore;

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            TotalScoreText.text = "Total Score: " + total;
            PlayerNameText.text = PlayerPrefs.GetString("CharacterName", "Nome do Avatar");
        }

        return total;
    }


    public void AddQuizScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("QuizScore", 0);
        if (score + add <= max)
        {
            Debug.Log("Adicionou no quiz score + " + add);
            score += add;
            PlayerPrefs.SetInt("QuizScore", score);
            PlayerPrefs.Save();
        }


        QuizScore = PlayerPrefs.GetInt("QuizScore", 0);
        Debug.Log("Quiz Score: " + QuizScore);
    }

    public void AddTrueFalseScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("TrueFalseScore", 0);
        if (score + add <= max)
        {
            Debug.Log("Adicionou no true false score + " + add);
            score += add;
            PlayerPrefs.SetInt("TrueFalseScore", score);
            PlayerPrefs.Save();
        }


        TrueFalseScore = PlayerPrefs.GetInt("TrueFalseScore", 0);
        Debug.Log("TrueFalse Score: " + TrueFalseScore);
    }

    public void AddWashHandsScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("WashHandsScore", 0);
        if (score + add <= max)
        {
            Debug.Log("Adicionou no wash score + " + add);
            score += add;
            PlayerPrefs.SetInt("WashHandsScore", score);
            PlayerPrefs.Save();
        }


        WashHandsScore = PlayerPrefs.GetInt("WashHandsScore", 0);
        Debug.Log("WashHands Score: " + WashHandsScore);
    }

    public void AddFoodScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("FoodScore", 0);
        if (score + add <= max)
        {
            Debug.Log("Adicionou no food score + " + add);
            score += add;
            PlayerPrefs.SetInt("FoodScore", score);
            PlayerPrefs.Save();
        }


        FoodScore = PlayerPrefs.GetInt("FoodScore", 0);
        Debug.Log("Food Score: " + FoodScore);
    }

    public void AddBedRoomScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("BedRoomScore", 0);
        if (score + add <= max)
        {
            Debug.Log("Adicionou no BedRoom score + " + add);
            score += add;
            PlayerPrefs.SetInt("BedRoomScore", score);
            PlayerPrefs.Save();
        }


        BedRoomScore = PlayerPrefs.GetInt("BedRoomScore", 0);
        Debug.Log("BedRoom Score: " + BedRoomScore);
    }

    public void AddDragCircleScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("DragCircleScore", 0);
        if (score + add <= max)
        {
            Debug.Log("Adicionou no DragCircle score + " + add);
            score += add;
            PlayerPrefs.SetInt("DragCircleScore", score);
            PlayerPrefs.Save();
        }


        DragCircleScore = PlayerPrefs.GetInt("DragCircleScore", 0);
        Debug.Log("DragCircle Score: " + DragCircleScore);
    }

}
