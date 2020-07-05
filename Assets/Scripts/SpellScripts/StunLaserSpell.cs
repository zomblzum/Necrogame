using UnityEngine;
using System.Collections;

public class StunLaserSpell : Spell
{
	[Header("Длина луча")] public int laserDistance = 20;
	[Header("Начало")] public Transform startPos;
	[Header("Направление")] public Transform targetPos;
	[Header("Фильтр по слоям")] public LayerMask ignoreMask;

	private LineRenderer lr;
	private bool cast;
	private float curTime;

    public override void Cast()
    {
		cast = true;
		lr.enabled = true;
	}

    public override void StopCast()
    {
		cast = false;
		lr.enabled = false;
	}

    void Awake()
    {
		lr = GetComponent<LineRenderer>();
		cast = false;
	}

	void FixedUpdate()
	{
		curTime += Time.deltaTime;
		if (cast)
		{
			lr.SetPosition(0, startPos.position);
			RaycastHit hit;
			if (Physics.Raycast(startPos.position, targetPos.forward, out hit,laserDistance, ~ignoreMask))
			{
				if (hit.collider)
				{
					lr.SetPosition(1, hit.point);
					StunCharacter(hit.collider.gameObject.GetComponent<IStunable>());
				}
			}
			else
			{
				lr.SetPosition(1, targetPos.forward * laserDistance);
			}
			if (curTime >= 1f)
			{
				magicBehaviour.DryManaFromPlayer(price);
				curTime = 0f;
			}
		} 
		if (!magicBehaviour.HaveMana(price))
        {
			StopCast();
        }
	}

	/// <summary>
	/// Застанить врага
	/// </summary>
	private void StunCharacter(IStunable character)
    {
		if(character != null)
        {
			character.StunForTime(0.1f);
		}
    }
}