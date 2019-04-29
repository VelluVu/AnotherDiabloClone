using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBufferTilemapRectangle {
	#if UNITY_2018_1_OR_NEWER

    static Vector2D zero = Vector2D.Zero();
	static Vector2D vA = Vector2D.Zero(), vB = Vector2D.Zero(), vC = Vector2D.Zero(), vD = Vector2D.Zero();
	static Vector2D pA = Vector2D.Zero(), pB = Vector2D.Zero();

	public static Vector2D offset = Vector2D.Zero();
	public static Vector2D polyOffset = Vector2D.Zero();
	public static Vector2D tilemapOffset = Vector2D.Zero();
	public static Vector2D inverseOffset = Vector2D.Zero();

	static LightingTile tile;
	static Polygon2D poly = null;
	static Pair2D p;
	public static List<Pair2D> pairList;

	static Vector2 tileSize = Vector2.zero;
	static Vector2 scale = Vector2.zero;
	static Vector2 polyOffset2 = Vector2.zero;

	static Sprite penumbraSprite;
	static Rect uvRect = new Rect();
	public static Vector2Int newPositionInt = new Vector2Int();
	static VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();
	static LightingManager2D manager;
	static Vector2D tileSize2 = new Vector2D(1, 1);

	static int sizeInt;

	const float uv0 = 0f;
	const float uv1 = 1f;

	static float uvRectX;
	static float uvRectY;
	static float uvRectWidth;
	static float uvRectHeight;

	static float angleA, angleB;
	static double rot;
	static List<Polygon2D> polygons = null;
	static List<List<Pair2D>> pairList2 = null;
	static Polygon2D polygon;

	static public void Shadow(LightingBuffer2D buffer, LightingTilemapCollider2D id, float lightSizeSquared, float z) {
		if (id.colliderType == LightingTilemapCollider2D.ColliderType.None) {
			return;
		}

		if (id.colliderType == LightingTilemapCollider2D.ColliderType.Collider) {
			// Adjust Color?
			Fill_Component(buffer, id, lightSizeSquared, z);
			Penumbra_Component(buffer, id, lightSizeSquared, z);
			
			return;
		}
		
		if (id.map == null) {
			return;
		}
		
		manager = LightingManager2D.Get();

		SetupLocation(buffer, id);
		CalculatePenumbra();
		
		bool penumbra = manager.drawPenumbra;
		bool drawAbove = buffer.lightSource.whenInsideCollider == LightingSource2D.WhenInsideCollider.DrawAbove;
	
		for(int x = newPositionInt.x - sizeInt; x < newPositionInt.x + sizeInt; x++) {
			for(int y = newPositionInt.y - sizeInt; y < newPositionInt.y + sizeInt; y++) {
				if (x < 0 || y < 0) {
					continue;
				}

				if (x >= id.area.size.x || y >= id.area.size.y) {
					continue;
				}
			
				tile = id.map[x, y];
				if (tile == null) {
					continue;
				}
			
				polygons = tile.GetPolygon(id);

				if (polygons == null || polygons.Count < 1) {
					continue;
				}

				polyOffset.x = x + tilemapOffset.x;
				polyOffset.y = y + tilemapOffset.y;
					
				if (id.mapType == LightingTilemapCollider2D.MapType.SuperTilemapEditor) {
					polyOffset.x -= id.area.size.x / 2;
					polyOffset.y -= id.area.size.y / 2;
				}

				polyOffset.x *= scale.x;
				polyOffset.y *= scale.y;

				polyOffset2.x = (float)polyOffset.x;
				polyOffset2.y = (float)polyOffset.y;

				if (LightingManager2D.culling && Vector2.Distance(polyOffset2, buffer.lightSource.transform.position) > 2 + buffer.lightSource.lightSize) {
					LightingDebug.culled ++;
					continue;
				}

				polyOffset.x += offset.x;
				polyOffset.y += offset.y;
			
				inverseOffset.x = -polyOffset.x;
				inverseOffset.y = -polyOffset.y;

				pairList2 = tile.GetPairs(id);

				for(int i = 0; i < polygons.Count; i++) {
					polygon = polygons[i];

					if (drawAbove && polygon.PointInPoly (inverseOffset)) {
						continue;
					}
					
					LightingDebug.shadowGenerations ++;

					pairList = pairList2[i];

					GL.Color(Color.black);
					for(int e = 0; e < pairList.Count; e++) {
						p = pairList[e];
						
						vA.x = p.A.x + polyOffset.x;
						vA.y = p.A.y + polyOffset.y;

						vB.x = p.B.x + polyOffset.x;
						vB.y = p.B.y + polyOffset.y;

						vC.x = p.A.x + polyOffset.x;
						vC.y = p.A.y + polyOffset.y;

						vD.x = p.B.x + polyOffset.x;
						vD.y = p.B.y + polyOffset.y;
						
						rot = System.Math.Atan2 (vA.y - zero.y, vA.x - zero.x);
						vA.x += System.Math.Cos(rot) * lightSizeSquared;
						vA.y += System.Math.Sin(rot) * lightSizeSquared;

						rot = System.Math.Atan2 (vB.y - zero.y, vB.x - zero.x);
						vB.x += System.Math.Cos(rot) * lightSizeSquared;
						vB.y += System.Math.Sin(rot) * lightSizeSquared;

						GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
						GL.Vertex3((float)vC.x, (float)vC.y, z);

						GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
						GL.Vertex3((float)vD.x, (float)vD.y, z);

						GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
						GL.Vertex3((float)vA.x, (float)vA.y, z);


						GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
						GL.Vertex3((float)vA.x, (float)vA.y, z);

						GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
						GL.Vertex3((float)vB.x, (float)vB.y, z);

						GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
						GL.Vertex3((float)vD.x, (float)vD.y, z);
					}	

					if (penumbra) {
						GL.Color(Color.white);
						for(int e = 0; e < pairList.Count; e++) {
							p = pairList[e];

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

							angleA = (float)System.Math.Atan2 (vA.y - zero.y, vA.x - zero.x);
							angleB = (float)System.Math.Atan2 (vB.y - zero.y, vB.x - zero.x);

							vA.x += System.Math.Cos(angleA) * lightSizeSquared;
							vA.y += System.Math.Sin(angleA) * lightSizeSquared;

							vB.x += System.Math.Cos(angleB) * lightSizeSquared;
							vB.y += System.Math.Sin(angleB) * lightSizeSquared;

							rot = angleA - Mathf.Deg2Rad * buffer.lightSource.occlusionSize;
							pA.x += System.Math.Cos(rot) * lightSizeSquared;
							pA.y += System.Math.Sin(rot) * lightSizeSquared;

							rot = angleB + Mathf.Deg2Rad * buffer.lightSource.occlusionSize;
							pB.x += System.Math.Cos(rot) * lightSizeSquared;
							pB.y += System.Math.Sin(rot) * lightSizeSquared;
						
							GL.TexCoord3(uvRectX, uvRectY, 0);
							GL.Vertex3((float)vC.x,(float)vC.y, z);

							GL.TexCoord3(uvRectWidth, uvRectHeight, 0);
							GL.Vertex3((float)vA.x, (float)vA.y, z);
							
							GL.TexCoord3((float)uvRectX, uvRectHeight, 0);
							GL.Vertex3((float)pA.x,(float)pA.y, z);


							GL.TexCoord3(uvRectX, uvRectY, 0);
							GL.Vertex3((float)vD.x,(float)vD.y, z);

							GL.TexCoord3(uvRectWidth, uvRectY, 0);
							GL.Vertex3((float)vB.x, (float)vB.y, z);
							
							GL.TexCoord3(uvRectX, uvRectHeight, 0);
							GL.Vertex3((float)pB.x, (float)pB.y, z);
						}
					}
				}
			}
		}
	}

	static public void MaskSpriteWithAtlas(LightingBuffer2D buffer, LightingTilemapCollider2D id, Vector2D offset, float z) {
		if (id.maskType != LightingTilemapCollider2D.MaskType.Sprite) {
			return;
		}
		
		if (id.map == null) {
			return;
		}

		SetupLocation(buffer, id);

		Sprite reqSprite;
		PartiallyBatched_Tilemap batched;

		for(int x = newPositionInt.x - sizeInt; x < newPositionInt.x + sizeInt; x++) {
			for(int y = newPositionInt.y - sizeInt; y < newPositionInt.y + sizeInt; y++) {
				if (x < 0 || y < 0) {
					continue;
				}

				if (x >= id.area.size.x || y >= id.area.size.y) {
					continue;
				}

				tile = id.map[x, y];
				if (tile == null) {
					continue;
				}

				if (tile.GetOriginalSprite() == null) {
					continue;
				}

				polyOffset.x = x + tilemapOffset.x;
				polyOffset.y = y + tilemapOffset.y;

				polyOffset.x *= scale.x;
				polyOffset.y *= scale.y;

				polyOffset2.x = (float)polyOffset.x;
				polyOffset2.y = (float)polyOffset.y;

				if (LightingManager2D.culling && Vector2.Distance(polyOffset2, buffer.lightSource.transform.position) > 2 + buffer.lightSource.lightSize) {
					LightingDebug.culled ++;
					continue;
				}

				polyOffset.x += offset.x;
				polyOffset.y += offset.y;
				
				spriteRenderer.sprite = tile.GetAtlasSprite();

				if (spriteRenderer.sprite == null) {
					reqSprite = SpriteAtlasManager.RequestSprite(tile.GetOriginalSprite(), SpriteRequest.Type.WhiteMask);
					if (reqSprite == null) {
						// Add Partialy Batched
						batched = new PartiallyBatched_Tilemap();

						batched.virtualSpriteRenderer = new VirtualSpriteRenderer();
						batched.virtualSpriteRenderer.sprite = tile.GetOriginalSprite();

						batched.polyOffset = polyOffset.ToVector2();

						batched.tileSize = tileSize;

						buffer.partiallyBatchedList_Tilemap.Add(batched);
						continue;
					} else {
						tile.SetAtlasSprite(reqSprite);
						spriteRenderer.sprite = reqSprite;
					}
				}

				polyOffset2.x = (float)polyOffset.x;
				polyOffset2.y = (float)polyOffset.y;

				Max2D.DrawSpriteBatched_Tris(spriteRenderer, buffer.lightSource.layerSetting[0], id.maskMode, polyOffset2, tileSize, 0, z);
				
				LightingDebug.maskGenerations ++;
			}	
		}
	}

	static public void MaskSpriteWithoutAtlas(LightingBuffer2D buffer, LightingTilemapCollider2D id, Material materialA, Material materialB, Vector2D offset, float z) {
		if (id.maskType != LightingTilemapCollider2D.MaskType.Sprite) {
			return;
		}
		
		if (id.map == null) {
			return;
		}

		Material material;

		SetupLocation(buffer, id);

		bool maskEffect = (buffer.lightSource.layerSetting[0].effect == LightingLayerEffect.InvisibleBellow);
		bool invisible = (id.maskMode == LightingMaskMode.Invisible);

		for(int x = newPositionInt.x - sizeInt; x < newPositionInt.x + sizeInt; x++) {
			for(int y = newPositionInt.y - sizeInt; y < newPositionInt.y + sizeInt; y++) {
				if (x < 0 || y < 0) {
					continue;
				}

				if (x >= id.area.size.x || y >= id.area.size.y) {
					continue;
				}

				tile = id.map[x, y];
				if (tile == null) {
					continue;
				}

				if (tile.GetOriginalSprite() == null) {
					return;
				}

				polyOffset.x = x + tilemapOffset.x;
				polyOffset.y = y + tilemapOffset.y;

				polyOffset.x *= scale.x;
				polyOffset.y *= scale.y;

				polyOffset2.x = (float)polyOffset.x;
				polyOffset2.y = (float)polyOffset.y;
				
				if (LightingManager2D.culling && Vector2.Distance(polyOffset2, buffer.lightSource.transform.position) > 2 + buffer.lightSource.lightSize) {
					LightingDebug.culled ++;
					continue;
				}

				polyOffset.x += offset.x;
				polyOffset.y += offset.y;

				spriteRenderer.sprite = tile.GetOriginalSprite();

				polyOffset2.x = (float)polyOffset.x;
				polyOffset2.y = (float)polyOffset.y;

				if (invisible || (maskEffect && polyOffset2.y < 0)) {
					material = materialB;
				} else {
					material = materialA;
				}
				
				material.mainTexture = spriteRenderer.sprite.texture;
	
				Max2D.DrawSprite(material, spriteRenderer, polyOffset2, tileSize, 0, z);
				
				material.mainTexture = null;

				LightingDebug.maskGenerations ++;
			}	
		}
	}

	static public void MaskShape(LightingBuffer2D buffer, LightingTilemapCollider2D id, Vector2D offset, float z) {
		if (id.maskType == LightingTilemapCollider2D.MaskType.SpriteCustomPhysicsShape || id.maskType == LightingTilemapCollider2D.MaskType.Tile) {
		} else {
			return;
		}

		if (id.map == null) {
			return;
		}

		SetupLocation(buffer, id);

		Vector2 vecA, vecB, vecC;

		LightingTile tile;
		Mesh tileMesh = null;	

		int triangleCount;

		tileSize2.x = 1;
		tileSize2.y = 1;

		if (id.maskType == LightingTilemapCollider2D.MaskType.Tile) {
			tileMesh = LightingTile.GetStaticTileMesh();
		}

		for(int x = newPositionInt.x - sizeInt; x < newPositionInt.x + sizeInt; x++) {
			for(int y = newPositionInt.y - sizeInt; y < newPositionInt.y + sizeInt; y++) {
				if (x < 0 || y < 0) {
					continue;
				}

				if (x >= id.area.size.x || y >= id.area.size.y) {
					continue;
				}

				tile = id.map[x, y];
				if (tile == null) {
					continue;
				}
				
				polyOffset.x = x + tilemapOffset.x;
				polyOffset.y = y + tilemapOffset.y;

				polyOffset.x *= scale.x;
				polyOffset.y *= scale.y;

				polyOffset2.x = (float)polyOffset.x;
				polyOffset2.y = (float)polyOffset.y;
				
				if (LightingManager2D.culling && Vector2.Distance(polyOffset2, buffer.lightSource.transform.position) > (id.cellSize.x * 2f) + buffer.lightSource.lightSize) {
					LightingDebug.culled ++;
					continue;
				}

				polyOffset.x += offset.x;
				polyOffset.y += offset.y;

				if (id.maskType == LightingTilemapCollider2D.MaskType.SpriteCustomPhysicsShape) {
					tileMesh = null;
					tileMesh = tile.GetTileDynamicMesh();
				}

				if (tileMesh == null) {
					continue;
				}

				polyOffset2.x = (float)polyOffset.x;
				polyOffset2.y = (float)polyOffset.y;

				// Batch and Optimize???
				triangleCount = tileMesh.triangles.GetLength (0);
				for (int i = 0; i < triangleCount; i = i + 3) {
					vecA = tileMesh.vertices [tileMesh.triangles [i]];
					vecB = tileMesh.vertices [tileMesh.triangles [i + 1]];
					vecC = tileMesh.vertices [tileMesh.triangles [i + 2]];
					Max2DMatrix.DrawTriangle(vecA, vecB, vecC, polyOffset2, z, tileSize2);
				}		
				LightingDebug.maskGenerations ++;				
			}
		}
	}
	




















	public static void CalculatePenumbra() {
		penumbraSprite = manager.materials.GetAtlasPenumbraSprite();

		uvRect.x = penumbraSprite.rect.x / penumbraSprite.texture.width;
		uvRect.y = penumbraSprite.rect.y / penumbraSprite.texture.height;
		uvRect.width = penumbraSprite.rect.width / penumbraSprite.texture.width;
		uvRect.height = penumbraSprite.rect.height / penumbraSprite.texture.height;
		
		uvRect.width += uvRect.x;
		uvRect.height += uvRect.y;

		uvRect.x += 1f / 2048;
		uvRect.y += 1f / 2048;
		uvRect.width -= 1f / 2048;
		uvRect.height -= 1f / 2048;

		uvRectX = uvRect.x;
		uvRectY = uvRect.y;
		uvRectWidth = uvRect.width;
		uvRectHeight = uvRect.height;
	}
	
	static public void SetupLocation(LightingBuffer2D buffer, LightingTilemapCollider2D id) {
		Vector3 rot = Math2D.GetPitchYawRollRad(id.transform.rotation);

		float rotationYScale = Mathf.Sin(rot.x + Mathf.PI / 2);
		float rotationXScale = Mathf.Sin(rot.y + Mathf.PI / 2);

		scale.x = id.transform.lossyScale.x * rotationXScale * id.cellSize.x;
		scale.y = id.transform.lossyScale.y * rotationYScale * id.cellSize.y;

		sizeInt = LightTilemapSize(id, buffer);

		LightTilemapOffset(id, scale, buffer);
		
		offset.x = -buffer.lightSource.transform.position.x;
		offset.y = -buffer.lightSource.transform.position.y;

		tilemapOffset.x = id.transform.position.x + id.area.position.x + id.cellAnchor.x;
		tilemapOffset.y = id.transform.position.y + id.area.position.y + id.cellAnchor.y;

		if (id.mapType == LightingTilemapCollider2D.MapType.SuperTilemapEditor) {
			tilemapOffset.x -= id.area.size.x / 2;
			tilemapOffset.y -= id.area.size.y / 2;
		}

		tileSize.x = scale.x / id.cellSize.x;
		tileSize.y = scale.y / id.cellSize.y;
	}

	static public int LightTilemapSize(LightingTilemapCollider2D id, LightingBuffer2D buffer) {
		if (id.mapType == LightingTilemapCollider2D.MapType.SuperTilemapEditor) {
			return((int)buffer.lightSource.lightSize);
		} else {
			return((int)buffer.lightSource.lightSize + 1);
		}
	}

	static public void LightTilemapOffset(LightingTilemapCollider2D id, Vector2 scale, LightingBuffer2D buffer) {
		Vector2 newPosition = buffer.lightSource.transform.position;

		newPosition.x -= id.area.position.x;
		newPosition.y -= id.area.position.y;
		
		newPosition.x -= id.transform.position.x;
		newPosition.y -= id.transform.position.y;
			
		newPosition.x -= id.cellAnchor.x;
		newPosition.y -= id.cellAnchor.y;

		// Cell Size Is Not Calculated Correctly

		if (id.mapType == LightingTilemapCollider2D.MapType.SuperTilemapEditor) {
			newPosition.x += id.area.size.x / 2;
			newPosition.y += id.area.size.y / 2;
		} else {
			newPosition.x += 1;
			newPosition.y += 1;
		}

		newPosition.x *= scale.x;
		newPosition.y *= scale.y;

		newPositionInt.x = (int)newPosition.x;
		newPositionInt.y = (int)newPosition.y;
	}

	





















	static public void Fill_Component(LightingBuffer2D buffer, LightingTilemapCollider2D id, float lightSizeSquared, float z) {
		Vector2D vA, vB;
		foreach(Polygon2D polygon in id.edgeColliders) {
			Vector2D polyOffset = new Vector2D (-buffer.lightSource.transform.position);
		
			Polygon2D poly = polygon.Copy();
			poly.ToOffsetItself(polyOffset);

			foreach (Pair2D p in Pair2D.GetList(poly.pointsList, false)) {
				vA = p.A.Copy();
				vB = p.B.Copy();
				
				vA.Push (Vector2D.Atan2 (vA, zero), lightSizeSquared);
				vB.Push (Vector2D.Atan2 (vB, zero), lightSizeSquared);
				
				Max2DMatrix.DrawTriangle(p.A ,p.B, vA, zero, z);
				Max2DMatrix.DrawTriangle(vA, vB, p.B, zero, z);
			}	
			
			LightingDebug.shadowGenerations ++;
		}

		foreach(Polygon2D polygon in id.polygonColliders) {
			Vector2D polyOffset = new Vector2D (-buffer.lightSource.transform.position);
		
			Polygon2D poly = polygon.Copy();
			poly.ToOffsetItself(polyOffset);

			foreach (Pair2D p in Pair2D.GetList(poly.pointsList)) {
				vA = p.A.Copy();
				vB = p.B.Copy();
				
				vA.Push (Vector2D.Atan2 (vA, zero), lightSizeSquared);
				vB.Push (Vector2D.Atan2 (vB, zero), lightSizeSquared);
				
				Max2DMatrix.DrawTriangle(p.A ,p.B, vA, zero, z);
				Max2DMatrix.DrawTriangle(vA, vB, p.B, zero, z);
			}	
			
			LightingDebug.shadowGenerations ++;
		} 
	}
	
	
	public static void Penumbra_Component(LightingBuffer2D buffer, LightingTilemapCollider2D id, float lightSizeSquared, float z) {
		Vector2D vA, pA, vB, pB;

		foreach(Polygon2D polygon in id.edgeColliders) {
			Vector2D polyOffset = new Vector2D (-buffer.lightSource.transform.position);
		
			Polygon2D poly = polygon.Copy();
			poly.ToOffsetItself(polyOffset);

			foreach (Pair2D p in Pair2D.GetList(poly.pointsList, false)) {
				vA = p.A.Copy();
				pA = p.A.Copy();

				vB = p.B.Copy();
				pB = p.B.Copy();

				float angleA = (float)Vector2D.Atan2 (vA, zero);
				float angleB = (float)Vector2D.Atan2 (vB, zero);

				vA.Push (angleA, lightSizeSquared);
				pA.Push (angleA - Mathf.Deg2Rad * buffer.lightSource.occlusionSize, lightSizeSquared);

				vB.Push (angleB, lightSizeSquared);
				pB.Push (angleB + Mathf.Deg2Rad * buffer.lightSource.occlusionSize, lightSizeSquared);

				GL.TexCoord2(uv0, uv0);
				GL.Vertex3((float)p.A.x,(float) p.A.y, z);
				GL.TexCoord2(uv1, uv0);
				GL.Vertex3((float)vA.x, (float)vA.y, z);
				GL.TexCoord2((float)uv0, uv1);
				GL.Vertex3((float)pA.x,(float) pA.y, z);

				GL.TexCoord2(uv0, uv0);
				GL.Vertex3((float)p.B.x,(float) p.B.y, z);
				GL.TexCoord2(uv1, uv0);
				GL.Vertex3((float)vB.x, (float)vB.y, z);
				GL.TexCoord2(uv0, uv1);
				GL.Vertex3((float)pB.x, (float)pB.y, z);
			}
			//LightingDebug.penumbraGenerations ++; 
			
		}

		foreach(Polygon2D polygon in id.polygonColliders) {
			Vector2D polyOffset = new Vector2D (-buffer.lightSource.transform.position);
		
			Polygon2D poly = polygon.Copy();
			poly.ToOffsetItself(polyOffset);

			foreach (Pair2D p in Pair2D.GetList(poly.pointsList)) {
				vA = p.A.Copy();
				pA = p.A.Copy();

				vB = p.B.Copy();
				pB = p.B.Copy();

				float angleA = (float)Vector2D.Atan2 (vA, zero);
				float angleB = (float)Vector2D.Atan2 (vB, zero);

				vA.Push (angleA, lightSizeSquared);
				pA.Push (angleA - Mathf.Deg2Rad * buffer.lightSource.occlusionSize, lightSizeSquared);

				vB.Push (angleB, lightSizeSquared);
				pB.Push (angleB + Mathf.Deg2Rad * buffer.lightSource.occlusionSize, lightSizeSquared);

				GL.TexCoord2(uv0, uv0);
				GL.Vertex3((float)p.A.x,(float) p.A.y, z);
				GL.TexCoord2(uv1, uv0);
				GL.Vertex3((float)vA.x, (float)vA.y, z);
				GL.TexCoord2((float)uv0, uv1);
				GL.Vertex3((float)pA.x,(float) pA.y, z);

				GL.TexCoord2(uv0, uv0);
				GL.Vertex3((float)p.B.x,(float) p.B.y, z);
				GL.TexCoord2(uv1, uv0);
				GL.Vertex3((float)vB.x, (float)vB.y, z);
				GL.TexCoord2(uv0, uv1);
				GL.Vertex3((float)pB.x, (float)pB.y, z);
			}
			//LightingDebug.penumbraGenerations ++;
		}
	}

	#endif
	
}
