using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }

    [SerializeField] private List<Text> names;
    [SerializeField] private List<Text> ranks;
    [SerializeField] private List<Text> scores;

    public string publicLeaderboardKey = "d28465b89985929d23a736a246562e429b8547ab5cf51088979b94cda94cffae";

    public UnityEvent<string, int> submitScoreEvent;

    private void Awake()
    {
        // Garantir que apenas uma instância do Leaderboard exista
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Submeter pontuação e atualizar leaderboard ao iniciar
        SubmitScore();
        GetLeaderboard();
    }

    // Atualizar o leaderboard
    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (msg) =>
        {
            int loopLength = Mathf.Min(msg.Length, names.Count);

            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
                ranks[i].text = msg[i].Rank.ToString();

                // Salvar o rank do jogador no PlayerPrefs se o nome coincidir
                if (names[i].text == PlayerPrefs.GetString("CharacterName", "Nome do Avatar"))
                {
                    PlayerPrefs.SetString("PlayerRank", ranks[i].text);
                    PlayerPrefs.Save();
                    Debug.Log("Ranking atual == " + ranks[i].text);
                }
            }
        });
    }

    // Submeter uma nova entrada ao leaderboard
    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, (msg) =>
        {
            GetLeaderboard();
        });
    }

    // Submeter a pontuação do jogador atual
    public void SubmitScore()
    {
        string inputName = PlayerPrefs.GetString("CharacterName", "Nome do Avatar");
        int inputScore = PlayerPrefs.GetInt("TotalScore", 0);
        submitScoreEvent.Invoke(inputName, inputScore);
    }
}
