using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class FoodPopup : MonoBehaviour
{
    public Sprite[] KidneysSprite;
    [SerializeField] Image _image; 
    [SerializeField] Button _button1;
    [SerializeField] Text _mainPopupText;

    public void Init(Transform canvas, string popupMessage, Action action)
    {
        _mainPopupText.text = popupMessage;

        //Debug.Log("IndexHelper antes: " + indexHelper);
        SetKidneysSprite(KidneysSprite[IndexIncreaser()]);
        IndexIncreaser();
        //Debug.Log("IndexHelper depois: " + indexHelper);


        transform.SetParent(canvas);
        transform.localScale = Vector3.one;
        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;

        _button1.onClick.AddListener(() => {
            action();

            GameObject.Destroy(this.gameObject);
        });


    }


    private void SetKidneysSprite(Sprite sprite)
    {
        if (_image != null)
        {
            _image.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("O objeto de imagem não está atribuído.");
        }
    }


    private int IndexIncreaser()
    {
        int indexHelper;
        //Debug.Log("Entrou no indexIncreaser");
        indexHelper = PlayerPrefs.GetInt("FoodQuestionIndex");


        if (indexHelper > 0)
        {
            indexHelper *= 2;
        }

        if (PlayerPrefs.GetInt("FoodAnswer") == 1) // se acertou 
        {
            return indexHelper;
        }
        else
        {
            return (indexHelper + 1);
        }

            
    }
}

