using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
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

        StartCoroutine(LoginRequest(login, password));
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

    public void handleSignUpButton()
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
    class SignUpRequestBody
    {
        public string newUsername;
        public string newEmail;
        public string newPassword;
    }
    [Serializable]
    class LoginRequestBody
    {
        public string email;
        public string password;
    }

    private IEnumerator LoginRequest(string email, string password)
    {
        LoginRequestBody requestBody = new LoginRequestBody
        {
            email = email,
            password = password
        };

        string jsonData = JsonUtility.ToJson(requestBody);

        UnityWebRequest request = new UnityWebRequest(apiUrl + "/login", "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Debug.Log("Resposta recebida: " + request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Erro na requisição: " + request.error);
            yield break;
        }

        if (request.responseCode == 200)
        {
            cameraAnimator.SetBool("authorized", true);
            Debug.Log("animation: " + cameraAnimator.runtimeAnimatorController.animationClips[0].name + " / duration" + cameraAnimator.runtimeAnimatorController.animationClips[0].length);
            Debug.Log("animation: " + cameraAnimator.runtimeAnimatorController.animationClips[1].name + " / duration" + cameraAnimator.runtimeAnimatorController.animationClips[1].length);
            Debug.Log("animation: " + cameraAnimator.runtimeAnimatorController.animationClips[2].name + " / duration" + cameraAnimator.runtimeAnimatorController.animationClips[2].length);
            yield return WaitForAnimationToEnd(3f);

            SceneManager.LoadScene("test_track");
        }


    }

    private IEnumerator SignUpRequest(string username, string password, string email)
    {
        SignUpRequestBody requestBody = new SignUpRequestBody
        {
            newUsername = username,
            newEmail = email,
            newPassword = password
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
            message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = request.error;
            Debug.LogError("Erro na requisição: " + request.error);
            yield break;
        }

        Debug.Log("Resposta recebida: " + request.downloadHandler.text);

        if (request.responseCode == 201)
        {
            message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
            message.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = request.downloadHandler.text;
            ClearInputFields();
            message.SetActive(true);
        }
    }

    private void ClearInputFields()
    {
        canvasNewUsername.GetComponent<TMP_InputField>().text = "";
        canvasNewEmail.GetComponent<TMP_InputField>().text = "";
        canvasNewPassword.GetComponent<TMP_InputField>().text = "";
        canvasNewPasswordConfirm.GetComponent<TMP_InputField>().text = "";
    }

    private IEnumerator WaitForAnimationToEnd(float time)
    {
        yield return new WaitForSeconds(time);
    }

}

