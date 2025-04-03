using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInit : MonoBehaviour
{
	[SerializeField] ParticleSystem particles;
	[SerializeField] float DyingTime = 0.3f;
	// Start is called before the first frame update
	void Start()
	{
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		HealthSystem _healthSystem = GetComponent<HealthSystem>();
		_healthSystem.BeforeEntityHit += (s, e) =>
		{
			if (e.HitInfo.Hitter.transform.root.CompareTag("Enemy"))
				e.IsCancelled = true;
		};
		_healthSystem.EntityHit += (s, e) =>
		{
			rb.AddForce(e.HitInfo.Knockback, ForceMode2D.Impulse);
			UseHitParticles();
		};
		_healthSystem.EntityDied += (s, e) =>
		{
			EnemyMovement _enemyMovement = GetComponent<EnemyMovement>();
			_enemyMovement.SetDyingLogicToAi();
			Destroy(gameObject, DyingTime);
		};
	}

	void UseHitParticles()
	{
		//particles?.Play();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		var _hit = collision.GetComponent<IHitable>();

		if (_hit != null)
		{
			_hit.Hit(new HitInfo()
			{
				Damage = 1,
				Hitter = this.gameObject,
				Knockback = (collision.transform.position - gameObject.transform.position) * 10
			});
		}
	}
}
