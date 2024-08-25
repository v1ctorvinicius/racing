using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{
    private Scene scene;
    public GameObject[] canvas;
    public GameObject canvasLogin;
    public GameObject canvasPassword;
    public GameObject canvasNewUsername;
    public GameObject canvasNewEmail;
    public GameObject canvasNewPassword;
    public GameObject canvasNewPasswordConfirm;

    private string login;
    private string password;
    private string newUsername;
    private string newPassword;
    private string newPasswordConfirm;
    private string newEmail;

    public Animator cameraAnimator;
    private string apiUrl = "http://localhost:8080/api/players";

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

    public void handleSignUpSubmitButton()
    {
        newUsername = canvasNewUsername.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        newPassword = canvasNewPassword.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        newPasswordConfirm = canvasNewPasswordConfirm.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        newEmail = canvasNewEmail.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;

        bool isPasswordValid = Regex.Replace(newPassword, @"\p{C}+", "").Trim() == Regex.Replace(newPasswordConfirm, @"\p{C}+", "").Trim();
        bool isEmailValid = Regex.IsMatch(newEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        if (!isPasswordValid)
        {
            UnityEngine.Debug.Log("Passwords do not match");
            return;
        }

        if (!isEmailValid)
        {
            UnityEngine.Debug.Log("Invalid email");
            return;
        }

        StartCoroutine(signUpRequest(newUsername, newPassword, newEmail));
    }

    public void handleNewPlayerButton()
    {
        cameraAnimator.SetBool("signup", true);
    }

    public void handleBackButton()
    {
        cameraAnimator.SetBool("signup", false);
    }


    [Serializable]
    class requestBody
    {
        public string username;
        public string email;
        public string password;
    };
    private IEnumerator signUpRequest(string newUsername, string newPassword, string newEmail)
    {
        requestBody requestBody = new requestBody
        {
            username = newUsername,
            email = newEmail,
            password = newPassword
        };

        string jsonData = JsonUtility.ToJson(requestBody);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            UnityEngine.Debug.LogError("Erro na requisição: " + request.error);
        }
        else
        {
            UnityEngine.Debug.Log("Resposta recebida: " + request.downloadHandler.text);
        }
    }


}


