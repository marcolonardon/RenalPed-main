using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownloader : MonoBehaviour
{

    void Start()
    {
        //StartCoroutine(DownloadFile());
    }

    public void DownloadFile()
    {
        WebClient client = new WebClient();
        client.DownloadFile("https://drive.google.com/uc?export=download&id=1eM42pccoFAP3V-XtcWZnposnHNwX_hzx", @"Assets\Scripts\Database\RenalPedDatabase.json");
        Debug.Log("baixou");
    }
}

/*public void OnButtonClick()
    {
        Debug.Log("Entrou na funcao");
        StartCoroutine(DownloadFile());

    }*/