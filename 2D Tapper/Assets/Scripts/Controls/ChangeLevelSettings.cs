using UnityEngine;
using System.Collections;

public class ChangeLevelSettings : MonoBehaviour {

	public EventControls level_controller;
	public EventControls.Difficulty level_difficulty;
	public bool disable;

	void Clicked () {
		if (!disable)
		{
			level_controller.difficulty = level_difficulty;
			level_controller.LoadGame();
		}
	}
}
