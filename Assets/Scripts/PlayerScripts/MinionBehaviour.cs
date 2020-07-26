using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinionBehaviour : SpellBehaviour
{
    [Serializable]
    public class MinionGroup
    {
        [Header("Прислужники в группе")]
        public List<Minion> minions;
        [Header("Тип прислужников")]
        public string minionType;
        [Header("Иконка этой группы")]
        public Sprite minionIcon;
        [Header("Контроллер для особого поведения группы")]
        public MinionGroupController groupController;

        /// <summary>
        /// Добавить прислужника в группу
        /// </summary>
        /// <param name="minion">прислужник</param>
        public void AddMinion(Minion minion)
        {
            minion.SetCommandBehaviour(groupController.minionCommand);
            minion.SetAttackTarget(groupController.attackTarget);
            minion.SetMoveTarget(groupController.movePosition);
            Debug.Log(groupController.minionCommand.commandName + " активная команда у призваного моба");
            minions.Add(minion);
            groupController.MinionAdded();
        }

        public void RemoveMinion(Minion minion)
        {
            minions.Remove(minion);
            groupController.MinionRemoved();
        }

        /// <summary>
        /// Команда атаковать группе
        /// </summary>
        public void ChangeAttackTarget(GameObject attackTarget)
        {
            groupController.SetGroupAttackTarget(attackTarget);
        }

        /// <summary>
        /// Команда передвижения группе
        /// </summary>
        public void ChangeMovePosition(Vector3 position)
        {
            groupController.SetGroupMoveTarget(position);
        }

        /// <summary>
        /// Поменять текущее поведение прислужников
        /// </summary>
        public void ChangeCurrentCommand(MinionCommand minionCommand)
        {
            groupController.ChangeCurrentCommand(minionCommand);
        }
    }

    [Header("Отряд прислужников")]
    public List<MinionGroup> minionGroups;
    [Header("Номер выбранной группы прислужников")]
    public int currentGroup;
    [Header("Максимальное количество миньонов в группе")]
    [Tooltip("Вынесено в паблик поле для случая, если понадобится расширять или уменьшать допустимый объем группы")]
    public int maxMinions = 10;
    [Header("Изображение выбранной группы")]
    public Image currentGroupImage;
    [Header("Количество прислужников в выбранной группе")]
    public TextMeshProUGUI currentGroupCount;
    [Header("Окно с командами для прислужников")]
    public GameObject minionCommandsPanel;
    [Header("Список возможных комманд")]
    public List<MinionCommand> minionCommands;

    private void Start()
    {
        ChangeSelectedGroupInfo();
        SetCommandWindowStatus(false);
        foreach(MinionGroup minionGroup in minionGroups)
        {
            minionGroup.groupController.SetGroupToController(minionGroup);
            minionGroup.ChangeCurrentCommand(new MinionCommand("Disgroup"));
        }
    }

    void Update()
    {
        if (Input.GetButtonDown(changeMinionGroupUp))
        {
            ChangeCurrentGroup(1);
        }
        if (Input.GetButtonDown(changeMinionGroupDown))
        {
            ChangeCurrentGroup(-1);
        }

        if (minionCommandsPanel.activeSelf)
        {
            if (Input.GetButtonDown(commandWindowButton))
            {
                player.spellMode = true;
                SetCommandWindowStatus(false);
            }
            if (Input.GetButtonDown(summonMinionsButton))
            {
                SetCommandWindowStatus(false);
            }
            // Если игрок под станом, то команды не обрабатываем
            if(basicBehaviour.Stunned())
            {
                return;
            }
            if (Input.GetButtonDown(firstSpellButton))
            {
                // команда атаковать
                GameObject attackTarget = FindObjectOfType<ThirdPersonOrbitCamBasic>().playerTarget.GetCurrentTarget();
                ChangeGroupCommand(new MinionCommand("Attack"));
                ChangeGroupAttackTarget(attackTarget);
            }
            if (Input.GetButtonDown(secondSpellButton))
            {
                // команда защиты
                ChangeGroupCommand(new MinionCommand("Defend"));
            }
            if (Input.GetButtonDown(thirdSpellButton))
            {
                // команда двигаться
                Vector3 position = FindObjectOfType<ThirdPersonOrbitCamBasic>().playerTarget.GetCurrentPosition();
                ChangeGroupCommand(new MinionCommand("Move"));
                ChangeGroupMoveTarget(position);
            }
            if (Input.GetButtonDown(fourthSpellButton))
            {
                // команда рассредоточиться
                ChangeGroupCommand(new MinionCommand("Disgroup"));
            }
        }
        else
        {
            if (Input.GetButtonDown(commandWindowButton))
            {
                player.spellMode = false;
                SetCommandWindowStatus(true);
            }
        }
    }

    /// <summary>
    /// Добавление прислужника в пул
    /// Вызывается при суммоне и в некоторых особых случаях (например тригер зомби)
    /// </summary>
    /// <param name="minion">Любая реализация прислужника</param>
    public void AddMinion(Minion minion)
    {
        // Добавляем прислужника в общую и соответствующую ему группу
        foreach (MinionGroup group in minionGroups)
        {
            if (group.minionType == "All" || group.minionType == minion.characterName)
            {
                group.AddMinion(minion);
            }
        }

        // Если выбрана одна из двух вышеуказанных групп, то обновляем счетчик
        if(minionGroups[currentGroup].minionType == "All" || minionGroups[currentGroup].minionType == minion.characterName)
        {
            UpdateMinionsCounter();
        }
    }

    /// <summary>
    /// Поменять текущую команду в зависимости от выбранной группы
    /// </summary>
    /// <param name="minionCommand">Новая команда</param>
    public void ChangeGroupCommand(MinionCommand minionCommand)
    {
        if(minionGroups[currentGroup].minionType == "All")
        {
            foreach(MinionGroup minionGroup in minionGroups)
            {
                minionGroup.ChangeCurrentCommand(minionCommand);
            }
        }
        else
        {
            minionGroups[currentGroup].ChangeCurrentCommand(minionCommand);
        }
    }

    /// <summary>
    /// Поменять цель движения в зависимости от выбранной группы
    /// </summary>
    /// <param name="movePosition">Точка для перемещения</param>
    public void ChangeGroupMoveTarget(Vector3 movePosition)
    {
        if (minionGroups[currentGroup].minionType == "All")
        {
            foreach (MinionGroup minionGroup in minionGroups)
            {
                minionGroup.ChangeMovePosition(movePosition);
            }
        }
        else
        {
            minionGroups[currentGroup].ChangeMovePosition(movePosition);
        }
    }

    /// <summary>
    /// Поменять цель атаки в зависимости от выбранной группы
    /// </summary>
    /// <param name="attackTarget">Новая цель для атаки</param>
    public void ChangeGroupAttackTarget(GameObject attackTarget)
    {
        if (minionGroups[currentGroup].minionType == "All")
        {
            foreach (MinionGroup minionGroup in minionGroups)
            {
                minionGroup.ChangeAttackTarget(attackTarget);
            }
        }
        else
        {
            minionGroups[currentGroup].ChangeAttackTarget(attackTarget);
        }
    }

    /// <summary>
    /// Удалить прислужника из всех групп
    /// Срабатывает при смерти прислужника
    /// </summary>
    /// <param name="minion"></param>
    public void RemoveMinion(Minion minion)
    {
        // Аналогично как с добавлением
        // Небольшое дублированние кода, но так он более очевидный и чистый
        foreach (MinionGroup group in minionGroups)
        {
            if (group.minionType == "All" || group.minionType == minion.characterName)
            {
                group.RemoveMinion(minion);
            }
        }

        if (minionGroups[currentGroup].minionType == "All" || minionGroups[currentGroup].minionType == minion.characterName)
        {
            UpdateMinionsCounter();
        }
    }

    /// <summary>
    /// Выбрать определенную группу прислужников для отдачи приказов
    /// </summary>
    /// <param name="direction">идти вперед или назад по списку</param>
    public void ChangeCurrentGroup(int direction)
    {
        currentGroup += direction;

        if (currentGroup < 0)
        {
            currentGroup = minionGroups.Count - 1;
        } 
        else if (currentGroup >= minionGroups.Count)
        {
            currentGroup = 0;
        }

        ChangeSelectedGroupInfo();
    }

    /// <summary>
    /// Обновление панели о выбранной группе
    /// </summary>
    public void ChangeSelectedGroupInfo()
    {
        currentGroupImage.sprite = minionGroups[currentGroup].minionIcon;
        UpdateMinionsCounter();
    }

    /// <summary>
    /// Изменение отображаемого количества миньонов
    /// Срабатывает при призыве новых и смерти старых
    /// </summary>
    public void UpdateMinionsCounter()
    {
        currentGroupCount.SetText(minionGroups[currentGroup].minions.Count.ToString() + "/" + maxMinions.ToString());
    }

    /// <summary>
    /// Открыть/закрыть окно команд
    /// </summary>
    public void SetCommandWindowStatus(bool status)
    {
        minionCommandsPanel.SetActive(status);
    }

    /// <summary>
    /// Определяет возможность добавить прислужника в общий пул
    /// </summary>
    public bool CanAddMinion()
    {
        foreach(MinionGroup minionGroup in minionGroups)
        {
            if(minionGroup.minionType == "All")
            {
                return minionGroup.minions.Count < maxMinions;
            }
        }

        // Если такой группы вообще нет, то не можем призвать
        // В текущих условиях такого никогда не случится
        return false;
    }
}
