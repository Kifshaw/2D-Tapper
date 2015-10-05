using UnityEngine;
using System.Collections;

public class SameCamera : MonoBehaviour {
	
	public Camera mainCam;
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Camera>().orthographicSize = mainCam.orthographicSize;
	}
}
