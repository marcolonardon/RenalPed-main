using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class SavePlayerPrefs : MonoBehaviour
{
    public Text ShowVersion;
    public Text StatusMessage;
    public Button PlayButton;

    User user = new User();


    async void Start()
    {

        // Desativa o botão PlayButton
        PlayButton.interactable = false;
        StatusMessage.text = "Verificando Atualizações";

        // Verifica a versão do bd para atualizar
        await CheckUpdates();

        // Atualiza o texto da versão
        ShowVersion.text = "V: " + PlayerPrefs.GetFloat("DataBaseVersion");

        // Reativa o botão PlayButton
        PlayButton.interactable = true;
        StatusMessage.text = "";
    }


    private async Task CheckUpdates()
    {
        StatusMessage.text = "Verificando Atualizações";
        float oldVersion = PlayerPrefs.GetFloat("DataBaseVersion");
        float newVersion;
        Debug.Log("OldVersion: " + oldVersion);

        // Espera a conclusão de getDataBaseVersion
        await getDataBaseVersion();

        newVersion = PlayerPrefs.GetFloat("DataBaseVersion");
        Debug.Log("NewVersion: " + newVersion);

        // Após obter os dados, verifica se houve mudança na versão
        if (oldVersion != newVersion)
        {
            Debug.Log("Versão desatualizada. Baixando nova");
            StatusMessage.text = "Atualizando...";
            await RetriveFromDatabase();
            await Task.Delay(1000);
        }
        else
        {
            Debug.Log("Não precisa baixar dados novos");
        }

        ShowVersion.text = "V: " + PlayerPrefs.GetFloat("DataBaseVersion");
        StatusMessage.text = "";
    }


    private async Task getTotalQuizQuestions()
    {
        // Cria um TaskCompletionSource para controlar a conclusão da requisição
        var tcs = new TaskCompletionSource<bool>();

        RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/QuizQuestions/.json").Then(response =>
        {
            PlayerPrefs.SetInt("TotalQuizQuestions", response.total_quiz_questions);
            PlayerPrefs.Save();
            tcs.SetResult(true); // Marca a tarefa como concluída com sucesso
        }).Catch(error =>
        {
            Debug.LogError("Erro ao obter dados do banco de dados: " + error);
            tcs.SetException(new System.Exception("Erro na requisição de TotalQuizQuestions")); // Marca a tarefa com exceção em caso de erro
        });

        // Aguarda a conclusão da tarefa
        await tcs.Task;
    }


    private async Task getQuizQuestions()
    {
        int totalQuestions = PlayerPrefs.GetInt("TotalQuizQuestions");

        // Lista para armazenar as tarefas individuais de requisição
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < totalQuestions; i++)
        {
            int questionIndex = i;

            // Cria um TaskCompletionSource para cada requisição individual
            var tcs = new TaskCompletionSource<bool>();

            RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/QuizQuestions/questions/" + i + "/.json").Then(response =>
            {
                PlayerPrefs.SetString("QuizQuestion" + questionIndex, response.question);
                PlayerPrefs.SetString("QuizCorrectAnswer" + questionIndex, response.correct_answer);
                PlayerPrefs.Save();

                // Completa a task quando a requisição é bem-sucedida
                tcs.SetResult(true);
            }).Catch(error =>
            {
                Debug.LogError("Erro ao obter dados do banco de dados: " + error);

                // Completa a task com exceção em caso de erro
                tcs.SetException(new System.Exception("Erro na requisição de QuizQuestions"));
            });

            // Adiciona a task do TaskCompletionSource à lista de tasks
            tasks.Add(tcs.Task);
        }

        // Aguarda a conclusão de todas as tasks na lista
        await Task.WhenAll(tasks);
    }


    private async Task getQuizAnswers()
    {
        int totalQuestions = PlayerPrefs.GetInt("TotalQuizQuestions");

        // Lista para armazenar as tarefas individuais de requisição
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < totalQuestions; i++)
        {
            int questionIndex = i;

            // Cria um TaskCompletionSource para cada requisição individual
            var tcs = new TaskCompletionSource<bool>();

            RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/QuizQuestions/questions/" + i + "/answer/.json").Then(response =>
            {
                PlayerPrefs.SetString("QuizAnswerA" + questionIndex, response.a_answer);
                PlayerPrefs.SetString("QuizAnswerB" + questionIndex, response.b_answer);
                PlayerPrefs.SetString("QuizAnswerC" + questionIndex, response.c_answer);
                PlayerPrefs.Save();

                // Completa a task quando a requisição é bem-sucedida
                tcs.SetResult(true);
            }).Catch(error =>
            {
                Debug.LogError("Erro ao obter dados do banco de dados: " + error);

                // Completa a task com exceção em caso de erro
                tcs.SetException(new System.Exception("Erro na requisição de QuizAnswers"));
            });

            // Adiciona a task do TaskCompletionSource à lista de tasks
            tasks.Add(tcs.Task);
        }

        // Aguarda a conclusão de todas as tasks na lista
        await Task.WhenAll(tasks);
    }


    private async Task getTotalTrueFalseQuestions()
    {
        // Cria um TaskCompletionSource para sinalizar a conclusão da requisição
        var tcs = new TaskCompletionSource<bool>();

        RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/TrueFalseQuestions/.json").Then(response =>
        {
            PlayerPrefs.SetInt("TotalTrueFalseQuestions", response.total_true_false_questions);
            PlayerPrefs.Save();
            //Debug.Log(PlayerPrefs.GetInt("TotalTrueFalseQuestions"));

            // Completa a task quando a requisição é bem-sucedida
            tcs.SetResult(true);
        }).Catch(error =>
        {
            Debug.LogError("Erro ao obter dados do banco de dados: " + error);

            // Completa a task com exceção em caso de erro
            tcs.SetException(new System.Exception("Erro na requisição de TotalTrueFalseQuestions"));
        });

        // Aguarda a conclusão da task antes de retornar
        await tcs.Task;
    }




    private async Task getDataBaseVersion()
    {
        var tcs = new TaskCompletionSource<bool>();

        RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/Version/.json").Then(response =>
        {
            PlayerPrefs.SetFloat("DataBaseVersion", response.data_base_version);
            PlayerPrefs.Save();
            Debug.Log("Versão Atualizada: " + PlayerPrefs.GetFloat("DataBaseVersion"));
            tcs.SetResult(true);
        }).Catch(error =>
        {
            Debug.LogError("Erro ao obter dados do banco de dados: " + error);
            tcs.SetException(error);
        });

        await tcs.Task;
    }






    private async Task getTrueFalseQuestions()
    {
        int totalQuestions = PlayerPrefs.GetInt("TotalTrueFalseQuestions");

        // Lista para armazenar as tarefas individuais de requisição
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < totalQuestions; i++)
        {
            int questionIndex = i;

            // Cria um TaskCompletionSource para cada requisição individual
            var tcs = new TaskCompletionSource<bool>();

            RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/TrueFalseQuestions/questions/" + i + "/.json").Then(response =>
            {
                PlayerPrefs.SetString("TrueFalseAnswer" + questionIndex, response.answer);
                PlayerPrefs.SetString("TrueFalseQuestion" + questionIndex, response.question);
                PlayerPrefs.Save();

                // Completa a task quando a requisição é bem-sucedida
                tcs.SetResult(true);
            }).Catch(error =>
            {
                Debug.LogError("Erro ao obter dados do banco de dados: " + error);

                // Completa a task com exceção em caso de erro
                tcs.SetException(new System.Exception("Erro na requisição de TrueFalseQuestions"));
            });

            // Adiciona a task do TaskCompletionSource à lista de tasks
            tasks.Add(tcs.Task);
        }

        // Aguarda a conclusão de todas as tasks na lista
        await Task.WhenAll(tasks);
    }


    private async Task getTotalCircleSlotQuestions()
    {
        var tcs = new TaskCompletionSource<bool>();

        RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/CircleSlotQuestions/.json").Then(response =>
        {
            PlayerPrefs.SetInt("TotalCircleSlotQuestions", response.total_circle_slot_questions);
            PlayerPrefs.Save();
            Debug.Log("Salvou TotalCircleSlotQuestions");
            tcs.SetResult(true);
        }).Catch(error =>
        {
            Debug.LogError("Erro ao obter dados do banco de dados: " + error);
            tcs.SetException(new System.Exception("Erro na requisição de TotalCircleSlotQuestions"));
        });

        await tcs.Task;
    }



    private async Task getCircleSlotQuestions()
    {
        int totalQuestions = PlayerPrefs.GetInt("TotalCircleSlotQuestions");
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < totalQuestions; i++)
        {
            int questionIndex = i;
            var tcs = new TaskCompletionSource<bool>();

            RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/CircleSlotQuestions/questions/" + i + "/.json").Then(response =>
            {
                PlayerPrefs.SetString("CircleSlotAnswer" + questionIndex, response.answer);
                PlayerPrefs.SetString("CircleSlotQuestion" + questionIndex, response.question);
                PlayerPrefs.Save();
                Debug.Log("Salvou CircleSlotAnswer e CircleSlotQuestion");
                tcs.SetResult(true);
            }).Catch(error =>
            {
                Debug.LogError("Erro ao obter dados do banco de dados: " + error);
                tcs.SetException(new System.Exception("Erro na requisição de CircleSlotQuestions"));
            });

            tasks.Add(tcs.Task);
        }

        await Task.WhenAll(tasks);
    }




    public async Task RetriveFromDatabase()
    {
        await getTotalQuizQuestions();
        await getTotalTrueFalseQuestions();
        await getTotalCircleSlotQuestions();
        await getQuizQuestions();
        await getQuizAnswers();
        await getCircleSlotQuestions();
        await getTrueFalseQuestions();
    }


}
