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
        Debug.Log("Speichere Benutzernamen: " + usernameInput.text.Trim());
        string username = usernameInput.text.Trim();

        if (string.IsNullOrWhiteSpace(username))
        {
            Debug.LogWarning("Benutzername darf nicht leer sein.");
            feedbackText.text = "Name darf nicht leer sein.";
            return;
        }

        StartCoroutine(CheckUsernameExists(username));
    }

    private IEnumerator CheckUsernameExists(string username)
    {
        Debug.Log("Überprüfe, ob Benutzername existiert: " + username);
        string url = "https://api.benjo.online/username-exists?name=" + UnityWebRequest.EscapeURL(username);
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Fehler beim Überprüfen des Benutzernamens: " + request.error);
            feedbackText.text = "Serverfehler: " + request.error;
            yield break;
        }

        UsernameCheckResponse response = JsonUtility.FromJson<UsernameCheckResponse>(request.downloadHandler.text);

        if (response.exists)
        {
            Debug.Log("Benutzername bereits vergeben: " + username);
            feedbackText.text = "Name bereits vergeben.";
        }
        else
        {
            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.Save();
            Debug.Log("Username gespeichert, Lade Mainmenu");
            SceneManager.LoadScene("MainMenu");
        }
    }
}
