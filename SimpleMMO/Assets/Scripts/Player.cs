using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private float defaultSpeed = 1.0f;
    [SerializeField] private float maxAllowedDesync = 1.0f;
    [SerializeField] private float defaultDamageFactor = 1.0f;
    public static float movementToLookAssignAngle = 0;
    [HideInInspector]
    public NetworkVariable<FixedString64Bytes> playerName = new();
    private float inputX= 0.0f;
    private float inputZ = 0.0f;
    private bool inputY = false;
    [HideInInspector]
    public NetworkVariable<Vector3> Position = new(); 
    [HideInInspector]
    public NetworkVariable<Vector3> NewPosition = new();
    [HideInInspector]
    public NetworkVariable<Vector3> Velocity = new();
    [HideInInspector]
    public NetworkVariable<Vector3> NewVelocity = new();
    [HideInInspector]
    public NetworkVariable<float> Speed = new();
    [HideInInspector]
    public NetworkVariable<float> VelocityToDamageFactor = new();
    private float speed;
    Rigidbody rb;
    [Rpc(SendTo.Server)]
    void SubmitPositionRequestServerRpc(Vector3 position, RpcParams rpcParams = default) => NewPosition.Value = position;
    [Rpc(SendTo.Server)]
    void SubmitInputRequestServerRpc(Vector3 input, RpcParams rpcParams = default)
    {
        inputX = input.x;
        inputZ = input.z;
        inputY = input.y > 0 ? true : false; 
    }
    [Rpc(SendTo.ClientsAndHost)]
    void SubmitPositionRequestClientRpc(Vector3 position, RpcParams rpcParams = default) => transform.position = Position.Value;
    [Rpc(SendTo.Server)]
    void SubmitVelocityRequestServerRpc(Vector3 velocity, RpcParams rpcParams = default) => NewVelocity.Value = velocity;
    [Rpc(SendTo.Server)]
    void SubmitNameRequestServerRpc(FixedString64Bytes newName, RpcParams rpcParams = default) => playerName.Value = newName;
    [Rpc(SendTo.ClientsAndHost)]
    void SubmitVelocityRequestClientRpc(Vector3 velocity, RpcParams rpcParams = default) => rb.linearVelocity= Velocity.Value;


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
        rb = this.gameObject.GetComponent<Rigidbody>();
        if (!IsServer)
        {
            speed = Speed.Value;
            if (IsOwner)
            {
                FixedString64Bytes tmp = PlayerPrefs.GetString(MainMenu.PLAYERPREFS_NAME_STRING);
                SubmitNameRequestServerRpc(tmp);
                FollowPlayer.SetFollowObject(this.transform);
            }
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
            inputX = Input.GetAxis("Horizontal");
            inputZ = Input.GetAxis("Vertical");
            Vector3 inputs = Quaternion.AngleAxis(movementToLookAssignAngle, Vector3.up) * new Vector3(inputX,0, inputZ);
            inputX = inputs.x;
            inputZ = inputs.z;
            //rotate inputs so it aligns with view
            inputY = Input.GetKey(KeyCode.Space);
            float y = inputY ? 1 : 0;
            SubmitInputRequestServerRpc(new Vector3(inputX,y,inputZ));
            speed = Speed.Value;
            Vector3 movement = new Vector3(inputX, 0f, inputZ) * speed * Time.deltaTime;
            rb.AddForce(movement);
            SubmitPositionRequestServerRpc(transform.position);
            SubmitVelocityRequestServerRpc(rb.linearVelocity);
        }
        if(IsServer)
        {
            if(Vector3.Distance(transform.position,NewPosition.Value) >= maxAllowedDesync) 
            {
                SubmitVelocityRequestClientRpc(rb.linearVelocity);
            }

            else 
            {
                Velocity.Value = NewVelocity.Value;
                Position.Value = NewPosition.Value;
                transform.position = NewPosition.Value;
                rb.linearVelocity = NewVelocity.Value;
            }
            if(Vector3.Distance(rb.linearVelocity,NewVelocity.Value) >= maxAllowedDesync) 
            {
                SubmitVelocityRequestClientRpc(rb.linearVelocity);
            }

            else 
            {
                Velocity.Value = NewVelocity.Value;
                rb.linearVelocity = NewVelocity.Value;
            }

        }
        else if (!IsServer && !IsOwner)
        {
            transform.position = Position.Value;
        }

        

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damageable")) 
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(VelocityToDamageFactor.Value*Velocity.Value.magnitude);
        }
    }

}
