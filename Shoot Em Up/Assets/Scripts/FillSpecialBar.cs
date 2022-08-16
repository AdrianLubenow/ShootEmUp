using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillSpecialBar : MonoBehaviour
{
    public Image fillImage;

    private Slider specialSlider;
    private PlayerController playerSpecial;
    private float maxCharge = 100f;

    private void OnEnable()
    {
        PlayerController.OnSpecialChargeChange += SetCharge;
    }
    private void OnDisable()
    {
        PlayerController.OnSpecialChargeChange += SetCharge;
    }

    void Awake()
    {
        specialSlider = GetComponent<Slider>();
        playerSpecial = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        specialSlider.maxValue = maxCharge;
    } 
    private void SetCharge(float fillValue)
    {
        specialSlider.value = fillValue * specialSlider.maxValue;
    }
}
