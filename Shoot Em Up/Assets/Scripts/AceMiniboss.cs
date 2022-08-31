using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AceMiniboss : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector2 targetPosition = Vector2.zero;

    public float healthPoints = 40000;
    public float fireRate = 1.5f;
    public float moveSpeed;

    public Transform targetPlayer;

    public GameObject bulletPoint;
    public GameObject aceBullet;
    public GameObject explosion;

    private void OnEnable()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();
        LevelManager.enemyCount++;
        UIManager.instance.isCheckForEnemiesPaused = false;
    }

    void Start()
    {
        targetPosition = new Vector2(targetPlayer.position.x, transform.position.y);

        InvokeRepeating("Shoot", fireRate * 1.5f, fireRate);
    }


    void Update()
    {
        if (healthPoints <= 0)
            Die();

        if (targetPlayer != null)
        {
            if (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(targetPlayer.position.x)) >= 0.5f)
                targetPosition = new Vector2(targetPlayer.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

    }

    private void Die()
    {
        Instantiate(explosion, transform.position, transform.rotation = Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("ShipDeath");

        if (LevelManager.enemyCount == 1)
            LevelManager.instance.SpawnRandomPowerUp(transform.position, 1);

        LevelManager.instance.SpawnRandomHealingBuff(transform.position, 1);
        Destroy(gameObject);
        LevelManager.enemyCount--;
    }


    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (collider.gameObject.GetComponent<PlayerController>().HasShield())
                collider.gameObject.GetComponent<PlayerController>().DeactivateShield();
            else
                collider.gameObject.GetComponent<PlayerHealth>().Damage(2000);

            FindObjectOfType<AudioManager>().Play("HitSoundEffect");
        }
    }


    public void Damage(float damagePoints)
    {
        healthPoints -= damagePoints;
    }

    private void Shoot()
    {
        aceBullet = ObjectPool.instance.GetMiniBossPooledObject();
        if (aceBullet != null)
        {
            aceBullet.transform.position = bulletPoint.transform.position;
            FindObjectOfType<AudioManager>().Play("AceShipFire");
            aceBullet.SetActive(true);
        }
    }
}
