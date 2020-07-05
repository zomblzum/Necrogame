using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public abstract class Summoner : MonoBehaviour
{
    [Serializable]
    public class SummonableMinion
    {
        [Header("Призываемое существо")]
        public Minion minion;
        [Header("Необходимое время зажатия кнопки")]
        public float summonTime;
    }

    [Header("Список призываемых прислужников")]
    public List<SummonableMinion> summonableMinions;
    [Header("Место появления")]
    public Transform summonPosition;
    [Header("Тип призываемых миньонов")]
    public string minionType;

    protected Minion minionToSummon;

    private float curTime = 0f;
    private bool isStarted = false;

    private void Update()
    {
        if (isStarted) 
        { 
            curTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Стартуем процесс призыва
    /// </summary>
    public virtual void StartSummoning() 
    {
        isStarted = true;
    }

    /// <summary>
    /// Запущен ли призыв
    /// </summary>
    public bool IsSummonStart()
    {
        return isStarted;
    }

    /// <summary>
    /// Призыв прислужника
    /// </summary>
    /// <returns>Появившийся миньон, который будет добавлен под контроль игрока</returns>
    public virtual Minion Summon()
    {
        minionToSummon = GetMinionForSummon();

        if (minionToSummon != null)
        {
            InstantiateMinion(minionToSummon);
        }

        StopSummoner();
        return minionToSummon;
    }

    protected virtual Minion GetMinionForSummon()
    {
        for (int i = 0; i < summonableMinions.Count; i++)
        {
            if (curTime >= summonableMinions[i].summonTime)
            {
                return summonableMinions[i].minion;
            }
        }
        return null;
    }

    protected virtual void InstantiateMinion(Minion minion)
    {
        minionToSummon = Instantiate(minion, summonPosition.position, minion.transform.rotation);
    }

    /// <summary>
    /// Остановить призыв
    /// </summary>
    public virtual void StopSummoner()
    {
        isStarted = false;
        curTime = 0f;
    }

    /// <summary>
    /// Заплатить за призыв
    /// </summary>
    /// <param name="player">Ссылка на игрока</param>
    public abstract void TakeCost(Player player);

    /// <summary>
    /// Возможность запустить механизм призыва
    /// </summary>
    /// <param name="player">Призывающий игрок</param>
    /// <returns>Наличие всех необходимых ресурсов</returns>
    public abstract bool CanSummon(Player player);

}
