using UnityEngine;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{

    public static UIController Instance;

    public Transform MainCanvas;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public MenuPopup CreateMenuPopup()
    {
        GameObject popUpGo = Instantiate(Resources.Load("UI/MenuPopUp") as GameObject);
        return popUpGo.GetComponent<MenuPopup>();
    }


    public BonusPopup CreateBonusPopup()
    {
        GameObject popUpGo = Instantiate(Resources.Load("UI/BonusPopup") as GameObject);
        return popUpGo.GetComponent<BonusPopup>();
    }

    public FoodPopup CreateFoodPopup()
    {
        GameObject popUpGo = Instantiate(Resources.Load("UI/FoodCharacterPopup") as GameObject);
        return popUpGo.GetComponent<FoodPopup>();
    }

    public ScorePopup CreateScorePopup()
    {
        GameObject popUpGo = Instantiate(Resources.Load("UI/ScorePopup") as GameObject);
        return popUpGo.GetComponent<ScorePopup>();
    }

}