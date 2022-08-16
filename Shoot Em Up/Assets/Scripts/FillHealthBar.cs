using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillHealthBar : MonoBehaviour
{
    public Image fillImage;

    private Slider slider;
    private PlayerHealth player;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChange += SetHealth;
    }
    private void OnDisable()
    {
        PlayerHealth.OnHealthChange -= SetHealth;
    }

    void Awake()
    {
        slider = GetComponent<Slider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        slider.maxValue = player.maxHealth;
    }

    private void SetHealth(float fillValue)
    {
        slider.value = fillValue * slider.maxValue;
    }
}
