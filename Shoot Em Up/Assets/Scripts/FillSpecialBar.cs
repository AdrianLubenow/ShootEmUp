using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillSpecialBar : MonoBehaviour
{
    public Image fillImage;
    public TMP_Text loadedRocketsText;

    private Slider specialSlider;
    private readonly float maxCharge = 100f;

    private void OnEnable()
    {
        PlayerController.OnSpecialChargeChange += SetCharge;
        PlayerController.OnLoadedRocketsChange += UpdateLoadedRockets;

        specialSlider = GetComponent<Slider>();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        specialSlider.maxValue = maxCharge;
    }
    private void OnDisable()
    {
        PlayerController.OnSpecialChargeChange -= SetCharge;
        PlayerController.OnLoadedRocketsChange -= UpdateLoadedRockets;
    }
    private void SetCharge(float fillValue)
    {
        specialSlider.value = fillValue * specialSlider.maxValue;
    }
    private void UpdateLoadedRockets(float amount)
    {
        loadedRocketsText.text = $"{amount}";
    }
}
