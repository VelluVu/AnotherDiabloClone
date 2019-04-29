using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DemoFpsHelper : MonoBehaviour {
	int fps = 0;
	float timer = 0;
	int fpsResult = 0;
	Text text;

	void Start() {
		text = GetComponent<Text> ();
	}

	void OnRenderObject() {
		if (Camera.current != Camera.main) {
			return;
		}
		fps += 1;

		if (Time.realtimeSinceStartup > timer + 1){
			timer = Time.realtimeSinceStartup;
			fpsResult = fps;
			fps = 0;
		}

		if (Application.targetFrameRate > 0) {
			text.text = "fps " + fpsResult + "/" + Application.targetFrameRate;
		} else {
			text.text = "fps " + fpsResult;
		}
	}
}
