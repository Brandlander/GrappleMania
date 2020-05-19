using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timeToLive;

    void Start()
    {
        Decay();
    }

    void Update()
    {
        Travel();
    }

    public virtual void Decay()
    {
        Destroy(gameObject, timeToLive);
    }

    public virtual void Travel()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        print("Hit : " + other.name);
    }
}
