﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Скрипт чтобы выйти из игры в дебаг версии
public class ExitGame : MonoBehaviour
{
    
    public void CloseGame()
    {
        Application.Quit(0);
    }
}
