using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public struct Board
{
	public float[] time;
	public string[] name;
}

public class Leaderboard {
	Board[] leaderboard;
	string name;
	
	public Leaderboard() {
	
	}

	public void load_data(int num) {
		string fileName = "/Leaderboard"+num.ToString()+".txt";
        string path = Application.persistentDataPath + fileName;
        string rawFileData;
		
		leaderboard = new Board[3];
		leaderboard[0].time = new float[5];
		leaderboard[0].name = new string[5];
		leaderboard[1].time = new float[5];
		leaderboard[1].name = new string[5];
		leaderboard[2].time = new float[5];
		leaderboard[2].name = new string[5];
		
		using (StreamReader inFile = File.OpenText(path))
        {
			name = inFile.ReadLine();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					rawFileData = inFile.ReadLine();
					leaderboard[i].time[j] = (float)Convert.ToDouble(rawFileData);
					rawFileData = inFile.ReadLine();
					leaderboard[i].name[j] = rawFileData;
				}
				rawFileData = inFile.ReadLine(); //Consume crappy data
			}
		}
	}
	
	public void save_data(int num) {
		string fileName = "/Leaderboard"+num.ToString()+".txt";
        string path = Application.persistentDataPath + fileName;
		
		using (StreamWriter outfile = File.CreateText(path))
        {
			outfile.WriteLine(name);
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					outfile.WriteLine(leaderboard[i].time[j]);
					outfile.WriteLine(leaderboard[i].name[j]);
				}
				outfile.WriteLine("-----");
			}
		}
	}
	
	public bool check_time(int board, float time) {
		if (time < leaderboard[board].time[4])
		return true;
		else
		return false;
	}
	
	public bool add_entry(int board, float time, string name) {
		int index_to_move = 5;
		for (int i = 4; i >=0; i--)
		{
			if (time < leaderboard[board].time[i])
			{
				index_to_move = i;
			}
		}
		
		if (index_to_move == 5)
		return false;
		
		for (int i = 4; i > index_to_move; i--)
		{
			leaderboard[board].time[i] = leaderboard[board].time[i-1];
			leaderboard[board].name[i] = leaderboard[board].name[i-1];
		}
		
		leaderboard[board].time[index_to_move] = time;
		leaderboard[board].name[index_to_move] = name;
		
		return true;
	}
	
	public float get_worst_time(int board) {
		return leaderboard[board].time[4];
	}
	
	public Board get_board(int num) {
		return leaderboard[num];
	}
	
	public Board[] get_all_boards() {
		return leaderboard;
	}
	
	public string get_name() {
		return name;
	}
	
}
