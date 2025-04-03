using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootingWeapon : Weapon
{
	[SerializeField] GameObject bullet;
	[SerializeField] GameObject gunPoint;
	[SerializeField] float bulletSpeed = 10f;
	[SerializeField] float baseFireSpeed = 1f;
	List<float> multiplyerFireSpeed = new List<float>();
	float multiplyerFireSpeedValue => multiplyerFireSpeed.Aggregate(0f, (total, next) => total += next);
	float currentFireSpeed => baseFireSpeed * multiplyerFireSpeedValue;
	[SerializeField] AnimationCurve fireSpeedToDelayConverter;

	[SerializeField] Cooldown delayBetweenShots = new Cooldown();
	// Start is called before the first frame update
	void Start()
	{
		WeaponHit += (s, e) =>
		{
			e.Hit.Hit(new HitInfo() 
			{
				Damage = this.Damage,
				Hitter = gameObject,
				Knockback = Knockback * (e.Collision.transform.position - transform.position)
			});
		};
	}

	public void AddFireSpeed(float Speed)
	{
		multiplyerFireSpeed.Add(Speed);
	}
	public void UpdateStats()
	{
		delayBetweenShots.CooldownTime = fireSpeedToDelayConverter.Evaluate(currentFireSpeed);
	}
	public void Shoot()
	{
		if (delayBetweenShots.IsCoolingDown) return;

		var obj = Instantiate(bullet, gunPoint.transform.position, gunPoint.transform.rotation);

		obj.GetComponent<Rigidbody2D>()?.AddForce(bulletSpeed * (gunPoint.transform.root.up), ForceMode2D.Impulse);

		delayBetweenShots.StartCooldown();
	}
}
