using TMPro;
using UnityEngine;

public class PlayerTag : MonoBehaviour
{
    [SerializeField] TextMeshPro nameTag;
    Transform _target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(Player target) 
    {
        nameTag.text = target.playerName.Value.ToString();
        _target = target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.position;
    }
}
