using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    public Text volumeText; // Opcional, para exibir o valor do volume

    private const string VolumePrefKey = "Volume";

    void Start()
    {
        // Carrega o volume salvo ou usa o valor padrão de 100
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 100f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        // Adiciona um listener ao slider para chamar a função SetVolume sempre que o valor mudar
        volumeSlider.onValueChanged.AddListener(delegate { SetVolume(volumeSlider.value); });
    }

    public void SetVolume(float volume)
    {
        // Atualiza o texto do volume, se fornecido
        if (volumeText != null)
        {
            volumeText.text = volume.ToString("0");
        }

        // Salva o volume no PlayerPrefs
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();

        Debug.Log("Volume salvo >> " + PlayerPrefs.GetFloat(VolumePrefKey));
    }
}
