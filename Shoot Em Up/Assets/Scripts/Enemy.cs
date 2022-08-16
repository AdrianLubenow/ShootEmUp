using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    public float healthPoints;
    public bool canShoot;
    public float fireRate;

    public GameObject bombPoint;
    public GameObject bomb;
    public GameObject explosion;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (!canShoot)
            return;
        if (canShoot)
        {
            fireRate = fireRate + (Random.Range(fireRate / -2, fireRate / 2));
            InvokeRepeating("Shoot", fireRate / 2, fireRate);
        }
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
            if (collider.gameObject.GetComponent<PlayerController>().HasShield())
                collider.gameObject.GetComponent<PlayerController>().DeactivateShield();
            else
                collider.gameObject.GetComponent<PlayerHealth>().Damage(1000);
            Die();
        }

    }
    private void Die()
    {
        Instantiate(explosion, transform.position, transform.rotation = Quaternion.identity);
        Destroy(gameObject);
    }
    public void Damage(float damagePoints)
    {
        healthPoints -= damagePoints;
    }

    private void Shoot()
    {
        bomb = ObjectPool.SharedInstance.GetEnemyPooledObject();
        if (bomb != null)
        {
            bomb.transform.position = bombPoint.transform.position;
            bomb.SetActive(true);
        }
    }
}
