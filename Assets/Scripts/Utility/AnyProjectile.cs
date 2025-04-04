using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyProjectile : MonoBehaviour
{
	[SerializeField] float timeToLive = 5f;
	[SerializeField] float damage = 1f;
	[SerializeField] float knockback = 10f;
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
			_response = _hit.Hit(new HitInfo() 
			{
				Damage = this.damage,
				Hitter = this.gameObject,
				Knockback = (collision.transform.position - gameObject.transform.position) * knockback
			});
			//hits.Add(collision.gameObject);
		}
		if (!_response.HasFlag(HitResponse.PassThrough))
			Destroy(gameObject);
	}
}
