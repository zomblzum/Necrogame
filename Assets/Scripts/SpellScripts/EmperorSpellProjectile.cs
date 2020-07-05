using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmperorSpellProjectile : Projectile
{
    [Header("Множитель урона по прислужникам")] 
    public int minionsDamageModifier;

    private void Start()
    {
        magicBehaviour = FindObjectOfType<MagicBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Сначала проверяем на дружественную цель
        Minion minion = other.gameObject.GetComponent<Minion>();
        if (minion != null)
        {
            minion.GetHit(magicBehaviour.gameObject, magicBehaviour.CalculateSpellDamage(damage) * minionsDamageModifier);
            DestroyThis();
        }

        //Пока что так, если стреляем а так тригер коллайдер, то игнорим это и летим дальше
        if (other.isTrigger)
        {
            return;
        }

        // Затем проверка на любого персонажа
        Character character = other.gameObject.GetComponent<Character>();
        if (character != null)
        {
            character.GetHit(magicBehaviour.gameObject, magicBehaviour.CalculateSpellDamage(damage));
            DestroyThis();
        }

        //Уничтожаем снаряд, если коллайдим не с игроком
        if (other.gameObject.GetComponent<Player>() == null && other.gameObject.GetComponent<Corpse>() == null)
        {
            DestroyThis();
        }
    }
}
