using System;
using System.Collections;
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
    public GameObject canvasNewUsername;
    public GameObject canvasNewEmail;
    public GameObject canvasNewPassword;
    public GameObject canvasNewPasswordConfirm;
    public GameObject message;

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
        login = canvasLogin.GetComponent<TMP_InputField>().text;
        password = canvasPassword.GetComponent<TMP_InputField>().text;

        login = SanitizeInput(login);
        password = SanitizeInput(password);

        if (login == "admin" && password == "admin")
        {
            Debug.Log("Login successful");
            SceneManager.LoadScene("test_track");
        }
    }

    public void handleSignUpSubmitButton()
    {
        newUsername = canvasNewUsername.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        newPassword = canvasNewPassword.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        newPasswordConfirm = canvasNewPasswordConfirm.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
        newEmail = canvasNewEmail.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;

        bool isPasswordValid = SanitizeInput(newPassword) == SanitizeInput(newPasswordConfirm);
        bool isEmailValid = Regex.IsMatch(newEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        if (!isPasswordValid)
        {
            Debug.Log("Passwords do not match");
            return;
        }

        if (!isEmailValid)
        {
            Debug.Log("Invalid email");
            return;
        }

        StartCoroutine(SignUpRequest(newUsername, newPassword, newEmail));
    }

    public void handleNewPlayerButton()
    {
        cameraAnimator.SetBool("signup", true);
    }

    public void handleBackButton()
    {
        cameraAnimator.SetBool("signup", false);
    }


    private string SanitizeInput(string input)
    {
        return Regex.Replace(input, @"\p{C}+", "").Trim();
    }

    [Serializable]
    class RequestBody
    {
        public string username;
        public string email;
        public string password;
    }

    private IEnumerator SignUpRequest(string newUsername, string newPassword, string newEmail)
    {
        RequestBody requestBody = new RequestBody
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
            Debug.LogError("Erro na requisição: " + request.error);
        }
        else
        {
            Debug.Log("Resposta recebida: " + request.downloadHandler.text);
            if (request.responseCode == 201)
            {
                message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player created";
                ClearInputFields();
                message.SetActive(true);
            }
        }
    }

    private void ClearInputFields()
    {
        canvasNewUsername.GetComponent<TMP_InputField>().text = "";
        canvasNewEmail.GetComponent<TMP_InputField>().text = "";
        canvasNewPassword.GetComponent<TMP_InputField>().text = "";
        canvasNewPasswordConfirm.GetComponent<TMP_InputField>().text = "";
    }
}
