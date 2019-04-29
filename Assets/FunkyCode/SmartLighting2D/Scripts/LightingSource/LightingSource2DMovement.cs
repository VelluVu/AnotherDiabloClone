using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightingSource2DMovement {
	public bool update = true;

	public Vector3 updatePosition = Vector3.zero;
	public Color updateColor = Color.white;
	public float updateRotation = 0f;
	public float updateSize = 0f;
	public float updateAlpha = 0.5f;
	
	public bool move = false;
	public Vector3 movePosition = Vector3.zero;

	public void SetPosition(Vector3 position) {
		movePosition = position;
		move = true;
	}

	public void ForceUpdate() {
		update = true;
	}

	public void Update(LightingSource2D source) {
		Transform transform = source.transform;
		
		if (updatePosition != transform.position) {
			updatePosition = transform.position;

			update = true;
		}

		if (updateRotation != transform.rotation.eulerAngles.z) {
			updateRotation = transform.rotation.eulerAngles.z;

			update = true;
		}

		if (updateSize != source.lightSize) {
			updateSize = source.lightSize;

			update = true;
		}

		if (updateColor.Equals(source.lightColor) == false) {
			updateColor = source.lightColor;
		}

		if (updateAlpha != source.lightAlpha) {
			updateAlpha = source.lightAlpha;
		}

		if (move == true) {
			if (updatePosition != movePosition) {
				updatePosition = movePosition;

				transform.position = movePosition;

				update = true;
			}
			move = false;
		}
	}
}