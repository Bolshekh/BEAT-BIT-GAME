using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
	
	// Start is called before the first frame update
	void Start()
	{
		var health = GetComponent<HealthSystem>();

		health.BeforeEntityHit += (s, e) =>
		{
			if (e.HitInfo.Hitter.CompareTag("PlayerBullet"))
			{
				e.IsCancelled = true;
				e.OverrideResponse = true;
				e.OverridenResponse = HitResponse.Ignore | HitResponse.PassThrough;
			}
		};
		health.EntityHit += (s, e) =>
		{
			Debug.Log(e.HitInfo.Hitter.name);
		};
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
