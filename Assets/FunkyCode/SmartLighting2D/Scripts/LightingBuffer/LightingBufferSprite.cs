using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBufferSprite {

	static VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();
 
	public static void MaskWithoutAtlas(LightingBuffer2D buffer, LightingCollider2D id, Material material, Vector2D offset, float z) {
		if (id.shape.maskType == LightingCollider2D.MaskType.None) {
			return;
		}

		if (id.shape.maskType != LightingCollider2D.MaskType.Sprite) {
			return;
		}
		
		if (id.isVisibleForLight(buffer) == false) {
			return;
		}

		Sprite sprite = id.shape.GetOriginalSprite();
		if (sprite == null || id.spriteRenderer == null) {
			return;
		}

		Vector2 p = id.transform.position;
		p.x += (float)offset.x;
		p.y += (float)offset.y;

		Vector2 scale = id.transform.lossyScale;

		material.mainTexture = sprite.texture;

		Max2D.DrawSprite(material, id.spriteRenderer, p, scale, id.transform.rotation.eulerAngles.z, z);

		material.mainTexture = null;
		
		LightingDebug.maskGenerations ++;		
	}

	// Y Axis
	public static void MaskDepthWithoutAtlas(LightingBuffer2D buffer, LightingCollider2D id, Material material, Vector2D offset, float z) {
		if (id.shape.maskType == LightingCollider2D.MaskType.None) {
			return;
		}
		
		if (id.shape.maskType != LightingCollider2D.MaskType.Sprite) {
			return;
		}

		if (id.isVisibleForLight(buffer) == false) {
			return;
		}

		Sprite sprite = id.shape.GetOriginalSprite();
		if (sprite == null || id.spriteRenderer == null) {
			return;
		}

		Vector2 p = id.transform.position;
		Vector2 scale = id.transform.lossyScale;

		material.mainTexture = sprite.texture;

		Max2D.DrawSprite(material, id.spriteRenderer, offset.ToVector2() + p, scale, id.transform.rotation.eulerAngles.z, z);

		material.mainTexture = null;

		LightingDebug.maskGenerations ++;		
	}

	public static void MaskWithAtlas(LightingBuffer2D buffer, LightingCollider2D id, Vector2D offset, float z) {
		if (id.shape.maskType == LightingCollider2D.MaskType.None) {
			return;
		}

		if (id.shape.maskType != LightingCollider2D.MaskType.Sprite) {
			return;
		}

		if (id.isVisibleForLight(buffer) == false) {
			return;
		}

		if (id.spriteRenderer == null) {
			return;
		}
		
		if (id.shape.GetOriginalSprite() == null) {
			return;
		}

		Sprite sprite = id.shape.GetAtlasSprite();
		if (sprite == null) {
			Sprite reqSprite = SpriteAtlasManager.RequestSprite(id.shape.GetOriginalSprite(), SpriteRequest.Type.WhiteMask);
			if (reqSprite == null) {
				PartiallyBatched_Collider batched = new PartiallyBatched_Collider();

				batched.collider2D = id;

				buffer.partiallyBatchedList_Collider.Add(batched);
				return;
			} else {
				id.shape.SetAtlasSprite(reqSprite);
				sprite = reqSprite;
			}
		}
		
		Vector2 p = id.transform.position;
		p.x += (float)offset.x;
		p.y += (float)offset.y;

		Vector2 scale = id.transform.lossyScale;

		spriteRenderer.sprite = sprite;

		Max2D.DrawSpriteBatched_Tris(spriteRenderer, buffer.lightSource.layerSetting[0], id.maskMode, p, scale, id.transform.rotation.eulerAngles.z, z);
		
		LightingDebug.maskGenerations ++;		
	}

	public static void MaskDepthWithAtlas(LightingBuffer2D buffer, LightingLayerEffect lightingLayerEffect, LightingCollider2D id, Vector2D offset, float z) {
		if (id.shape.maskType == LightingCollider2D.MaskType.None) {
			return;
		}

		if (id.shape.maskType != LightingCollider2D.MaskType.Sprite) {
			return;
		}

		if (id.isVisibleForLight(buffer) == false) {
			return;
		}

		Sprite sprite = id.shape.GetAtlasSprite();
		if (id.spriteRenderer == null) {
			return;
		}

		if (sprite == null) {
			Sprite reqSprite = SpriteAtlasManager.RequestSprite(id.shape.GetOriginalSprite(), SpriteRequest.Type.WhiteMask);
			if (reqSprite == null) {
				PartiallyBatched_Collider batched = new PartiallyBatched_Collider();

				batched.collider2D = id;

				buffer.partiallyBatchedList_Collider.Add(batched);
				return;
			} else {
				id.shape.SetAtlasSprite(reqSprite);
				sprite = reqSprite;
			}
		}

		GL.Color(Color.white);
		if (lightingLayerEffect == LightingLayerEffect.InvisibleBellow && buffer.lightSource.transform.position.y > id.transform.position.y) {
			GL.Color(Color.black);
		}

		Vector2 p = id.transform.position;
		p.x += (float)offset.x;
		p.y += (float)offset.y;

		Vector2 scale = id.transform.lossyScale;

		spriteRenderer.sprite = sprite;

		Max2D.DrawSpriteBatched_Tris(spriteRenderer, buffer.lightSource.layerSetting[0], id.maskMode, p, scale, id.transform.rotation.eulerAngles.z, z);

		LightingDebug.maskGenerations ++;		
	}
}