using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Dan.Main;

public class LeaderboardUpdater : MonoBehaviour
{
    public Text rankText;

    public string publicLeaderboardKey = "d28465b89985929d23a736a246562e429b8547ab5cf51088979b94cda94cffae";

    public UnityEvent<string, int> submitScoreEvent;

    private const string LastScoreKey = "LastScore";

    private void Start()
    {
        CheckAndUpdateScore();
    }

    private void CheckAndUpdateScore()
    {
        int savedScore = PlayerPrefs.GetInt(LastScoreKey, 0);
        int currentScore = PlayerPrefs.GetInt("TotalScore", 0);

        if (currentScore != savedScore)
        {
            Debug.Log("Nova pontuação detectada. Atualizando leaderboard...");
            SubmitScore();
        }
        else
        {
            Debug.Log("Pontuação não alterada.");
            GetLeaderboard();
        }
    }

    public void SubmitScore()
    {
        string inputName = PlayerPrefs.GetString("CharacterName", "Nome do Avatar");
        int inputScore = PlayerPrefs.GetInt("TotalScore", 0);

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, inputName, inputScore, (msg) =>
        {
            PlayerPrefs.SetInt(LastScoreKey, inputScore);
            PlayerPrefs.Save();
            Debug.Log("Pontuação enviada. Atualizando leaderboard...");
            GetLeaderboard();
        });
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (msg) =>
        {
            for (int i = 0; i < msg.Length; i++)
            {
                if (msg[i].Username == PlayerPrefs.GetString("CharacterName", "Nome do Avatar"))
                {
                    PlayerPrefs.SetString("PlayerRank", msg[i].Rank.ToString());
                    PlayerPrefs.Save();
                    Debug.Log("Ranking atual == " + msg[i].Rank.ToString());
                    rankText.text = "Ranking: #" + PlayerPrefs.GetString("PlayerRank", "..."); 
                    break;
                }
            }
        });
    }
}
