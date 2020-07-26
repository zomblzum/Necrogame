using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonBehaviour : SpellBehaviour
{
    [Header("Реализованные механизмы призыва")]
    public List<Summoner> summoners;
    [Header("Окно с обозначениями призываемых миньонов")]
    public GameObject minionSummonPanel;

    private MinionBehaviour minionBehaviour;

    void Start()
    {
        minionBehaviour = GetComponent<MinionBehaviour>();
        SetSummonWindowStatus(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (minionSummonPanel.activeSelf)
        {
            // Закрыть меню и возможность призыва
            // Также отменяем все активные запущенные призывы
            if (Input.GetButtonDown(commandWindowButton))
            {
                SetSummonWindowStatus(false);
                StopAllSummoners();
            }
            // Закрыть меню и возможность призыва
            // Также отменяем все активные запущенные призывы
            if (Input.GetButtonDown(summonMinionsButton))
            {
                player.spellMode = true;
                SetSummonWindowStatus(false);
                StopAllSummoners();
            }

            // Если игрок под станом, то команды не обрабатываем
            if (basicBehaviour.Stunned())
            {
                return;
            }

            // Начать призыв, зажав кнопку
            if (Input.GetButtonDown(firstSpellButton))
            {
                StartSummoner(0);
            }
            if (Input.GetButtonDown(secondSpellButton))
            {
                StartSummoner(1);
            }
            if (Input.GetButtonDown(thirdSpellButton))
            {
                StartSummoner(2);
            }
            if (Input.GetButtonDown(fourthSpellButton))
            {
                StartSummoner(3);
            }
            // Призвать существо, в зависимости от времени нажатия
            if (Input.GetButtonUp(firstSpellButton))
            {
                SummonMinion(0);
            }
            if (Input.GetButtonUp(secondSpellButton))
            {
                SummonMinion(1);
            }
            if (Input.GetButtonUp(thirdSpellButton))
            {
                SummonMinion(2);
            }
            if (Input.GetButtonUp(fourthSpellButton))
            {
                SummonMinion(3);
            }
        }
        else
        {
            if (Input.GetButtonDown(summonMinionsButton))
            {
                player.spellMode = false;
                SetSummonWindowStatus(true);
            }
        }
    }

    private void StopAllSummoners()
    {
        foreach (Summoner summoner in summoners)
        {
            summoner.StopSummoner();
        }
    }

    private void StartSummoner(int summonerId)
    {
        if(summoners[summonerId].CanSummon(player) && minionBehaviour.CanAddMinion())
        {
            summoners[summonerId].StartSummoning();
        }
    }

    private void SummonMinion(int summonerId)
    {
        if (summoners[summonerId].IsSummonStart())
        {
            Minion minion = summoners[summonerId].Summon();

            if (minion != null)
            {
                summoners[summonerId].TakeCost(player);
                minionBehaviour.AddMinion(minion);
            }
            else
            {
                Debug.Log("Ошибка призыва");
            }
        }
    }

    /// <summary>
    /// Открыть/закрыть окно команд
    /// </summary>
    public void SetSummonWindowStatus(bool status)
    {
        minionSummonPanel.SetActive(status);
    }
}
