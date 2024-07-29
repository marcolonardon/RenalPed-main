using UnityEngine;

public class AudioNarration : MonoBehaviour
{
    public AudioClip[] narrationClip; // O clipe de áudio que será reproduzido
    private AudioSource audioSource;

    private const string VolumePrefKey = "Volume";

    public void PlayNarration()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Carregar o volume salvo e aplicá-lo ao AudioSource
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 100f);

        SetVolume(savedVolume);

        if (narrationClip != null)
        {
            audioSource.clip = narrationClip[0]; ///////////////////////////////
            audioSource.Play();
            Debug.Log("DeuPlay");
        }
        else
        {
            Debug.LogWarning("Nenhum clipe de narração foi atribuído.");
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
}
