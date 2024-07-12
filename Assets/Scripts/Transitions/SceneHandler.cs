using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public string sceneToLoad;

    [SerializeField] RectTransform fader;
    //[SerializeField] RectTransform fader2;
    void Start()
    {
        ScenesManager();
    }


    private void ScenesManager()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Bonus1" || currentSceneName == "Bonus2" || currentSceneName == "Bonus3")
        {
            fader.gameObject.SetActive(true);
            //fader2.gameObject.SetActive(false);

            LeanTween.scale(fader, new Vector3(1, 1, 1), 0);
            LeanTween.scale(fader, Vector3.zero, 1.5f).setEase(LeanTweenType.easeInOutQuart).setOnComplete(() =>
            {
                fader.gameObject.SetActive(false);
                //fader2.gameObject.SetActive(false);
            });
        }else if(currentSceneName == "SpeechBubblePage")
        {
            fader.gameObject.SetActive(true);
            //fader2.gameObject.SetActive(false);

            LeanTween.scale(fader, new Vector3(1, 1, 1), 0);
            LeanTween.move(fader, Vector3.zero, 1.5f).setOnComplete(() =>
            {
                fader.gameObject.SetActive(false);
                //fader2.gameObject.SetActive(false);
            });
        }
        else if(currentSceneName == "Menu")
        {
            fader.gameObject.SetActive(true);
            //fader2.gameObject.SetActive(false);

            LeanTween.scale(fader, new Vector3(1, 1, 1), 0);
            LeanTween.move(fader, Vector3.zero, 1.5f).setOnComplete(() =>
            {
                fader.gameObject.SetActive(false);
                //fader2.gameObject.SetActive(false);
            });
        }
        else
        {
            fader.gameObject.SetActive(true);
            //fader2.gameObject.SetActive(false);

            LeanTween.scale(fader, new Vector3(1, 1, 1), 0);
            LeanTween.move(fader, Vector3.zero, 4f).setOnComplete(() =>
            {
                fader.gameObject.SetActive(false);
                //fader2.gameObject.SetActive(false);
            });
        }
    }

    public void BonusTransition()
    {

         SceneManager.LoadScene(sceneToLoad);
         /*
         fader2.gameObject.SetActive(true);
         LeanTween.scale(fader2, Vector3.zero, 0f);
         LeanTween.scale(fader2, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(() =>
         {
        Invoke("LoadScene", 0.5f);
    });
    
    */

    }




    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
