
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public const string PLAYERPREFS_NAME_STRING = "PlayerName";
    [SerializeField] string playScene;
    [SerializeField] TextMeshProUGUI nameInput;
    public void Exit() 
    {
        Application.Quit();
    }

    public void Play() 
    {
        PlayerPrefs.SetString(PLAYERPREFS_NAME_STRING, nameInput.text);
        Debug.Log(PlayerPrefs.GetString(PLAYERPREFS_NAME_STRING));
        SceneManager.LoadScene(playScene);
    }
}
