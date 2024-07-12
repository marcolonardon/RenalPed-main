using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class ArrowButton : MonoBehaviour
{

    public Button button;
    public Text buttonText;
    public string ButtonText;

    private void Start()
    {
        buttonText.text = ButtonText;
    }


    public void Action()
    {
        Debug.Log("Entrou no action");

    }


}
