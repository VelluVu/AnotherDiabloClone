using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

#if UNITY_2018_1_OR_NEWER
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class LightingTilemapCollider2D : MonoBehaviour {
	public enum MapType {UnityEngineTilemapRectangle, UnityEngineTilemapIsometric, UnityEngineTilemapHexagon, SuperTilemapEditor};
	public enum ColliderType {None, Tile, Collider, SpriteCustomPhysicsShape};
	public enum MaskType {None, Sprite, Tile, SpriteCustomPhysicsShape};

	public MapType mapType = MapType.UnityEngineTilemapRectangle;

	public LightingLayer lightingCollisionLayer = LightingLayer.Layer1;
	public LightingLayer lightingMaskLayer = LightingLayer.Layer1;

	public ColliderType colliderType = ColliderType.Tile;
	public MaskType maskType = MaskType.Sprite;
	public LightingMaskMode maskMode = LightingMaskMode.Visible;

	// Day Lighting
	public bool dayHeight = false;
	public float height = 1;

	public List<Polygon2D> edgeColliders = new List<Polygon2D>();
	public List<Polygon2D> polygonColliders = new List<Polygon2D>();

	//public bool ambientOcclusion = false;
	//public float occlusionSize = 1f;

	public Vector2 cellSize = new Vector2(1, 1);
	public Vector2 cellAnchor = new Vector2(0.5f, 0.5f);

	public BoundsInt area;
	public LightingTile[,] map;

	private Tilemap tilemap2D;

	public static List<LightingTilemapCollider2D> list = new List<LightingTilemapCollider2D>();

	public void OnEnable() {
		list.Add(this);

		Initialize();

		foreach(LightingSource2D light in LightingSource2D.GetList()) {
			light.movement.ForceUpdate();
		}
	}

	public void OnDisable() {
		list.Remove(this);

		foreach(LightingSource2D light in LightingSource2D.GetList()) {
			light.movement.ForceUpdate();
		}
	}

	static public List<LightingTilemapCollider2D> GetList() {
		return(list);
	}

	public void Initialize() {
		switch(mapType) {
			case MapType.UnityEngineTilemapRectangle:
				InitializeUnityDefault();

			break;

			case MapType.SuperTilemapEditor:
				InitializeSuperTilemapEditor();

            break;
		}
	}

	void InitializeUnityDefault() {
		tilemap2D = GetComponent<Tilemap>();

		if (tilemap2D == null) {
			return;
		}

		Grid grid = tilemap2D.layoutGrid;

		if (grid == null) {
			Debug.LogError("Lighting 2D Error: Lighting Tilemap Collider is missing Grid");
		} else {
			cellSize = grid.cellSize;
		}

		cellAnchor = tilemap2D.tileAnchor;

		int minPos = Mathf.Min(tilemap2D.cellBounds.xMin, tilemap2D.cellBounds.yMin);
		int maxPos = Mathf.Max(tilemap2D.cellBounds.size.x, tilemap2D.cellBounds.size.y);

		area = new BoundsInt(minPos, minPos, 0, maxPos + Mathf.Abs(minPos), maxPos + Mathf.Abs(minPos), 1);

		TileBase[] tileArray = tilemap2D.GetTilesBlock(area);

		map = new LightingTile[area.size.x + 1, area.size.y + 1];

		for (int index = 0; index < tileArray.Length; index++) {
			TileBase tile = tileArray[index];
			if (tile == null) {
				continue;
			}

			TileData tileData = new TileData();

			ITilemap tilemap = (ITilemap) FormatterServices.GetUninitializedObject(typeof(ITilemap));
			typeof(ITilemap).GetField("m_Tilemap", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(tilemap, tilemap2D);
			tile.GetTileData(new Vector3Int(0, 0, 0), tilemap, ref tileData);

			LightingTile lightingTile = new LightingTile();
			lightingTile.SetOriginalSprite(tileData.sprite);
			lightingTile.GetShapePolygons();
	
			
			map[(index % area.size.x), (index / area.size.x)] = lightingTile;
			//map[(index % area.size.x), (index / area.size.y)] = true;
		}
	}

	void InitializeSuperTilemapEditor() {
		/* 

		CreativeSpore.SuperTilemapEditor.STETilemap tilemapSTE = GetComponent<CreativeSpore.SuperTilemapEditor.STETilemap>();
	
		cellSize = tilemapSTE.CellSize;

		map = new LightingTile[tilemapSTE.GridWidth + 2, tilemapSTE.GridHeight + 2];

		area.position = new Vector3Int((int)tilemapSTE.MapBounds.center.x, (int)tilemapSTE.MapBounds.center.y, 0);

		area.size = new Vector3Int((int)(tilemapSTE.MapBounds.extents.x + 1) * 2, (int)(tilemapSTE.MapBounds.extents.y + 1) * 2, 0);

		if (colliderType == ColliderType.Collider) {
			foreach(Transform t in transform) {
				foreach(Component c in t.GetComponents<EdgeCollider2D>()) {
					Polygon2D poly = Polygon2D.CreateFromEdgeCollider(c as EdgeCollider2D);
					poly = poly.ToWorldSpace(t);
					edgeColliders.Add(poly);
				}
				foreach(Component c in t.GetComponents<PolygonCollider2D>()) {
					Polygon2D poly = Polygon2DList.CreateFromPolygonColliderToWorldSpace(c as PolygonCollider2D)[0];
					polygonColliders.Add(poly);
				}
			}
		} else {
		
			for(int x = 0; x <= tilemapSTE.GridWidth; x++) {
				for(int y = 0; y <= tilemapSTE.GridHeight; y++) {
					int tileX = x + area.position.x - area.size.x / 2;
					int tileY = y + area.position.y - area.size.y / 2;
					if(tilemapSTE.GetTile(tileX, tileY) == null) {
						continue;
					}
					LightingTile lightingTile = new LightingTile();
					map[x, y] = lightingTile;
				}
			}	
		}
		
		*/
	}
}
#endif