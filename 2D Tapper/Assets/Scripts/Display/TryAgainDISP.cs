using UnityEngine;
using System.Collections;

public class TryAgainDISP : MonoBehaviour {

	public TextMesh time_disp;
	
	public float time;
	
	// Update is called once per frame
	void Update () {
		time_disp.text = time.ToString("0.00000") + " seconds";
	}
	
	public void SetTime(float t) {
		time = t;
	}
	
	void DestroyAll() {
		Destroy(gameObject);
	}
}
