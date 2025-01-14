using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour , IDamageable
{
    public float MaxHP;
    [HideInInspector] public NetworkVariable<float> HP = new();
    [SerializeField] private float currentHP; //local var only to Display current health
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
        }
        Debug.Log(this.gameObject.tag);
    }

    public bool TakeDamage(float damage) 
    {
        if (!IsOwner)
        {
            currentHP = HP.Value;
            if(currentHP <= 0)
            {
                return true;
            }
            return false;
        }

        HP.Value -= damage;
        currentHP -= HP.Value;
        if (HP.Value <= 0) 
        {
            return true; // enemy dies
        }

        return false;
    }
}
