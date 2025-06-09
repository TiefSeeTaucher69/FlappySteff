using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class FirstOpen : MonoBehaviour
{
    public InputField usernameInput;
    public Text feedbackText;

    [System.Serializable]
    public class UsernameCheckResponse
    {
        public bool exists;
    }

    public void SaveUsername()
    {
        string username = usernameInput.text.Trim();

        if (string.IsNullOrWhiteSpace(username))
        {
            feedbackText.text = "Name darf nicht leer sein.";
            return;
        }

        StartCoroutine(CheckUsernameExists(username));
    }

    private IEnumerator CheckUsernameExists(string username)
    {
        string url = "https://benjoapi.zapto.org/username-exists?name=" + UnityWebRequest.EscapeURL(username);
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            feedbackText.text = "Serverfehler: " + request.error;
            yield break;
        }

        UsernameCheckResponse response = JsonUtility.FromJson<UsernameCheckResponse>(request.downloadHandler.text);

        if (response.exists)
        {
            feedbackText.text = "Name bereits vergeben.";
        }
        else
        {
            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.Save();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
