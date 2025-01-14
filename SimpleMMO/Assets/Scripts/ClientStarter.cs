using Unity.Netcode;
using UnityEngine;

public class ClientStarter : MonoBehaviour
{

    void Start()
    {
        if (NetworkManager.Singleton != null) 
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
