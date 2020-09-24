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
    
    private bool scaled = false;
    private float canvasSize;

    public void UpdateHealthBar(int value)
    {
        healthBar.value = value;
    }

    public void SetName(string value)
    {
        characterName.SetText(value);
    }

    public bool IsScaled()
    {
        return scaled;
    }

    public void SetScaleMode(bool scaled)
    {
        this.scaled = scaled;
        if(scaled)
        {
            characterPanel.transform.localScale = new Vector3(canvasSize + 0.004f, canvasSize + 0.004f, canvasSize + 0.004f);
            characterPanel.GetComponentInChildren<Image>().color = new Color(0.3f, 0, 0, 0.3f); //красный прозрачный
        }
        else
        {
            characterPanel.transform.localScale = new Vector3(canvasSize, canvasSize, canvasSize);
            characterPanel.GetComponentInChildren<Image>().color = new Color(0, 0, 0, 0.3f); //черный прозрачный
        }
    }

    private void Update()
    {
        characterPanel.transform.LookAt(Camera.main.transform.position);
    }

    private void Start()
    {
        canvasSize = characterPanel.transform.localScale.x;
    }
}
