using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class BonusPopup : MonoBehaviour
{

    [SerializeField] Button _button1;
    [SerializeField] Text _titleText;
    [SerializeField] Text _leftSheetText;
    [SerializeField] Text _rightSheetText;
    public void Init(Transform canvas, string TitleText, string LeftSheetText, string RightSheetText, Action action)
    {
        _titleText.text = TitleText;
        _leftSheetText.text = LeftSheetText;
        _rightSheetText.text = RightSheetText;

        transform.SetParent(canvas);
        transform.localScale = Vector3.one;
        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;

        _button1.onClick.AddListener(() => {
            GameObject.Destroy(this.gameObject);
        });



    }


}
