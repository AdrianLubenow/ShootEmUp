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

    [HideInInspector] public Vector2 powerUpSpawnPoint;

    private void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        LevelManager.enemyCount++;
        UIManager.instance.isCheckForEnemiesPaused = false;
    }

    void Start()
    {
        if (!canShoot)
            return;
        if (canShoot)
        {
            fireRate = fireRate + (Random.Range(fireRate / -2, fireRate / 2));
            InvokeRepeating("Shoot", fireRate * 1.5f, fireRate);
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
        FindObjectOfType<AudioManager>().Play("ShipDeath");

        if (LevelManager.enemyCount == 1)
            LevelManager.instance.SpawnRandomPowerUp(transform.position, 1);
        else
            LevelManager.instance.SpawnRandomPowerUp(transform.position, Random.Range(0, 100));

        LevelManager.instance.SpawnRandomHealingBuff(transform.position, Random.Range(0, 100));
        
        Destroy(gameObject);
        LevelManager.enemyCount--;
        
    }
    public void Damage(float damagePoints)
    {
        healthPoints -= damagePoints;
    }

    private void Shoot()
    {
        bomb = ObjectPool.instance.GetEnemyPooledObject();
        if (bomb != null)
        {
            bomb.transform.position = bombPoint.transform.position;
            FindObjectOfType<AudioManager>().Play("AshdacFire");
            bomb.SetActive(true);
        }
    }   
}
