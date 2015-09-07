using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UILabel scoreLabel = this.GetComponent<UILabel> ();
		gameTetrisLogic scoreValue = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameTetrisLogic>();
		scoreLabel.text = scoreValue._score.ToString ();
	}
}
