using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Дополнительная сила атаки")]  public int attack = 0;
    [Header("Дополнительная скорость атаки")] public float speed = 0;
    [Header("VFX при атаке")] public GameObject hitEffect;

    private int totalAttack;
    private Collider collider;
    private AttackBehaviour attackBehaviour;
    private List<int> hittedEnemys;

    private void Start()
    {
        collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
        hittedEnemys = new List<int>();
        attackBehaviour = FindObjectOfType<AttackBehaviour>();
    }

    /// <summary>
    /// Нанесение удара
    /// Фишка в том, чтобы на время атаки активировать коллайдер
    /// </summary>
    public void Attack()
    {
        totalAttack = attack + attackBehaviour.attackPower;
        collider.enabled = true;
    }

    /// <summary>
    /// Отключение коллайдера меча после удара или для скрытия оружия
    /// </summary>
    public void HideWeapon()
    {
        collider.enabled = false;
        // Нужно очищать, чтобы можно было сново бить этого врага
        hittedEnemys.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        //Находим врагов в момент атаки и наносим им урон
        Character enemy = other.gameObject.GetComponent<Character>();
        if(enemy != null && !hittedEnemys.Contains(enemy.GetHashCode()))
        {
            hittedEnemys.Add(enemy.GetHashCode());
            enemy.GetHit(attackBehaviour.gameObject, totalAttack);
            Instantiate(hitEffect, other.transform.position, Quaternion.Euler(hitEffect.transform.rotation.eulerAngles));
            //Защита от мульти удара по одному врагу
        }
    }
}
