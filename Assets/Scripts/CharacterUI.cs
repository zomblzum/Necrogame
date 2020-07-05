using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [Header("Индикатор здоровья")] public Slider healthBar;
    [Header("Поле с именем")] public TextMeshProUGUI characterName;
    [Header("Панель с данными персонажа")]  public Canvas characterPanel;


    public void Start()
    {
        
    }

    public void UpdateHealthBar(int value)
    {
        healthBar.value = value;
    }

    public void SetName(string value)
    {
        characterName.SetText(value);
    }

    private void Update()
    {
        characterPanel.transform.LookAt(Camera.main.transform.position);
    }
}
