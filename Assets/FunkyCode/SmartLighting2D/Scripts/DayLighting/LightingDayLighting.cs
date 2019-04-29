using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingDayLighting {
	const float uv0 = 0;
	const float uv1 = 1;
	const float pi2 = Mathf.PI / 2;
    
    public static void Draw(Vector2D offset, float z) {
		DrawColliders(offset, z);

		DrawTilemaps(offset, z);

		DrawTint();

		DrawCollidersMasking(offset, z);
	}

	static public void DrawTint() {
		float ratio = (float)Screen.width / Screen.height;
		Camera camera = LightingManager2D.Get().GetCamera();
        Camera bufferCamera = LightingMainBuffer2D.Get().bufferCamera;

		Vector2 size = new Vector2(bufferCamera.orthographicSize * ratio, bufferCamera.orthographicSize);
		Vector3 pos = camera.transform.position;

		LightingManager2D manager = LightingManager2D.Get();

		Max2D.DrawImage(manager.materials.GetAdditive(), new Vector2D(pos), new Vector2D(size), pos.z);
	}

	static public void DrawCollidersMasking(Vector2D offset, float z) {
		LightingManager2D manager = LightingManager2D.Get();

		foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
			if (id.generateDayMask == false) {
				continue;
			}

			// Distance Check?
			if (id.InCamera() == false) {
				continue;
			}

			switch(id.shape.maskType) {
				case LightingCollider2D.MaskType.SpriteCustomPhysicsShape:
					Max2D.SetColor (Color.white);
					Max2D.DrawMesh (Max2D.defaultMaterial, id.shape.GetMesh_Shape(), id.transform, offset, z);

				break;

				case LightingCollider2D.MaskType.Collider:
					Max2D.SetColor (Color.white);
					Max2D.DrawMesh (Max2D.defaultMaterial, id.shape.GetMesh_Collider(id.transform), id.transform, offset, z);

				break;

				case LightingCollider2D.MaskType.Sprite:
					if (id.spriteRenderer == null || id.spriteRenderer.sprite == null) {
						break;
					}
					
					Material material = manager.materials.GetWhiteSprite();
					material.mainTexture = id.spriteRenderer.sprite.texture;

					GL.PushMatrix ();
					
					Max2D.DrawSprite(material, id.spriteRenderer, new Vector2(id.transform.position.x, id.transform.position.y) + offset.ToVector2(), new Vector2(1, 1), id.transform.rotation.eulerAngles.z, z);
					
					GL.PopMatrix ();
				break;
			}
		}
		
	}

	static public void DrawColliders(Vector2D offset, float z) {
		float sunDirection = LightingManager2D.GetSunDirection ();

		LightingManager2D manager = LightingManager2D.Get();

		// Day Soft Shadows
		GL.PushMatrix();
		Max2D.defaultMaterial.SetPass(0);
		GL.Begin(GL.TRIANGLES);
		GL.Color(Color.black);

		Vector3 vecA, vecB, vecC;
		Vector2D zA, zB, zC;
		Vector2D pA, pB;

		foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
			if (id.dayHeight == false || id.height <= 0) {
				continue;
			}

			if (id.shape.colliderType == LightingCollider2D.ColliderType.Mesh) {
				continue;
			}

			// Distance Check?
			if (id.InCamera() == false) {
				continue;
			}

			DayLightingShadowCollider shadow = id.GetDayLightingShadow(sunDirection * Mathf.Rad2Deg);
	
			foreach(Mesh mesh in shadow.meshes) {			
				for (int i = 0; i < mesh.triangles.GetLength (0); i = i + 3) {
					vecA = mesh.vertices [mesh.triangles [i]];
					vecB = mesh.vertices [mesh.triangles [i + 1]];
					vecC = mesh.vertices [mesh.triangles [i + 2]];
					Max2DMatrix.DrawTriangle(vecA.x, vecA.y, vecB.x, vecB.y, vecC.x, vecC.y, offset + new Vector2D(id.transform.position), z);
				}
			}
		}

		GL.End();
		GL.PopMatrix();
		
		GL.PushMatrix ();

		manager.materials.GetShadowBlur().SetPass (0);

		GL.Begin (GL.TRIANGLES);
		Max2D.SetColor (Color.white);

		foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
			if (id.dayHeight == false || id.height <= 0) {
				continue;
			}

			if (id.shape.colliderType == LightingCollider2D.ColliderType.Mesh) {
				continue;
			}

			// Distance Check?
			if (id.InCamera() == false) {
				continue;
			}

			DayLightingShadowCollider shadow = id.GetDayLightingShadow(sunDirection * Mathf.Rad2Deg);

			Vector2D gameObjectOffset = new Vector2D(id.transform.position);
			
			foreach(Polygon2D polygon in shadow.polygons) {
				foreach (DoublePair2D p in DoublePair2D.GetList(polygon.pointsList)) {
					zA = p.A.Copy();
					zA += offset + gameObjectOffset;

					zB = p.B.Copy();
					zB += offset + gameObjectOffset;

					zC = zB.Copy();

					pA = zA.Copy();
					pB = zB.Copy();

					zA.Push (Vector2D.Atan2 (p.A, p.B) + pi2, .5f);
					zB.Push (Vector2D.Atan2 (p.A, p.B) + pi2, .5f);
					zC.Push (Vector2D.Atan2 (p.B, p.C) + pi2, .5f);
					
					GL.TexCoord2 (uv0, uv0);
					Max2D.Vertex3 (pB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pA, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (zA, z);
				
					GL.TexCoord2 (uv0, uv1);
					Max2D.Vertex3 (zA, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (zB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pB, z);
					
					GL.TexCoord2 (uv0, uv1);
					Max2D.Vertex3 (zB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pB, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (zC, z);
				}
			}
		}

		GL.End();
		GL.PopMatrix();
	}

	static public void DrawTilemaps(Vector2D offset, float z) {
		#if UNITY_2018_1_OR_NEWER

		LightingManager2D manager = LightingManager2D.Get();

		GL.PushMatrix();

		Max2D.defaultMaterial.SetPass(0);

		GL.Begin(GL.TRIANGLES);
		GL.Color(Color.black);
	
		// Day Soft Shadows
		foreach (LightingTilemapCollider2D id in LightingTilemapCollider2D.GetList()) {
			if (id.map == null) {
				continue;
			}

			if (id.dayHeight == false) {
				continue;
			}

			Vector2D tilesetOffset = new Vector2D(offset);
			tilesetOffset += new Vector2D(id.area.position.x, id.area.position.y);

			for(int x = 0; x < id.area.size.x; x++) {
				for(int y = 0; y < id.area.size.y; y++) {
					if (id.map[x, y] == null) {
						continue;
					}

					Vector2D tileOffset = tilesetOffset.Copy();
					tileOffset += new Vector2D(x, y); 

					DrawSoftShadowTile(tileOffset, z, id.height);
				}
			}	
		}

		GL.End();
		GL.PopMatrix();

		GL.PushMatrix ();

		manager.materials.GetShadowBlur().SetPass (0);

		GL.Begin (GL.TRIANGLES);
		Max2D.SetColor (Color.white);
		
		// Day Soft Shadows Penumbra
		foreach (LightingTilemapCollider2D id in LightingTilemapCollider2D.GetList()) {
			if (id.map == null) {
				continue;
			}

			if (id.dayHeight == false) {
				continue;
			}

			Vector2D tilesetOffset = new Vector2D(offset);
			tilesetOffset += new Vector2D(id.area.position.x, id.area.position.y);

			for(int x = 0; x < id.area.size.x; x++) {
				for(int y = 0; y < id.area.size.y; y++) {
					if (id.map[x, y] == null) {
						continue;
					}

					Vector2D tileOffset = tilesetOffset.Copy();
					tileOffset += new Vector2D(x , y); 

					DrawSoftShadowTileBlur(tileOffset, z, id.height);	
				}
			}	
		}
	
		GL.End();
		GL.PopMatrix();

		Material materialWhite = manager.materials.GetWhiteSprite();

		// Tilemap Daylighting Masks
		foreach (LightingTilemapCollider2D id in LightingTilemapCollider2D.GetList()) {
			if (id.map == null) {
				continue;
			}

			if (id.dayHeight == false) {
				continue;
			}

			Vector3 rot = Math2D.GetPitchYawRollRad(id.transform.rotation);

			float rotationYScale = Mathf.Sin(rot.x + Mathf.PI / 2);
			float rotationXScale = Mathf.Sin(rot.y + Mathf.PI / 2);

			float scaleX = id.transform.lossyScale.x * rotationXScale * id.cellSize.x;
			float scaleY = id.transform.lossyScale.y * rotationYScale * id.cellSize.y;

			Vector2D tilesetOffset = new Vector2D(offset);
			tilesetOffset += new Vector2D(id.area.position.x, id.area.position.y);
			tilesetOffset += new Vector2D(id.cellAnchor.x,id.cellAnchor.y);

			//GL.PushMatrix ();

			for(int x = 0; x < id.area.size.x; x++) {
				for(int y = 0; y < id.area.size.y; y++) {
					LightingTile tile = id.map[x, y];
					if (tile == null) {
						continue;
					}

					if (tile.GetOriginalSprite() == null) {
						continue;
					}

					VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();
					spriteRenderer.sprite = tile.GetOriginalSprite();

					materialWhite.mainTexture = tile.GetOriginalSprite().texture;

					Vector2D tileOffset = tilesetOffset.Copy();
					tileOffset += new Vector2D(x , y); 


					//Max2D.DrawSprite(materialWhite, spriteRenderer, tileOffset.ToVector2(), new Vector2(scaleX / id.cellSize.x, scaleY / id.cellSize.y), 0, z);
					
					materialWhite.mainTexture = null;				
				}
			}	
		}

		//GL.PopMatrix ();

		#endif
	}

	static void DrawSoftShadowTile(Vector2D offset, float z, float height) {
		float sunDirection = LightingManager2D.GetSunDirection ();

		Polygon2D poly = Polygon2DList.CreateFromRect(new Vector2(1, 1) * 0.5f);
		poly = poly.ToOffset(new Vector2D(0.5f, 0.5f));

		foreach (Pair2D p in Pair2D.GetList(poly.pointsList)) {
			Vector2D vA = p.A.Copy();
			Vector2D vB = p.B.Copy();

			vA.Push (sunDirection, height);
			vB.Push (sunDirection, height);

			Max2DMatrix.DrawTriangle(p.A, p.B, vA, offset, z);
			Max2DMatrix.DrawTriangle(vA, vB, p.B, offset, z);
		}
	}

	static void DrawSoftShadowTileBlur(Vector2D offset, float z, float height) {
		float sunDirection = LightingManager2D.GetSunDirection ();

		Polygon2D poly = Polygon2DList.CreateFromRect(new Vector2(1, 1) * 0.5f);
		offset += new Vector2D(0.5f, 0.5f);
		
		Polygon2D convexHull = Polygon2D.GenerateShadow(new Polygon2D(poly.pointsList), sunDirection, height);
		
		foreach (DoublePair2D p in DoublePair2D.GetList(convexHull.pointsList)) {
			Vector2D zA = new Vector2D (p.A + offset);
			Vector2D zB = new Vector2D (p.B + offset);
			Vector2D zC = zB.Copy();

			Vector2D pA = zA.Copy();
			Vector2D pB = zB.Copy();

			zA.Push (Vector2D.Atan2 (p.A, p.B) + pi2, .5f);
			zB.Push (Vector2D.Atan2 (p.A, p.B) + pi2, .5f);
			zC.Push (Vector2D.Atan2 (p.B, p.C) + pi2, .5f);
			
			GL.TexCoord2 (uv0, uv0);
			Max2D.Vertex3 (pB, z);
			GL.TexCoord2 (0.5f - uv0, uv0);
			Max2D.Vertex3 (pA, z);
			GL.TexCoord2 (0.5f - uv0, uv1);
			Max2D.Vertex3 (zA, z);
		
			GL.TexCoord2 (uv0, uv1);
			Max2D.Vertex3 (zA, z);
			GL.TexCoord2 (0.5f - uv0, uv1);
			Max2D.Vertex3 (zB, z);
			GL.TexCoord2 (0.5f - uv0, uv0);
			Max2D.Vertex3 (pB, z);
			
			GL.TexCoord2 (uv0, uv1);
			Max2D.Vertex3 (zB, z);
			GL.TexCoord2 (0.5f - uv0, uv0);
			Max2D.Vertex3 (pB, z);
			GL.TexCoord2 (0.5f - uv0, uv1);
			Max2D.Vertex3 (zC, z);
		}
	}
}
