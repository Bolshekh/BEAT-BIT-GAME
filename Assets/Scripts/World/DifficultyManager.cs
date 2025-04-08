using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
	public static DifficultyManager Manager { get; private set; }
	[SerializeField] Slider difficultySlider;
	[SerializeField] int maxDifficulty;
	public int CurrentDiffculty { get; private set; }
	[SerializeField] int difficultyToUpgrade;
	[SerializeField] int difficultyStep = 10;
	List<int> diffStepMod = new List<int>();
	int diffStepTotal => difficultyStep - diffStepMod.Aggregate(0, (total, next) => total += next);
	int diffStepBuffered;
	public int EnemyAmountBuff { get; private set; } = 0;
	void Start()
	{
		Manager = this;
		difficultySlider.maxValue = maxDifficulty;
		difficultySlider.value = 0;

		diffStepBuffered = diffStepTotal;
		if (diffStepBuffered < 1)
			diffStepBuffered = 1;

		BeatManager.Manager.OnBeatMax += (s, e) =>
		{
			CurrentDiffculty++;
			difficultySlider.value = CurrentDiffculty;
			if(CurrentDiffculty >= maxDifficulty)
			{
				//TODO: WIN!!
			}

			if (CurrentDiffculty >= difficultyToUpgrade)
			{
				difficultyToUpgrade += diffStepBuffered;

				EnemyAmountBuff++;
				EnemyManager.Manager.Upgrade(Health: 0.25f, EnemyAmount: 1);

				Upgrade();
			}
		};
	}
	public void Upgrade(int Amount)
	{
		diffStepMod.Add(Amount);

		diffStepBuffered = diffStepTotal;
		if (diffStepBuffered < 1)
			diffStepBuffered = 1;
	}
	void Upgrade()
	{
		UpgradeSystem.Manager.UpgradeEnemy();
	}
}
