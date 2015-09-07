using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class BestScoreSave : MonoBehaviour {

	public string _textScore;
	public gameTetrisLogic _scoreSaveAndRead;
	public GameObject _warningBackground;
	public GameObject _warningLabel;

	public void SaveBestScore()
	{
		_scoreSaveAndRead = this.GetComponent<gameTetrisLogic> ();
		if(!Directory.Exists("Settings"))
			Directory.CreateDirectory("Settings");
		
		File.WriteAllLines("Settings/settings.ini", new[] {_scoreSaveAndRead._score.ToString()});
	}

	public int ReadBestScore()
	{
		_scoreSaveAndRead = this.GetComponent<gameTetrisLogic> ();
		if((Directory.Exists("Settings")) && (File.Exists("Settings/settings.ini")))
		   {
			_textScore = File.ReadAllText("Settings/settings.ini");
		}

		return Convert.ToInt32(_textScore);

	}

	public void WarningWindowsExit(bool flag)
	{
		_warningBackground.SetActive (true);
		_scoreSaveAndRead = this.GetComponent<gameTetrisLogic> ();
		if (flag)
		_warningLabel.GetComponent<UILabel> ().text = "YOU LOSE \n Your Score:" + _scoreSaveAndRead._score.ToString() + "\n Your Best Score:" + ReadBestScore().ToString();
		else
		_warningLabel.GetComponent<UILabel> ().text = "YOU LOSE \n NEW RECORD! \n Your Score:" + _scoreSaveAndRead._score.ToString();
		_warningLabel.SetActive (true);
	}
}
