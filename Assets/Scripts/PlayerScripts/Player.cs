using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Основной класс с данными персонажа
/// </summary>
public class Player : MonoBehaviour, IAttackable, IDieable
{
    [Header("Максимальное здоровье")] 
    public int health = 100;
    [Header("Максимальная мана")] 
    public int mana = 100;
    [Header("Деньги")] 
    public int money = 100;
    [Header("Скорость восполнения здоровья")] 
    public float healthRegenerateTime = 1f;
    [Header("Скорость восполнения маны")] 
    public float manaRegenerateTime = 1f;
    [Header("Интерфейс персонажа")]
    public PlayerUI playerUI;
    [Header("Текущее здоровье")]
    public int curHealth;
    [Header("Текущая мана")]
    public int curMana;
    [Header("Задержка перед восстановлением ресурсов")]
    public float waitingRegenerationTime;
    [Header("Активный режим заклинаний")]
    public bool spellMode;
    [Header("Список дебафов на увеличение получаемого урона")]
    public List<DamageBuff> damageBuffs;

    private float curHealthTime = 0f;
    private float curManaTime = 0f;
    float curWaitingTime = 0f;

    void Start()
    {
        playerUI = FindObjectOfType<PlayerUI>();
        playerUI.SetHealthBarMax(health);
        playerUI.SetManaBarMax(mana);
        playerUI.UpdateHealthBar(health);
        playerUI.UpdateManaBar(mana);
        playerUI.UpdateMoneyText(money);

        curWaitingTime = waitingRegenerationTime;
        curHealth = health;
        curMana = mana;
    }

    void LateUpdate()
    {
        if(curWaitingTime >= waitingRegenerationTime)
        {
            ResourceRegenerate();
        }
        else
        {
            curWaitingTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Восстановление ресурсов персонажа со временем
    /// </summary>
    private void ResourceRegenerate()
    {
        curHealthTime += Time.deltaTime;
        curManaTime += Time.deltaTime;

        if (curHealthTime >= healthRegenerateTime && curHealth < health)
        {
            curHealth += 1;
            curHealthTime = 0;
            playerUI.UpdateHealthBar(curHealth);
        }

        if (curManaTime >= manaRegenerateTime && curMana < mana)
        {
            curMana += 1;
            curManaTime = 0;
            playerUI.UpdateManaBar(curMana);
        }
    }

    public void Die()
    {
        //Тестовая версия смерти
        //Включаем курсор обратно и меняем сцену на меню
        //Потом это лучше вынести в отдельный скрипт в меню
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        new SceneChanger().ChangeScene("MenuScene");
    }

    public void GetHit(GameObject attackFrom, int damage)
    {
        if (curHealth > 0)
        {
            //Проходимся по всем модификаторам урона
            foreach(DamageBuff damageBuff in damageBuffs)
            {
                damage += damageBuff.GetAdditionalDamage(damage);
            }
            //Вычитаем суммированый урон
            curHealth -= damage;
            playerUI.UpdateHealthBar(curHealth);
            if (curHealth <= 0)
            {
                curHealth = 0;
                Die();
            }
        }
    }

    public bool IsAlive()
    {
        return curHealth > 0;
    }

    /// <summary>
    /// Расход маны
    /// </summary>
    /// <param name="manaToSpend"></param>
    public void SpendMana(int manaToSpend)
    {
        curWaitingTime = 0f;
        curMana -= manaToSpend;
        playerUI.UpdateManaBar(curMana);
    }

    public void SpendMoney(int moneyToSpend)
    {
        money -= moneyToSpend;
        playerUI.UpdateMoneyText(money);
    }
}
