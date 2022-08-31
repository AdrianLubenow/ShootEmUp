using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipSelection : MonoBehaviour
{
    public GameObject[] ships = new GameObject[2];
    public int selectedShip = 0;


    public void SelectHumanShip()
    {
        selectedShip = 0;
        PlayerPrefs.SetInt("selectedShip", selectedShip);
        PlayGame();
    }
    public void SelectGrewakShip()
    {
        selectedShip = 1;
        PlayerPrefs.SetInt("selectedShip", selectedShip);
        PlayGame();
    }
    public void PlayGame()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        LevelManager.instance.GetComponent<LoadSelectedShip>().enabled = true;
        UIManager.instance.mainMenu.SetActive(false);
        UIManager.instance.pauseAction.Enable();
    }
}
