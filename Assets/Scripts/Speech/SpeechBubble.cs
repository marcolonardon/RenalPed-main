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

    private int mainIndex = 0, topIndex = 0;

    private bool isClickAllowed = true; // Flag para controle de clique duplo
    private float clickDelay = .8f; // Delay entre cliques permitidos

    void Start()
    {
        MainBubbleText.text = mainText[0];
        TopBubbleText.text = topText[0];

        // Desativa as imagens no início
        image1.gameObject.SetActive(false);
        image2.gameObject.SetActive(false);
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
        if (mainIndex == mainText.Length - 1 && topIndex == topText.Length - 1)
        {
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        // Aguarda até o clique
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
