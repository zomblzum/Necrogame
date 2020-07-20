using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DashSpell : Spell
{
    [Header("Сила рывка")] public float dashSpeed = 1f;
    //[Header("Время рывка")] public float dashTime = 0.2f;
    [Header("Прицел для телепорта")] public Transform target;

    private Rigidbody playerBody;
    private float playerMass;
    private bool dash;

    public override void Cast()
    {
        dash = true;
    }

    void Awake()
    {
        playerBody = FindObjectOfType<Player>().gameObject.GetComponent<Rigidbody>();
        playerMass = playerBody.mass;
        dash = false;
    }

    void FixedUpdate()
    {
        if (active && dash)
        {
            Dash();
        }
    }

    private void Dash()
    {

        //Уменьшаем массу игрока, иначе он будет в рывке сносить весь мир своим телом
        //Если же убирать коллайдеры или менять тип тела, то проблем будет еще больше
        playerBody.mass = 0.0001f;

        //Прыгаем только по x и z, так что нужно убрать y
        Vector3 targetDirection = target.forward;
        targetDirection.y = 0;

        //Прыгаем
        playerBody.AddForce(targetDirection * dashSpeed, ForceMode.Impulse);
        StopCast();
    }

    public override void StopCast()
    {
        playerBody.mass = playerMass;
        playerBody.velocity = Vector3.zero;
        dash = false;
    }
}
