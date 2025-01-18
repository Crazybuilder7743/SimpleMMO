
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class MainMenu : MonoBehaviour
{
    public const string PLAYERPREFS_NAME_STRING = "PlayerName";
    public const string PLAYERPREFS_IP_STRING = "IP";
    public const string PLAYERPREFS_LOCALHOST_STRING = "localhost";
    [SerializeField] string playScene;
    [SerializeField] Toggle isLocalHost;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField ipInput;

    public void Awake()
    {
        isLocalHost.isOn = PlayerPrefs.GetInt(PLAYERPREFS_LOCALHOST_STRING) == 1;
    }
    public void Exit() 
    {
        Application.Quit();
    }


    public void SetLocalHost() 
    {
        int tmp = isLocalHost.isOn ? 1 : 0;
        PlayerPrefs.SetInt(PLAYERPREFS_LOCALHOST_STRING, tmp);
    }

    public void Play() 
    {
        PlayerPrefs.SetString(PLAYERPREFS_NAME_STRING, nameInput.text);
        PlayerPrefs.SetString(PLAYERPREFS_IP_STRING, ipInput.text);
        SceneManager.LoadScene(playScene);
    }
}
