using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingOcclussion 
{
    const float uv0 = 0;
    const float uv1 = 1;

    public static void Draw(Vector2D offset, float z) {
		Draw_Collider_Strict(offset, z);
		Draw_Collider_Smooth(offset, z);

		//Draw_Tilemap(offset, z);
	}

	static void Draw_Collider_Strict(Vector2D offset, float z) {
		LightingManager2D manager = LightingManager2D.Get();

		GL.PushMatrix ();

		manager.materials.GetOcclusionBlur().SetPass (0);

		GL.Begin (GL.QUADS);
		GL.Color(Color.white);

		/*
		
		foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
			if (id.ambientOcclusion == false || id.smoothOcclusionEdges == true) {
				continue;
			}
			
			// Do not call Create From Collider
			List<Polygon2D> polygons = null;
			switch(id.colliderType) {
				case LightingCollider2D.ColliderType.Collider:
					polygons = id.GetColliderPolygons();
					break;

				case LightingCollider2D.ColliderType.SpriteCustomPhysicsShape:
					polygons = id.GetShapePolygons();
					break;
			}

			if (polygons == null || polygons.Count < 1) {
				continue;
			}

			foreach(Polygon2D polygon in polygons) {
				Polygon2D poly = polygon.ToWorldSpace (id.gameObject.transform);

				poly.Normalize();

				Vector2D first = null;

				List<Pair2D> iterate1 =  Pair2D.GetList(poly.pointsList);
				List<Pair2D> iterate2 =  Pair2D.GetList(PreparePolygon(poly, id.occlusionSize).pointsList);

				int i = 0;
				foreach (Pair2D pA in iterate1) {
					if (id.edgeCollider2D == true && first == null) {
						first = pA.A;
						continue;
					}

					if (i >= iterate2.Count) {
						continue;
					}

					Pair2D pB = iterate2[i];

					GL.TexCoord2 (uv0, uv0);
					Max2D.Vertex3 (pA.A + offset, z);
					GL.TexCoord2 (uv1, uv0);
					Max2D.Vertex3 (pA.B + offset, z);
					GL.TexCoord2 (uv1, uv1);
					Max2D.Vertex3 (pB.B + offset, z);
					GL.TexCoord2 (uv0, uv1);
					Max2D.Vertex3 (pB.A + offset, z);

					i ++;
				}
			}
		}

		*/
		GL.End ();
		GL.PopMatrix ();
	}

	static void Draw_Collider_Smooth(Vector2D offset, float z) {
		LightingManager2D manager = LightingManager2D.Get();
		
		GL.PushMatrix ();

		manager.materials.GetOcclusionEdge().SetPass (0);

		GL.Begin (GL.TRIANGLES);

		/*
		
		foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
			if (id.ambientOcclusion == false || id.smoothOcclusionEdges == false) {
				continue;
			}

			// Do not call Create From Collider
			List<Polygon2D> polygons = null;
			switch(id.colliderType) {
				case LightingCollider2D.ColliderType.Collider:
					polygons = id.GetColliderPolygons();
					break;

				case LightingCollider2D.ColliderType.SpriteCustomPhysicsShape:
					polygons = id.GetShapePolygons();
					break;
			}

			if (polygons == null || polygons.Count < 1) {
				continue;
			}
			
			foreach(Polygon2D polygon in polygons) {
				Polygon2D poly = polygon.ToWorldSpace (id.gameObject.transform);

				poly.Normalize();

				foreach (DoublePair2D p in DoublePair2D.GetList(poly.pointsList)) {
					Vector2D vA = p.A + offset;
					Vector2D vB = p.B + offset;
					Vector2D vC = p.B + offset;

					Vector2D pA = p.A + offset;
					Vector2D pB = p.B + offset;

					vA.Push (Vector2D.Atan2 (p.A, p.B) - Mathf.PI / 2, id.occlusionSize);
					vB.Push (Vector2D.Atan2 (p.A, p.B) - Mathf.PI / 2, id.occlusionSize);
					vC.Push (Vector2D.Atan2 (p.B, p.C) - Mathf.PI / 2, id.occlusionSize);

					GL.TexCoord2 (uv0, uv0);
					Max2D.Vertex3 (pB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pA, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (vA, z);

					GL.TexCoord2 (uv0, uv1);
					Max2D.Vertex3 (vA, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (vB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pB, z);
		
					GL.TexCoord2 (uv1, uv0);
					Max2D.Vertex3 (vB, z);
					GL.TexCoord2 (0.5f - uv0, uv0);
					Max2D.Vertex3 (pB, z);
					GL.TexCoord2 (0.5f - uv0, uv1);
					Max2D.Vertex3 (vC, z);
				}
			}
		}

		*/
		
		GL.End ();
		GL.PopMatrix ();
	}

	static void Draw_Tilemap(Vector2D offset, float z) {
		/* 
		GL.PushMatrix ();
		LightingManager2D.Get().occlusionEdgeMaterial.SetPass (0);
		GL.Begin (GL.TRIANGLES);

		foreach (LightingTilemapCollider2D id in LightingTilemapCollider2D.GetList()) {
			if (id.map == null) {
				continue;
			}
			if (id.ambientOcclusion == false) {
				continue;
			}
			for(int x = 0; x < id.area.size.x; x++) {
				for(int y = 0; y < id.area.size.y; y++) {
					if (id.map[x, y] == null) {
						continue;
					}

					Vector2D offs = offset.Copy();
					offs += new Vector2D(0.5f, 0.5f);
					offs += new Vector2D(id.area.position.x, id.area.position.y);
					offs += new Vector2D(-id.area.size.x / 2, -id.area.size.y / 2);

					DrawTileOcclussion(offs + new Vector2D(x, y), z, id);
				}
			}
		}
		GL.End ();
		GL.PopMatrix ();
		*/
	}
	
	#if UNITY_2018_1_OR_NEWER
	static void DrawTileOcclussion(Vector2D offset, float z, LightingTilemapCollider2D id) {
		Polygon2D poly = Polygon2DList.CreateFromRect(new Vector2(0.5f, 0.5f));

		foreach (DoublePair2D p in DoublePair2D.GetList(poly.pointsList)) {
			Vector2D vA = p.A + offset;
			Vector2D vB = p.B + offset;
			Vector2D vC = p.B + offset;

			Vector2D pA = p.A + offset;
			Vector2D pB = p.B + offset;

			vA.Push (Vector2D.Atan2 (p.A, p.B) - Mathf.PI / 2, -1);
			vB.Push (Vector2D.Atan2 (p.A, p.B) - Mathf.PI / 2, -1);
			vC.Push (Vector2D.Atan2 (p.B, p.C) - Mathf.PI / 2, -1);

			GL.TexCoord2 (uv0, uv0);
			Max2D.Vertex3 (pB, z);
			GL.TexCoord2 (0.5f - uv0, uv0);
			Max2D.Vertex3 (pA, z);
			GL.TexCoord2 (0.5f - uv0, uv1);
			Max2D.Vertex3 (vA, z);

			GL.TexCoord2 (uv0, uv1);
			Max2D.Vertex3 (vA, z);
			GL.TexCoord2 (0.5f - uv0, uv1);
			Max2D.Vertex3 (vB, z);
			GL.TexCoord2 (0.5f - uv0, uv0);
			Max2D.Vertex3 (pB, z);

			GL.TexCoord2 (uv1, uv0);
			Max2D.Vertex3 (vB, z);
			GL.TexCoord2 (0.5f - uv0, uv0);
			Max2D.Vertex3 (pB, z);
			GL.TexCoord2 (0.5f - uv0, uv1);
			Max2D.Vertex3 (vC, z);
		}
	}
	#endif

	// Remove or Change!!!
    // Polygon Occlusion
	static public Polygon2D PreparePolygon(Polygon2D polygon, float size) {
		Polygon2D newPolygon = new Polygon2D();

		DoublePair2D pair;
		foreach (Vector2D pB in polygon.pointsList) {
			int indexB = polygon.pointsList.IndexOf (pB);

			int indexA = (indexB - 1);
			if (indexA < 0) {
				indexA += polygon.pointsList.Count;
			}

			int indexC = (indexB + 1);
			if (indexC >= polygon.pointsList.Count) {
				indexC -= polygon.pointsList.Count;
			}

			pair = new DoublePair2D (polygon.pointsList[indexA], pB, polygon.pointsList[indexC]);

			float rotA = (float)Vector2D.Atan2(pair.B, pair.A);
			float rotC = (float)Vector2D.Atan2(pair.B, pair.C);

			Vector2D pairA = new Vector2D(pair.A);
			pairA.Push(rotA - Mathf.PI / 2, -size);

			Vector2D pairC = new Vector2D(pair.C);
			pairC.Push(rotC + Mathf.PI / 2, -size);
			
			Vector2D vecA = new Vector2D(pair.B);
			vecA.Push(rotA - Mathf.PI / 2, -size);
			vecA.Push(rotA, 110f);

			Vector2D vecC = new Vector2D(pair.B);
			vecC.Push(rotC + Mathf.PI / 2, -size);
			vecC.Push(rotC, 110f);

			Vector2D result = Math2D.GetPointLineIntersectLine(new Pair2D(pairA, vecA), new Pair2D(pairC, vecC));

			if (result != null) {
				newPolygon.AddPoint(result);
			}
		}

		return(newPolygon);
	} 
}
