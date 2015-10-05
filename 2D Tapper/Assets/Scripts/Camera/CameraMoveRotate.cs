using UnityEngine;
using System.Collections;

public class CameraMoveRotate : MonoBehaviour {

	private Vector3 begin_position;
	private Vector3 end_position;
	private Quaternion begin_rotation;
	private Quaternion end_rotation;
	
	public bool moving;
	private float rotation_speed = 0.1f;
	private float total_time = 1f;
	private float half_time;
	private float time;
	
	private float begin_size = 5f;
	private float end_size = 14f;
	private float math_constant;
	private bool zoom_out;
	private bool debug = false;
	
	void Start () {
		moving = false;
		time = 0f;
		half_time = total_time/2f;
	}
	
	
	void Update () {
		if (moving)
		{
			time += Time.deltaTime;
			transform.position = Vector3.Lerp(begin_position, end_position, time/total_time);
			transform.rotation = Quaternion.Slerp(begin_rotation, end_rotation, time/total_time);//*Time.deltaTime);
			
			Camera.main.orthographicSize = math_constant*Mathf.Pow(time-half_time,2f) + end_size;
			
			if (zoom_out && debug)
			{
				//Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, end_size,rotation_speed*1.4f);
				//Camera.main.orthographicSize = -math_constant*(time-(total_time/2f))*(time-(total_time/2f))+begin_size;
				//Camera.main.orthographicSize = (-36f*time*time) + (36f*time) + 5f;
				//Camera.main.orthographicSize = math_constant*Mathf.Pow(time-half_time,2f) + begin_size;
				Debug.Log("Zoom Out: "+Camera.main.orthographicSize);
			}
			else if (debug)
			{
				Debug.Log("Zoom In: "+Camera.main.orthographicSize);
			}
			
			
			
			if (time >= total_time*0.5f || Camera.main.orthographicSize >= end_size)
			{
				zoom_out = false;
			}
			
			if (time >= total_time)
			{
				moving = false;
				transform.rotation = end_rotation;
				transform.position = end_position;
				Camera.main.orthographicSize = begin_size;
			}
			
			if (transform.rotation == end_rotation && transform.position == end_position)
			{
				moving = false;
			}
		}
	}
	
	public void SetEnd(Vector3 end, Quaternion new_rotation) {
		begin_position = gameObject.transform.position;
		end_position = new Vector3(end.x,end.y,-10f);
		begin_rotation = gameObject.transform.rotation;
		end_rotation = new_rotation;
		
		math_constant = (end_size-begin_size)*(end_size-begin_size)/(total_time);
		
		math_constant = (begin_size-end_size)/Mathf.Pow(half_time,2f);
		zoom_out = true;
		
		moving = true;
		time = 0f;
	}
}
