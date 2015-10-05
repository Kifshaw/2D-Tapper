using UnityEngine;
using System.Collections;

public class FakeControls : MonoBehaviour {

	public CameraMove move;
	public CameraMoveRotate move_2;

	public GameObject Alpha_1;
	public GameObject Alpha_2;
	public GameObject Alpha_3;
	public GameObject Alpha_4;
	public GameObject Alpha_5;
	
	void Start() {
	
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			move_2.SetEnd(Alpha_1.transform.position, Alpha_1.transform.rotation);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			move_2.SetEnd(Alpha_2.transform.position, Alpha_2.transform.rotation);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			move_2.SetEnd(Alpha_3.transform.position, Alpha_3.transform.rotation);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			
		}
	}
}
