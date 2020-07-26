using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Атака оружием
/// </summary>
public class AttackBehaviour : GenericBehaviour
{

    [Header("Сила атаки")] public int attackPower = 100;
    [Header("Скорость атаки")] public float attackSpeed = 1f;
    [Header("Текущее оружие")] public Weapon weapon;

    private string attackButton = "Attack";
    private int attackSpeedFloat;
    private int attackBool;

    void Start()
    {
        attackBool = Animator.StringToHash("Attack");
        attackSpeedFloat = Animator.StringToHash("AttackSpeed");
    }

    void Update()
    {
        
        // Отслеживание нажатия клавиши атаки
        if (Input.GetButtonDown(attackButton) && !behaviourManager.GetAnim.GetBool(attackBool) && !behaviourManager.Stunned())
        {
            behaviourManager.GetAnim.SetFloat(attackSpeedFloat, attackSpeed + weapon.speed);
            behaviourManager.GetAnim.SetBool(attackBool, true);
        }
    }

    /// <summary>
    /// Вызывается в момент удара оружием
    /// </summary>
    public void Hit()
    {
        weapon.Attack();
    }

    /// <summary>
    /// Вызывается в момент окончания атаки
    /// </summary>
    public void EndAttack()
    {
        behaviourManager.GetAnim.SetBool(attackBool, false);
        weapon.HideWeapon();
    }
}
