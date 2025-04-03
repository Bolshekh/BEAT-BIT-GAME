using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoots : MonoBehaviour
{
	ShootingWeapon playerWeapon;
	// Start is called before the first frame update
	void Start()
	{
		playerWeapon = GetComponentInChildren<ShootingWeapon>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
			playerWeapon.Shoot();

	}
	
}
