using Unity.Netcode;
using UnityEngine;

public class ClientStarter : MonoBehaviour
{
    [SerializeField] private Player prefab;

    void Start()
    {
        if (NetworkManager.Singleton != null) 
        {
            NetworkManager.Singleton.StartClient();
            Player initPlayer = Instantiate(prefab);
            initPlayer.GetComponent<NetworkObject>().Spawn();
        }
    }
}
