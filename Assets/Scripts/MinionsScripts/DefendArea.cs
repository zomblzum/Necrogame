using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendArea : MonoBehaviour
{
    [SerializeField]
    [Header("Миньон")]
    private Minion minion;
    [SerializeField]
    [Header("Зона защиты")]
    private CapsuleCollider areaCollider;

    private List<GameObject> enemies;

    private void Start()
    {
        enemies = new List<GameObject>();
    }

    private void Update()
    {
        if (minion)
        {
            transform.position = minion.transform.position;

            //вынес это в update, так что можно считать костылем
            //когда этот код был в Ontriggerexit, то миньоны возвращались к своим точкам только после любого нажатия кнопки игроком
            //такой хрени я еще не видел конечно
            enemies.RemoveAll(item => item == null);
            if (enemies.Count == 0 && minion.inAggro)
            {
                minion.inAggro = false;
                minion.ReturnToDefaultPosition();
            }
        }
    }

    public void SetAreaRadius(float radius)
    {
        areaCollider.radius = radius;
    }

    public void SetMinion(Minion minion)
    {
        this.minion = minion;
    }

    public int EnemysCount()
    {
        return enemies.Count;
    }

    public List<GameObject> GetEnemies()
    {
        return enemies;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            enemies.Add(other.gameObject);
            minion.inAggro = true;

            if (minion.GetCurrentTarget() == null)
            {
                minion.SetAttackTarget(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            enemies.Remove(other.gameObject);

            if (enemies.Count > 0 && (minion.GetCurrentTarget() == other.gameObject || minion.GetCurrentTarget() == null))
            {
                minion.SetAttackTarget(enemies[0]);
            }
        }
    }
}
