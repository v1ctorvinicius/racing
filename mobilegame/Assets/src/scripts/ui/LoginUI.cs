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

    public void Start()
    {
        // scene = SceneManager.GetActiveScene();
        // canvas = scene.GetRootGameObjects();
    }

    public void handleSubmitButton()
    {



        String login = canvasLogin.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        String senha = canvasPassword.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        UnityEngine.Debug.Log(login);
        UnityEngine.Debug.Log(senha);


        // if (login == "" || senha == "")
        // {
        //     return;
        // }

        if (login == "admin" && password == "admin")
        {
            SceneManager.LoadScene("test_track");
        }

    }


}
