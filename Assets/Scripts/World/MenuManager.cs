using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public static MenuManager Manager { private set; get; }
	[SerializeField] GameObject menu;
	bool isPaused = false;
	private void Start()
	{
		Manager = this;
	}
	public void SwitchPause()
	{
		if (isPaused) Resume();
		else Pause();
	}
	public void Pause()
	{
		isPaused = true;
		menu.SetActive(true);
		TimeManager.Manager.StopTimeSwitch(true, this);
	}
	public void Resume()
	{
		TimeManager.Manager.StopTimeSwitch(false, this);
		isPaused = false;
		menu.SetActive(false);
	}
	public void MainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
	public void LoadGame()
	{
		SceneManager.LoadScene("MainScene");
	}
	public void Exit()
	{
		Application.Quit();
	}
}
