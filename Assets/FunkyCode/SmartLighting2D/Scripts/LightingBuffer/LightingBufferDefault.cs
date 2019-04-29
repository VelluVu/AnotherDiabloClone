using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBufferDefault {

	static Vector2D offset = Vector2D.Zero();
	static List<LightingCollider2D> colliderList;
	static LightingManager2D manager;

	static bool drawMask = false;
	static bool drawShadows = false;

   	#if UNITY_2018_1_OR_NEWER
        static List<LightingTilemapCollider2D> tilemapList;
    #endif

	public static void DrawShadowsAndMask(LightingBuffer2D buffer, int layer) {
		buffer.CalculateCoords();

		drawMask = false;
        if (buffer.lightSource.layerSetting[layer].type != LightingLayerType.ShadowOnly) {
            drawMask = true;
        }

        drawShadows = false;
        if (buffer.lightSource.layerSetting[layer].type != LightingLayerType.MaskOnly) {
            drawShadows = true;
        }

		GL.PushMatrix();

		if (LightingManager2D.Get().lightingSpriteAtlas && SpriteAtlasManager.Get().atlasTexture != null) {
			DrawWithAtlas(buffer, layer);
		} else {
			DrawWithoutAtlas(buffer, layer);
		}

		GL.PopMatrix();	
	}

	public static void DrawWithoutAtlas(LightingBuffer2D buffer, int layer) {
		if (drawShadows) {
			LightingBufferDefault.DrawShadowsWithoutAtlas(buffer, layer);
		}

		if (drawMask) {
			LightingBufferDefault.DrawMaskWithoutAtlas(buffer, layer);
		}
	}

   	public static void DrawShadowsWithoutAtlas(LightingBuffer2D buffer, int layer) {
		float lightSizeSquared = Mathf.Sqrt(buffer.lightSource.lightSize * buffer.lightSource.lightSize + buffer.lightSource.lightSize * buffer.lightSource.lightSize);
		float z = buffer.transform.position.z;

        offset.x = -buffer.lightSource.transform.position.x;
		offset.y = -buffer.lightSource.transform.position.y;

		manager = LightingManager2D.Get();
        colliderList = LightingCollider2D.GetList();

		#if UNITY_2018_1_OR_NEWER
			tilemapList = LightingTilemapCollider2D.GetList();
		#endif

		manager.materials.GetAtlasMaterial().SetPass(0);

		GL.Begin(GL.TRIANGLES);

		for(int id = 0; id < colliderList.Count; id++) {
			if ((int)colliderList[id].lightingCollisionLayer != layer) {
				continue;
			}

			LightingBufferMesh.Shadow(buffer, colliderList[id], lightSizeSquared, z);
			
			LightingBufferShape.Shadow(buffer, colliderList[id], lightSizeSquared, z, offset);
		}

		#if UNITY_2018_1_OR_NEWER
			for(int id = 0; id < tilemapList.Count; id++) {
				if ((int)tilemapList[id].lightingCollisionLayer != layer) {
					continue;
				}

				LightingBufferTilemapRectangle.Shadow(buffer, tilemapList[id], lightSizeSquared, z);
			}
		#endif 
 
		GL.End();
	}
	
	static public void DrawMaskWithoutAtlas(LightingBuffer2D buffer, int layer) {
		float z = buffer.transform.position.z;

        offset.x = -buffer.lightSource.transform.position.x;
		offset.y = -buffer.lightSource.transform.position.y;
        
		manager = LightingManager2D.Get();
        colliderList = LightingCollider2D.GetList();

		LayerSetting layerSetting = buffer.lightSource.layerSetting[layer];
		bool maskEffect = (layerSetting.effect == LightingLayerEffect.InvisibleBellow);
	
		Material materialWhite = manager.materials.GetWhiteSprite();
		Material materialBlack = manager.materials.GetBlackSprite();
		Material material;

		#if UNITY_2018_1_OR_NEWER
			tilemapList = LightingTilemapCollider2D.GetList();
		#endif
		
        //materialWhite.SetPass(0);
		manager.materials.GetAtlasMaterial().SetPass(0);

        GL.Begin(GL.TRIANGLES);
        GL.Color(Color.white);

		// Collider Shape Mask
		for(int id = 0; id < colliderList.Count; id++) {
			if ((int)colliderList[id].lightingMaskLayer != layer) {
				continue;
			}
			LightingBufferShape.Mask(buffer, colliderList[id], layerSetting, offset, z);
		}

		GL.Color(Color.white);
		
        // Tilemap Shape Mask
		#if UNITY_2018_1_OR_NEWER
			for(int id = 0; id < tilemapList.Count; id++) {
				if ((int)tilemapList[id].lightingMaskLayer != layer) {
					continue;
				}
				LightingBufferTilemapRectangle.MaskShape(buffer, tilemapList[id], offset, z);
			}
		#endif

		GL.End();

		// Collider Sprite Mask
		if (colliderList.Count > 0) {
			for(int id = 0; id < colliderList.Count; id++) {
				if ((int)colliderList[id].lightingMaskLayer != layer) {
					continue;
				}
				material = materialWhite;

				if (colliderList[id].maskMode == LightingMaskMode.Invisible) {
					material = materialBlack;
				} else if (maskEffect && colliderList[id].transform.position.y < buffer.lightSource.transform.position.y) {
					material = materialBlack;
				}
			
				LightingBufferSprite.MaskWithoutAtlas(buffer, colliderList[id], material, offset, z);
			}
		}

		
		// Tilemap Sprite Mask
		#if UNITY_2018_1_OR_NEWER
			for(int id = 0; id < tilemapList.Count; id++) {
				if ((int)tilemapList[id].lightingMaskLayer != layer) {
					continue;
				}
				LightingBufferTilemapRectangle.MaskSpriteWithoutAtlas(buffer, tilemapList[id], materialWhite, materialBlack, offset, z);
			}
		#endif
	}

	public static void DrawWithAtlas(LightingBuffer2D buffer, int layer) {
		float lightSizeSquared = Mathf.Sqrt(buffer.lightSource.lightSize * buffer.lightSource.lightSize + buffer.lightSource.lightSize * buffer.lightSource.lightSize);
		float z = buffer.transform.position.z;

        offset.x = -buffer.lightSource.transform.position.x;
		offset.y = -buffer.lightSource.transform.position.y;

		manager = LightingManager2D.Get();
        colliderList = LightingCollider2D.GetList();

		LayerSetting layerSetting = buffer.lightSource.layerSetting[layer];
		bool maskEffect = (layerSetting.effect == LightingLayerEffect.InvisibleBellow);
	
		#if UNITY_2018_1_OR_NEWER
			tilemapList = LightingTilemapCollider2D.GetList();
		#endif

		// Shadow Fill
		manager.materials.GetAtlasMaterial().SetPass(0);

		GL.Begin(GL.TRIANGLES);

		if (drawShadows) {

			for(int id = 0; id < colliderList.Count; id++) {
				if ((int)colliderList[id].lightingCollisionLayer != layer) {
					continue;
				}

				// Collider Shadow
				LightingBufferMesh.Shadow(buffer, colliderList[id], lightSizeSquared, z);
				LightingBufferShape.Shadow(buffer, colliderList[id], lightSizeSquared, z, offset);
			}

			#if UNITY_2018_1_OR_NEWER
				for(int id = 0; id < tilemapList.Count; id++) {
					if ((int)tilemapList[id].lightingCollisionLayer != layer) {
						continue;
					}

					// Tilemap Shadow
					LightingBufferTilemapRectangle.Shadow(buffer, tilemapList[id], lightSizeSquared, z);
				}
			#endif 

		}

		if (drawMask) {

			if (colliderList.Count > 0) {
				for(int id = 0; id < colliderList.Count; id++) {
					if ((int)colliderList[id].lightingMaskLayer != layer) {
						continue;
					}

					// Collider Shape Mask
					LightingBufferShape.Mask(buffer, colliderList[id], layerSetting, offset, z);

					// Collider Sprite Mask
					LightingBufferSprite.MaskWithAtlas(buffer, colliderList[id], offset, z);
				}
			}
			
			#if UNITY_2018_1_OR_NEWER		
				for(int id = 0; id < tilemapList.Count; id++) {
					if ((int)tilemapList[id].lightingMaskLayer != layer) {
						continue;
					}

					// Tilemap Shape Mask
					LightingBufferTilemapRectangle.MaskShape(buffer, tilemapList[id], offset, z);

					// Tilemap Sprite Mask
					LightingBufferTilemapRectangle.MaskSpriteWithAtlas(buffer, tilemapList[id], offset, z);
				}
			#endif
		}
		
		GL.End();

		// Partialy Batched (Default Edition)
		if (buffer.partiallyBatchedList_Collider.Count > 0) {
			Material materialWhite = manager.materials.GetWhiteSprite();
			Material materialBlack = manager.materials.GetBlackSprite();
			Material material;

			PartiallyBatched_Collider batch;

			materialWhite.mainTexture = SpriteAtlasManager.Get().atlasTexture.texture;
			materialWhite.SetPass (0);

			for(int i = 0; i < buffer.partiallyBatchedList_Collider.Count; i++) {
				batch = buffer.partiallyBatchedList_Collider[i];

				material = materialWhite;

				if (maskEffect && colliderList[i].transform.position.y < buffer.lightSource.transform.position.y) {
					material = materialBlack;
				}

				LightingBufferSprite.MaskWithoutAtlas(buffer, batch.collider2D, material, offset, z);
			}

			materialWhite.mainTexture = null;

			buffer.partiallyBatchedList_Collider.Clear();
		}

		if (buffer.partiallyBatchedList_Tilemap.Count > 0) {
			Material materialWhite = manager.materials.GetWhiteSprite();
			Material materialBlack = manager.materials.GetBlackSprite();
			PartiallyBatched_Tilemap batch;
			Material material;

			// Draw Each Partialy Batched
			for(int i = 0; i < buffer.partiallyBatchedList_Tilemap.Count; i++) {
				batch = buffer.partiallyBatchedList_Tilemap[i];

				material = materialWhite;

				if (maskEffect && batch.polyOffset.y < 0) {
					material = materialBlack;
				}

				material.mainTexture = batch.virtualSpriteRenderer.sprite.texture;

				Max2D.DrawSprite(material, batch.virtualSpriteRenderer, batch.polyOffset, batch.tileSize, 0, z);
				
				material.mainTexture = null;
			}
			
			buffer.partiallyBatchedList_Tilemap.Clear();
		}
	}
}