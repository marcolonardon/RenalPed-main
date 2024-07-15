using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] Button _button1;

    public void Init(Transform canvas)
    {

        transform.SetParent(canvas);
        transform.localScale = Vector3.one;
        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;

        _button1.onClick.AddListener(() => {
            SceneManager.LoadScene("Menu");
            Close();
        });

    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
