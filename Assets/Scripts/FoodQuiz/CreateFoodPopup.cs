using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class CreateFoodPopup : MonoBehaviour
{
    //public string sceneToLoad;
    public string[] MainText;

    private int indexHelper;

    // Start is called before the first frame update
    void Start()
    {
        Action action = () =>
        {
            Debug.Log("Entrou no action");
            if (PlayerPrefs.GetInt("TotalAnswers") == 4)
            {
                SceneManager.LoadScene("Bonus1");
                Debug.Log("Mudou de cena pelo action");
            }
            
        };

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {

            if (PlayerPrefs.GetInt("FoodAnswer") == 1) // acertou a resposta
            {
                IndexIncreaser();

                FoodPopup popup = UIController.Instance.CreateFoodPopup();
                popup.Init(UIController.Instance.MainCanvas,
                    MainText[indexHelper], // mensagem principal 
                    action
                    );

                //Debug.Log(" index "+indexHelper);
                //Debug.Log("ACERTOU");

            }
            else
            {
                IndexIncreaser();

                FoodPopup popup = UIController.Instance.CreateFoodPopup();
                popup.Init(UIController.Instance.MainCanvas,
                    MainText[(indexHelper + 1)], // mensagem principal 
                    action
                    );

                //Debug.Log(" index " + (indexHelper + 1));
                //Debug.Log("ERROU");

            }


            
        });
    }



    private void IndexIncreaser()
    {
        indexHelper = PlayerPrefs.GetInt("FoodQuestionIndex");


        if (indexHelper > 0)
        {
            indexHelper *= 2;
        }
    }


}
