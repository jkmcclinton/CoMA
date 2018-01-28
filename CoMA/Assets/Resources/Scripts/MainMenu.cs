
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public Button startButton;
	public Button creditButton;
	public Button quitButton;

	void Awake()
	{
		startButton.onClick.AddListener(() => OnClickStart());
		creditButton.onClick.AddListener(() => OnClickCredits());
		quitButton.onClick.AddListener (() => OnClickQuit ());
	}

	void OnClickStart()
	{
		SceneManager.LoadScene("Donut Sex Life");
	}
	void OnClickCredits()
	{
		SceneManager.LoadScene("CreditsMenu");
	}
	void OnClickQuit()
	{
		Application.Quit ();
	}
}