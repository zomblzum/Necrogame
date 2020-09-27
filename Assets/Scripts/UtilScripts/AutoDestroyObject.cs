using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyObject : MonoBehaviour
{
    [Header("Время до уничтожения")]
    public float destroyTime = 1f;

    private float curTime = 0;

    void Update()
    {
        if(curTime <= destroyTime)
        {
            curTime += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
