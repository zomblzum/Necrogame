using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Поведение персонажа для использования магии
/// </summary>
public class MagicBehaviour : SpellBehaviour
{
    [Header("Набор способностей")]
    public List<Spell> spells;

    //private Player player;

    private int currentSpell;
    private AttackBehaviour attackBehaviour;

    void Start()
    {
        attackBehaviour = GetComponent<AttackBehaviour>();
        //player = GetComponent<Player>();

        ChangeSpell(0);
    }

    void Update()
    {
        //Кастуем в любом режиме
        if (Input.GetButtonDown(castButton) && spells[currentSpell].coolDown <= 0f && HaveMana(spells[currentSpell].price))
        {
            player.SpendMana(spells[currentSpell].price);
            spells[currentSpell].Cast();
            spells[currentSpell].StartCooldown();
        }
        if (Input.GetButtonUp(castButton))
        {
            spells[currentSpell].StopCast();
        }
        //Обрабатываем только если поведение доступно
        if (player.spellMode)
        {
            if (Input.GetButtonDown(firstSpellButton))
            {
                ChangeSpell(0);
            }
            if (Input.GetButtonDown(secondSpellButton))
            {
                ChangeSpell(1);
            }
            if (Input.GetButtonDown(thirdSpellButton))
            {
                ChangeSpell(2);
            }
            if (Input.GetButtonDown(fourthSpellButton))
            {
                ChangeSpell(3);
            }
        } 
    }

    /// <summary>
    /// Поменять активную способность
    /// </summary>
    private void ChangeSpell(int spellId)
    {
        // Сначала отменяем все способности
        foreach (Spell spell in spells)
        {
            spell.ChangeSpellStatus(false);
        }
        //После чего активируем нужную
        spells[spellId].ChangeSpellStatus(true);
        currentSpell = spellId;
    }

    public void DryManaFromPlayer(int manaCost)
    {
        player.SpendMana(manaCost);
    }

    /// <summary>
    /// Имеет ли игрок нужное количество маны
    /// </summary>
    public bool HaveMana(int manaNeeded)
    {
        return player.curMana >= manaNeeded;
    }

    /// <summary>
    /// Расчет урона для заклинания
    /// </summary>
    /// <param name="spellDamage">урон от используемого заклинания</param>
    /// <returns>урон с учетом атаки игрока и оружия</returns>
    public int CalculateSpellDamage(int spellDamage)
    {
        float minDamage = (spellDamage + attackBehaviour.attackPower * attackBehaviour.weapon.attack) + ((spellDamage + attackBehaviour.attackPower * attackBehaviour.weapon.attack) / 100 * -20);
        float maxDamage = (spellDamage + attackBehaviour.attackPower * attackBehaviour.weapon.attack) + ((spellDamage + attackBehaviour.attackPower * attackBehaviour.weapon.attack) / 100 * 20);

        return (int) Random.Range(minDamage, maxDamage);
    }
}