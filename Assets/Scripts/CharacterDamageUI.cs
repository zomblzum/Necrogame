using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDamageUI : MonoBehaviour
{
    [Header("Панель с данными персонажа")] public GameObject damageSpawnPosition;
    [Header("Панель с данными персонажа")] public GameObject damageText;

    public void SpawnDamageText(string text)
    {
        GameObject spawnedText = Instantiate(damageText, damageSpawnPosition.transform);
        spawnedText.GetComponent<TextMeshProUGUI>().text = text;
    }
}
