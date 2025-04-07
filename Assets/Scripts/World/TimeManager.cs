using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	[SerializeField] float timeScale = 0.5f; 
	public static TimeManager Manager { get; private set; }
	CancellationTokenSource cts = new CancellationTokenSource();
	private void Start()
	{
		Manager = this;
		//SlowTime();
	}
	private void Awake()
	{
		SlowTime();
	}

	List<Task> slowMotions = new List<Task>();
	private const float normalTimeScale = 1f;
	private async Task SlowTime(int millisecons, CancellationToken token)
	{
		token.ThrowIfCancellationRequested();

		
		await Task.Delay(millisecons);
	}

	private async void SlowTime()
	{
		while (true)
		{
			if (slowMotions.Count > 0)
			{
				try
				{
					Time.timeScale = timeScale;

					await Task.WhenAll(slowMotions);

					slowMotions.Clear();
					Time.timeScale = normalTimeScale;
				}
				catch (OperationCanceledException)
				{
					Debug.Log("slow motion tasks cancelled");
				}
			}
			await Task.Delay(100);
		}
	}
	public void SlowMotion(int milliseconds)
	{
		slowMotions.Add(SlowTime(milliseconds, cts.Token));
	}
	public void StopTimeSwich(bool IsStopped)
	{
		Time.timeScale = IsStopped ? 0.01f : normalTimeScale;
	}
}
