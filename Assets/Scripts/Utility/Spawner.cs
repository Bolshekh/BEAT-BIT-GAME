using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] GameObject objectToSpawn;
	[SerializeField] float delaySeconds;
	CancellationTokenSource cts = new CancellationTokenSource();
	public GameObject Enemy => objectToSpawn;
	public event EventHandler<GameObjectEventArgs> OnEntitySpawn;
	// Start is called before the first frame update
	void Start()
	{
		PrepareSpawn();
		int _beats = 3;
		BeatManager.Manager.OnBeat += (s, e) =>
		{
			_beats--;
			if (_beats == 0)
				Spawn();
		};
	}

	async void Spawn(float DelaySeconds, CancellationToken token)
	{
		try
		{
			PrepareSpawn();

			await Task.Delay((int)(DelaySeconds * 1000));

			token.ThrowIfCancellationRequested();

			Spawn();

		}
		catch (OperationCanceledException)
		{
			Debug.Log("spawner task cancelled");
		}

	}
	void PrepareSpawn()
	{
		EnemyManager.Manager.AddEnemy();
	}
	void Spawn()
	{
		var _enemy = Instantiate(objectToSpawn, transform.position, transform.rotation);

		OnEntitySpawn?.Invoke(this, new GameObjectEventArgs() { Entity = _enemy });

		Destroy(gameObject);
	}
}
