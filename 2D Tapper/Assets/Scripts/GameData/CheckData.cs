using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class CheckData : MonoBehaviour {

	public int myFlag; //nonzero will force data to reset
	public TextAsset leaderboard1;
	public TextAsset leaderboard2;

	void check_data_file() {
		string path = Application.persistentDataPath;
		if (!File.Exists(path + "/Leaderboard1.txt"))
        {
            using (StreamWriter outfile = File.CreateText(path + "/Leaderboard1.txt"))
			{
				outfile.Write(leaderboard1.text);
			}
        }
		if (!File.Exists(path + "/Leaderboard2.txt"))
        {
            using (StreamWriter outfile = File.CreateText(path + "/Leaderboard2.txt"))
			{
				outfile.Write(leaderboard2.text);
			}
        }
	}
	
	
	void create_data_file() {
		string path = Application.persistentDataPath;
		using (StreamWriter outfile = File.CreateText(path + "/Leaderboard1.txt"))
		{
			outfile.Write(leaderboard1.text);
		}
		using (StreamWriter outfile = File.CreateText(path + "/Leaderboard2.txt"))
		{
			outfile.Write(leaderboard2.text);
		}
	}

	void Awake() {
		check_data_file();
		if (myFlag != 0)
		{
			create_data_file();
		}
	}
}
