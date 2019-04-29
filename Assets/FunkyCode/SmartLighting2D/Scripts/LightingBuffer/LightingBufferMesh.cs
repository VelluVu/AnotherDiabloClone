using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBufferMesh {
   static Vector2D zero = Vector2D.Zero();

	const float uv0 = 0f;
	const float uv1 = 1f;

	public static void Shadow(LightingBuffer2D buffer, LightingCollider2D id, float lightSizeSquared, float z) {
		if (id.shape.colliderType != LightingCollider2D.ColliderType.Mesh) {
			return;
		}

		if (id.isVisibleForLight(buffer) == false) {
			return;
		}

		if (id.meshFilter == null) {
			return;
		}

		List<Polygon2D> polygons = new List<Polygon2D>();

		Mesh mesh = id.meshFilter.sharedMesh;

		Vector3 vecA, vecB, vecC;
		
		Vector2D vA, pA, vB, pB;
		float angleA, angleB;
		
		for (int i = 0; i <  mesh.triangles.GetLength (0); i = i + 3) {
			vecA = buffer.transform.TransformPoint(mesh.vertices [mesh.triangles [i]]);
			vecB = buffer.transform.TransformPoint(mesh.vertices [mesh.triangles [i + 1]]);
			vecC = buffer.transform.TransformPoint(mesh.vertices [mesh.triangles [i + 2]]);

			Polygon2D poly = new Polygon2D();
			poly.AddPoint(vecA.x, vecA.y);
			poly.AddPoint(vecB.x, vecB.y);
			poly.AddPoint(vecC.x, vecC.y);
			//polygons.Add(poly);
		}

		if (polygons.Count < 1) {
			return;
		}

		Sprite penumbraSprite = LightingManager2D.Get().materials.GetAtlasPenumbraSprite();

		Rect uvRect = new Rect((float)penumbraSprite.rect.x / penumbraSprite.texture.width, (float)penumbraSprite.rect.y / penumbraSprite.texture.height, (float)penumbraSprite.rect.width / penumbraSprite.texture.width , (float)penumbraSprite.rect.height / penumbraSprite.texture.height);
		
		uvRect.width += uvRect.x;
		uvRect.height += uvRect.y;

		uvRect.x += 1f / 2048;
		uvRect.y += 1f / 2048;
		uvRect.width -= 1f / 2048;
		uvRect.height -= 1f / 2048;

		GL.Color(Color.white);

		foreach(Polygon2D polygon in polygons) {
			Polygon2D poly = polygon.ToWorldSpace (id.gameObject.transform);
			poly.ToOffsetItself (new Vector2D (-buffer.lightSource.transform.position));
			
			if (poly.PointInPoly (zero)) {
				continue;
			}

			foreach (Pair2D p in Pair2D.GetList(poly.pointsList)) {
				vA = p.A.Copy();
				pA = p.A.Copy();

				vB = p.B.Copy();
				pB = p.B.Copy();

				angleA = (float)Vector2D.Atan2 (vA, zero);
				angleB = (float)Vector2D.Atan2 (vB, zero);

				vA.Push (angleA, lightSizeSquared);
				pA.Push (angleA - Mathf.Deg2Rad * buffer.lightSource.occlusionSize, lightSizeSquared);

				vB.Push (angleB, lightSizeSquared);
				pB.Push (angleB + Mathf.Deg2Rad * buffer.lightSource.occlusionSize, lightSizeSquared);

				GL.TexCoord2(uvRect.x, uvRect.y);
				GL.Vertex3((float)p.A.x,(float)p.A.y, z);

				GL.TexCoord2(uvRect.width, uvRect.y);
				GL.Vertex3((float)vA.x, (float)vA.y, z);

				GL.TexCoord2((float)uvRect.x, uvRect.height);
				GL.Vertex3((float)pA.x,(float)pA.y, z);

				GL.TexCoord2(uvRect.x, uvRect.y);
				GL.Vertex3((float)p.B.x,(float)p.B.y, z);

				GL.TexCoord2(uvRect.width, uvRect.y);
				GL.Vertex3((float)vB.x, (float)vB.y, z);

				GL.TexCoord2(uvRect.x, uvRect.height);
				GL.Vertex3((float)pB.x, (float)pB.y, z);
			}
			//LightingDebug.penumbraGenerations ++;
		}

		GL.Color(Color.black);

		foreach(Polygon2D polygon in polygons) {
			Polygon2D poly = polygon.ToWorldSpace (id.gameObject.transform);
			poly.ToOffsetItself (new Vector2D (-buffer.lightSource.transform.position));

			if (poly.PointInPoly (zero)) {
				continue;
			}

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
	}
}