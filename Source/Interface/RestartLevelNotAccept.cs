using UnityEngine;
using System.Collections;

public class RestartLevelNotAccept : RestartLevel {

	void OnClick()
	{
		_gameObj = GameObject.FindGameObjectWithTag ("GameController");
		_script = _gameObj.GetComponent<gameTetrisLogic>();

		_background.SetActive (false);
		_label .SetActive (false);
		Time.timeScale = 1;

		_deRestart.SetActive (false);
		_script.enabled = true;
	}
}

