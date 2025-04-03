using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] float attackDistance;
	[SerializeField] float moveSpeed;
	public float AttackDistance => attackDistance;
	public bool IsInAttackDistance => Vector2.Distance(transform.position, target.transform.position) <= attackDistance;
	public bool IsEnemyDying { get; private set; } = false;

	GameObject target;
	Rigidbody2D enemyRB;

	// Start is called before the first frame update
	void Start()
	{
		target = PlayerManager.Player.gameObject;
		enemyRB = GetComponent<Rigidbody2D>();
	}
	/// <summary>
	/// It wont kill enemy, only disable some of it tracking AI
	/// </summary>
	public void SetDyingLogicToAi()
	{
		IsEnemyDying = true;
	}
	// Update is called once per frame
	void Update()
	{
		if (!IsInAttackDistance && !IsEnemyDying)
		{
			enemyRB.AddForce(moveSpeed * transform.up, ForceMode2D.Force);
		}
	}

	private void FixedUpdate()
	{
		if (target != null && !IsEnemyDying)
		{
			transform.rotation = Quaternion.FromToRotation(Vector2.up, target.transform.position - transform.position);
		}
	}
}
