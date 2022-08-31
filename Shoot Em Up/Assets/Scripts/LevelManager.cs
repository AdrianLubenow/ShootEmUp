using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    //private LoadSelectedShip selectedShips;

    public static int enemyCount = 0;
    [HideInInspector] public bool areAllEnemiesDead = false;
    [Header("--- Dont Destroy On Load Objects ---")]
    public GameObject player;
    public GameObject oPool;
    public Transform spawnPoint;

    [SerializeField] private LoadSelectedShip levelLoader;
    public static UnityEvent OnLevelStart;

    [Header("--- Powerups ---")]
    public GameObject[] powerUps;
    public GameObject[] healingBuffs;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }
    private void OnEnable()
    {
        LoadSelectedShip.UpdatePlayerAction += UpdatePlayer;
        if (SceneManager.GetActiveScene().buildIndex == 0)
            GetComponent<LoadSelectedShip>().enabled = false;
    }

    private void OnDisable()
    {
        LoadSelectedShip.UpdatePlayerAction -= UpdatePlayer;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            if(!UIManager.instance.mainMenu.activeSelf)
                if (!UIManager.instance.deathMenu.gameObject.activeSelf && !UIManager.instance.isCheckForEnemiesPaused)
                    CheckIfEnemiesAreDead();

    }

    private void CheckIfEnemiesAreDead()
    {
        areAllEnemiesDead = enemyCount <= 0 ? true : false;
    }

    public void ResetEnemyCount()
    {
        enemyCount = 0;
    }

    public void UpdatePlayer(GameObject playerShip)
    {
        player = playerShip;
    }

    public void StartFirstLevel()
    {
        SceneManager.LoadScene(1);
        levelLoader.LoadTheChosenShip();
    }

    public void SpawnRandomPowerUp(Vector2 position, float percentage)
    {
        if (percentage <= 3)
        {
            int randomIndex = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[randomIndex], position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("PowerUpSpawn");
        }
        else return;
    }
    public void SpawnRandomHealingBuff(Vector2 position, float percentage)
    {
        if (position == null)
        {
            position = Vector2.zero;
        }

        if (percentage <= 5)
        {
            int randomIndex = Random.Range(0, healingBuffs.Length);
            Instantiate(healingBuffs[randomIndex], position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("PowerUpSpawn");
        }
        else return;
    }
}
