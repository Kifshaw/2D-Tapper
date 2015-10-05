using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	public Camera camera;
	
	public bool moving;
	private float total_time; //time occuring in the move
	private Vector3 start_position;
	private Vector3 end_position;
	private Vector3 change;
	private float distance;
	private float speed_per_second; //determines the speed at which the camera will move
	public float Number_of_Seconds; //number of seconds for move to occur - will be between 0 and 1
	
	private float  change_in_size;
	private float ortho_size_begin;
	private float ortho_size_end;
	private bool size_up;
	void Start () {
		ortho_size_begin = camera.orthographicSize;
		ortho_size_end = 20f;
	}
	
	void Update () {
		if (moving)
		{
			total_time += Time.deltaTime; //sums total time
		
			gameObject.transform.position += (change.normalized*speed_per_second*Time.deltaTime); //linearly changes position of camera
			
			if (size_up) //Refers to camera's orthographic size
			{
				camera.orthographicSize += (change_in_size*Time.deltaTime);
				if (camera.orthographicSize > ortho_size_end)
				{
					size_up = false;
				}
			}
			else
			{
				if (camera.orthographicSize > ortho_size_begin)
				camera.orthographicSize -= (change_in_size*Time.deltaTime);
			}
		
		
			if (total_time >= Number_of_Seconds)
			{
				moving = false;
				gameObject.transform.position = new Vector3(end_position.x,end_position.y,-10f);
				camera.orthographicSize = ortho_size_begin;
			}
		}
	}
	
	
	public void SetEnd(Vector3 end)
	{
		moving = true;
		total_time = 0f;
		start_position = gameObject.transform.position;
		end_position = end;
		
		change = end_position-start_position - new Vector3(0,0,10f);
		distance = change.magnitude;
		speed_per_second = distance/Number_of_Seconds;
		
		change_in_size = ((ortho_size_end-ortho_size_begin)*2f)/Number_of_Seconds;
		size_up = true;
	}
}
