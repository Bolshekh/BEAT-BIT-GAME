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
	HashSet<object> stoppers = new HashSet<object>();
	int stoppersPrev = 0;
	List<Task> slowMotions = new List<Task>();
	private const float normalTimeScale = 1f;
	private void Start()
	{
		Manager = this;
		Time.timeScale = normalTimeScale;
		//SlowTime();
	}
	private void Awake()
	{
		SlowTime();
	}
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
					UpdateTimeScale();
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
	public void StopTimeSwitch(bool IsStopped, object sender)
	{
		if(IsStopped) stoppers.Add(sender);
		else stoppers.Remove(sender);
		Debug.Log(IsStopped.ToString() + " " + stoppers.Count);
		UpdateTimeScale();
	}
	void UpdateTimeScale()
	{
		Time.timeScale = stoppers.Count > 0 ? 0.01f : normalTimeScale;
		if (stoppers.Count != stoppersPrev) BeatManager.Manager.RestartTheBeat();
		stoppersPrev = stoppers.Count;
	}
}
