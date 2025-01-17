using Unity.Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    static CameraFollow instance;
    [SerializeField] private CinemachineCamera _cam;
    private Vector2 _lastMousePosition;
    bool setTarget = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }

        else 
        {
            Destroy(this);
        }
        _lastMousePosition = Input.mousePosition;
        
    }


    public static void SetTarget(Transform target) 
    {
        if (target == null) return;
        instance._cam.Target.TrackingTarget = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (!setTarget && FollowPlayer.GetInst() != null) 
        {
            SetTarget(FollowPlayer.GetInst());
            setTarget = true;
        }
        Player.movementToLookAssignAngle = this.transform.rotation.eulerAngles.y;
        Vector2 rotationamount = _lastMousePosition - (Vector2)Input.mousePosition;
        //instance._cam.R
        _lastMousePosition = Input.mousePosition;
    }
}
