using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingSpriteBuffer {
	static List<LightingSpriteRenderer2D> spriteRendererList;
	static LightingManager2D manager;
	static VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();
	static LightingSpriteRenderer2D id;
	static Material material;
	static Vector2 position, scale;
	static float rot;
	static Color color;

    public static void Draw(Vector2D offset, float z) {
		spriteRendererList = LightingSpriteRenderer2D.GetList();
		manager = LightingManager2D.Get();

		GL.PushMatrix ();

		if (manager.lightingSpriteAtlas) {
			DrawWithAtlas(offset, z);
		} else {
			DrawWithoutAtlas(offset, z);
		}

		GL.PopMatrix();
    }

	static public void DrawWithAtlas(Vector2D offset, float z) {

		material = manager.materials.GetAdditive();
		material.SetColor ("_TintColor", Color.white);
		material.mainTexture = SpriteAtlasManager.Get().atlasTexture.texture;

		material.SetPass (0);

		GL.Begin (GL.TRIANGLES);
		
		for(int i = 0; i < spriteRendererList.Count; i++) {
			id = spriteRendererList[i];

			if (id.type != LightingSpriteRenderer2D.Type.Particle) {
				continue;
			}

			if (id.GetSprite() == null) {
				continue;
			}

			if (id.InCamera() == false) {
				continue;
			}

			position = id.transform.position;

			scale = id.transform.lossyScale;
			scale.x *= id.offsetScale.x;
			scale.y *= id.offsetScale.y;

			rot = id.offsetRotation;
			if (id.applyTransformRotation) {
				rot += id.transform.rotation.eulerAngles.z;
			}

			color = id.color;
			color.a = id.alpha;

			GL.Color(color);

			spriteRenderer.sprite = SpriteAtlasManager.RequestSprite(id.GetSprite(), SpriteRequest.Type.Normal);
			
			Max2D.DrawSpriteBatched_Tris_Day(spriteRenderer, offset.ToVector2() + position + id.offsetPosition, scale, rot, z);

			LightingDebug.SpriteRenderersDrawn ++;
		}

		GL.End();

		material = manager.materials.GetAtlasMaterial();
		material.SetPass (0);

		GL.Begin (GL.TRIANGLES);
		
		for(int i = 0; i < spriteRendererList.Count; i++) {
			id = spriteRendererList[i];

			if (id.type == LightingSpriteRenderer2D.Type.Particle) {
				continue;
			}

			if (id.GetSprite() == null) {
				continue;
			}

			if (id.InCamera() == false) {
				continue;
			}

			position = id.transform.position;

			scale = id.transform.lossyScale;
			scale.x *= id.offsetScale.x;
			scale.y *= id.offsetScale.y;

			rot = id.offsetRotation;
			if (id.applyTransformRotation) {
				rot += id.transform.rotation.eulerAngles.z;
			}

			switch(id.type) {
				case LightingSpriteRenderer2D.Type.WhiteMask:
					GL.Color(Color.white);

					spriteRenderer.sprite = SpriteAtlasManager.RequestSprite(id.GetSprite(), SpriteRequest.Type.WhiteMask);
					
					Max2D.DrawSpriteBatched_Tris_Day(spriteRenderer, offset.ToVector2() + position + id.offsetPosition, scale, rot, z);
					break;

				case LightingSpriteRenderer2D.Type.BlackMask:
					GL.Color(Color.black);

					spriteRenderer.sprite = SpriteAtlasManager.RequestSprite(id.GetSprite(), SpriteRequest.Type.WhiteMask);
					
					Max2D.DrawSpriteBatched_Tris_Day(spriteRenderer, offset.ToVector2() + position + id.offsetPosition, scale, rot, z);

					break;
			}

			LightingDebug.SpriteRenderersDrawn ++;
		}

		GL.End();
	}

	static public void DrawWithoutAtlas(Vector2D offset, float z) {
        for(int i = 0; i < spriteRendererList.Count; i++) {
			id = spriteRendererList[i];

			if (id.GetSprite() == null) {
				continue;
			}

			if (id.InCamera() == false) {
				continue;
			}

			LightingDebug.SpriteRenderersDrawn ++;

			position = id.transform.position;

			scale = id.transform.lossyScale;
			scale.x *= id.offsetScale.x;
			scale.y *= id.offsetScale.y;

			rot = id.offsetRotation;
			if (id.applyTransformRotation) {
				rot += id.transform.rotation.eulerAngles.z;
			}

			switch(id.type) {
				case LightingSpriteRenderer2D.Type.Particle: 

					color = id.color;
					color.a = id.alpha;

					material = manager.materials.GetAdditive();
					material.SetColor ("_TintColor", color);

					material.mainTexture = id.GetSprite().texture;
					Max2D.DrawSprite(material, id.spriteRenderer, offset.ToVector2() + position + id.offsetPosition, scale, rot, z);
					material.mainTexture = null;
				
					break;

				case LightingSpriteRenderer2D.Type.WhiteMask:

					material = manager.materials.GetWhiteSprite();
					
					material.mainTexture = id.GetSprite().texture;
					Max2D.DrawSprite(material, id.spriteRenderer, offset.ToVector2() + position + id.offsetPosition, scale, rot, z);
					material.mainTexture = null;
				
					break;

				case LightingSpriteRenderer2D.Type.BlackMask:

					material = manager.materials.GetBlackSprite();

					material.mainTexture = id.sprite.texture;
					Max2D.DrawSprite(material, id.spriteRenderer, offset.ToVector2() + position + id.offsetPosition, scale, rot, z);
					material.mainTexture = null;
				
					break;
			}
		}
	}
}