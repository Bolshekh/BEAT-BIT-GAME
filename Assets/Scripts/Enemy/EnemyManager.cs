using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager Manager { get; private set; }
	[SerializeField] GameObject spawnAroundObject;
	[SerializeField] GameObject enemy;
	[SerializeField] int maxEnemies;
	[SerializeField] float spawnDiameter;
	List<GameObject> enemies = new List<GameObject>();
	[SerializeField] bool autoSpawn = false;
	// Start is called before the first frame update
	void Start()
	{
		Manager = this;
	}

	// Update is called once per frame
	void Update()
	{
		if(enemies.Count < maxEnemies && autoSpawn)
		{
			SpawnEnemy();
		}
	}
	public void SwitchAutoSpawn()
	{
		autoSpawn = !autoSpawn;
	}
	public void SpawnEnemy(int Amount)
	{
		for (int i = 0; i < Amount; i++)
		{
			SpawnEnemy();
		}
	}
	public void SpawnEnemy()
	{
		var _playerPos = spawnAroundObject.transform.position;
		float _x = Random.Range(0, spawnDiameter);
		int _coin = Random.Range(0, 2);
		float _y = Mathf.Pow((spawnDiameter*_x) - (_x*_x), 0.5f);
		_y *= _coin == 0 ? 1 : -1;

		Vector2 _point = new Vector2(_x-(spawnDiameter*0.5f) + _playerPos.x, _y + _playerPos.y);

		var _enemy = Instantiate(enemy, _point, Quaternion.identity);
		var _hs = _enemy.GetComponent<HealthSystem>();
		_hs.EntityDied += (s, e) =>
		{
			enemies.Remove(_enemy);
		};
		enemies.Add(_enemy);
		
	}
}
