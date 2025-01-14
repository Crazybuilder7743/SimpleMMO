using Unity.Netcode;
using UnityEngine;

public class ServerStarter : MonoBehaviour
{

    void Start()
    {
        if(NetworkManager.Singleton != null) 
        {
            NetworkManager.Singleton.StartServer();
        }
    }
}
