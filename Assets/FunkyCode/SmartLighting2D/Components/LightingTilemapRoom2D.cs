using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

#if UNITY_2018_1_OR_NEWER
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class LightingTilemapRoom2D : MonoBehaviour {
    public enum MapType {UnityEngineTilemapRectangle};
    public MapType mapType = MapType.UnityEngineTilemapRectangle;
    //public Color color = Color.black;

    public Vector2 cellSize = new Vector2(1, 1);
	public Vector2 cellAnchor = new Vector2(0.5f, 0.5f);

	public BoundsInt area;
	public LightingTile[,] map;

	private Tilemap tilemap2D;

    public static List<LightingTilemapRoom2D> list = new List<LightingTilemapRoom2D>();

    static public List<LightingTilemapRoom2D> GetList() {
		return(list);
	}

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

    public void Initialize() {
		switch(mapType) {
			case MapType.UnityEngineTilemapRectangle:
				InitializeUnityDefault();
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
}

#endif