using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody bulletRB;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 10f;
        bulletRB.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Doei");
        Destroy(gameObject);
    }
}
