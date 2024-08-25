using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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

    private string login;
    private string password;

    public Animator uiAnimator;

    public void handleSubmitButton()
    {
        login = canvasLogin.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        password = canvasPassword.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;

        login = Regex.Replace(login, @"\p{C}+", "").Trim();
        password = Regex.Replace(password, @"\p{C}+", "").Trim();

        if (login == "admin" && password == "admin")
        {
            UnityEngine.Debug.Log("Login successful");
            SceneManager.LoadScene("test_track");
        }
    }

    public void handleNewPlayerButton()
    {
        uiAnimator.SetBool("signup", true);
    }

    public void handleBackButton()
    {
        uiAnimator.SetBool("signup", false);
    }


}
