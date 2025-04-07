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
	[SerializeField] float smoothing;
	[SerializeField] Animator sliderAnimator;
	// Start is called before the first frame update
	void Start()
	{
		var _health = GetComponent<HealthSystem>();
		healthSlider.maxValue = 3f;
		healthSlider.value = 3f;
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
			TimeManager.Manager.SlowMotion((int)(slowMotionOnHit * 1000));
		};
		_health.EntityDied += (s, e) =>
		{
			//TODO: game end
		};
		BeatManager.Manager.OnBeat += (s, e) =>
		{
			//sliderAnimator.SetFloat("Beat", 1 / BeatManager.Manager.BeatDelay);
			Debug.Log(beatTarget);
			beatTarget = beatTarget < 1.3f ? 1.3f : 1f;
		};
	}
	private void Update()
	{
		float _smoothScale = Mathf.SmoothDamp(healthSlider.transform.localScale.x, beatTarget, ref velocity, smoothing * Time.deltaTime);

		healthSlider.gameObject.GetComponent<RectTransform>().localScale = new Vector3(_smoothScale, _smoothScale, _smoothScale);
	}
}
