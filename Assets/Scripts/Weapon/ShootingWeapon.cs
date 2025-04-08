using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
//upgrade scatter = very bad practice. need to change it next dev
public class ShootingWeapon : Weapon
{
	[SerializeField] GameObject bullet;
	[SerializeField] GameObject gunPoint;

	//bulletspeed
	[SerializeField] float bulletSpeed = 10f;

	List<float> bulletSpeedMod = new List<float>();
	float bulletSpeedTotal => bulletSpeedMod.Aggregate(0f, (total, next) => total += next) + bulletSpeed;
	float bulletSpeedBuffered;

	//firespeed
	[SerializeField] float baseFireSpeed = 1f;
	List<float> fireSpeedMod = new List<float>();
	float fireSpeedTotal => fireSpeedMod.Aggregate(0f, (total, next) => total += next) * baseFireSpeed;
	[SerializeField] AnimationCurve fireSpeedToDelayConverter;

	//damage and knockback
	[SerializeField] float damage = 1f;
	[SerializeField] float knockback = 10f;

	//passthrough
	//float passThrough = 0;

	[SerializeField] Cooldown delayBetweenShots = new Cooldown();
	ParticleSystem shootingParticles;
	// Start is called before the first frame update
	void Start()
	{
		shootingParticles = GetComponent<ParticleSystem>();
		bulletSpeedBuffered = bulletSpeedTotal;
		AddFireSpeed(1);
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
		fireSpeedMod.Add(Speed);
		UpdateStats();
	}
	public void AddBulletSpeed(float Speed)
	{
		bulletSpeedMod.Add(Speed);
		UpdateStats();
	}
	public void Upgrade(float? damage = null, float? knockback = null)
	{
		if (damage != null) this.damage += (float)damage;
		if (knockback != null) this.knockback += (float)knockback;
	}
	public void UpdateStats()
	{
		delayBetweenShots.CooldownTime = fireSpeedToDelayConverter.Evaluate(fireSpeedTotal);
		bulletSpeedBuffered = bulletSpeedTotal;
	}
	public async void Shoot()
	{
		if (delayBetweenShots.IsCoolingDown) return;

		var obj = Instantiate(bullet, gunPoint.transform.position, gunPoint.transform.rotation);
		obj.GetComponent<Rigidbody2D>()?.AddForce(bulletSpeedBuffered * (gunPoint.transform.root.up), ForceMode2D.Impulse);


		delayBetweenShots.StartCooldown();

		//visuals
		await Task.Delay((int)(delayBetweenShots.CooldownTime * 150));
		shootingParticles.Play();
	}
	public HitResponse Hit(IHitable target, Collider2D collision, Vector3 BulletPosition)
	{
		//if (Hits.Contains(collision.gameObject)) passthough upgrade??
		//{
		//	if(passThrough )
		//	return HitResponse.Ignore;
		//}


		//Hits.Add(collision.gameObject);
		
		return target.Hit(new HitInfo()
		{
			Damage = this.damage,
			Hitter = this.gameObject,
			Knockback = knockback * (collision.transform.position - BulletPosition)
		});
	}
}
