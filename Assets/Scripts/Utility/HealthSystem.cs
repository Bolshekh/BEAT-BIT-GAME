using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IHitable
{
	[SerializeField] float healthPoints = 3f;
	[SerializeField] float maxHealthPoints = 3f;
	public float MaxHealthPoints => maxHealthPoints;

	public bool IsDied { get; private set; } = false;

	//events
	public event EventHandler<EntityHitEventArgs> BeforeEntityHit;
	public event EventHandler<EntityHitEventArgs> EntityHit;
	public event EventHandler EntityDied;
	public event EventHandler<EntityHealedEventArgs> EntityHealed;
	public HitResponse Hit(HitInfo hitInfo)
	{
		EntityHitEventArgs _beh = new EntityHitEventArgs(hitInfo) { HealthBefore = healthPoints, HealthAfter = healthPoints - hitInfo.Damage };
		BeforeEntityHit?.Invoke(gameObject, _beh);

		if (_beh.IsCancelled) return _beh.OverrideResponse? _beh.OverridenResponse : HitResponse.Ignore;

		healthPoints -= hitInfo.Damage;
		EntityHit?.Invoke(gameObject, new EntityHitEventArgs(hitInfo) 
		{
			HealthBefore = healthPoints + hitInfo.Damage, HealthAfter = healthPoints
		});

		if(healthPoints <= 0 && !IsDied)
		{
			IsDied = true;
			EntityDied?.Invoke(gameObject, EventArgs.Empty);
		}
		return HitResponse.Hit;
	}
	public void Heal(float Points)
	{
		EntityHealed?.Invoke(gameObject, new EntityHealedEventArgs()
		{
			PointsHealed = Points,
			IsEntityHealed = healthPoints < maxHealthPoints,
			HealthBefore = healthPoints,
			HealthAfter = Mathf.Min(healthPoints + Points, maxHealthPoints)
		});

		if (healthPoints < maxHealthPoints)
			healthPoints += Points;

		if (healthPoints > maxHealthPoints)
			healthPoints = maxHealthPoints;
	}
	public void MaxHealthUpgrade(float Amount)
	{
		maxHealthPoints += Amount;
		Heal(Amount);
	}
}
