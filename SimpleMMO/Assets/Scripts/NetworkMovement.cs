using UnityEngine;
using Unity.Netcode;

public class NetworkMovement : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new();

    [Rpc(SendTo.Server)]
    void SubmitPositionRequestServerRpc(Vector3 position, RpcParams rpcParams = default) => Position.Value = position;
    void Update()
    {
        if(IsOwner && (!IsServer || IsHost)) 
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 movement =new Vector3(moveX,0f,moveZ) * 5 * Time.deltaTime;
            transform.Translate(movement,Space.World);
            SubmitPositionRequestServerRpc(transform.position);
        }

        if (IsServer || IsHost) 
        {
            transform.position = Position.Value;
        }
        
    }
}
