using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {
	public bool _pauseFlag = true;
	public GameObject _gameObj;
	public gameTetrisLogic _script;

	public void pause()
	{
		_gameObj = GameObject.FindGameObjectWithTag ("GameController");
		_script = _gameObj.GetComponent<gameTetrisLogic>();
		_pauseFlag = !_pauseFlag;
		if (!_pauseFlag) {
			Time.timeScale = 0;
			_script.enabled = false;
		} else {
			Time.timeScale = 1;
			_script.enabled = true;
		}
	}
	void OnClick()
	{
		pause (); 
	}
}
