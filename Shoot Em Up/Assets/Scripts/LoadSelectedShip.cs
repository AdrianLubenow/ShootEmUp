using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSelectedShip : MonoBehaviour
{
    public GameObject[] shipPrefabs = new GameObject[2];
    

    [HideInInspector] public GameObject clone;

    public static Action<GameObject> UpdatePlayerAction;

    private void OnEnable()
    {
        LoadTheChosenShip();
    }
    public void LoadTheChosenShip()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedShip");
        GameObject prefab = shipPrefabs[selectedCharacter];
        clone = Instantiate(prefab, LevelManager.instance.spawnPoint.position, Quaternion.identity);
        clone.SetActive(true);
        UpdatePlayerAction?.Invoke(clone);

        UIManager.instance.healthBar.gameObject.SetActive(true);
        UIManager.instance.specialBar.gameObject.SetActive(true);

        LevelManager.instance.oPool.SetActive(true);
    }
}
