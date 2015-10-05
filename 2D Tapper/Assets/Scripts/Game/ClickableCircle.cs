using UnityEngine;
using System.Collections;

public class ClickableCircle : MonoBehaviour {
	public enum CircleType {
		STABLE,
		MOVING
	}
	public int index;
	public int wave;
	private string number;
	
	public TextMesh number_image;
	private Animator anim;
	public CircleType type;
	public EventControls controller;
	private bool activated;

	void Start() {
		number = "0";
		activated = false;
		anim = gameObject.GetComponent<Animator>();
	}

	// Use this for initialization
	void BeginClick() {
		controller.CircleClicked(wave,index, this);
	}
	
	public void SetValues(int wav, int ind, string num) {
		wave = wav;
		index = ind;
		number = num;
		number_image.text = number;
		activated = true;
	}
	
	public void AddForce(Vector3 upper_right) {
	 Vector2 force = new Vector2(Random.Range(-upper_right.y,upper_right.x), Random.Range(-upper_right.y,upper_right.y));
		GetComponent<Rigidbody2D>().AddForce(force ,ForceMode2D.Impulse);
	}
	
	public void SuccessfulClick() {
		Destroy(gameObject);
	}
	
	public void FailClick() {
		Debug.Log("FAIL CLICK");
	}
	
	public void ShowValue() {
		number_image.GetComponent<Renderer>().enabled = true;
	}
}
