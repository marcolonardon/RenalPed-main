using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MenuPopup : MonoBehaviour
{

    [SerializeField] Button _button1;
    [SerializeField] Button _characterButton;
    [SerializeField] Button _homeButton;
    [SerializeField] Button _infoButton;
    [SerializeField] Button _rankingButton;

    private Action onPopupOpen;
    private Action onPopupClose; 
    public void Init(Transform canvas, Action onPopupOpen, Action onPopupClose)
    {

        this.onPopupOpen = onPopupOpen;
        this.onPopupClose = onPopupClose;
        transform.SetParent(canvas);
        transform.localScale = Vector3.one;
        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;

        _button1.onClick.AddListener(() => {
            Close();
        });

        _characterButton.onClick.AddListener(() => {
            SceneManager.LoadScene("CharacterPage1");
            GameObject.Destroy(this.gameObject);
        });

        _homeButton.onClick.AddListener(() => {
            SceneManager.LoadScene("InitialPage");
            GameObject.Destroy(this.gameObject);
        });

        _infoButton.onClick.AddListener(() => {
            SceneManager.LoadScene("InfoPage");
            GameObject.Destroy(this.gameObject);
        });

        _rankingButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Ranking");
            GameObject.Destroy(this.gameObject);
        });

        onPopupOpen?.Invoke();

    }

    public void Close()
    {
        onPopupClose?.Invoke();
        Destroy(gameObject); 
    }


}
