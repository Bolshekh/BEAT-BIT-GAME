using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	Rigidbody2D playerRB;
	Vector2 moveDirection;
	Vector2 lookDirection;
	Animator animator;
	//PlayerAttacks attacks;

	//basic movement
	[SerializeField] float moveSpeed;
	[SerializeField] float speedMultiplier;
	//[SerializeField] float moveDumping = 0.8f;
	//[SerializeField] float moveFree = 5f;
	[SerializeField] AnimationCurve forceMultiplier;
	float GetForceMultiplierValue(float value) => forceMultiplier.Evaluate(value);
	void Start()
	{
		playerRB = GetComponent<Rigidbody2D>();
		//attacks = GetComponent<PlayerAttacks>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		CheckMovement();
	}
	void FixedUpdate()
	{
		ApplyMovement();
		ApplyRotation();
	}

	protected virtual void CheckMovement()
	{
		var _hor = Input.GetAxis("Horizontal");
		var _vert = Input.GetAxis("Vertical");

		moveDirection = new Vector2(_hor, _vert).normalized;

		var _mouse = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

		lookDirection = _mouse - (Vector2)transform.position;

		//if (Input.GetButtonDown("Fire1"))
		//	attacks.Attack();

		//if (Input.GetButtonUp("Fire2"))
		//	attacks.Defend();
	}

	protected virtual void ApplyMovement()
	{
		if (moveDirection.magnitude != 0)
		{
			playerRB.AddForce(moveSpeed * speedMultiplier * GetForceMultiplierValue(playerRB.velocity.magnitude) * moveDirection, ForceMode2D.Force);
		}
	}
	protected virtual void ApplyRotation()
	{
		transform.rotation = Quaternion.FromToRotation(Vector2.up, lookDirection);
	}
}
