using Proyecto26;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class NameGenerator : MonoBehaviour
{
    public InputField inputFieldNameText;

    public static int MaxNameIndex = 19; // Inicialize com um valor padrão
    public static int MaxSurnameIndex = 19; // Inicialize com um valor padrão
    public static string playerName, playerSurname;
    public static int nameIndex, surnameIndex;

    void Start()
    {
        // Obtenha os índices máximos antes de gerar nomes
        GetMaxIndex().ContinueWith(task =>
        {
            if (PlayerPrefs.GetInt("Customized", 0) != 1)
            {
                Generate();
            }

        });
    }

    private int GenerateRandomNumber(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1); // +1 porque Random.Range é exclusivo do limite superior
    }

    private async Task GenerateName()
    {
        nameIndex = GenerateRandomNumber(0, MaxNameIndex);

        var tcs = new TaskCompletionSource<bool>();

        RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/Player/random_names/0/" + nameIndex+".json").Then(response =>
        {
            playerName = response.name;
            tcs.SetResult(true);
            //Debug.Log("Nome --> " + playerName);
        }).Catch(error =>
        {
            Debug.LogError("Erro ao obter dados do banco de dados: " + error);
            tcs.SetException(new Exception("Erro na requisição de GenerateName"));

        });

        await tcs.Task;
    }

    private async Task GenerateLastName()
    {
        surnameIndex = GenerateRandomNumber(0, MaxSurnameIndex);

        var tcs = new TaskCompletionSource<bool>();

        RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/Player/random_names/1/" + surnameIndex + ".json").Then(response =>
        {
            playerSurname = response.surname;
            tcs.SetResult(true);
            //Debug.Log("Sobrenome --> " + playerSurname);
        }).Catch(error =>
        {
            Debug.LogError("Erro ao obter dados do banco de dados: " + error);
            tcs.SetException(new Exception("Erro na requisição de GenerateLastName"));
        });

        await tcs.Task;
    }

    private async Task GetMaxIndex()
    {
        var tcs = new TaskCompletionSource<bool>();

        RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/Player/num_of_names/.json").Then(response =>
        {
            MaxNameIndex = response.total_names;
            MaxSurnameIndex = response.total_surnames;
            PlayerPrefs.SetInt("MaxNamesIndex", MaxNameIndex);
            PlayerPrefs.SetInt("MaxLastNamesIndex", MaxSurnameIndex);
            PlayerPrefs.Save();
            //Debug.Log("Baixou do Firebase ----> " + PlayerPrefs.GetInt("MaxNamesIndex"));
            tcs.SetResult(true); // Marca a tarefa como concluída com sucesso
        }).Catch(error =>
        {
            Debug.LogError("Erro ao obter quantidade de nomes: " + error);
            tcs.SetException(new Exception("Erro na requisição de GetMaxIndex")); // Marca a tarefa com exceção em caso de erro
        });

        // Aguarda a conclusão da tarefa
        await tcs.Task;
    }

    public async void Generate()
    {
        await GenerateName();
        await GenerateLastName();
        PlayerPrefs.SetString("CharacterName", playerName+playerSurname);
        PlayerPrefs.Save();
        inputFieldNameText.text = PlayerPrefs.GetString("CharacterName");
        Debug.Log("Nome completo --->>>" + PlayerPrefs.GetString("CharacterName"));
    }
}
