using UnityEngine;
using System.Collections;

public class EventControls : MonoBehaviour {

	public enum GameType
	{
		THROWAWAY,
		SIMPLE,
		MOVING
	}
	
	public enum Difficulty
	{
		EASY,
		MEDIUM,
		HARD
	}
	
	public enum GameState
	{
		PLAYING,
		CHECKING_TIME,
		INTERMISSION,
		ENTER_NAME,
		BACK_TO_MAIN
	}
	
	[System.Serializable]
	public struct Boundaries
	{
		public GameObject left;
		public BoxCollider2D left_coll;
		public GameObject right;
		public BoxCollider2D right_coll;
		public GameObject up;
		public BoxCollider2D up_coll;
		public GameObject down;
		public BoxCollider2D down_coll;
	}
	
	private GameState state;
	
	public GameType game_type;
	public Difficulty difficulty;
	public GameObject stable_circle;
	public GameObject moving_circle;
	
	private Vector3 upper_right;
	
	private string[,] numbers;
	private bool[,] if_clicked;
	private ClickableCircle[] circles;
	
	public Boundaries bounds;
	public TextMesh time_mesh;
	public float time;
	public int wave_number;
	private GameObject circle_to_spawn;
	
	private int number_of_waves = 3;
	private int number_of_circles = 7;
	
	private LeaderboardControl leaderboard;
	
	private TouchScreenKeyboard keyboard;
	private bool keyboard_active;
	private string input_name;
	public GameObject sorry_you_lost;
	public GameObject enter_name_prefab;
	private GameObject enter_nameG;
	private TextMesh enter_nameT;
	public GameObject back;
	
	public bool DEBUG_LOG;
	private float buffer_time;
	public bool PC;
	
	void Start () {
		numbers = new string[number_of_waves,number_of_circles];
		if_clicked = new bool[number_of_waves,number_of_circles];
		circles = new ClickableCircle[number_of_circles];
		
		time = 0f;
		circle_to_spawn = stable_circle; //this is just to make sure that it is intialized
		
		state = GameState.INTERMISSION;
		
		upper_right = Camera.main.ViewportToWorldPoint(new Vector3(1f,1f, 10f)) - Camera.main.transform.position;
		time_mesh.transform.position = transform.position + (-upper_right.x*transform.right) + (upper_right.y*transform.up);
		SetBoundaries();
		
		leaderboard = GameObject.Find("Leaderboards").GetComponent<LeaderboardControl>();
		
		TouchScreenKeyboard.hideInput = true;
		keyboard_active = false;
		input_name = "";
	}
	
	// Update is called once per frame
	void Update () {
		time_mesh.text = " "+time.ToString("0.00000") + " sec";
		switch (state)
		{
			case GameState.PLAYING:
				time += Time.deltaTime;
				break;
			case GameState.INTERMISSION:
				if (buffer_time < 2f)
				buffer_time += Time.deltaTime;
				else
				time = 0;
				break;
			case GameState.CHECKING_TIME:
				if (leaderboard.CheckNewTime((int)game_type,(int)difficulty,time))
				{
					state = GameState.ENTER_NAME;
					input_name = "";
					if (!PC)
					{
						keyboard = TouchScreenKeyboard.Open(input_name, TouchScreenKeyboardType.Default, false, false, false, false, "");
						keyboard_active = true;
					
						enter_nameG = (GameObject)Instantiate(enter_name_prefab,transform.position,transform.rotation);
						enter_nameT = enter_nameG.GetComponent<TextMesh>();
					}
				}
				else
					state = GameState.BACK_TO_MAIN;
					
				break;
			case GameState.ENTER_NAME:
				if(!PC)
				{
					input_name = keyboard.text;
					enter_nameT.text = "Enter your name: "+input_name;
				}
				if (!PC && (keyboard.done || !keyboard_active))
				{
					keyboard_active = false;
					Destroy(enter_nameG);
					leaderboard.SetNewPosition((int)game_type,(int)difficulty,time,input_name);
					Instantiate(back, transform.position,transform.rotation);
					state = GameState.INTERMISSION;
					buffer_time = 0f;
				}
				else if (PC)
				{
					input_name = "Cam's Computer";
					leaderboard.SetNewPosition((int)game_type,(int)difficulty,time,input_name);
					Instantiate(back, transform.position,transform.rotation);
					state = GameState.INTERMISSION;
					buffer_time = 0f;
				}
				break;
			case GameState.BACK_TO_MAIN:
				GameObject temp = (GameObject)Instantiate(sorry_you_lost, transform.position, transform.rotation);
				temp.GetComponent<TryAgainDISP>().SetTime(leaderboard.GetWorstTime((int)game_type,(int)difficulty));
				state = GameState.INTERMISSION;
				buffer_time = 0f;
				break;
			default:
				break;
		}
	}
	
	public void SetDifficulty(int diff) {
		difficulty = (Difficulty)diff;
		LoadGame();
	}
	
	public void LoadGame() {
		if (DEBUG_LOG)
		Debug.Log("LOADING GAME");
	
		wave_number = 0;
		switch (difficulty)
		{
			case Difficulty.EASY:
				for (int i = 0; i < number_of_waves; i++)
				{
					for (int j = 0; j < number_of_circles; j++)
					{
						numbers[i,j] = (j+1).ToString();
					}
				}
				break;
			case Difficulty.MEDIUM:
				for (int i = 0; i < number_of_waves; i++)
				{
					int last_number = 0;
					for (int j = 0; j < number_of_circles; j++)
					{
						last_number = last_number+(int)Random.Range(2f,11f);
						numbers[i,j] = last_number.ToString();
					}
				}
				break;
			case Difficulty.HARD:
				break;
			default:
				break;
		}
		switch (game_type)
		{
			case GameType.SIMPLE:
				circle_to_spawn = stable_circle;
				SpawnNextWave();
				break;
			case GameType.MOVING:
				circle_to_spawn = moving_circle;
				SpawnNextWave();
				break;
			default:
				break;
		}
	}
	
	void StartGame() {
		if (DEBUG_LOG)
		Debug.Log("STARTING GAME");
		
		time = 0f;
		wave_number = 0;
		state = GameState.PLAYING;
		
		for (int i = 0; i < number_of_circles; i++)
		{
			circles[i].ShowValue();
			if (game_type == GameType.MOVING)
			    circles[i].AddForce(upper_right);
		}
	}
	
	void EndGame() {
		if (DEBUG_LOG)
		Debug.Log("ENDING GAME");
		
		
		if_clicked = new bool[number_of_waves,number_of_circles];
		
		state = GameState.CHECKING_TIME;
	}
	
	void SpawnNextWave() {
		if (DEBUG_LOG)
		Debug.Log("Spawning wave: "+wave_number);
		
		GameObject temp;
		Vector3 random_position;
		Vector2 position_in_world_space;
		BoxCollider2D temp_collider;
		Collider2D[] temp_collider_array = new Collider2D[2];
		if (wave_number == 0) //spawn circle with index of "-1" and a "0" on it
		{
			position_in_world_space = new Vector2(0.9f*Random.Range(-upper_right.x,upper_right.x), 0.9f*Random.Range(-upper_right.y,upper_right.y));
			random_position = gameObject.transform.position + (position_in_world_space.x*transform.right) + (position_in_world_space.y*transform.up); 
			temp = (GameObject)Instantiate(circle_to_spawn, random_position , transform.rotation);
			temp_collider = temp.GetComponent<BoxCollider2D>();
			while (Physics2D.OverlapAreaNonAlloc(temp_collider.bounds.min, temp_collider.bounds.max, temp_collider_array) > 1)
			{
				position_in_world_space = new Vector2(0.9f*Random.Range(-upper_right.x,upper_right.x), 0.9f*Random.Range(-upper_right.y,upper_right.y));
				random_position = gameObject.transform.position + (position_in_world_space.x*transform.right) + (position_in_world_space.y*transform.up);
				temp.transform.position = random_position;
			}
			temp.GetComponent<ClickableCircle>().SetValues(0,-1,"0");
			temp.GetComponent<ClickableCircle>().ShowValue();
			temp.GetComponent<ClickableCircle>().controller = this;
		}
		
		for (int i = 0; i < number_of_circles; i++)
		{
			position_in_world_space = new Vector2(0.9f*Random.Range(-upper_right.x,upper_right.x), 0.9f*Random.Range(-upper_right.y,upper_right.y));
			random_position = gameObject.transform.position + (position_in_world_space.x*transform.right) + (position_in_world_space.y*transform.up); 
			temp = (GameObject)Instantiate(circle_to_spawn, random_position ,transform.rotation);
			temp_collider = temp.GetComponent<BoxCollider2D>();
			while (Physics2D.OverlapAreaNonAlloc(temp_collider.bounds.min, temp_collider.bounds.max, temp_collider_array) > 1)
			{
				position_in_world_space = new Vector2(0.9f*Random.Range(-upper_right.x,upper_right.x), 0.9f*Random.Range(-upper_right.y,upper_right.y));
				random_position = gameObject.transform.position + (position_in_world_space.x*transform.right) + (position_in_world_space.y*transform.up);
				temp.transform.position = random_position;
			}
			circles[i] = temp.GetComponent<ClickableCircle>();
			circles[i].SetValues(wave_number, i, numbers[wave_number,i]);
			circles[i].controller = this;
			
			if (wave_number != 0)
			{
				circles[i].ShowValue();
                if (game_type == GameType.MOVING)
                    circles[i].AddForce(upper_right);
            }
		}
	}
	
	public void CircleClicked(int wave, int index, ClickableCircle circ) {
	
		if (index == -1)
		{
			circ.SuccessfulClick();
			StartGame(); 
			return;
		}
	
		bool activate_circle = false;
		if (wave == 0 && index == 0)
		{
			activate_circle = true;
		}
		else if (wave > 0 && index == 0 && if_clicked[wave-1,number_of_circles-1] == true)
		{
			activate_circle = true;
		}
		else if (if_clicked[wave,index-1] == true)
		{
			activate_circle = true;
		}
		
		if (index == number_of_circles-1 && activate_circle && wave != number_of_waves-1)
		{
			wave_number++;
			SpawnNextWave();
		}
		
		if (activate_circle)
		{
			if_clicked[wave,index] = true;
			circ.SuccessfulClick();
		}
		else
		{
			circ.FailClick();
		}
		
		if (if_clicked[number_of_waves-1,number_of_circles-1])
		{
			EndGame();
		}
	}
	
	void SetBoundaries() {
		bounds.left.transform.position = bounds.left.transform.position + (-upper_right.x*transform.right);
		bounds.right.transform.position = bounds.right.transform.position + (upper_right.x*transform.right);
		bounds.up.transform.position = bounds.up.transform.position + (upper_right.y*transform.up);
		bounds.down.transform.position = bounds.down.transform.position + (-upper_right.y*transform.up);
		
		bounds.left_coll.size = new Vector2(0.5f, (upper_right.y+1f)*2f);
		bounds.right_coll.size = new Vector2(0.5f, (upper_right.y+1f)*2f);
		bounds.up_coll.size = new Vector2((upper_right.x+1f)*2f,0.5f);
		bounds.down_coll.size = new Vector2((upper_right.x+1f)*2f,0.5f);
	}
}
