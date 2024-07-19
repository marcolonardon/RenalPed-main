using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }

    [SerializeField] private List<Text> names;
    [SerializeField] private List<Text> ranks;
    [SerializeField] private List<Text> scores;


    public string publicLeaderboardKey = "a596ccf54ff34bb440e9425704a73df4181f0c67645112c444554f4962e042fa";

    public UnityEvent<string, int> submitScoreEvent;

    private void Awake()
    {
        Debug.Log("XXXXX AWAKE XXXXX");
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

        SubmitScore();
        GetLeaderboard();
        
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            Debug.Log("msg.Length ---> " + msg.Length + " names.Count ---> " + names.Count);

            for(int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username.ToString();
                scores[i].text = msg[i].Score.ToString();
                ranks[i].text = msg[i].Rank.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }

    public void SubmitScore()
    {
        string inputName = PlayerPrefs.GetString("CharacterName", "Nome do Avatar");
        int inputScore = PlayerPrefs.GetInt("TotalScore", 0);
        submitScoreEvent.Invoke(inputName, inputScore);
    }

}
