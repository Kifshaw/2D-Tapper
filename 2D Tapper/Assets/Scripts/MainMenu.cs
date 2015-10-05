using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	private LevelNumber number;
	
	// Use this for initialization
	void Start () {
		number = GameObject.Find("Level Number").GetComponent<LevelNumber>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void LoadLevel(int level) {
		number.level = level;
		Application.LoadLevel(1);
	}
}
