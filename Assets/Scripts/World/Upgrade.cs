using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
	[SerializeField] protected TMPro.TMP_Text cardText;

	protected float amount;
	protected bool IsUpgradeChosen = false;

	Upgrades upgrade;

	Downgrades downgrades;

	Grades grades;
	public void SetUpUpgrade(Upgrades Upgrade, float Amount)
	{
		cardText.text = Upgrade.ToString().Replace('_', ' ') + "\n" + (Amount > 0 ? "+" : "") + Amount.ToString();
		this.amount = Amount;
		this.upgrade = Upgrade;
		IsUpgradeChosen = false;
		grades = Grades.Upgrade;
	}
	public void SetUpDowngrade(Downgrades downGrade, float Amount)
	{
		cardText.text = downGrade.ToString().Replace('_', ' ') + "\n" + (Amount > 0 ? "+" : "") + Amount.ToString();
		this.amount = Amount;
		this.downgrades = downGrade;
		IsUpgradeChosen = false;
		grades = Grades.Downgrade;
	}
	public void UpdateParams()
	{
		if (IsUpgradeChosen) return;
		IsUpgradeChosen =true;
		if(grades == Grades.Upgrade)
			switch (upgrade)
			{
				case Upgrades.Beat_Delay:
					BeatManager.Manager.Upgrade(amount);
					break;
				case Upgrades.Enemy_Exp_Drop:
					EnemyManager.Manager.Upgrade(ExpDrop: amount);
					break;
				case Upgrades.Beat_Max:
					BeatManager.Manager.UpgradeBeatMax((int)amount);
					break;
				case Upgrades.Damage:
					PlayerManager.Manager.PlayerWeapon.Upgrade(damage: amount);
					break;
				case Upgrades.FireSpeed:
					PlayerManager.Manager.PlayerWeapon.AddFireSpeed(amount);
					break;
				case Upgrades.BulletSpeed:
					PlayerManager.Manager.PlayerWeapon.AddBulletSpeed(amount);
					break;
				case Upgrades.Difficulty:
					DifficultyManager.Manager.Upgrade((int)amount);
					break;
				case Upgrades.Knockback:
					PlayerManager.Manager.PlayerWeapon.Upgrade(knockback: amount);
					break;
				case Upgrades.Health:
					PlayerManager.Manager.PlayerHealth.MaxHealthUpgrade(amount);
					break;
				default: break;
			}

		if(grades == Grades.Downgrade)
			switch (downgrades)
			{
				case Downgrades.Enemy_Attack_Distance:
					EnemyManager.Manager.Upgrade(AttackDistance: amount);
					break;
				case Downgrades.Enemy_Knockback:
					EnemyManager.Manager.Upgrade(Knockback: amount);
					break;
				case Downgrades.Enemy_Damage:
					EnemyManager.Manager.Upgrade(Damage: amount);
					break;
				case Downgrades.Enemy_Attack_Force:
					EnemyManager.Manager.Upgrade(AttackForce: amount);
					break;
				case Downgrades.Enemy_Health:
					EnemyManager.Manager.Upgrade(Health: amount);
					break;
				case Downgrades.Enemy_Movespeed:
					EnemyManager.Manager.Upgrade(Movespeed: amount);
					break;
				case Downgrades.Beat_Max:
					BeatManager.Manager.UpgradeBeatMax((int)amount);
					break;
				case Downgrades.Beat_Delay:
					BeatManager.Manager.Upgrade(amount);
					break;
				case Downgrades.Difficulty:
					DifficultyManager.Manager.Upgrade((int)amount);
					break;
				default: break;
			}

		UpgradeSystem.Manager.ClearCards();
	}
}
