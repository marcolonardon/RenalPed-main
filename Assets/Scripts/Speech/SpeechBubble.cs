using System;
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

    public Image[] images;


    public Button targetButton; // Botão específico para transição de cena

    private int mainIndex = 0, topIndex = 0;

    private bool isClickAllowed = true; // Flag para controle de clique duplo
    private float clickDelay = .4f; // Delay entre cliques permitidos

    void Start()
    {
        MainBubbleText.text = mainText[0];
        TopBubbleText.text = topText[0];

        // Desativa as imagens no início
        RemoveImages();

        // Associa o botão ao método de mudança de cena
        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(false);
            targetButton.onClick.AddListener(() => StartCoroutine(LoadNextScene()));
        }
    }

    void Update()
    {
       // PrintImages();
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
            PrintImages();

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

    private void PrintImages()
    {
        int index = PlayerPrefs.GetInt("SpeechBubbleAudioIndex", 0);
        switch (index)
        {
            case 2:
                images[0].gameObject.SetActive(true);
                images[1].gameObject.SetActive(false);
                images[2].gameObject.SetActive(false);
                images[3].gameObject.SetActive(false);
                break;
            case 3:
                images[0].gameObject.SetActive(false);
                images[1].gameObject.SetActive(true);
                images[2].gameObject.SetActive(false);
                images[3].gameObject.SetActive(false);
                break;
            case 4:
                images[0].gameObject.SetActive(false);
                images[1].gameObject.SetActive(false);
                images[2].gameObject.SetActive(true);
                images[3].gameObject.SetActive(false);
                break;
            case 5:
                images[0].gameObject.SetActive(false);
                images[1].gameObject.SetActive(false);
                images[2].gameObject.SetActive(false);
                images[3].gameObject.SetActive(true);
                break;
            default:
                RemoveImages();
                break;
        }
    }

    private void RemoveImages()
    {
        if (images != null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].gameObject.SetActive(false);
            }
        }
    }
}
