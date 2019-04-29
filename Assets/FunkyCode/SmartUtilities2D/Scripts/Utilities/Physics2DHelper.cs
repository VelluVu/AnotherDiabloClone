using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2018 Unity Physics Changes
public class Physics2DHelper {
	public static void AddForceAtPosition(Rigidbody2D body, Vector2 force,  Vector2 position) {
		string v = Application.unityVersion;
	
		int iv = int.Parse(v.Split("."[0])[0]);
		
		if (iv == 2018) {
			body.AddForceAtPosition(force, body.gameObject.transform.InverseTransformPoint(position));
		} else {
			body.AddForceAtPosition(force, position);
		}
	}	
}

