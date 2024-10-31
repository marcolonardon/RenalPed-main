using Proyecto26;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class TesteFirebase : MonoBehaviour
{
    public void Start()
    {
        GetLeaderboard();
    }

    private string basePath = "https://renal-ped-f3573-default-rtdb.firebaseio.com/leaderboard.json";

    public void GetLeaderboard()
    {
        RestClient.Get(basePath).Then(response => {
            // Converte o JSON de resposta para um objeto manipul�vel
            JObject leaderboardData = JObject.Parse(response.Text);

            // Cria uma lista para armazenar os dados de leaderboard
            List<(string userName, int score)> leaderboardList = new List<(string, int)>();

            // Percorre todos os usu�rios e seus dados
            foreach (var user in leaderboardData)
            {
                string userName = (string)user.Value["name"];
                int score = (int)user.Value["score"];
                leaderboardList.Add((userName, score));
            }

            // Ordena a lista de forma decrescente pela pontua��o
            leaderboardList.Sort((x, y) => y.score.CompareTo(x.score));

            // Exibe os 10 melhores jogadores com coloca��o
            for (int i = 0; i < leaderboardList.Count; i++)
            {
                int rank = i + 1; // Coloca��o come�a em 1
                string userName = leaderboardList[i].userName;
                int score = leaderboardList[i].score;

                // Exibe a coloca��o, nome e pontua��o
                Debug.Log($"{rank}� Lugar: {userName}, Pontua��o: {score}");
            }
        }).Catch(error => {
            Debug.LogError("Error fetching leaderboard: " + error);
        });
    }
}
