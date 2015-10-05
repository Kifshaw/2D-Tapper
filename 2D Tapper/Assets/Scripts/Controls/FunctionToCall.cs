using UnityEngine;
using System.Collections;

public class FunctionToCall : MonoBehaviour {

	public GameObject object_to_use;
	public string function;

	void Clicked() {
		object_to_use.SendMessage(function,SendMessageOptions.DontRequireReceiver);
	}
}
