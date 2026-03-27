using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class FirstOpen : MonoBehaviour
{
    public InputField usernameInput;
    public Text feedbackText;

    async void Start()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void SaveUsername()
    {
        string username = usernameInput.text.Trim();

        if (string.IsNullOrWhiteSpace(username))
        {
            feedbackText.text = "Name darf nicht leer sein.";
            return;
        }

        feedbackText.text = "Wird gespeichert...";

        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.Save();
            Debug.Log("Username gespeichert, Lade Mainmenu");
            SceneManager.LoadScene("MainMenu");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Fehler beim Setzen des Namens: " + e.Message);
            feedbackText.text = "Fehler beim Speichern: " + e.Message;
        }
    }
}
