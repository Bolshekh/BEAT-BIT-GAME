using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager Manager { get; private set; }
	[SerializeField] GameObject spawnAroundObject;
	[SerializeField] GameObject enemy;
	[SerializeField] int maxEnemies;
	[SerializeField] float spawnDiameter;
	int enemies = 0;
	[SerializeField] bool autoSpawn = false;

	//health upgrade
	[SerializeField] float baseHealth = 3f;
	List<float> healthModifiers = new List<float>();
	float healthTotal => healthModifiers.Aggregate(0f, (total, next) => total += next) + baseHealth;

	//force upgrade
	[SerializeField] float attackForce = 4f;
	List<float> attackForceMod = new List<float>();
	float attackForceTotal => attackForceMod.Aggregate(0f, (total, next) => total += next) * attackForce;

	//damage upgrade
	[SerializeField] float damage = 1f;
	List<float> damageMod = new List<float>();
	float damageTotal => damageMod.Aggregate(0f, (total, next) => total += next) + damage;

	//knockback upgrade
	[SerializeField] float knockback = 5f;
	List<float> knockbackMod = new List<float>();
	float knockbackTotal => knockbackMod.Aggregate(0f, (total, next) => total += next) + knockback;

	//attack distance
	[SerializeField] float baseAttackDistance = 3f;
	List<float> attackDistanceMod = new List<float>();
	float AttackDistanceTotal => attackDistanceMod.Aggregate(0f, (total, next) => total += next) * baseAttackDistance;
	
	//movespeed
	[SerializeField] float baseMovespeed = 2f;
	List<float> movespeedMod = new List<float>();
	float MovespeedTotal => movespeedMod.Aggregate(0f, (total, next) => total += next) * baseExpDrop;

	//exp
	[SerializeField] float baseExpDrop = 1f;
	List<float> expMod = new List<float>();
	float ExpTotal => expMod.Aggregate(0f, (total, next) => total += next) * baseExpDrop;

	// Start is called before the first frame update
	void Start()
	{
		Manager = this;
		BeatManager.Manager.OnBeat += (s, e) =>
		{
			if (enemies < maxEnemies && autoSpawn)
			{
				SpawnEnemy(maxEnemies - enemies);
			}
		};
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
		float _y = Mathf.Pow((spawnDiameter * _x) - (_x * _x), 0.5f);
		_y *= _coin == 0 ? 1 : -1;

		Vector2 _point = new Vector2(_x - (spawnDiameter * 0.5f) + _playerPos.x, _y + _playerPos.y);

		var _enemy = Instantiate(enemy, _point, Quaternion.identity);

		var _en = _enemy.GetComponent<Spawner>().Enemy;

		var _enHealth = _en.GetComponent<HealthSystem>();
		var _enAttacks = _en.GetComponent<EnemyAttacks>();
		var _enMovement = _en.GetComponent<EnemyMovement>();

		_enHealth.EntityDied += (s, e) =>
		{
			PlayerManager.Manager.PlayerExperience.GiveExp(ExpTotal);
		};
		_enHealth.MaxHealthUpgrade(healthTotal);
		_enAttacks.SetUpgrades(attackForceTotal, damageTotal, knockbackTotal);
		_enMovement.SetUpgrades(AttackDistanceTotal, MovespeedTotal);

		//enemies.Add(_enemy);

	}
	public void AddEnemy()
	{
		enemies++;
	}
	public void RemoveEnemy()
	{
		
		enemies--;
		if (enemies < 0) enemies = 0;
	}
}
