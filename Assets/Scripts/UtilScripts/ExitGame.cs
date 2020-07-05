using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Скрипт чтобы выйти из игры в дебаг версии
public class ExitGame : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit(0);
        }
    }
}
