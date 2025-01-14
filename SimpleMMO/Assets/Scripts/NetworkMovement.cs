using UnityEngine;
using Unity.Netcode;

public class NetworkMovement : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new();
    public NetworkVariable<Vector3> Velocity = new();
    public NetworkVariable<float> Speed = new();
    private float speed;
    [SerializeField] Rigidbody rb;
    [Rpc(SendTo.Server)]
    void SubmitPositionRequestServerRpc(Vector3 position, RpcParams rpcParams = default) => Position.Value = position;  
    [Rpc(SendTo.Server)]
    void SubmitVelocityRequestServerRpc(Vector3 velocity, RpcParams rpcParams = default) => Velocity.Value = velocity;  
    [Rpc(SendTo.Server)]
    void SubmitSpeedRequestServerRpc(float speed, RpcParams rpcParams = default) => Speed.Value = speed;
    void Update()
    {
        if(IsOwner && (!IsServer || IsHost)) 
        {
            Debug.Log("Test");
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            speed = Speed.Value;
            Vector3 movement =new Vector3(moveX,0f,moveZ) * speed * Time.deltaTime;
            rb.AddForce(movement);
            SubmitPositionRequestServerRpc(transform.position);
            SubmitPositionRequestServerRpc(rb.linearVelocity);
        }

        if (IsServer || IsHost) 
        {
            transform.position = Position.Value;
            rb.linearVelocity = Velocity.Value;
        }

        else 
        {
            rb.linearVelocity = Velocity.Value;
            transform.position = Position.Value;
        }
        
    }
}
