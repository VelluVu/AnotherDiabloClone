using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBufferShape {
    static Vector2D zero = Vector2D.Zero();

	const float uv0 = 0f;
	const float uv1 = 1f;

	static List<Polygon2D> polygons = null;
	static List<List<Pair2D>> polygonPairs = null;
	static List<Pair2D> pairList = null;

	static Vector2D vA = Vector2D.Zero(), pA = Vector2D.Zero(), vB = Vector2D.Zero(), pB = Vector2D.Zero(), vC = Vector2D.Zero(), vD = Vector2D.Zero();
	static Vector2D inverseOffset = Vector2D.Zero();
	
	static Sprite penumbraSprite;

	static Pair2D p;
	static float angleA, angleB;

	static Rect uvRect = new Rect();

	static VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();

	static float uvRectX = uvRect.x;
	static float uvRectY = uvRect.y;
	static float uvRectWidth = uvRect.width;
	static float uvRectHeight = uvRect.height;

	static Color color = Color.white;

	static double rot;

    public static void Shadow(LightingBuffer2D buffer, LightingCollider2D id, float lightSizeSquared, float z, Vector2D offset) {
		if (false == (id.shape.colliderType == LightingCollider2D.ColliderType.Collider || id.shape.colliderType == LightingCollider2D.ColliderType.SpriteCustomPhysicsShape || id.shape.colliderType == LightingCollider2D.ColliderType.Mesh)) {	
			return;
		}
		
		if (id.isVisibleForLight(buffer) == false) {
			return;
		}

		spriteRenderer.sprite = id.shape.GetOriginalSprite();

		if (id.spriteRenderer != null) {
			spriteRenderer.flipX = id.spriteRenderer.flipX;
			spriteRenderer.flipY = id.spriteRenderer.flipY;
		} else {
			spriteRenderer.flipX = false;
			spriteRenderer.flipY = false;
		}
		
		polygons = id.shape.GetPolygons_World_ColliderType(id.transform, spriteRenderer);
		
		if (polygons.Count < 1) {
			return;
		}

		polygonPairs = id.shape.GetPolygons_Pair_World_ColliderType(id.transform, spriteRenderer);
		
		inverseOffset.x = -offset.x;
		inverseOffset.y = -offset.y;

		CalculatePenumbra();

		bool penumbra = LightingManager2D.Get().drawPenumbra;
		bool drawAbove = buffer.lightSource.whenInsideCollider == LightingSource2D.WhenInsideCollider.DrawAbove;

		// Draw Inside Collider Works Fine?
		for(int i = 0; i < polygons.Count; i++) {

			if (drawAbove && polygons[i].PointInPoly (inverseOffset)) {
				continue;
			}

			LightingDebug.shadowGenerations ++;

			pairList = polygonPairs[i];
			
			if (penumbra) {
				GL.Color(Color.white);
				for(int x = 0; x < pairList.Count; x++) {
					p = pairList[x];
					
					vA.x = p.A.x + offset.x;
					vA.y = p.A.y + offset.y;

					pA.x = p.A.x + offset.x;
					pA.y = p.A.y + offset.y;

					vB.x = p.B.x + offset.x;
					vB.y = p.B.y + offset.y;

					pB.x = p.B.x + offset.x;
					pB.y = p.B.y + offset.y;

					vC.x = p.A.x + offset.x;
					vC.y = p.A.y + offset.y;

					vD.x = p.B.x + offset.x;
					vD.y = p.B.y + offset.y;

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

					GL.TexCoord3(uvRectWidth, uvRectY, 0);
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
			
			GL.Color(Color.black);
			for(int x = 0; x < pairList.Count; x++) {
				p = pairList[x];

				vA.x = p.A.x + offset.x;
				vA.y = p.A.y + offset.y;

				vB.x = p.B.x + offset.x;
				vB.y = p.B.y + offset.y;

				vC.x = p.A.x + offset.x;
				vC.y = p.A.y + offset.y;

				vD.x = p.B.x + offset.x;
				vD.y = p.B.y + offset.y;
				
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
		}
	}

	public static void Mask(LightingBuffer2D buffer, LightingCollider2D id, LayerSetting layerSetting, Vector2D offset, float z) {
		if (false == (id.shape.maskType == LightingCollider2D.MaskType.Collider || id.shape.maskType == LightingCollider2D.MaskType.SpriteCustomPhysicsShape || id.shape.maskType == LightingCollider2D.MaskType.Mesh)) {	
			return;
		}
		
		if (id.isVisibleForLight(buffer) == false) {
			return;
		}

		Mesh mesh = null;
		MeshVertices vertices = null;

		if (id.shape.maskType == LightingCollider2D.MaskType.Mesh) {
			if (id.meshFilter == null) {
				return;
			}
			mesh = id.meshFilter.sharedMesh;
		} else {
			mesh = id.shape.GetMesh_MaskType(id.transform);
			vertices = id.shape.GetMesh_Vertices_MaskType(id.transform);
		}

		if (mesh == null) {
			return;
		}

		bool maskEffect = (layerSetting.effect == LightingLayerEffect.InvisibleBellow);

		LightingMaskMode maskMode = id.maskMode;
		MeshVertice vertice;

		if (maskMode == LightingMaskMode.Invisible) {
			GL.Color(Color.black);

		} else if (layerSetting.effect == LightingLayerEffect.InvisibleBellow) {
			float c = (float)offset.y / layerSetting.maskEffectDistance +  layerSetting.maskEffectDistance * 2;
			if (c < 0) {
				c = 0;
			}

			color.r = c;
			color.g = c;
			color.b = c;
			color.a = 1;

			GL.Color(color);
		} else {
			GL.Color(Color.white);
		}

		for (int i = 0; i < vertices.list.Count; i ++) {
			vertice = vertices.list[i];
			Max2DMatrix.DrawTriangle(vertice.a, vertice.b, vertice.c, offset, z);
		}

		LightingDebug.maskGenerations ++;		
	}





	
	public static void CalculatePenumbra() {
		penumbraSprite = LightingManager2D.Get().materials.GetAtlasPenumbraSprite();

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
}

/* 
		case LightingShadowCollider.ColliderType.Collider:
			// Additional Code To Draw "Holes" Shadows Too
			List<Polygon2D> polygonsAndHoles = new List<Polygon2D>();
			Polygon2D hole;
			
			for(int i = 0; i < polygons.Count; i++) {
				polygon = polygons[i];
				poly = polygon.ToWorldSpace (id.gameObject.transform);
				poly.ToOffsetItself (offset);

				if (buffer.lightSource.drawInsideCollider == false && poly.PointInPoly (zero)) {
					return;
				}

				polygonsAndHoles.Add(poly);

				if (polygon.holesList.Count > 0) {
					for(int x = 0; x < polygon.holesList.Count; x++) {
						hole = polygon.holesList[x];

						poly = hole.ToWorldSpace (id.gameObject.transform);
						poly.ToOffsetItself (offset);

						polygonsAndHoles.Add(poly);
					}
				}
			}

			polygons = polygonsAndHoles;
			
			for(int i = 0; i < polygons.Count; i++) {
				polygon = polygons[i];

				List<Pair2D> pairList = Pair2D.GetList(polygon.pointsList);
				for(int x = 0; x < pairList.Count; x++) {
					p = pairList[x];
					
					vA.x = p.A.x;
					vA.y = p.A.y;

					vB.x = p.B.x;
					vB.y = p.B.y;
					
					vA.Push (Vector2D.Atan2 (vA, zero), lightSizeSquared);
					vB.Push (Vector2D.Atan2 (vB, zero), lightSizeSquared);

					Max2DMatrix.DrawTriangle(p.A ,p.B, vA, zero, z);
					Max2DMatrix.DrawTriangle(vA, vB, p.B, zero, z);
				}

				LightingManager2D.LightingDebug.shadowGenerations ++;	
			}
			break;
	} 
	*/