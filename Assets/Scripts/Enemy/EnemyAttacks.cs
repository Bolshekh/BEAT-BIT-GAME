using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
	/// <summary>
	/// Object with trigger that will hit things
	/// </summary>
	[SerializeField] GameObject AttackTriggerObject;
	[SerializeField] Rigidbody2D enemyRigidbody;
	[SerializeField] float attackForce = 4f;
	[SerializeField] float damage = 1f;
	[SerializeField] float knockback = 5f;


	HashSet<GameObject> hits = new HashSet<GameObject>();
	//public bool IsAttacking { get; protected set; }
	private void Start()
	{
		AttackTriggerObject.SetActive(false);
	}

	public async void StartAttack(float StopDelay)
	{
		enemyRigidbody.AddForce(attackForce * transform.up, ForceMode2D.Impulse);
		AttackTriggerObject.SetActive(true);

		await Task.Delay((int)(StopDelay * 1000));

		StopAttack();
		//IsAttacking = true;
	}
	public void StopAttack()
	{
		AttackTriggerObject.SetActive(false);
		hits.Clear();
		//IsAttacking = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var _hit = collision.GetComponent<IHitable>();
		if (_hit != null && !hits.Contains(collision.gameObject))
		{
			_hit.Hit(new HitInfo()
			{
				Damage = damage,
				Hitter = this.gameObject,
				Knockback = knockback * (collision.transform.position - gameObject.transform.position)
			});
			hits.Add(collision.gameObject);
		}
	}
	public void SetUpgrades(float AttackForce, float Damage, float Knockback)
	{
		attackForce = AttackForce;
		damage = Damage;
		knockback = Knockback;
	}
}
