using UnityEngine;

// Вынес это в отдельный класс, потому что иначе приходится бесконечно дублировать поля
// Также есть шанс, что здесь появится общая логика
public class SpellBehaviour : MonoBehaviour
{
    protected string changeMinionGroupUp = "ChangeMinionGroupUp";
    protected string changeMinionGroupDown = "ChangeMinionGroupDown";
    protected string commandWindowButton = "CommandMinion";
    protected string summonMinionsButton = "SummonMinion";
    protected string firstSpellButton = "Spell1";
    protected string secondSpellButton = "Spell2";
    protected string thirdSpellButton = "Spell3";
    protected string fourthSpellButton = "Spell4";
    protected string castButton = "Spell";

    protected Player player;

    protected void Awake()
    {
        player = GetComponent<Player>();
    }
}
