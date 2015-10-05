using UnityEngine;
using System.Collections;

public class LeaderboardIndv : MonoBehaviour {

	[System.Serializable]
	public struct Entry {
		public TextMesh time;
		public TextMesh name;
	}

	public TextMesh board_name;
	public Entry[] easy;
	public Entry[] medium;
	public Entry[] hard;
	
	public string boardname;
	public Board[] entries;
	
	
	void Update () {
		UpdateText();
	}
	
	void UpdateText() {
		board_name.text = boardname;
		
		for (int i = 0; i < 5; i++)
		{
			easy[i].time.text = entries[0].time[i].ToString("0.00000");
			easy[i].name.text = entries[0].name[i];
			medium[i].time.text = entries[1].time[i].ToString("0.00000");
			medium[i].name.text = entries[1].name[i];
			hard[i].time.text = entries[2].time[i].ToString("0.00000");
			hard[i].name.text = entries[2].name[i];
		}
	}
}
