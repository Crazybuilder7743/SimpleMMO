using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    static FollowPlayer inst;
    Transform player;

    private void Awake()
    {
        if(inst == null) 
        {
            inst = this;
        }

        else 
        {
            Destroy(this.gameObject);
        }
    }
    public static void SetFollowObject(Transform target) 
    {
        inst.player = target;
    }

    public static Transform GetInst() 
    {
        return inst.transform;
    }

    private void Update()
    {
        if (player != null)
        {
                this.transform.position = player.transform.position;
        }
    }
}
