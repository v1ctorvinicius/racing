using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{


    private Scene scene;
    public GameObject[] canvas;
    public GameObject canvasLogin;
    public GameObject canvasPassword;

    private String login;
    private String password;

    public void handleSubmitButton()
    {

        login = canvasLogin.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        password = canvasPassword.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;

        

        // StartCoroutine(GetDataFromAPI("https://localhost:8080/players"));

        // if (login == "" || password == "")
        // {
        //     return;
        // }

        if (login == "admin" && password == "admin")
        {
            SceneManager.LoadScene("test_track");
        }

    }


}
