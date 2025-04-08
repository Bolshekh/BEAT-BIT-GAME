using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInit : MonoBehaviour
{
	[SerializeField] Slider healthSlider;
	[SerializeField] float slowMotionOnHit;
	float beatTarget;
	float velocity;
	[SerializeField] float smoothing = 20f;
	[SerializeField] Animator sliderAnimator;
	float healthVel;
	float health;
	[SerializeField] GameObject deathScreen;
	// Start is called before the first frame update
	void Start()
	{
		var _health = GetComponent<HealthSystem>();
		var _partls = transform.GetChild(1).GetComponent<ParticleSystem>();
		healthSlider.maxValue = 3f;
		healthSlider.value = 3f;
		health = 3f;
		_health.BeforeEntityHit += (s, e) =>
		{
			if (e.HitInfo.Hitter.CompareTag("PlayerBullet"))
			{
				e.IsCancelled = true;
				e.OverrideResponse = true;
				e.OverridenResponse = HitResponse.Ignore | HitResponse.PassThrough;
			}
		};
		_health.EntityHit += (s, e) =>
		{
			healthSlider.value = e.HealthAfter;
			health = e.HealthAfter;
			TimeManager.Manager.SlowMotion((int)(slowMotionOnHit * 1000));
			_partls.Play();
		};
		_health.EntityHealed += (s, e) =>
		{
			health = e.HealthAfter;
			healthSlider.maxValue = _health.MaxHealthPoints;
		};
		_health.EntityDied += (s, e) =>
		{
			TimeManager.Manager.StopTimeSwitch(true, this);
			deathScreen.SetActive(true);
		};
		BeatManager.Manager.OnBeat += (s, e) =>
		{
			sliderAnimator.CrossFade("beat", 0.15f, 0);
		};
	}
	private void Update()
	{
		healthSlider.value = Mathf.SmoothDamp(healthSlider.value, health, ref healthVel, smoothing * Time.deltaTime);
	}
}
