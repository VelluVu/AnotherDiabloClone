using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightingColliderMovement {
	public bool moved = false;

	private Vector2 movedPosition = Vector2.zero;
	private Vector2 movedScale = Vector3.zero;
	private float movedRotation = 0;
	
	private bool flipX = false;
	private bool flipY = false;

	public void Reset() {
		movedPosition = Vector2.zero;
		movedRotation = 0;
		movedScale = Vector3.zero;
	}

	public void Update(LightingCollider2D collider) {
		Vector2 position = collider.transform.position;
		Vector2 scale = collider.transform.lossyScale;
		float rotation = collider.transform.rotation.eulerAngles.z;

		moved = false;

		if (movedPosition != position) {
			movedPosition = position;
			moved = true;
		}
				
		if (movedScale != scale) {
			movedScale = scale;
			moved = true;
		}

		if (movedRotation != rotation) {
			movedRotation = rotation;
			moved = true;
		}

		if (collider.shape.maskType == LightingCollider2D.MaskType.SpriteCustomPhysicsShape || collider.shape.colliderType == LightingCollider2D.ColliderType.SpriteCustomPhysicsShape) {
			if (collider.spriteRenderer != null) {
				if (collider.spriteRenderer.flipX != flipX || collider.spriteRenderer.flipY != flipY) {
					flipX = collider.spriteRenderer.flipX;
					flipY = collider.spriteRenderer.flipY;

					moved = true;
					
					collider.shape.ResetWorld();
				}
				
				if (collider.shape.GetOriginalSprite() != collider.spriteRenderer.sprite) {
					collider.shape.SetOriginalSprite(collider.spriteRenderer.sprite);
					collider.shape.SetAtlasSprite(null); // Only For Sprite Mask?

					moved = true;
					
					collider.shape.ResetLocal();
				}
			}
		}

		if (collider.shape.maskType == LightingCollider2D.MaskType.Sprite) {
			if (collider.spriteRenderer != null && collider.shape.GetOriginalSprite() != collider.spriteRenderer.sprite) {
				collider.shape.SetOriginalSprite(collider.spriteRenderer.sprite);
				collider.shape.SetAtlasSprite(null);

				moved = true;
			}
		}
	}
}