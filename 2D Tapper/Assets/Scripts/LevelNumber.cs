using UnityEngine;
using System.Collections;

public class LevelNumber : MonoBehaviour {
	
	public int level;

	// Use this for initialization
	void Start () {
		level = 0;
		DontDestroyOnLoad(gameObject);
	}
}
