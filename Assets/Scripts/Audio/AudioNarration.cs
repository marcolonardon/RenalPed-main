using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioNarration : MonoBehaviour
{
    public AudioClip[] narrationClip; // O clipe de �udio que ser� reproduzido
    private AudioSource audioSource;
    private const string VolumePrefKey = "Volume";

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayNarration(Button clickedButton)
    {
        // Se o �udio estiver tocando, pare-o e saia do m�todo
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            return;
        }

        // Carregar o volume salvo e aplic�-lo ao AudioSource
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 100f);
        SetVolume(savedVolume);

        // Verificar a cena ativa e definir o �ndice do �udio correspondente
        if (SceneManager.GetActiveScene().name == "FoodQuizPage")
        {
            int questionIndex;

            // Verifica o nome do bot�o clicado
            if (clickedButton.name == "AudioButton")
            {
                questionIndex = PlayerPrefs.GetInt("FoodQuestionAudioIndex", 0);
                Debug.Log("Clicou no bot�o com audio no index " + questionIndex);
            }
            else
            {
                questionIndex = PlayerPrefs.GetInt("FoodPopupAudioIndex");
            }

            // Garante que o �ndice esteja dentro dos limites do array narrationClip
            if (questionIndex >= 0 && questionIndex < narrationClip.Length)
            {
                audioSource.clip = narrationClip[questionIndex];
            }
            else
            {
                Debug.LogWarning("�ndice fora dos limites ou nenhum clipe de narra��o foi atribu�do para FoodQuizPage.");
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
                Debug.LogWarning("�ndice fora dos limites ou nenhum clipe de narra��o foi atribu�do para SpeechBubblePage.");
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
                Debug.LogWarning("�ndice fora dos limites ou nenhum clipe de narra��o foi atribu�do para QuizPage.");
            }
        }
        else
        {
            // C�digo original: toca o primeiro �udio do array
            if (narrationClip != null && narrationClip.Length > 0)
            {
                audioSource.clip = narrationClip[0];
            }
            else
            {
                Debug.LogWarning("Nenhum clipe de narra��o foi atribu�do.");
            }
        }

        // Reproduz o �udio desde o in�cio
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
        Debug.Log("Resetou o �ndice de �udio");
        PlayerPrefs.SetInt("SpeechBubbleAudioIndex", 0);
        PlayerPrefs.Save();
    }
}
