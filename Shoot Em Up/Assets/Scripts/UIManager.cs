using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("--- General ---")]
    public Canvas canvas;
    public Toggle fullscreenToggle;
    public GameObject optionsMenu;
    public GameObject controlsMenu;
    public GameObject audioMenu;
    public Slider healthBar;
    public Slider specialBar;

    [Header("--- Main Menu ---")]
    public GameObject mainMenu;

    [Header("--- Pause Menu ---")]
    public GameObject pauseMenu;
    public static bool isPaused = false;

    [HideInInspector]public PauseAction pauseAction;


    [Header("--- Audio ---")]
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    [Header("--- Death Menu ---")]
    public GameObject deathMenu;
    [HideInInspector] public bool isCheckForEnemiesPaused = false;


    [Header("--- Winning Screen ---")]
    public GameObject nextLevelScreen;
    public TMP_Text winningText;
    public Button nextLevelButton;
    public TMP_Text nextLevelButtonText;
    private bool wasWinningScreenActivated = false;
    [HideInInspector] public bool coroutineStarted = false;


    private bool isMainMenuOpened = false;
    private bool isPauseMenuOpened = false;

    private void Awake()
    {
        pauseAction = new PauseAction();

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
        canvas.worldCamera = Camera.main;

        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }

    private void Start()
    {
        pauseAction.Pause.PauseGame.performed += _ => DeterminePause();
    }

    private void Update()
    {
        if (LevelManager.instance.areAllEnemiesDead && !coroutineStarted && !wasWinningScreenActivated)
        {
            coroutineStarted = true;
            StartCoroutine(DelayRoutine());
        }
    }

    private void DeterminePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = false;
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        isPaused = false;
        pauseMenu.SetActive(false);

        if (optionsMenu.activeSelf || audioMenu.activeSelf || controlsMenu.activeSelf)
        {
            optionsMenu.SetActive(false);
            audioMenu.SetActive(false);
            controlsMenu.SetActive(false);
        }
    }

    public void ReturnToMainMenu()
    {
        if (LevelManager.instance.player != null)
            Destroy(LevelManager.instance.player.gameObject);

        mainMenu.SetActive(true);
        pauseAction.Disable();
        healthBar.gameObject.SetActive(false);
        specialBar.gameObject.SetActive(false);
        LevelManager.instance.oPool.SetActive(false);
        LevelManager.instance.GetComponent<LoadSelectedShip>().enabled = false;
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        if (deathMenu.gameObject != null)
            ToggleDeathPanel();
    }

    public void ToggleDeathPanel()
    {
        deathMenu.SetActive(!deathMenu.activeSelf);
    }

    public void RestartGame()
    {
        LevelManager.instance.ResetEnemyCount();
        isCheckForEnemiesPaused = true;
        ToggleDeathPanel();
        LevelManager.instance.StartFirstLevel();
    }

    public void EnableNextLevelScreen()
    {
        wasWinningScreenActivated = true;

        pauseAction.Disable();
        nextLevelScreen.SetActive(true);
        FindObjectOfType<AudioManager>().Play("VictorySFX");

        if (SceneManager.GetActiveScene().buildIndex > 0 && SceneManager.GetActiveScene().buildIndex < 5)
        {
            winningText.text = $"LEVEL {SceneManager.GetActiveScene().buildIndex} COMPLETED";
            nextLevelButtonText.text = $"NEXT LEVEL";

            if (LevelManager.instance.player != null)
                LevelManager.instance.player.SetActive(false);

            if (LevelManager.instance.oPool != null)
                LevelManager.instance.oPool.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            winningText.fontSize = 41;
            winningText.text = $"YOU WON! YOU HAVE COMPLETED THE GAME!";
            nextLevelButtonText.text = $"CONTINUE";

            if (LevelManager.instance.player != null)
                Destroy(LevelManager.instance.player.gameObject);

            if (LevelManager.instance.oPool != null)
                LevelManager.instance.oPool.SetActive(false);
        }
    }

    public void NextLevel()
    {
        coroutineStarted = false;
        wasWinningScreenActivated = false;
        isCheckForEnemiesPaused = true;
        LevelManager.instance.areAllEnemiesDead = false;

        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            mainMenu.SetActive(true);
            SceneManager.LoadScene(0);
        }
        else
        {
            if (LevelManager.instance.player != null)
                LevelManager.instance.player.SetActive(true);

            if (LevelManager.instance.oPool != null)
                LevelManager.instance.oPool.SetActive(true);

            if (SceneManager.GetActiveScene().buildIndex == 1)
                LevelManager.instance.GetComponent<LoadSelectedShip>().enabled = false;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            if (LevelManager.instance.player != null)
            {
                LevelManager.instance.player.transform.position = LevelManager.instance.spawnPoint.transform.position;
            }
        }
        pauseAction.Enable();
        nextLevelScreen.SetActive(false);
        wasWinningScreenActivated = false;
    }

    public void OnBackButtonClicked()
    {
        if (isMainMenuOpened)
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            isMainMenuOpened = false;
        }
        else if(isPauseMenuOpened)
        {
            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
            isPauseMenuOpened = false;
        }
    }

    public void SetMainMenuOpened()
    {
        isMainMenuOpened = true;
    }
    public void SetPauseMenuOpened()
    {
        isPauseMenuOpened = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleFullscreen()//bool isFullScreen)
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log(Screen.fullScreen);
    }

    public IEnumerator DelayRoutine()
    {
        yield return new WaitForSeconds(5);
        EnableNextLevelScreen();
    }

    public void PlayClickSound()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);
        AudioManager.instance.Save();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
        AudioManager.instance.Save();
    }

    public void SetSoundFXVolume(float volume)
    {
        audioMixer.SetFloat("SoundFX", volume);
        AudioManager.instance.Save();
    }

    public void ToggleAudio()
    {
        AudioListener.pause = !AudioListener.pause;
    }
}
