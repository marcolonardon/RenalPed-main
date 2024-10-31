using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Dan.Main;
using UnityEngine.SocialPlatforms.Impl;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public string publicLeaderboardKey = "d28465b89985929d23a736a246562e429b8547ab5cf51088979b94cda94cffae";
    public UnityEvent<string, int> submitScoreEvent;
    // Singleton instance
    public static ScoreManager Instance { get; private set; }

    public Text TotalScoreText;
    public Text PlayerNameText;
    public Text PlayerRank;

    public int QuizScore { get; private set; }
    public int TrueFalseScore { get; private set; }
    public int WashHandsScore { get; private set; }
    public int FoodScore { get; private set; }
    public int BedRoomScore { get; private set; }
    public int DragCircleScore { get; private set; }
    public int PaintScore { get; private set; }

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

    public void ResetScore()
    {
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

        PlayerPrefs.Save();

        Debug.Log("Resetou os scores");
    }


    public void UpdateScore()
    {
        PaintScore = PlayerPrefs.GetInt("PaintScore", 0);
        QuizScore = PlayerPrefs.GetInt("QuizScore", 0);
        TrueFalseScore = PlayerPrefs.GetInt("TrueFalseScore", 0);
        WashHandsScore = PlayerPrefs.GetInt("WashScore", 0);
        FoodScore = PlayerPrefs.GetInt("FoodScore", 0);
        BedRoomScore = PlayerPrefs.GetInt("BedRoomScore", 0);
        DragCircleScore = PlayerPrefs.GetInt("DragCircleScore", 0);
    }

    public int GetScore()
    {
        UpdateScore();
        int total = PaintScore +
                    QuizScore +
                    TrueFalseScore +
                    WashHandsScore +
                    FoodScore +
                    BedRoomScore +
                    DragCircleScore;
        //Debug.LogWarning("Somando: " + PaintScore + " + " + QuizScore + " + " + TrueFalseScore + " + " + WashHandsScore + " + " + FoodScore + " + " + BedRoomScore + " + " + DragCircleScore);
        if (SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "Ranking")
        {
            TotalScoreText.text += total.ToString();
            PlayerNameText.text += PlayerPrefs.GetString("CharacterName", "Nome do Avatar");
            PlayerRank.text += PlayerPrefs.GetString("PlayerRank");
        }

        PlayerPrefs.SetInt("TotalScore", total);
        return total;
    }

    private void UpdateScoreIfHigher(string key, int score)
    {
        int currentScore = PlayerPrefs.GetInt(key, 0);
        if (score > currentScore)
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();
        }
    }

    public void AddQuizScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("QuizScore", 0);
        if (score < max)
        {
            score = add;
            UpdateScoreIfHigher("QuizScore", score);
        }
        QuizScore = PlayerPrefs.GetInt("QuizScore", 0);
        Debug.Log("Quiz Score no manager: " + QuizScore);
    }

    public void AddTrueFalseScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("TrueFalseScore", 0);
        if (score < add)
        {
            score = add;
            UpdateScoreIfHigher("TrueFalseScore", score);
        }
        TrueFalseScore = PlayerPrefs.GetInt("TrueFalseScore", 0);
        Debug.Log("TrueFalse Score: " + TrueFalseScore);
    }

    public void AddWashHandsScore(int max, int add)
    {
        Debug.LogWarning("Valor que veio do wash == " + add);
        int score = PlayerPrefs.GetInt("WashScore", 0);
        if (score < add)
        {
            score = add;
            UpdateScoreIfHigher("WashScore", score);
        }
        WashHandsScore = PlayerPrefs.GetInt("WashScore", 0);
        Debug.Log("WashHands Score: " + WashHandsScore);
    }

    public void AddFoodScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("FoodScore", 0);
        if (score < add)
        {
            score = add;
            UpdateScoreIfHigher("FoodScore", score);
        }
        FoodScore = PlayerPrefs.GetInt("FoodScore", 0);
        Debug.Log("Food Score: " + FoodScore);
    }

    public void AddBedRoomScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("BedRoomScore", 0);
        if (score < add)
        {
            score = add;
            UpdateScoreIfHigher("BedRoomScore", score);
        }
        BedRoomScore = PlayerPrefs.GetInt("BedRoomScore", 0);
        Debug.Log("BedRoom Score: " + BedRoomScore);
    }

    public void AddDragCircleScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("DragCircleScore", 0);
        if (score < add)
        {
            score = add;
            UpdateScoreIfHigher("DragCircleScore", score);
        }
        DragCircleScore = PlayerPrefs.GetInt("DragCircleScore", 0);
        Debug.Log("DragCircle Score: " + DragCircleScore);
    }

    public void AddPaintScore(int max, int add)
    {
        int score = PlayerPrefs.GetInt("PaintScore", 0);
        if (score < add)
        {
            score = add;
            UpdateScoreIfHigher("PaintScore", score);
        }
        PaintScore = PlayerPrefs.GetInt("PaintScore", 0);
        Debug.Log("Paint Score no AddPaintScore: " + PaintScore);
    }
}
