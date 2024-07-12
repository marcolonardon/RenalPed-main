/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class TesteCriarPopup : MonoBehaviour
{
   //public string sceneToLoad;
   //public string TopText;
   //public string MainText;
   //public string LeftButtonText;
   //public string RightButtonText;

    // Start is called before the first frame update
    void Start()
    {
        Action action = () =>
        {
            //SceneManager.LoadScene(sceneToLoad);
        };

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            Popup popup = UIController.Instance.CreatePopup();
            popup.Init(UIController.Instance.MainCanvas,
               //MainText, // mensagem principal 
               //TopText, // mensagem do topo
               //LeftButtonText, // botão vermelho
               //RightButtonText, // botão verde
                action
                );
        });
    }


}

*/