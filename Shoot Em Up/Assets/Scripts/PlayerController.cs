using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("--- Combat ---")]
    public GameObject Bullet;
    public int BulletAmount = 0;
    public GameObject pointA;
    public GameObject pointB;
    public GameObject pointC;
    public GameObject rocketPoint;
    public GameObject specialRocket;
    public GameObject shield;

    [HideInInspector] public List<GameObject> activeWeapons = new List<GameObject>();
    private readonly float maxWeapons = 3;

    public static UnityEvent<float> OnBulletHit = new UnityEvent<float>();
    public static Action<float> OnSpecialChargeChange;

    [HideInInspector] public float updatedBulletSpeed;
    [HideInInspector] public float updatedBulletDamage;

    [HideInInspector] public bool HasShield()
    {
        return shield.activeSelf;
    }

    [Header("--- Movement ---")]
    public Rigidbody2D rigidBody;
    public float moveSpeed;
    public PlayerInputActions playerControls;
    private readonly float maxMoveSpeed = 15f;

    [HideInInspector] public int maxCharge;
    [HideInInspector] public float damageDealt;
    [HideInInspector] public float updatedCharge;
    [HideInInspector] public int loadedRockets;
    [HideInInspector] public float maxRockets;

    public InputAction move;
    private Vector2 moveDirection = Vector2.zero;

    private readonly float bulletDelay = 0.3f;
    private float bulletTimer = 0.3f;


    private void Awake()
    {
        playerControls = new PlayerInputActions();
        OnBulletHit.AddListener(UpdateDamageDealt);
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        activeWeapons.Add(pointA);
        ObjectPool.SharedInstance.SetPlayerObjectPool(Bullet, BulletAmount);
        move.Enable();
        DeactivateShield();
    } 

    private void OnDisable()
    {
        move.Disable();
        activeWeapons.Clear();
    }
    void Start()
    {
        maxCharge = 100;
        maxRockets = 3;
        loadedRockets = 0;
        updatedCharge = 0;
        var fillValue = updatedCharge / maxCharge;
        OnSpecialChargeChange?.Invoke(fillValue);
        damageDealt = 0;
        updatedBulletDamage = Bullet.GetComponent<Bullet>().bulletDamage;
        updatedBulletSpeed = Bullet.GetComponent<Bullet>().bulletSpeed;
    }


    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        if (Keyboard.current[Key.Space].IsPressed())
        {
            bulletTimer -= Time.deltaTime;
            if (bulletTimer <= 0)
            {
                bulletTimer += bulletDelay;
                Shoot();
            } 
        }

        if (Keyboard.current[Key.Z].wasPressedThisFrame)
            Special();

        if (moveSpeed > maxMoveSpeed)
            moveSpeed = maxMoveSpeed;
    }


    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public void ActivateShield()
    {
            shield.SetActive(true);
            HasShield();
    }
    public void DeactivateShield()
    {
        shield.SetActive(false);
    }

    public void AddBulletPoint()
    {
        if (activeWeapons.Count == 1)
            activeWeapons.Add(pointB);
        else if (activeWeapons.Count == 2)
            activeWeapons.Add(pointC);
        else if (activeWeapons.Count >= maxWeapons)
            Debug.Log("Can't add any more weapons!");
    }

    public void IncreaseBulletDamage(float value)
    {
        updatedBulletDamage += value;
    }

    public void IncreaseBulletSpeed(float value)
    {
        updatedBulletSpeed += value;
    }


    private void Shoot()
    {
         for (int i = 0; i < activeWeapons.Count; i++)
        {
            GameObject bullet = ObjectPool.SharedInstance.GetPlayerPooledObject();
            bullet.GetComponent<Bullet>().bulletDamage = updatedBulletDamage;
            bullet.GetComponent<Bullet>().bulletSpeed = updatedBulletSpeed;
            if (bullet != null)
            {
                bullet.transform.position = activeWeapons[i].transform.position;
                bullet.SetActive(true);
            }
        }
    }

    public void UpdateDamageDealt(float damagePoints)
    {
        damageDealt += damagePoints;
        updatedCharge = damageDealt / 400;
        var fillValue = updatedCharge / maxCharge;
        OnSpecialChargeChange?.Invoke(fillValue);

        if (updatedCharge >= maxCharge)
        {
            loadedRockets += 1;
            updatedCharge = 0;
            OnSpecialChargeChange?.Invoke(fillValue);

            if (damageDealt >= maxCharge * 400)
                damageDealt -= maxCharge * 400;

            if (loadedRockets > maxRockets)
                loadedRockets = 3;
        }
    }

    public void UpdateSpecialChargeOnPowerup(float value)
    {
        updatedCharge += value;
        var fillValue = updatedCharge / maxCharge;
        OnSpecialChargeChange?.Invoke(fillValue);
    }

    private void Special()
    {
        if (loadedRockets <= 0)
            Debug.Log($"You have only done {damageDealt} damage.");
        else
        {
            Instantiate(specialRocket, transform.position, Quaternion.identity);
            loadedRockets--;
        }
    }
    public void AddMoveSpeed(float value)
    {
        moveSpeed += value;
    }
}