using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
	float expToLvlUp = 10f;
	[SerializeField] float expToLvlUpIncrease = 2f;
	float currentExp = 0f;
	[SerializeField] Slider expSlider;
	[SerializeField] float smoothing = 20f;
	float vel;

	//events
	public event EventHandler OnExpRecieve;
	public event EventHandler OnLvlUp;

	private void Start()
	{
		expSlider.maxValue = expToLvlUp;
		expSlider.value = 0;
	}
	public void GiveExp(float Exp)
	{
		currentExp += Exp;
		OnExpRecieve?.Invoke(this, EventArgs.Empty);
		if(currentExp >= expToLvlUp)
		{
			currentExp -= expToLvlUp;
			expToLvlUp += (expToLvlUpIncrease + DifficultyManager.Manager.EnemyAmountBuff);
			expSlider.maxValue = expToLvlUp;
			OnLvlUp?.Invoke(this, EventArgs.Empty);
			Upgrade();
		}
	}
	private void Update()
	{
		expSlider.value = Mathf.SmoothDamp(expSlider.value, currentExp, ref vel, smoothing * Time.deltaTime);
	}
	void Upgrade()
	{
		UpgradeSystem.Manager.UpgradePlayer();
	}
}
