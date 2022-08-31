using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillHealthBar : MonoBehaviour
{
    public Image fillImage;

    private Slider slider;
    private PlayerHealth playerHealth;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChange += SetHealth;

        slider = GetComponent<Slider>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        slider.maxValue = playerHealth.maxHealth;

    }
    private void OnDisable()
    {
        PlayerHealth.OnHealthChange -= SetHealth;
    }

    private void SetHealth(float fillValue)
    {
        slider.value = fillValue * slider.maxValue;
    }
}
