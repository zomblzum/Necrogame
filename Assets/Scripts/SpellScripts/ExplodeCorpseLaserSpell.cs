using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeCorpseLaserSpell : Spell
{
	[Header("Длина луча")] public int laserDistance = 200;
	[Header("Начало")] public Transform startPos;
	[Header("Направление")] public Transform targetPos;
	[Header("Фильтр по слоям")] public LayerMask ignoreMask;
	[Header("Урон")] public int damage;

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
	}

	void FixedUpdate()
	{
		curTime += Time.deltaTime;
		if (cast)
		{
			lr.SetPosition(0, startPos.position);
			RaycastHit hit;
			if (Physics.Raycast(startPos.position, targetPos.forward, out hit, laserDistance, ~ignoreMask))
			{
				if (hit.collider)
				{
					lr.SetPosition(1, hit.point);
					ExplodeCorpse(hit.collider.gameObject.GetComponent<Corpse>());
				}
			}
			else
			{
				lr.SetPosition(1, targetPos.forward * laserDistance);
			}
			if (curTime >= 1f)
			{
				magicBehaviour.DryManaFromPlayer(usingPrice);
				curTime = 0f;
			}
		}
		if (!magicBehaviour.HaveMana(usingPrice))
		{
			StopCast();
		}
	}

	/// <summary>
	/// Взорвать труп
	/// </summary>
	private void ExplodeCorpse(Corpse corpse)
	{
		if (corpse != null)
		{
			corpse.Explode(magicBehaviour.CalculateSpellDamage(damage));
		}
	}
}
