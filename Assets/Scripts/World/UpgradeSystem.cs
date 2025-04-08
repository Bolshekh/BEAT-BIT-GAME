using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
	public static UpgradeSystem Manager { get;private set; }
	[SerializeField] GameObject upgrades;
	[SerializeField] GameObject upgradeCardPrefab;
	private void Start()
	{
		Manager = this;
	}
	public void UpgradePlayer()
	{
		TimeManager.Manager.StopTimeSwich(true);
		List<Upgrades> _upgrades = Generate(((Upgrades[])Enum.GetValues(typeof(Upgrades))).ToList());
		foreach (Upgrades upgrade in _upgrades)
		{
			var _card = Instantiate(upgradeCardPrefab, upgrades.transform);
			_card.GetComponent<Upgrade>().SetUpUpgrade(upgrade, GetUpgradeAmount(upgrade));
		}
	}
	public void UpgradeEnemy()
	{
		TimeManager.Manager.StopTimeSwich(true);
		List<Downgrades> _upgrades = Generate(((Downgrades[])Enum.GetValues(typeof(Downgrades))).ToList());
		foreach (Downgrades downgrade in _upgrades)
		{
			var _card = Instantiate(upgradeCardPrefab, upgrades.transform);
			_card.GetComponent<Upgrade>().SetUpDowngrade(downgrade, GetDowngradeAmount(downgrade));
		}
	}
	float GetUpgradeAmount(Upgrades upgrade) => upgrade switch
	{
		Upgrades.Damage => 1,
		Upgrades.FireSpeed => 1.1f,
		Upgrades.BulletSpeed => 2,
		Upgrades.Health => 1,
		Upgrades.Difficulty => -1,
		Upgrades.Beat_Delay => 0.1f,
		Upgrades.Beat_Max => 1,
		Upgrades.Enemy_Exp_Drop => 0.3f,
		Upgrades.Knockback => 0.5f,
		_ => 0
	};
	float GetDowngradeAmount(Downgrades downgrade) => downgrade switch
	{
		Downgrades.Enemy_Damage => 1,
		Downgrades.Enemy_Attack_Distance => 0.2f,
		Downgrades.Enemy_Attack_Force => 0.2f,
		Downgrades.Enemy_Health => 1,
		Downgrades.Enemy_Knockback => 2,
		Downgrades.Enemy_Movespeed => 0.3f,
		Downgrades.Difficulty => 1,
		Downgrades.Beat_Delay => -0.1f,
		Downgrades.Beat_Max => -1f,
		_ => 0
	};
	List<T> Generate<T>(List<T> objects)
	{
		return objects.OrderBy(o=> Guid.NewGuid()).Take(3).ToList();
	}
	public void ClearCards()
	{
		foreach (Transform child in upgrades.transform)
		{
			GameObject.Destroy(child.gameObject);
		}
		TimeManager.Manager.StopTimeSwich(false);
		BeatManager.Manager.RestartTheBeat();
	}
}
