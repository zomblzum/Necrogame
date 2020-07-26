using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MinionCommand
{
    [Header("Название комманды")]
    public string commandName;

    public MinionCommand(string commandName)
    {
        this.commandName = commandName;
    }
}
