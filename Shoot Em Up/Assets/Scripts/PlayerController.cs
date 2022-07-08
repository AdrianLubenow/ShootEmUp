using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static UnityEvent<float> OnSpecialBulletHit = new UnityEvent<float>();

    public Rigidbody2D rigidBody;
    public PlayerInputActions playerControls;

    public float moveSpeed;
    public float healthPoints;

    [SerializeField] public GameObject pointA;
    [SerializeField] public GameObject pointB;
    [SerializeField] public GameObject pointC;

    [SerializeField] public GameObject rocketPoint;
    public GameObject specialRocket;

    private List<GameObject> activeWeapons = new List<GameObject>();

    public InputAction move;

    private Vector2 moveDirection = Vector2.zero;

    private float delay = 0.3f;                                         //bullet delays
    private float timer = 0.3f;


    private int maxCharge;
    private float damageDealt;
    private float updatedCharge;
    private int loadedRockets;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        OnSpecialBulletHit.AddListener(UpdateDamageDealt);
    }

    void Start()
    {
        loadedRockets = 1;
        maxCharge = 100;
        updatedCharge = 0;
        damageDealt = 0;
    }


    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        if (Keyboard.current[Key.Space].IsPressed())
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer += delay;
                Shoot();
            }
        }

        if (Keyboard.current[Key.Z].wasPressedThisFrame)
            Special();
    }

    public float HealthPoints
    {
        get => healthPoints;
        set
        {
            healthPoints = value;
            if (healthPoints <= 0)
                Die();
        }
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        activeWeapons.Add(pointA);
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public void Damage(int damagePoints)
    {

        healthPoints -= damagePoints;
        healthPoints = Mathf.Clamp(healthPoints - 1, 0, 20000);
    }

    private void Shoot()
    {
         for (int i = 0; i < activeWeapons.Count; i++)
        {
            GameObject bullet1 = ObjectPool.SharedInstance.GetPooledObject();
            if (bullet1 != null)
            {
                bullet1.transform.position = activeWeapons[i].transform.position;
                //bullet1.transform.eulerAngles = new Vector3(a.transform.rotation.x, a.transform.rotation.y, 90f);
                bullet1.SetActive(true);
            }
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }

    public void UpdateDamageDealt(float damagePoints)
    {
        damageDealt += damagePoints;
        updatedCharge = damageDealt / 400;
        if (updatedCharge >= maxCharge)
        {
            loadedRockets += 1;
            updatedCharge = 0;

            if (damageDealt >= maxCharge * 400)
                damageDealt -= maxCharge * 400;

            if (loadedRockets > 3)
                loadedRockets = 3;
        }
    }


    private void Special()
    {
        if (loadedRockets <= 0)
            Debug.Log($"Loaded rockets are {loadedRockets}");
        else if (loadedRockets > 0)
        {
            Instantiate(specialRocket, transform.position, Quaternion.identity);
            loadedRockets--;
        }
    }
}