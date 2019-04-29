using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingRoomTilemap {

	public static Vector2D offset = Vector2D.Zero();
	public static Vector2D polyOffset = Vector2D.Zero();
	public static Vector2D tilemapOffset = Vector2D.Zero();
	public static Vector2D inverseOffset = Vector2D.Zero();

   	static Vector2 tileSize = Vector2.zero;
	static Vector2 scale = Vector2.zero;
	static Vector2 polyOffset2 = Vector2.zero;

	static LightingTile tile;

    static VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();

    static Vector2D tileSize2 = new Vector2D(1, 1);

    public static Vector2Int newPositionInt = new Vector2Int();
	static int sizeInt;

   static public void MaskSpriteWithoutAtlas(Camera camera, LightingTilemapRoom2D id, Material materialA, Material materialB, Vector2D offset, float z) {
		//if (id.maskType != LightingTilemapCollider2D.MaskType.Sprite) {
		//	return;
		//}
		
		if (id.map == null) {
			return;
		}

		Material material;

		SetupLocation(camera, id);

		//bool invisible = true; //(id.maskMode == LightingMaskMode.Invisible);

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
				
				//if (LightingManager2D.culling && Vector2.Distance(polyOffset2, buffer.lightSource.transform.position) > 2 + buffer.lightSource.lightSize) {
				//	LightingDebug.culled ++;
				//	continue;
				//}

				polyOffset.x += offset.x;
				polyOffset.y += offset.y;

				spriteRenderer.sprite = tile.GetOriginalSprite();

				polyOffset2.x = (float)polyOffset.x;
				polyOffset2.y = (float)polyOffset.y;

				material = materialB;
			
				material.mainTexture = spriteRenderer.sprite.texture;
	
				Max2D.DrawSprite(material, spriteRenderer, polyOffset2, tileSize, 0, z);
				
				material.mainTexture = null;

				LightingDebug.maskGenerations ++;
			}	
		}
	}

    static public void SetupLocation(Camera camera, LightingTilemapRoom2D id) {
		Vector3 rot = Math2D.GetPitchYawRollRad(id.transform.rotation);

		float rotationYScale = Mathf.Sin(rot.x + Mathf.PI / 2);
		float rotationXScale = Mathf.Sin(rot.y + Mathf.PI / 2);

		scale.x = id.transform.lossyScale.x * rotationXScale * id.cellSize.x;
		scale.y = id.transform.lossyScale.y * rotationYScale * id.cellSize.y;

		sizeInt = LightTilemapSize(id, camera);

		LightTilemapOffset(id, scale, camera);
		
		offset.x = -camera.transform.position.x;
		offset.y = -camera.transform.position.y;

		tilemapOffset.x = id.transform.position.x + id.area.position.x + id.cellAnchor.x;
		tilemapOffset.y = id.transform.position.y + id.area.position.y + id.cellAnchor.y;

		//if (id.mapType == LightingTilemapCollider2D.MapType.SuperTilemapEditor) {
		//	tilemapOffset.x -= id.area.size.x / 2;
		//	tilemapOffset.y -= id.area.size.y / 2;
		//}

		tileSize.x = scale.x / id.cellSize.x;
		tileSize.y = scale.y / id.cellSize.y;
	}

    static public int LightTilemapSize(LightingTilemapRoom2D id, Camera camera) {
		return((int)camera.orthographicSize + 5);
	}

    static public void LightTilemapOffset(LightingTilemapRoom2D id, Vector2 scale, Camera camera) {
		Vector2 newPosition = camera.transform.position;

		newPosition.x -= id.area.position.x;
		newPosition.y -= id.area.position.y;
		
		newPosition.x -= id.transform.position.x;
		newPosition.y -= id.transform.position.y;
			
		newPosition.x -= id.cellAnchor.x;
		newPosition.y -= id.cellAnchor.y;

		// Cell Size Is Not Calculated Correctly

		//if (id.mapType == LightingTilemapCollider2D.MapType.SuperTilemapEditor) {
		//	newPosition.x += id.area.size.x / 2;
		//	newPosition.y += id.area.size.y / 2;
		//} else {
			newPosition.x += 1;
			newPosition.y += 1;
		

		newPosition.x *= scale.x;
		newPosition.y *= scale.y;

		newPositionInt.x = (int)newPosition.x;
		newPositionInt.y = (int)newPosition.y;
	}
}
