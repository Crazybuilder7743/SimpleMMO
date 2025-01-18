using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private float minExplosionStrength = 5;
    [SerializeField] private float maxExplosionStrength = 10;
    [SerializeField] private float explosionRadius = 10;
    [SerializeField] List<Rigidbody> childs = new List<Rigidbody>();
    private void Awake()
    {
        foreach (Rigidbody t in childs) 
        {
            t.AddExplosionForce(Random.Range(minExplosionStrength,maxExplosionStrength),this.transform.position,explosionRadius);
        }
    }
}
