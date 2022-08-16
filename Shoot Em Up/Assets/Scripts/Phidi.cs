using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Phidi : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Transform targetPlayer;

    public float healthPoints = 100000;

    private float fireRate = 5f;
    private float moveSpeed = 2.5f;

    //public Transform targetPlayer;
    //private Vector2 targetPosition = Vector2.zero;


    private Vector2 targetPosition;

    private float chargeSpeed = 7f;
    private float chargeCoolDown = 12f;
    private Vector3 positionToChargeTowards;
    private bool _shouldCharge = false;
    private bool _isCharging = false;

    public GameObject bulletPointLeft;
    public GameObject bulletPointMiddle;
    public GameObject bulletPointRight;
    public GameObject explosion;
    private List<GameObject> bulletPoints = new List<GameObject>();

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        targetPosition = GetRandomPosition();

        InvokeRepeating("Shoot", fireRate, fireRate);
    }

    private void OnEnable()
    {
        bulletPoints.Add(bulletPointLeft);
        bulletPoints.Add(bulletPointRight);
        bulletPoints.Add(bulletPointMiddle);
    }

    private void OnDisable()
    {
        bulletPoints.Clear();
    }



    void Update()
    {
        if (healthPoints <= 0)
            Die();
    }

    private void FixedUpdate()
    {
        if (targetPlayer == null) return;

        if (_shouldCharge)
        {
            Charge();
        }

        if (chargeCoolDown <= 0f)
        {
            if (!_isCharging)
                positionToChargeTowards = targetPlayer.position;


            _shouldCharge = true;
        }
        else
        {
            RandomMovement();
            chargeCoolDown -= Time.deltaTime;
        }
    }

    private void Charge() 
    {
        if (targetPlayer != null)
        {
            _isCharging = true;
            var pos = positionToChargeTowards;

            if (transform.position != pos)
            {
                transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * chargeSpeed);
            }

            else
            {
                chargeCoolDown = 12f;
                _shouldCharge = false;
                _isCharging = false;
            }
        }
    }


    private Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(-7.5f, 7.5f);
        float randomY = Random.Range(-1f, 4f);
        return new Vector2(randomX, randomY);
    }

    private void RandomMovement()
    {
        if ((Vector2)transform.position != targetPosition)
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        else
            targetPosition = GetRandomPosition();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().HasShield())
                collision.gameObject.GetComponent<PlayerController>().DeactivateShield();
            else
                collision.gameObject.GetComponent<PlayerHealth>().Damage(3000);
        }
    }

    private void Shoot()
    {
        for (int i = 0; i < bulletPoints.Count; i++)
        {
            GameObject bossBullet = ObjectPool.SharedInstance.GetBossPooledObject();
            if (bossBullet != null)
            {
                bossBullet.transform.position = bulletPoints[i].transform.position;
                bossBullet.SetActive(true);
            }
        }
    }
}
