using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("Скорость полета")] public float speed = 1f;
    [Header("Урон")] public int damage = 10;

    protected MagicBehaviour magicBehaviour;

    void Start()
    {
        magicBehaviour = FindObjectOfType<MagicBehaviour>();
    }

    protected void DestroyThis()
    {
        Destroy(gameObject);
    }
}