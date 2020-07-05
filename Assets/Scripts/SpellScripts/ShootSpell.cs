using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSpell : Spell
{
    [Header("Выстреливаемый снаряд")] public Projectile objectToShoot;
    [Header("Направление полета")] public Transform target;
    [Header("Скорость выстрела")] public float speed = 1f;

    private bool shoot;

    public override void Cast()
    {
        shoot = true;
    }

    public override void StopCast()
    {
        shoot = false;
    }

    void FixedUpdate()
    {
        if (active && shoot)
        {
            Rigidbody projectile = Instantiate(objectToShoot.gameObject, transform.position, Quaternion.Euler(new Vector3(0,0))).GetComponent<Rigidbody>();
            projectile.AddForce(target.forward * speed * objectToShoot.speed);
            StopCast();
        }
    }
}
