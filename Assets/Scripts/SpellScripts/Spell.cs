using UnityEngine;

/// <summary>
/// Магическая способность персонажа
/// </summary>
public abstract class Spell : MonoBehaviour
{
    [Header("Здоровье")]
    public int healthBonus = 0;
    [Header("Мана")]
    public int manaBonus = 0;
    [Header("Атака")]
    public int attackBonus = 0;
    [Header("Цена покупки")]
    public int buyingPrice = 0;
    [Space(20)]
    [Header("Выбрана ли способность")] public bool active;
    [Header("Стоймость использования")] public int usingPrice = 1;
    [Header("Время восстановления")] public float coolDown = 3f;
    [Header("Интерфейсная составляющая")] public SpellUI spellUI;

    protected MagicBehaviour magicBehaviour;
    
    private float coolDownMax;

    /// <summary>
    /// Использование способности
    /// </summary>
    public abstract void Cast();

    /// <summary>
    /// Отключение способности
    /// </summary>
    public abstract void StopCast();

    public void Start()
    {
        magicBehaviour = FindObjectOfType<MagicBehaviour>();
        coolDownMax = coolDown;
        coolDown = 0f;

        magicBehaviour.player.health += healthBonus;
        magicBehaviour.player.mana += manaBonus;
        magicBehaviour.attackBehaviour.attackPower += attackBonus;
    }

    /// <summary>
    /// Включить/выключить способность
    /// </summary>
    public void ChangeSpellStatus(bool status)
    {
        StopCast();
        active = status;
        spellUI.SetSpellStatus(status);
    }

    protected void Update()
    {
        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
        }
        spellUI.ShowCooldownTime(coolDown);
    }

    /// <summary>
    /// Запустить кулдаун способности
    /// </summary>
    public void StartCooldown()
    {
        coolDown = coolDownMax;
    }
}
