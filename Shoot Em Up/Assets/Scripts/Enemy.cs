using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    public float healthPoints;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
    }

    
    void Update()
    {
        if (healthPoints <= 0)
            Die();
    }


    private void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.tag=="Player")
        {
            collider.gameObject.GetComponent<PlayerController>().Damage(1000);
            Die();
        }

    }
    private void Die()
    {
        Destroy(gameObject);
    }
    public void Damage(float damagePoints)
    {
        healthPoints -= damagePoints;
    }
}
