using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Класс для обработки трупов
/// </summary>
public class Corpse : MonoBehaviour
{
    [Header("Радиус взрыва")] public float explosionRadius = 20;
    [Header("VFX при взрыве")] public GameObject explosionEffect;
    [Header("VFX радиуса взрыва")]  public List<ParticleSystem> explosionsAuraList;
    [Header("Воскрешаемый миньон")] public GameObject resurectedObject;
    [Tooltip("так как у нас убитые персонажи исчезают, а на месте их сразу появляются трупы, то не видно последний полученный ими удар(т.к. канвас исчезает вместес мобов)")]
    [Header("Интерфейс обработчик урона")] public CharacterDamageUI characterDamageUI;

    protected int deadBool;
    protected bool resurect = false;

    void Start()
    {
        //Запускаем анимацию смерти
        deadBool = Animator.StringToHash("Dead");
        GetComponent<Animator>().SetBool(deadBool,true);
        //Рисуем область поражения от взрыва
        foreach(ParticleSystem particle in explosionsAuraList)
        {
            ParticleSystem.ShapeModule shape = particle.shape;
            shape.radius = explosionRadius;
        }
    }

    /// <summary>
    /// Взорвать труп
    /// </summary>
    public void Explode(int damage)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider collider in colliders)
        {
            Character character = collider.GetComponent<Character>();
            if(character !=null)
            {
                character.GetHit(FindObjectOfType<Player>().gameObject, damage);
            }
        }
        Instantiate(explosionEffect, transform.position, Quaternion.Euler(new Vector3(0f, 0f)));
        Destroy(gameObject);
    }

    /// <summary>
    /// Воскресить труп
    /// </summary>
    public void Resurect(MinionBehaviour minionBehaviour)
    {
        if (!resurect)
        {
            resurect = true;
            Minion newZombie = Instantiate(resurectedObject, gameObject.transform.position, gameObject.transform.rotation).GetComponent<Minion>();
            minionBehaviour.AddMinion(newZombie);
            Destroy(gameObject);
        }
    }

    public bool ResurectInProcess()
    {
        return resurect;
    }
}
