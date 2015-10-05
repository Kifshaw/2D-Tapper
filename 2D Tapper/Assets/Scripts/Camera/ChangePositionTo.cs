using UnityEngine;
using System.Collections;

public class ChangePositionTo : MonoBehaviour {

	private ClickControls moving_controls;
	public string name_of_location;
	
	void Start() {
		moving_controls = GameObject.Find("Main Camera").GetComponent<ClickControls>();
	}
	
	// Update is called once per frame
	void Clicked () {
		moving_controls.ChangeLocation(name_of_location);
	}
}
