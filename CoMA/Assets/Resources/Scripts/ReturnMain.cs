
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnMain : MonoBehaviour {
	
	public Button mainButton;

	void Awake()
	{
		mainButton.onClick.AddListener(() => OnClickMain());
	}

	void OnClickMain()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
