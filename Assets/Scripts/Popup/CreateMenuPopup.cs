using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateMenuPopup : MonoBehaviour
{
    [SerializeField] GameObject characterStats;

    void Start()
    {
        Action onPopupOpen = () =>
        {
            characterStats.SetActive(false);
        };

        Action onPopupClose = () =>
        {
            characterStats.SetActive(true);
        };

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            Debug.Log("Opening Menu Popup");
            MenuPopup popup = UIController.Instance.CreateMenuPopup();
            popup.Init(UIController.Instance.MainCanvas, onPopupOpen, onPopupClose);
        });
    }
}
