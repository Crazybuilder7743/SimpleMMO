
using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour , IDamageable
{
    public float MaxHP;
    [HideInInspector] public NetworkVariable<float> HP = new();
    [SerializeField] private float currentHP; //local var only to Display current health
    [SerializeField] private GameObject shatterEffectObject;

    [Rpc(SendTo.NotServer)]
    void SendDeathClientsRpc(RpcParams rpcParams = default) { Shatter(); }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer) 
        {
            HP.Value = MaxHP;
            currentHP = HP.Value;
        }
        else 
        {
            currentHP = HP.Value;
            HP.OnValueChanged += Die;
        }
    }


    private void Die(float oldHealth,float newHealth) 
    {
        if(newHealth <= 0) 
        {
            if (IsServer)
            {
                Destroy(this.gameObject);
            }
            else 
            {
                Shatter();
            }
        }
    }

    public bool TakeDamage(float damage) 
    {
        if (!IsOwner)
        {
            currentHP = HP.Value;
            return HP.Value<=0;
        }

        HP.Value -= damage;
        currentHP -= HP.Value;
        if (HP.Value <= 0) 
        {
            Die(0,HP.Value);
            return true; // enemy dies
        }

        return false;
    }

    private void Shatter() 
    {
        Destroy(gameObject.transform.GetChild(0));
        //play shatter effect
    }
}
