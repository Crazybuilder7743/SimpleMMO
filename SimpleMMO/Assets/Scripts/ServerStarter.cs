using Unity.Netcode;
using UnityEngine;

public class ServerStarter : MonoBehaviour
{
    [SerializeField] Player playerPrefab;
    void Start()
    {
        if(NetworkManager.Singleton != null) 
        {
            NetworkManager.Singleton.StartServer();
            OnNetworkSpawn();
            NetworkManager.Singleton.OnServerStopped += OnNetworkDespawn;
        }

    }


    public void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
        }
       
    }

    public void OnNetworkDespawn(bool l)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayer;
        }
        NetworkManager.Singleton.OnServerStopped -= OnNetworkDespawn;
    }


    void SpawnPlayer(ulong playerId) 
    {
    
        Player initPlayer = Instantiate(playerPrefab);
        initPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(playerId);
    
    }

}
