using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public Text MainBubbleText;
    public Text TopBubbleText;
    public string[] mainText;
    public string[] topText;
    public string sceneToLoad;

    public Image image1;
    public Image image2;

    public int loadImage1At;
    public int loadImage2At;

    public Button targetButton; // Botão específico para transição de cena

    private int mainIndex = 0, topIndex = 0;

    private bool isClickAllowed = true; // Flag para controle de clique duplo
    private float clickDelay = .4f; // Delay entre cliques permitidos

    void Start()
    {
        MainBubbleText.text = mainText[0];
        TopBubbleText.text = topText[0];

        // Desativa as imagens no início
        if (image1 != null)
            image1.gameObject.SetActive(false);
        if (image2 != null)
            image2.gameObject.SetActive(false);

        // Associa o botão ao método de mudança de cena
        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(false);
            targetButton.onClick.AddListener(() => StartCoroutine(LoadNextScene()));
        }
    }

    public void onClick()
    {
        if (isClickAllowed)
        {
            isClickAllowed = false;
            StartCoroutine(HandleClick());

            if (mainIndex < mainText.Length - 1)
            {
                mainIndex++;
                PlayerPrefs.SetInt("SpeechBubbleAudioIndex", mainIndex);
            }

            if (topIndex < topText.Length - 1)
            {
                topIndex++;
            }

            MainBubbleText.text = mainText[mainIndex];
            TopBubbleText.text = topText[topIndex];

            // Verifica se o índice atual é o índice em que as imagens devem ser ativadas
            if (mainIndex == loadImage1At)
            {
                image1.gameObject.SetActive(true);
            }

            if (mainIndex == loadImage2At)
            {
                image2.gameObject.SetActive(true);
            }

            StartMission();
        }
    }

    private IEnumerator HandleClick()
    {
        yield return new WaitForSeconds(clickDelay);
        isClickAllowed = true;
    }

    private void StartMission()
    {
        if (mainIndex == mainText.Length - 1 && targetButton != null)
        {
            // Ativa o botão específico para transição de cena
            targetButton.gameObject.SetActive(true);
        }
    }

    private IEnumerator LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
        yield return null;
    }
}
