using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellUI : MonoBehaviour
{
    [Header("Отображение активации спелла")] public Image activeAura;
    [Header("Отображение восстановления спелла")] public TextMeshProUGUI cooldownTimer;

    /// <summary>
    /// Отобразить выбранную пособность
    /// </summary>
    /// <param name="status">выбрал ли игрок эту способность</param>
    public void SetSpellStatus(bool status)
    {
        activeAura.enabled = status;
    }

    /// <summary>
    /// Отобразить время восстановления способности
    /// </summary>
    public void ShowCooldownTime(float cooldown)
    {
        if (cooldown > 0)
        {
            cooldownTimer.enabled = true;
            cooldownTimer.SetText(((int)cooldown + 1).ToString());
        }
        else
        {
            cooldownTimer.enabled = false;
        }
    }
}