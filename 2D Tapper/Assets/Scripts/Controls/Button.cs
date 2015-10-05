using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	private SpriteRenderer button;
	public Sprite buttonON;
	public Sprite buttonOFF;
	
	public bool beginClick;
	public bool ignoreSpriteChanging;
	// Use this for initialization
	void Start () {
		beginClick = false;
		if (!ignoreSpriteChanging)
		{
			button = GetComponent<SpriteRenderer>();
			button.sprite = buttonOFF;
		}
	}
	
	void BeginClick() {
		beginClick = true;
		if (!ignoreSpriteChanging)
		button.sprite = buttonON;
	}
	
	void CanceledClick() {
		beginClick = false;
		if (!ignoreSpriteChanging)
		button.sprite = buttonOFF;
	}
	
	void EndClick() {
		if (beginClick)
		{
			beginClick = false;
			gameObject.SendMessage("Clicked",SendMessageOptions.DontRequireReceiver);
			if (!ignoreSpriteChanging)
			button.sprite = buttonOFF;
		}
	}
}