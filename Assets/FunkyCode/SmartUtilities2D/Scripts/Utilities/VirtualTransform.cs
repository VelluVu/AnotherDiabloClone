using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualTransform {
	// World Space Fields
	public Vector3 position;
	public Vector3 lossyScale;
	public Quaternion rotation;

	public VirtualTransform() {
		position = new Vector3(0, 0, 0);

		lossyScale = new Vector3(1, 1, 1);

		rotation = Quaternion.Euler(0, 0, 0);
	}
}
