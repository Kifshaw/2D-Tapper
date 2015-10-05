using UnityEngine;
using System.Collections;

public class LeaderboardControl : MonoBehaviour {

	public int number_of_leaderboards;
	private Leaderboard[] leaderboards;
	public GameObject sample_board;
	
	public GameObject[] leaderboard_positions; //Should be length of 'number_of_leaderboards'
	private LeaderboardIndv[] individual_boards;
	
	void Start () {
	
		leaderboards = new Leaderboard[number_of_leaderboards];
		individual_boards = new LeaderboardIndv[number_of_leaderboards];
		GameObject temp;
		for (int i = 0; i < number_of_leaderboards; i++)
		{
			leaderboards[i] = new Leaderboard();
			leaderboards[i].load_data(i+1);
			
			temp = (GameObject)Instantiate(sample_board, leaderboard_positions[i].transform.position, leaderboard_positions[i].transform.rotation);
			individual_boards[i] = temp.GetComponent<LeaderboardIndv>();
			individual_boards[i].boardname = leaderboards[i].get_name();
			individual_boards[i].entries = leaderboards[i].get_all_boards();
		}
	}
	
	void SetText() {
		for (int i = 0; i < number_of_leaderboards; i++)
		{
			individual_boards[i].entries = leaderboards[i].get_all_boards();
		}
	}
	
	public bool CheckNewTime(int game_type, int difficulty, float time) {
		return leaderboards[game_type-1].check_time(difficulty,time);
	}
	
	public void SetNewPosition(int game_type, int difficulty, float time, string name) {
		leaderboards[game_type-1].add_entry(difficulty,time,name);
		leaderboards[game_type-1].save_data(game_type);
		
		SetText();
	}
	
	public float GetWorstTime(int game_type, int difficulty) {
		return leaderboards[game_type-1].get_worst_time(difficulty);
	
	}
}
