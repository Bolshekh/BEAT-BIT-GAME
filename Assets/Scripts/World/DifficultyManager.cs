using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
	[SerializeField] Slider difficultySlider;
	[SerializeField] int maxDifficulty;
	public int CurrentDiffculty { get; private set; }
	void Start()
	{
		difficultySlider.maxValue = maxDifficulty;
		BeatManager.Manager.OnBeatMax += (s, e) =>
		{
			CurrentDiffculty++;
		};
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
