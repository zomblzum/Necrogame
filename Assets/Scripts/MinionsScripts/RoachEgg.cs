using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoachEgg : MonoBehaviour
{
    [Header("Время для вылупления")]
    public float hatchTime = 60f;
    [Header("Модификатор урона от яиц")]
    public RoachEggsDamageBuff roachEggsDamageBuff;

    private RoachMinion roach;

    public void StartHatch(RoachMinion roach)
    {
        this.roach = roach;
        roach.gameObject.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(Hatch());
    }

    private IEnumerator Hatch()
    {
        roachEggsDamageBuff.EggHatchStart();
        yield return new WaitForSeconds(hatchTime);

        roach.transform.position = transform.position;
        roach.gameObject.SetActive(true);
        roachEggsDamageBuff.EggHatchEnd();
        gameObject.SetActive(false);
    }
}
