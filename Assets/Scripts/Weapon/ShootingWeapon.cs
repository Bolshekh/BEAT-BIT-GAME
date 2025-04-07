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


	[SerializeField] Cooldown delayBetweenShots = new Cooldown();
	ParticleSystem shootingParticles;
	// Start is called before the first frame update
	void Start()
	{
		shootingParticles = GetComponent<ParticleSystem>();
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
	}
	public void AddBulletSpeed(float Speed)
	{
		bulletSpeedMod.Add(Speed);
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
}
