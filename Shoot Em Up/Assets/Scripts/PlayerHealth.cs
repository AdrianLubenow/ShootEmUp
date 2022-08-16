using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject explosion;
    public float maxHealth;
    public float currentHealth;

    private PlayerController player;

    public static Action<float> OnHealthChange;

    private void Start()
    {
        currentHealth = maxHealth;

        var fillValue = currentHealth / maxHealth;
        OnHealthChange?.Invoke(fillValue);
    }

    void Update()
    {
        if (currentHealth <= 0)
            Die();
        else if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void Damage(float damagePoints)
    {
        currentHealth -= damagePoints;

        var fillValue = currentHealth / maxHealth;
        OnHealthChange?.Invoke(fillValue);
    }

    private void Die()
    {
        Instantiate(explosion, transform.position, transform.rotation = Quaternion.identity);
        Destroy(gameObject);
    }

    public void AddHealth(float value)
    {
        currentHealth += value;
        var fillValue = currentHealth / maxHealth;
        OnHealthChange?.Invoke(fillValue);
    }

    public void AddMaxHealthOnPowerup(float value)
    {
        maxHealth += value;
        currentHealth += value;
        var fillValue = currentHealth / maxHealth;
        OnHealthChange?.Invoke(fillValue);
    }
}
