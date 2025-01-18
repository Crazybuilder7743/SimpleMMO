using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class ClientStarter : MonoBehaviour
{
    private const string DEFAULT_IP = "16.170.228.149";
    [SerializeField] UnityTransport localHostConnection;
    [SerializeField] UnityTransport serverConnetion;
    void Start()
    {
        if (PlayerPrefs.GetInt(MainMenu.PLAYERPREFS_LOCALHOST_STRING) == 1)
        {
            localHostConnection.gameObject.SetActive(true);
        }
        else 
        {
            serverConnetion.gameObject.SetActive(true);
            string ip = PlayerPrefs.GetString(MainMenu.PLAYERPREFS_IP_STRING);
            if (string.IsNullOrEmpty(ip)) 
            {
                ip = DEFAULT_IP;
            }
            serverConnetion.ConnectionData.Address = ip;
        }
        if (NetworkManager.Singleton != null) 
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
