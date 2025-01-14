using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class NetworkUptime : NetworkBehaviour
{
    private NetworkVariable<float> ServerUptimeNetworkVariable = new NetworkVariable<float>();
    private float last_t = 0f;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        Assert.IsNotNull(text);
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer) 
        {
            ServerUptimeNetworkVariable.Value = 0f;
            Debug.Log("Uptime init as " + ServerUptimeNetworkVariable.Value);
        }
    }
    void Update()
    {
        var t_now = Time.time;
        if (IsServer) 
        {
            ServerUptimeNetworkVariable.Value = ServerUptimeNetworkVariable.Value + Time.deltaTime;
            if(t_now - last_t  > 0.5f) 
            {
                last_t = t_now;
            }
        }
        else 
        {
            text.text = ServerUptimeNetworkVariable.Value.ToString();
        }
        
    }
}
