using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyProjectile : MonoBehaviour
{
	[SerializeField] float timeToLive = 5f;
	//List<GameObject> hits = new List<GameObject>();
	void Start()
	{
		timeToLive += Time.time;
	}
	void Update()
	{
		if(Time.time > timeToLive)
		{
			Destroy(gameObject);
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		HitResponse _response = HitResponse.Ignore;
		var _hit = collision.GetComponent<IHitable>();
		if (_hit != null/* && !hits.Contains(collision.gameObject)*/)
		{
			_response = PlayerManager.Manager.PlayerWeapon.Hit(_hit, collision, transform.position);
		}
		if (!_response.HasFlag(HitResponse.Ignore))
			Destroy(gameObject);
	}
}
