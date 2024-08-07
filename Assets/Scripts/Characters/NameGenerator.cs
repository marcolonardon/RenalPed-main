using Proyecto26;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class NameGenerator : MonoBehaviour
{
    public InputField inputFieldNameText;
    public Button continueButton; 

    public static int MaxNameIndex = 19; 
    public static int MaxSurnameIndex = 19; 
    public static string playerName, playerSurname;
    public static int nameIndex, surnameIndex;

    void Start()
    {
        // Adicione um listener ao inputField para verificar o nome digitado
        inputFieldNameText.onValueChanged.AddListener(delegate { CheckNameInput(); });

        // Obtenha os índices máximos antes de gerar nomes
        GetMaxIndex().ContinueWith(task =>
        {
            if (PlayerPrefs.GetInt("Customized", 0) != 1)
            {
                continueButton.interactable = false; // Desativa o botão de continuar se o nome ainda não foi personalizado
                Generate();
                
            }
            else
            {
                // Se já estiver customizado, carregar o nome salvo e habilitar o botão se necessário
                inputFieldNameText.text = PlayerPrefs.GetString("CharacterName", "");
                CheckNameInput(); // Verifica se o nome foi carregado e ativa o botão de continuar
            }
        });
    }

    private void CheckNameInput()
    {
        // Ativa o botão de continuar apenas se o campo de nome não estiver vazio
        continueButton.interactable = !string.IsNullOrEmpty(inputFieldNameText.text);
    }

    private int GenerateRandomNumber(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1); // +1 porque Random.Range é exclusivo do limite superior
    }

    private async Task GenerateName()
    {
        nameIndex = GenerateRandomNumber(0, MaxNameIndex);

        var tcs = new TaskCompletionSource<bool>();

        RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/Player/random_names/0/" + nameIndex + ".json").Then(response =>
        {
            playerName = response.name;
            tcs.SetResult(true);
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
            tcs.SetResult(true);
        }).Catch(error =>
        {
            Debug.LogError("Erro ao obter quantidade de nomes: " + error);
            tcs.SetException(new Exception("Erro na requisição de GetMaxIndex"));
        });

        await tcs.Task;
    }

    public async void Generate()
    {
        await GenerateName();
        await GenerateLastName();
        PlayerPrefs.SetString("CharacterName", playerName + playerSurname);
        PlayerPrefs.Save();
        inputFieldNameText.text = PlayerPrefs.GetString("CharacterName");
        Debug.Log("Nome completo --->>>" + PlayerPrefs.GetString("CharacterName"));
        CheckNameInput(); // Verifica se o nome foi gerado e ativa o botão de continuar
    }
}
