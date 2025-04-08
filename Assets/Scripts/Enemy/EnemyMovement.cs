using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] float attackDistance;
	[SerializeField] float moveSpeed;
	public float AttackDistance => attackDistance;
	public bool IsInAttackDistance => Vector2.Distance(transform.position, target.transform.position) <= attackDistance;

	bool PreviousAttackDistanceState = false;
	
	public event EventHandler OnAttackDistanceEntered;
	public event EventHandler OnAttackDistanceExited;
	public bool IsEnemyDying { get; private set; } = false;

	GameObject target;
	Rigidbody2D enemyRB;

	// Start is called before the first frame update
	void Start()
	{
		target = PlayerManager.Player.gameObject;
		enemyRB = GetComponent<Rigidbody2D>();

		AttackDistanceSwitch(IsInAttackDistance);
	}
	public void SetUpgrades(float AttackDistance, float MoveSpeed)
	{
		attackDistance = AttackDistance;
		moveSpeed = MoveSpeed;
	}
	/// <summary>
	/// It wont kill enemy, only disable some of it tracking AI
	/// </summary>
	public void SetDyingLogicToAi()
	{
		IsEnemyDying = true;
	}
	void AttackDistanceSwitch(bool IsInAttackDistance)
	{
		if (IsEnemyDying) return;
		if (PreviousAttackDistanceState == IsInAttackDistance) return;

		PreviousAttackDistanceState = IsInAttackDistance;

		if (IsInAttackDistance)
			EnterAttackDistance();
		else
			ExitAttackDistance();
	}
	void EnterAttackDistance()
	{
		OnAttackDistanceEntered?.Invoke(this, EventArgs.Empty);
	}
	void ExitAttackDistance()
	{
		OnAttackDistanceExited?.Invoke(this, EventArgs.Empty);
	}
	// Update is called once per frame
	void Update()
	{
		if (!IsInAttackDistance && !IsEnemyDying)
		{
			enemyRB.AddForce(moveSpeed * Time.timeScale * transform.up, ForceMode2D.Force);
		}
		AttackDistanceSwitch(IsInAttackDistance);
	}

	private void FixedUpdate()
	{
		if (target != null && !IsEnemyDying)
		{
			transform.rotation = Quaternion.FromToRotation(Vector2.up, target.transform.position - transform.position);
		}
	}
}
