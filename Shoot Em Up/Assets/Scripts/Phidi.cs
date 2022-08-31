using System.Collections.Generic;
using UnityEngine;

public class Phidi : MonoBehaviour
{
    private Transform targetPlayer;

    public float healthPoints = 100000;

    private readonly float fireRate = 5f;
    private readonly float moveSpeed = 2.5f;

    private Vector2 targetPosition;

    private readonly float chargeSpeed = 4.5f;
    private const float kChargeCoolDown = 12f;
    private float chargeCounter = 12f;
    private Vector3 positionToChargeTowards;
    private float timeForCharge = 2.5f;
    private bool isCharging = false;

    public GameObject bulletPointLeft;
    public GameObject bulletPointMiddle;
    public GameObject bulletPointRight;
    public GameObject explosion;
    private List<GameObject> bulletPoints = new List<GameObject>();

    private void OnEnable()
    {
        bulletPoints.Add(bulletPointLeft);
        bulletPoints.Add(bulletPointRight);
        bulletPoints.Add(bulletPointMiddle);

        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        LevelManager.enemyCount++;
        UIManager.instance.isCheckForEnemiesPaused = false;
    }

    void Start()
    {
        targetPosition = GetRandomPosition();

        InvokeRepeating("Shoot", fireRate * 1.5f, fireRate);
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

        if (chargeCounter <= 0f)
        {
            Charge();
        }
        else
        {
            RandomMovement();
            chargeCounter -= Time.deltaTime;
            isCharging = false;
        }
    }

    private void Charge()
    {
        if (targetPlayer == null)
            return;

        if (!isCharging)
        {
            FindObjectOfType<AudioManager>().Play("PhidiCharge");
            isCharging = true;
        }

        positionToChargeTowards = targetPlayer.position;

        if (transform.position != positionToChargeTowards && timeForCharge > 0)
        {
            timeForCharge -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, positionToChargeTowards, Time.deltaTime * chargeSpeed);
        }
        else
        {
            chargeCounter = kChargeCoolDown;
            timeForCharge = 2.5f;
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
        FindObjectOfType<AudioManager>().Play("ShipDeath");

        if (LevelManager.enemyCount == 1)
            LevelManager.instance.SpawnRandomPowerUp(transform.position, 1);

        LevelManager.instance.SpawnRandomHealingBuff(transform.position, 1);
        LevelManager.enemyCount--;
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

            FindObjectOfType<AudioManager>().Play("HitSoundEffect");
        }
    }

    private void Shoot()
    {
        for (int i = 0; i < bulletPoints.Count; i++)
        {
            GameObject bossBullet = ObjectPool.instance.GetBossPooledObject();
            if (bossBullet != null)
            {
                bossBullet.transform.position = bulletPoints[i].transform.position;
                FindObjectOfType<AudioManager>().Play("PhidiFire");
                bossBullet.SetActive(true);
            }
        }
    }
}
