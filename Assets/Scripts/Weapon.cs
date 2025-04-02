using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
	[SerializeField] float weaponDamage;
	public float Damage => weaponDamage;

	[SerializeField] float weaponKnockback;
	public float Knockback => weaponKnockback;

	public event EventHandler<WeaponHitEventArgs> WeaponHit;

	protected List<GameObject> Hits = new List<GameObject>();
	void Start()
	{
		WeaponHit += (s, e) =>
		{
			e.Hit.Hit(new HitInfo() { Damage = this.Damage, Hitter = gameObject });
		};
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var _hit = collision.GetComponent<IHitable>();
		if (_hit != null)
		{
			WeaponHit?.Invoke(this, new WeaponHitEventArgs() { Hit = _hit, Collision = collision });
		}
	}
	public void ClearHitList()
	{
		Hits.Clear();
	}
}