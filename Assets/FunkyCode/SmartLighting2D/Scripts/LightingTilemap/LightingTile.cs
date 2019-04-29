using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightingTile {
	private Sprite originalSprite;
	private Sprite atlasSprite;

	public CustomPhysicsShape customPhysicsShape = null;

	private List<Polygon2D> shapePolygons = null;
	private Mesh shapeMesh = null;

	public List<Polygon2D> world_polygon = null;
	public List<List<Pair2D>> world_polygonPairs = null;

	public static Mesh staticTileMesh = null;

	public void SetOriginalSprite(Sprite sprite) {
		originalSprite = sprite;
	}

	public Sprite GetOriginalSprite() {
		return(originalSprite);
	}

	public Sprite GetAtlasSprite() {
		return(atlasSprite);
	}

	public void SetAtlasSprite(Sprite sprite) {
		atlasSprite = sprite;
	}

	public List<Polygon2D> GetPolygon(LightingTilemapCollider2D tilemap) {
		if (world_polygon == null) {
			if (tilemap.colliderType == LightingTilemapCollider2D.ColliderType.SpriteCustomPhysicsShape) {
				if (GetShapePolygons().Count < 1) {
					return(null);
				}
				
				world_polygon = GetShapePolygons(); //poly.ToScaleItself(defaultSize); // scale?
				
			} else {
				Vector2 size = tilemap.cellSize * 0.5f;

				world_polygon = new List<Polygon2D>();
				world_polygon.Add(Polygon2D.CreateFromRect(size));
			}
		}
		return(world_polygon);
	}

	public List<List<Pair2D>> GetPairs(LightingTilemapCollider2D tilemap) {
		if (world_polygonPairs == null) {
			world_polygonPairs = new List<List<Pair2D>>();

			if (GetPolygon(tilemap) == null) {
				return(world_polygonPairs);
			}

			foreach (Polygon2D poly in GetPolygon(tilemap)) {
				world_polygonPairs.Add(Pair2D.GetList(poly.pointsList));
			}
		}
		return(world_polygonPairs);
	}

	public List<Polygon2D> GetShapePolygons() {
		if (shapePolygons == null) {
			shapePolygons = new List<Polygon2D>();

			if (originalSprite == null) {
				return(shapePolygons);
			}

			#if UNITY_2018_1_OR_NEWER
				if (customPhysicsShape == null) {
					customPhysicsShape = SpriteAtlasManager.RequesCustomShape(originalSprite);
				}
				shapePolygons = customPhysicsShape.GetShape();
			#endif
		}
		return(shapePolygons);
	}

	public Mesh GetTileDynamicMesh() {
		if (shapeMesh == null) {
			if (shapePolygons != null && shapePolygons.Count > 0) {
				shapeMesh = customPhysicsShape.GetShapeMesh();
			}
		}
		return(shapeMesh);
	}

	public static Mesh GetStaticTileMesh() {
		if (staticTileMesh == null) {
			Polygon2D tilePoly = Polygon2D.CreateFromRect(new Vector2(0.5f + 0.01f, 0.5f + 0.01f));
			staticTileMesh  = tilePoly.CreateMesh(Vector2.zero, Vector2.zero);	
		}
		return(staticTileMesh);
	}
}