using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used When Sorting Objects
public class LightingBufferTile {
    static Vector2D vA = Vector2D.Zero(), vB = Vector2D.Zero(), pA = Vector2D.Zero(), pB = Vector2D.Zero();
    static Vector2D vC = Vector2D.Zero(), vD = Vector2D.Zero();
    static Vector3 vecA, vecB, vecC;

	static public void DrawShadow(LightingBuffer2D buffer, LightingTile tile, Vector2D polyOffset, LightingTilemapCollider2D tilemap, float lightSizeSquared, float z) {
		if (tile.GetPairs(tilemap).Count < 1) {
			return;
		}

		LightingBufferTilemapRectangle.pairList = tile.GetPairs(tilemap)[0];

		Pair2D p;

		Vector2D zero = Vector2D.Zero();

		GL.Color(Color.black);

		for (int s = 0; s < LightingBufferTilemapRectangle.pairList.Count; s++) {
			p = LightingBufferTilemapRectangle.pairList[s];
			
			vA.x = p.A.x + polyOffset.x;
			vA.y = p.A.y + polyOffset.y;

			vB.x = p.B.x + polyOffset.x;
			vB.y = p.B.y + polyOffset.y;

			vC.x = p.A.x + polyOffset.x;
			vC.y = p.A.y + polyOffset.y;

			vD.x = p.B.x + polyOffset.x;
			vD.y = p.B.y + polyOffset.y;
			
			vA.Push (Vector2D.Atan2 (vA, zero), lightSizeSquared);
			vB.Push (Vector2D.Atan2 (vB, zero), lightSizeSquared);
			
			Max2DMatrix.DrawTriangle(vC ,vD, vA, zero, z);
			Max2DMatrix.DrawTriangle(vA, vB, vD, zero, z);
		}	

		Sprite penumbraSprite = LightingManager2D.Get().materials.GetAtlasPenumbraSprite();

		float angleA, angleB;
			
		Rect uvRect = new Rect((float)penumbraSprite.rect.x / penumbraSprite.texture.width, (float)penumbraSprite.rect.y / penumbraSprite.texture.height, (float)penumbraSprite.rect.width / penumbraSprite.texture.width , (float)penumbraSprite.rect.height / penumbraSprite.texture.height);
		
		uvRect.width += uvRect.x;
		uvRect.height += uvRect.y;

		uvRect.x += 1f / 2048;
		uvRect.y += 1f / 2048;
		uvRect.width -= 1f / 2048;
		uvRect.height -= 1f / 2048;

		GL.Color(Color.white);

		for(int s = 0; s <  LightingBufferTilemapRectangle.pairList.Count; s++) {
			p =  LightingBufferTilemapRectangle.pairList[s];

			vA.x = p.A.x + polyOffset.x;
			vA.y = p.A.y + polyOffset.y;

			pA.x = p.A.x + polyOffset.x;
			pA.y = p.A.y + polyOffset.y;

			pB.x = p.B.x + polyOffset.x;
			pB.y = p.B.y + polyOffset.y;

			vB.x = p.B.x + polyOffset.x;
			vB.y = p.B.y + polyOffset.y;

			vC.x = p.A.x + polyOffset.x;
			vC.y = p.A.y + polyOffset.y;

			vD.x = p.B.x + polyOffset.x;
			vD.y = p.B.y + polyOffset.y;

			angleA = (float)Vector2D.Atan2 (vA, zero);
			angleB = (float)Vector2D.Atan2 (vB, zero);

			vA.Push (angleA, lightSizeSquared);
			pA.Push (angleA - Mathf.Deg2Rad * buffer.lightSource.occlusionSize, lightSizeSquared);

			vB.Push (angleB, lightSizeSquared);
			pB.Push (angleB + Mathf.Deg2Rad * buffer.lightSource.occlusionSize, lightSizeSquared);

			GL.TexCoord2(uvRect.x, uvRect.y);
			GL.Vertex3((float)vC.x,(float)vC.y, z);

			GL.TexCoord2(uvRect.width, uvRect.y);
			GL.Vertex3((float)vA.x, (float)vA.y, z);
			
			GL.TexCoord2((float)uvRect.x, uvRect.height);
			GL.Vertex3((float)pA.x,(float)pA.y, z);

			GL.TexCoord2(uvRect.x, uvRect.y);
			GL.Vertex3((float)vD.x,(float)vD.y, z);

			GL.TexCoord2(uvRect.width, uvRect.y);
			GL.Vertex3((float)vB.x, (float)vB.y, z);
			
			GL.TexCoord2(uvRect.x, uvRect.height);
			GL.Vertex3((float)pB.x, (float)pB.y, z);
		}

		LightingDebug.shadowGenerations ++;
	}
	
	// Lighting Buffer TILE
	static public void MaskShapeDepthWithAtlas(LightingBuffer2D buffer, LightingTile tile, LightRenderingOrder lightSourceOrder, LightingTilemapCollider2D id, Vector2D offset, float z) {
		Mesh tileMesh = null;

		if (id.maskType == LightingTilemapCollider2D.MaskType.Tile) {
			tileMesh = LightingTile.GetStaticTileMesh();
		} else if (id.maskType == LightingTilemapCollider2D.MaskType.SpriteCustomPhysicsShape) {
			tileMesh = tile.GetTileDynamicMesh();
		}

		if (tileMesh == null) {
			return;
		}

		// Set Color Black Or White?
		GL.Color(Color.white);
		
		int triangleCount = tileMesh.triangles.GetLength (0);
		for (int i = 0; i < triangleCount; i = i + 3) {
			vecA = tileMesh.vertices [tileMesh.triangles [i]];
			vecB = tileMesh.vertices [tileMesh.triangles [i + 1]];
			vecC = tileMesh.vertices [tileMesh.triangles [i + 2]];
			Max2DMatrix.DrawTriangle(vecA, vecB, vecC, offset.ToVector2(), z, new Vector2D(1, 1));
		}

		LightingDebug.maskGenerations ++;			
	}

	static public void MaskSpriteDepthWithAtlas(LightingBuffer2D buffer, LightingTile tile, LayerSetting layerSetting, LightingTilemapCollider2D id, Vector2D offset, float z) {
		if (id.maskType != LightingTilemapCollider2D.MaskType.Sprite) {
			return;
		}

		if (id.maskType == LightingTilemapCollider2D.MaskType.None) {
			return;
		}

		if (tile.GetOriginalSprite() == null) {
			return;
		}

		Sprite sprite = tile.GetAtlasSprite();

		Vector2 scale = new Vector2(1, 1);

		if (sprite == null) {
			Sprite reqSprite = SpriteAtlasManager.RequestSprite(tile.GetOriginalSprite(), SpriteRequest.Type.WhiteMask);
			if (reqSprite == null) {
				PartiallyBatched_Tilemap batched = new PartiallyBatched_Tilemap();

				batched.virtualSpriteRenderer = new VirtualSpriteRenderer();
				batched.virtualSpriteRenderer.sprite = tile.GetOriginalSprite();

				batched.polyOffset = offset.ToVector2();

				batched.tileSize = scale;

				batched.tilemap = id;

				buffer.partiallyBatchedList_Tilemap.Add(batched);
				return;
			} else {
				tile.SetAtlasSprite(reqSprite);
				sprite = reqSprite;
			}
		}

		VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();
		spriteRenderer.sprite = sprite;

		Max2D.DrawSpriteBatched_Tris(spriteRenderer, layerSetting, id.maskMode, offset.ToVector2(), scale, id.transform.rotation.eulerAngles.z, z);

		LightingDebug.maskGenerations ++;		
	}

	static public void DrawMask(LightingBuffer2D buffer, LightingTile tile, LightingLayerEffect maskEffect, Material materialA, Material materialB, Vector2D polyOffset, LightingTilemapCollider2D tilemap, float lightSizeSquared, float z) {
		VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();
		
		spriteRenderer.sprite = tile.GetOriginalSprite();

		if (spriteRenderer.sprite == null) {
			return;
		}

		Vector3 rot = Math2D.GetPitchYawRollRad(tilemap.transform.rotation);

		float rotationYScale = Mathf.Sin(rot.x + Mathf.PI / 2);
		float rotationXScale = Mathf.Sin(rot.y + Mathf.PI / 2);

		float scaleX = tilemap.transform.lossyScale.x * rotationXScale * tilemap.cellSize.x;
		float scaleY = tilemap.transform.lossyScale.y * rotationYScale * tilemap.cellSize.y;

		Vector2 tileSize = new Vector2(scaleX / tilemap.cellSize.x, scaleY / tilemap.cellSize.y);

		Material material = materialA;
		if (tilemap.maskMode == LightingMaskMode.Invisible || (maskEffect == LightingLayerEffect.InvisibleBellow && polyOffset.y < 0)) {
			material = materialB;
		}
		
		material.mainTexture = spriteRenderer.sprite.texture;

		Max2D.DrawSprite(material, spriteRenderer, polyOffset.ToVector2(), tileSize, 0, z);
		
		material.mainTexture = null;

		LightingDebug.maskGenerations ++;
	}
}
