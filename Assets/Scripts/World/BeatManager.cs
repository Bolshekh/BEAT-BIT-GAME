using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BeatManager : MonoBehaviour
{
	public static BeatManager Manager { get; private set; }

	//beat
	[SerializeField] Slider beatSlider;
	[SerializeField] int beatMax = 8;
	[SerializeField] float beatDelay = 0.65f;
	int beat = 0;

	//beat mods
	List<float> beatDelayMod = new List<float>();
	float beatDelayTotal => beatDelayMod.Aggregate(0f, (total, next) => total += next) * beatDelay;
	[SerializeField] float beatDelayBuffered;
	public float BeatDelay => beatDelayBuffered;
	//smooth
	float beatVelocity = 0;
	[SerializeField] float smoothTime = 100;

	public event EventHandler OnBeat;
	public event EventHandler OnBeatMax;

	CancellationTokenSource cts = new CancellationTokenSource();

	void Start()
	{
		Manager = this;
		beatSlider.maxValue = beatMax;
		beatSlider.value = beat;
		Upgrade(1);
		OnBeat += (s, e) =>
		{
			beat++;
			if (beat > beatMax)
			{
				OnBeatMax?.Invoke(this, EventArgs.Empty);
				beat = 0;
			}

			beatSlider.maxValue = beatMax;
		};
		StartTheBeat(cts.Token);
	}
	private void Update()
	{
		beatSlider.value = Mathf.SmoothDamp(beatSlider.value, beat, ref beatVelocity, smoothTime * Time.deltaTime);
	}
	async void StartTheBeat(CancellationToken token)
	{
		token.ThrowIfCancellationRequested();
		while (true)
		{
			try
			{
				await Task.Delay((int)(beatDelayBuffered * 1000));

				token.ThrowIfCancellationRequested();
				if (token.IsCancellationRequested) return;

				OnBeat?.Invoke(this, EventArgs.Empty);
			}
			catch (OperationCanceledException)
			{
				Debug.Log("beat task cancelled");
			}
		}
	}
	public void Upgrade(float Delay)
	{
		beatDelayMod.Add(Delay);
		beatDelayBuffered = beatDelayTotal;
	}
}
