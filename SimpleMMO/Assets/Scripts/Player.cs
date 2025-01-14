using Unity.Netcode;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private float defaultSpeed = 1.0f;
    [SerializeField] private float defaultDamageFactor = 1.0f;
    [HideInInspector]
    public NetworkVariable<Vector3> Position = new();
    [HideInInspector]
    public NetworkVariable<Vector3> Velocity = new();
    [HideInInspector]
    public NetworkVariable<float> Speed = new();
    [HideInInspector]
    public NetworkVariable<float> VelocityToDamageFactor = new();
    private float speed;
    Rigidbody rb;
    [Rpc(SendTo.Server)]
    void SubmitPositionRequestServerRpc(Vector3 position, RpcParams rpcParams = default) => Position.Value = position;
    [Rpc(SendTo.Server)]
    void SubmitVelocityRequestServerRpc(Vector3 velocity, RpcParams rpcParams = default) => Velocity.Value = velocity;
    [Rpc(SendTo.Server)]


    public void Awake()
    {
        Position.OnValueChanged += OnPositionChanged;
    }

    private void OnPositionChanged(Vector3 oldPos,Vector3 newPos) 
    {
        if (!IsOwner) 
        {
            transform.position = newPos;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
        {
            speed = Speed.Value;
            rb = this.gameObject.AddComponent<Rigidbody>();
        }
        else
        {
            Speed.Value = defaultSpeed;
            VelocityToDamageFactor.Value = defaultDamageFactor;
        }
    }
    void Update()
    {
        if (IsOwner)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            speed = Speed.Value;
            Vector3 movement = new Vector3(moveX, 0f, moveZ) * speed * Time.deltaTime;
            rb.AddForce(movement);
            SubmitPositionRequestServerRpc(transform.position);
            SubmitVelocityRequestServerRpc(rb.linearVelocity);
        }

        if (!IsServer && !IsOwner)
        {
            transform.position = Position.Value;
        }

        

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damageable")) 
        {
            string name = IsClient ? "Client" : "Server";

            Debug.Log(collision.gameObject.name + " Hit on " + name);
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(VelocityToDamageFactor.Value);
        }
    }

}
