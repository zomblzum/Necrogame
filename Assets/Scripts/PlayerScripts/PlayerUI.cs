using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Здоровье")] public Slider healthBar;
    [Header("Мана")] public Slider manaBar;
    [Header("Деньги")] public TextMeshProUGUI money;

    [Header("Индикатор низкого здоровья")] public GameObject lowHealthIndicator;
    [Header("Индикатор низкой маны")] public GameObject lowManaIndicator;

    /// <summary>
    /// Информация о здоровье
    /// </summary>
    public void UpdateHealthBar(int value)
    {
        healthBar.value = value;
    }

    /// <summary>
    /// Информация о мане
    /// </summary>
    public void UpdateManaBar(int value)
    {
        manaBar.value = value;
    }

    /// <summary>
    /// Информация о деньгах
    /// </summary>
    public void UpdateMoneyText(int value)
    {
        money.SetText(value.ToString());
    }

    /// <summary>
    /// Задать максимальное количество здоровья
    /// Необходимо делать при каждом повышении этой характеристики
    /// </summary>
    public void SetHealthBarMax(int value)
    {
        healthBar.maxValue = value;
    }

    /// <summary>
    /// Задать максимальное количество маны
    /// Необходимо делать при каждом повышении этой характеристики
    /// </summary>
    public void SetManaBarMax(int value)
    {
        manaBar.maxValue = value;
    }
}
