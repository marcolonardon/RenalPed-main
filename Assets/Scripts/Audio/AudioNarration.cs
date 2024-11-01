using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioNarration : MonoBehaviour
{
    public AudioClip[] narrationClip; // O clipe de áudio que será reproduzido
    private AudioSource audioSource;
    private const string VolumePrefKey = "Volume";

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayNarration(Button clickedButton)
    {
        // Se o áudio estiver tocando, pare-o e saia do método
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            return;
        }

        // Carregar o volume salvo e aplicá-lo ao AudioSource
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 100f);
        SetVolume(savedVolume);

        // Verificar a cena ativa e definir o índice do áudio correspondente
        if (SceneManager.GetActiveScene().name == "FoodQuizPage")
        {
            int questionIndex;

            // Verifica o nome do botão clicado
            if (clickedButton.name == "AudioButton")
            {
                questionIndex = PlayerPrefs.GetInt("FoodQuestionAudioIndex", 0);
                Debug.Log("Clicou no botão com audio no index " + questionIndex);
            }
            else
            {
                questionIndex = PlayerPrefs.GetInt("FoodPopupAudioIndex");
            }

            // Garante que o índice esteja dentro dos limites do array narrationClip
            if (questionIndex >= 0 && questionIndex < narrationClip.Length)
            {
                audioSource.clip = narrationClip[questionIndex];
            }
            else
            {
                Debug.LogWarning("Índice fora dos limites ou nenhum clipe de narração foi atribuído para FoodQuizPage.");
            }
        }
        else if (SceneManager.GetActiveScene().name == "SpeechBubblePage" || SceneManager.GetActiveScene().name == "SpeechBubblePage3")
        {
            int speechBubbleIndex = PlayerPrefs.GetInt("SpeechBubbleAudioIndex", 0);
            Debug.Log("INDEXXXXXXXXXXXXXXXXXXXX " + speechBubbleIndex);

            if (speechBubbleIndex >= 0 && speechBubbleIndex < narrationClip.Length)
            {
                audioSource.clip = narrationClip[speechBubbleIndex];
            }
            else
            {
                Debug.LogWarning("Índice fora dos limites ou nenhum clipe de narração foi atribuído para SpeechBubblePage.");
            }
        }
        else if (SceneManager.GetActiveScene().name == "QuizPage")
        {
            int quizIndex = PlayerPrefs.GetInt("QuizAudioIndex", 0);

            if (quizIndex >= 0 && quizIndex < narrationClip.Length)
            {
                audioSource.clip = narrationClip[quizIndex];
            }
            else
            {
                Debug.LogWarning("Índice fora dos limites ou nenhum clipe de narração foi atribuído para QuizPage.");
            }
        }
        else
        {
            // Código original: toca o primeiro áudio do array
            if (narrationClip != null && narrationClip.Length > 0)
            {
                audioSource.clip = narrationClip[0];
            }
            else
            {
                Debug.LogWarning("Nenhum clipe de narração foi atribuído.");
            }
        }

        // Reproduz o áudio desde o início
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    private void SetVolume(float volume)
    {
        // Garante que o volume esteja entre 0 e 200
        volume = Mathf.Clamp(volume, 0, 200);

        // Normaliza o volume para o AudioSource (0 a 1)
        audioSource.volume = volume / 200f;

        Debug.Log("Volume configurado: " + volume + " (AudioSource volume: " + audioSource.volume + ")");
    }

    public void ResetAudioIndex()
    {
        Debug.Log("Resetou o índice de áudio");
        PlayerPrefs.SetInt("SpeechBubbleAudioIndex", 0);
        PlayerPrefs.Save();
    }
}
