using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class EnemyInit : MonoBehaviour
{
	[SerializeField] ParticleSystem particles;
	[SerializeField] float dyingTime = 0.3f;
	CancellationTokenSource cts = new CancellationTokenSource();
	// Start is called before the first frame update
	void Start()
	{
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		HealthSystem _healthSystem = GetComponent<HealthSystem>();
		EnemyMovement _enemyMovement = GetComponent<EnemyMovement>();
		EnemyAttacks _enemyAttacks = GetComponent<EnemyAttacks>();
		Animator _animator = GetComponent<Animator>();

		var _length = _animator.runtimeAnimatorController.animationClips.Where(c => c.name == "Attack").First().length;

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
			Destroy(gameObject, dyingTime);
		};

		_enemyMovement.OnAttackDistanceEntered += (s, e) =>
		{
			if (cts.Token.IsCancellationRequested) cts = new CancellationTokenSource();

			if (!_healthSystem.IsDied)
				_enemyAttacks.StartAttack(_length, cts.Token);

			//or

			//_animator.CrossFade("Attack", 0);
		};
	}

	void UseHitParticles()
	{
		//particles?.Play();
	}
}
