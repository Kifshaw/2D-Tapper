using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ClickControls : MonoBehaviour {

	public ClickSprite click;
	public bool clicking;
	private ClickSprite.Clicked clicked_object;
	public CameraMoveRotate camera_mover;
	
	public bool pc_controls;
	private Vector2 previousPos;
	
	public GameObject[] locations;
	IDictionary<string, Vector3> locationLibrary = new Dictionary<string, Vector3>();
	IDictionary<string, Quaternion> rotationLibrary = new Dictionary<string, Quaternion>();
	// Use this for initialization
	void Start () {
		clicking = false;
		
		foreach (GameObject this_location in locations)
		{
			locationLibrary[this_location.name] = this_location.transform.position;
			rotationLibrary[this_location.name] = this_location.transform.rotation;
		}
	}
	
	
	void PC_Update()
    {

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 mouseDeltaPos = mousePos - previousPos;
            if (Input.GetMouseButtonDown(0))
            {
                clicked_object = click.Click(mousePos);
                if (clicked_object.isClicked)
                {
                    clicked_object.clicked_object.SendMessage("BeginClick", SendMessageOptions.DontRequireReceiver);
                    clicking = true;
					
					if (clicked_object.clicked_object.tag == "GameplayObjects")
					clicking = false;
                }
                else
                {
                    clicking = false;
                }
            }
            previousPos = mousePos;
            /*
             * clicked_object = click.Click(touches[0].position);
					if (clicked_object.isClicked)
					{
						clicked_object.clicked_object.SendMessage("BeginClick",SendMessageOptions.DontRequireReceiver);
						clicking = true;
					}
					else //move map
					{
						if (CheckMovePosition(touches[0].position))
						ChangeCameraPosition(touches[0].deltaPosition);
						clicking = false;
					}
             */

        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 mouseDeltaPos = mousePos - previousPos;
            if (clicking && clicked_object.clicked_object == click.Click(mousePos).clicked_object)
            {
                clicked_object.clicked_object.SendMessage("EndClick", SendMessageOptions.DontRequireReceiver);
                clicking = false;
            }
			clicking = false;

            previousPos = mousePos;
            /*
            if (clicking && clicked_object.clicked_object == click.Click(touches[0].position).clicked_object)
            {
                clicked_object.clicked_object.SendMessage("EndClick", SendMessageOptions.DontRequireReceiver);
                clicking = false;
            }
            else if (!clicking) //move map
            {
                if (CheckMovePosition(touches[0].position))
                    ChangeCameraPosition(touches[0].deltaPosition);
            }
            */
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (pc_controls)
		PC_Update();
	
		Touch[] touches = Input.touches;
		if (!camera_mover.moving)
		{
			foreach (Touch touch in touches)
			{
				switch (touch.phase)
				{
					case TouchPhase.Began:
						clicked_object = click.Click(touch.position);
						if (clicked_object.isClicked)
						{
							clicked_object.clicked_object.SendMessage("BeginClick",SendMessageOptions.DontRequireReceiver);
							clicking = true;
							
							if (clicked_object.clicked_object.tag == "GameplayObjects")
							clicking = false;
						}
						else
						{
							clicking = false;
						}
						break;
					case TouchPhase.Moved:
					case TouchPhase.Stationary:
						if (clicking && clicked_object.clicked_object.name != click.Click(touch.position).clicked_object.name)
						{
							clicked_object.clicked_object.SendMessage("CanceledClick",SendMessageOptions.DontRequireReceiver);
							clicking = false;
						}
						break;
					case TouchPhase.Ended:
						if (clicking && clicked_object.clicked_object == click.Click(touch.position).clicked_object)
						{
							clicked_object.clicked_object.SendMessage("EndClick",SendMessageOptions.DontRequireReceiver);
							clicking = false;
						}
						break;
					default:
						break;
				}
			}
		}
	}
	
	public void ChangeLocation(string name) {
		camera_mover.SetEnd(locationLibrary[name], rotationLibrary[name]);
	}
}
