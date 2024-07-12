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

        // Desativa o bot�o PlayButton
        PlayButton.interactable = false;
        StatusMessage.text = "Verificando Atualiza��es";

        // Verifica a vers�o do bd para atualizar
        await CheckUpdates();

        // Atualiza o texto da vers�o
        ShowVersion.text = "V: " + PlayerPrefs.GetFloat("DataBaseVersion");

        // Reativa o bot�o PlayButton
        PlayButton.interactable = true;
        StatusMessage.text = "";
    }


    private async Task CheckUpdates()
    {
        StatusMessage.text = "Verificando Atualiza��es";
        float oldVersion = PlayerPrefs.GetFloat("DataBaseVersion");
        float newVersion;
        Debug.Log("OldVersion: " + oldVersion);

        // Espera a conclus�o de getDataBaseVersion
        await getDataBaseVersion();

        newVersion = PlayerPrefs.GetFloat("DataBaseVersion");
        Debug.Log("NewVersion: " + newVersion);

        // Ap�s obter os dados, verifica se houve mudan�a na vers�o
        if (oldVersion != newVersion)
        {
            Debug.Log("Vers�o desatualizada. Baixando nova");
            StatusMessage.text = "Atualizando...";
            await RetriveFromDatabase();
            await Task.Delay(1000);
        }
        else
        {
            Debug.Log("N�o precisa baixar dados novos");
        }

        ShowVersion.text = "V: " + PlayerPrefs.GetFloat("DataBaseVersion");
        StatusMessage.text = "";
    }


    private async Task getTotalQuizQuestions()
    {
        // Cria um TaskCompletionSource para controlar a conclus�o da requisi��o
        var tcs = new TaskCompletionSource<bool>();

        RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/QuizQuestions/.json").Then(response =>
        {
            PlayerPrefs.SetInt("TotalQuizQuestions", response.total_quiz_questions);
            PlayerPrefs.Save();
            tcs.SetResult(true); // Marca a tarefa como conclu�da com sucesso
        }).Catch(error =>
        {
            Debug.LogError("Erro ao obter dados do banco de dados: " + error);
            tcs.SetException(new System.Exception("Erro na requisi��o de TotalQuizQuestions")); // Marca a tarefa com exce��o em caso de erro
        });

        // Aguarda a conclus�o da tarefa
        await tcs.Task;
    }


    private async Task getQuizQuestions()
    {
        int totalQuestions = PlayerPrefs.GetInt("TotalQuizQuestions");

        // Lista para armazenar as tarefas individuais de requisi��o
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < totalQuestions; i++)
        {
            int questionIndex = i;

            // Cria um TaskCompletionSource para cada requisi��o individual
            var tcs = new TaskCompletionSource<bool>();

            RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/QuizQuestions/questions/" + i + "/.json").Then(response =>
            {
                PlayerPrefs.SetString("QuizQuestion" + questionIndex, response.question);
                PlayerPrefs.SetString("QuizCorrectAnswer" + questionIndex, response.correct_answer);
                PlayerPrefs.Save();

                // Completa a task quando a requisi��o � bem-sucedida
                tcs.SetResult(true);
            }).Catch(error =>
            {
                Debug.LogError("Erro ao obter dados do banco de dados: " + error);

                // Completa a task com exce��o em caso de erro
                tcs.SetException(new System.Exception("Erro na requisi��o de QuizQuestions"));
            });

            // Adiciona a task do TaskCompletionSource � lista de tasks
            tasks.Add(tcs.Task);
        }

        // Aguarda a conclus�o de todas as tasks na lista
        await Task.WhenAll(tasks);
    }


    private async Task getQuizAnswers()
    {
        int totalQuestions = PlayerPrefs.GetInt("TotalQuizQuestions");

        // Lista para armazenar as tarefas individuais de requisi��o
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < totalQuestions; i++)
        {
            int questionIndex = i;

            // Cria um TaskCompletionSource para cada requisi��o individual
            var tcs = new TaskCompletionSource<bool>();

            RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/QuizQuestions/questions/" + i + "/answer/.json").Then(response =>
            {
                PlayerPrefs.SetString("QuizAnswerA" + questionIndex, response.a_answer);
                PlayerPrefs.SetString("QuizAnswerB" + questionIndex, response.b_answer);
                PlayerPrefs.SetString("QuizAnswerC" + questionIndex, response.c_answer);
                PlayerPrefs.Save();

                // Completa a task quando a requisi��o � bem-sucedida
                tcs.SetResult(true);
            }).Catch(error =>
            {
                Debug.LogError("Erro ao obter dados do banco de dados: " + error);

                // Completa a task com exce��o em caso de erro
                tcs.SetException(new System.Exception("Erro na requisi��o de QuizAnswers"));
            });

            // Adiciona a task do TaskCompletionSource � lista de tasks
            tasks.Add(tcs.Task);
        }

        // Aguarda a conclus�o de todas as tasks na lista
        await Task.WhenAll(tasks);
    }


    private async Task getTotalTrueFalseQuestions()
    {
        // Cria um TaskCompletionSource para sinalizar a conclus�o da requisi��o
        var tcs = new TaskCompletionSource<bool>();

        RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/TrueFalseQuestions/.json").Then(response =>
        {
            PlayerPrefs.SetInt("TotalTrueFalseQuestions", response.total_true_false_questions);
            PlayerPrefs.Save();
            //Debug.Log(PlayerPrefs.GetInt("TotalTrueFalseQuestions"));

            // Completa a task quando a requisi��o � bem-sucedida
            tcs.SetResult(true);
        }).Catch(error =>
        {
            Debug.LogError("Erro ao obter dados do banco de dados: " + error);

            // Completa a task com exce��o em caso de erro
            tcs.SetException(new System.Exception("Erro na requisi��o de TotalTrueFalseQuestions"));
        });

        // Aguarda a conclus�o da task antes de retornar
        await tcs.Task;
    }




    private async Task getDataBaseVersion()
    {
        var tcs = new TaskCompletionSource<bool>();

        RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/Version/.json").Then(response =>
        {
            PlayerPrefs.SetFloat("DataBaseVersion", response.data_base_version);
            PlayerPrefs.Save();
            Debug.Log("Vers�o Atualizada: " + PlayerPrefs.GetFloat("DataBaseVersion"));
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

        // Lista para armazenar as tarefas individuais de requisi��o
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < totalQuestions; i++)
        {
            int questionIndex = i;

            // Cria um TaskCompletionSource para cada requisi��o individual
            var tcs = new TaskCompletionSource<bool>();

            RestClient.Get<User>("https://renal-ped-f3573-default-rtdb.firebaseio.com/TrueFalseQuestions/questions/" + i + "/.json").Then(response =>
            {
                PlayerPrefs.SetString("TrueFalseAnswer" + questionIndex, response.answer);
                PlayerPrefs.SetString("TrueFalseQuestion" + questionIndex, response.question);
                PlayerPrefs.Save();

                // Completa a task quando a requisi��o � bem-sucedida
                tcs.SetResult(true);
            }).Catch(error =>
            {
                Debug.LogError("Erro ao obter dados do banco de dados: " + error);

                // Completa a task com exce��o em caso de erro
                tcs.SetException(new System.Exception("Erro na requisi��o de TrueFalseQuestions"));
            });

            // Adiciona a task do TaskCompletionSource � lista de tasks
            tasks.Add(tcs.Task);
        }

        // Aguarda a conclus�o de todas as tasks na lista
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
            tcs.SetException(new System.Exception("Erro na requisi��o de TotalCircleSlotQuestions"));
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
                tcs.SetException(new System.Exception("Erro na requisi��o de CircleSlotQuestions"));
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
