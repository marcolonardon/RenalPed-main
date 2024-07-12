using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class CreateBonusPopup : MonoBehaviour
{
    public string TitleText;
    public string LeftSheetText;
    public string RightSheetText;
    void Start()
    {
        Action action = () =>
        {
            //SceneManager.LoadScene(sceneToLoad);
        };

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            BonusPopup popup = UIController.Instance.CreateBonusPopup();
            popup.Init(UIController.Instance.MainCanvas,
                TitleText, // titulo
                LeftSheetText, // mensagem do topo
                RightSheetText, // botão verde
                action
                );
        });
    }


}
