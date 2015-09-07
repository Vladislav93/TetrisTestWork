using UnityEngine;
using System.Collections;

public class RestartLevel: Pause {
	public GameObject _background;
	public GameObject _label;
	public GameObject _deRestart;

	private void pauseForRestart()
	{
		Time.timeScale = 0;
		_gameObj = GameObject.FindGameObjectWithTag ("GameController");	
		_script = _gameObj.GetComponent<gameTetrisLogic> ();
		_script.enabled = false;
	}

	public void Restart ()
	{
		Application.LoadLevel (0);
		Time.timeScale = 1;
	}
	
	void OnClick()
	{
		pauseForRestart ();
		_background.SetActive (true);
		_label .SetActive (true);
		_deRestart.SetActive (true);
	
	}
}
