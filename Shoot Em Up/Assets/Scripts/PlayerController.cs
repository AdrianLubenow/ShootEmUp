using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public static Action<float> OnLoadedRocketsChange;

    [HideInInspector] public float updatedBulletSpeed;
    [HideInInspector] public float updatedBulletDamage;

    [HideInInspector]
    public bool HasShield()
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
    [HideInInspector] private float poweredUpCharge;
    [HideInInspector] public int loadedRockets;
    [HideInInspector] public float maxRockets;

    public InputAction move;
    private Vector2 moveDirection = Vector2.zero;

    private readonly float bulletDelay = 0.35f;
    private float bulletTimer = 0f;

    private InputManager _inputManager;


    private void Awake()
    {
        for (int i = 0; i < UnityEngine.Object.FindObjectsOfType<PlayerController>().Length; i++)
        {
            if (UnityEngine.Object.FindObjectsOfType<PlayerController>()[i] != this)
            {
                if (UnityEngine.Object.FindObjectsOfType<PlayerController>()[i].name == gameObject.name)
                {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        _inputManager = InputManager.instance;
        move = _inputManager.Move;
        move.Enable();

        OnBulletHit.AddListener(UpdateDamageDealt);

        ObjectPool.instance.SetPlayerObjectPool(Bullet, BulletAmount);

    }

    private void OnDisable()
    {
        move.Disable();
        OnBulletHit.RemoveAllListeners();
    }
    void Start()
    {
        maxCharge = 100;
        maxRockets = 3;
        loadedRockets = 0;
        updatedCharge = 0;
        poweredUpCharge = 0;
        var fillValue = updatedCharge / maxCharge;
        OnSpecialChargeChange?.Invoke(fillValue);
        damageDealt = 0;
        updatedBulletDamage = Bullet.GetComponent<Bullet>().bulletDamage;
        updatedBulletSpeed = Bullet.GetComponent<Bullet>().bulletSpeed;

        activeWeapons.Add(pointA);
    }


    private void Update()
    {
        Move();

        UpdateSpecialCharge();

        bulletTimer -= Time.deltaTime;
        if (bulletTimer <= 0)
            bulletTimer = 0;
        if (_inputManager.Fire.IsPressed())
        {
            if (bulletTimer <= 0)
            {
                bulletTimer += bulletDelay;
                Shoot();
            }
        }

        if (_inputManager.Special.WasPressedThisFrame())
            Special();

        if (moveSpeed > maxMoveSpeed)
            moveSpeed = maxMoveSpeed;

        updatedCharge += poweredUpCharge;
    }
    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void Move()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    public void ActivateShield()
    {
        FindObjectOfType<AudioManager>().Play("ShieldSFX");
        shield.SetActive(true);
        HasShield();
    }
    public void DeactivateShield()
    {
        FindObjectOfType<AudioManager>().Play("ShieldSFX");
        shield.SetActive(false);
    }

    public void AddBulletPoint()
    {
        if (activeWeapons.Count == 1)
        {
            activeWeapons.Remove(pointA);
            activeWeapons.Add(pointB);
            activeWeapons.Add(pointC);
        }
        else if (activeWeapons.Count == 2)
            activeWeapons.Add(pointA);
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
            GameObject bullet = ObjectPool.instance.GetPlayerPooledObject();
            bullet.GetComponent<Bullet>().bulletDamage = updatedBulletDamage;
            bullet.GetComponent<Bullet>().bulletSpeed = updatedBulletSpeed;
            if (bullet != null)
            {
                bullet.transform.position = activeWeapons[i].transform.position;
                FindObjectOfType<AudioManager>().Play("PlayerFire");
                bullet.SetActive(true);
            }
        }
    }

    public void UpdateDamageDealt(float damagePoints)
    {
        damageDealt += damagePoints;
    }

    public void UpdateSpecialCharge()
    {
        updatedCharge = damageDealt / 400;
        updatedCharge += poweredUpCharge;
        var fillValue = updatedCharge / maxCharge;
        OnSpecialChargeChange?.Invoke(fillValue);

        if (updatedCharge >= maxCharge)
        {
            loadedRockets += 1;
            OnLoadedRocketsChange?.Invoke(loadedRockets);
            FindObjectOfType<AudioManager>().Play("SpecialChargedUp");
            updatedCharge -= maxCharge;
            poweredUpCharge = 0;
            OnSpecialChargeChange?.Invoke(fillValue);

            if (damageDealt >= maxCharge * 400)
                damageDealt -= maxCharge * 400;

            if (loadedRockets > maxRockets)
            {
                loadedRockets = 3;
                OnLoadedRocketsChange?.Invoke(loadedRockets);
            }
        }
    }

    public void UpdateSpecialChargeOnPowerup(float value)
    {
        poweredUpCharge += value;
        updatedCharge += poweredUpCharge;
        var fillValue = updatedCharge / maxCharge;
        OnSpecialChargeChange?.Invoke(fillValue);
    }

    private void Special()
    {
        if (loadedRockets <= 0)
            Debug.Log($"You have only done {updatedCharge * 400} damage.");
        else if (loadedRockets > 0)
        {
            Instantiate(specialRocket, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("RocketFire");
            loadedRockets--;
            OnLoadedRocketsChange?.Invoke(loadedRockets);
        }
    }
    public void AddMoveSpeed(float value)
    {
        moveSpeed += value;
    }
}