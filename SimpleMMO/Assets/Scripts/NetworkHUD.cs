using TMPro;
using UnityEngine.Assertions;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class NetworkHUD : NetworkBehaviour
{
    NetworkVariable<int> PlayerCount = new NetworkVariable<int>();
    [SerializeField] TextMeshProUGUI playerIndexDisplay;
    int playerIndex;


    void IncrementPlayerCount(ulong id) 
    {
        PlayerCount.Value += 1;
        playerIndex +=1;
        DisplayUI();
    }
    void DecrementPlayerCount(ulong id) 
    {
        PlayerCount.Value -= 1;
        playerIndex -= 1;
        DisplayUI();
    }

    void SetPlayerCount(int intOLD, int intNew) 
    {

        playerIndex = intNew;
        DisplayUI();

    }


    public override void OnNetworkSpawn()
    {
        if(IsServer)
        { 
            NetworkManager.Singleton.OnClientConnectedCallback += IncrementPlayerCount;
            NetworkManager.Singleton.OnClientDisconnectCallback += DecrementPlayerCount;
        }
        else 
        {
            PlayerCount.OnValueChanged += SetPlayerCount;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= IncrementPlayerCount;
            NetworkManager.Singleton.OnClientDisconnectCallback -= DecrementPlayerCount;
        }
        else
        {
            PlayerCount.OnValueChanged -= SetPlayerCount;

        }
    }

    private void DisplayUI() 
    {
        playerIndexDisplay.text = playerIndex.ToString();
        if (IsServer) 
        {
        
        }

        else if(IsClient)
        {
        
        }
    }
}
