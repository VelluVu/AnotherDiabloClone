using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBufferSorted {
    private static List<LightingCollider2D> colliderList;
    private static List<LightingTilemapCollider2D> tilemapList;

    private static ColliderDepthList list = new ColliderDepthList();
  
    private static LightingManager2D manager = null;

    private static Vector2D offset = Vector2D.Zero();

    static ColliderDepth depth;

    static bool drawMask = false;
    static bool drawShadows = false;

    static float lightSizeSquared, z;

    static LayerSetting layerSettings;
    
    static LightingCollider2D collider;
    static LightingTilemapCollider2D tilemap;

    static public void DrawShadowsAndMask(LightingBuffer2D buffer, int layer) {
        buffer.CalculateCoords();

        offset.x = -buffer.lightSource.transform.position.x;
        offset.y = -buffer.lightSource.transform.position.y;

        manager = LightingManager2D.Get();

        layerSettings = buffer.lightSource.layerSetting[layer];

        z = buffer.transform.position.z;
        lightSizeSquared = Mathf.Sqrt(buffer.lightSource.lightSize * buffer.lightSource.lightSize + buffer.lightSource.lightSize * buffer.lightSource.lightSize);
       
        SortObjects(buffer, layer);

        drawMask = false;
        if (buffer.lightSource.layerSetting[layer].type != LightingLayerType.ShadowOnly) {
            drawMask = true;
        }

        drawShadows = false;
        if (buffer.lightSource.layerSetting[layer].type != LightingLayerType.MaskOnly) {
            drawShadows = true;
        }

        GL.PushMatrix();

        if (manager.lightingSpriteAtlas && SpriteAtlasManager.Get().atlasTexture != null) {
            DrawWithAtlas(buffer, layer);
        } else {
            DrawWithoutAtlas(buffer, layer);
        }

        GL.PopMatrix();
    }

    static void DrawWithAtlas(LightingBuffer2D buffer, int layer) {
        manager.materials.GetAtlasMaterial().SetPass(0);
        GL.Begin(GL.TRIANGLES);

        Material materialWhite = manager.materials.GetWhiteSprite();
        Material materialBlack = manager.materials.GetBlackSprite();
        Material material;

        PartiallyBatched_Collider batch_collider;              
        PartiallyBatched_Tilemap batch_tilemap;

        LayerSetting layerSetting = buffer.lightSource.layerSetting[layer];
        bool maskEffect = (layerSetting.effect == LightingLayerEffect.InvisibleBellow);

        for(int i = 0; i < list.count; i ++) {
            depth = list.list[i];

            switch (depth.type) {
                case ColliderDepth.Type.Collider:
                     if ((int)depth.collider.lightingCollisionLayer == layer && drawShadows) {	
                        LightingBufferShape.Shadow(buffer, depth.collider, lightSizeSquared, z, offset);
                    }

                    if ((int)depth.collider.lightingMaskLayer == layer && drawMask) {
                        LightingBufferSprite.MaskDepthWithAtlas(buffer, layerSettings.effect, depth.collider, offset, z);

                        LightingBufferShape.Mask(buffer, depth.collider, layerSetting, offset, z);  

                        // Partialy Batched (Depth Edition)
                        if (buffer.partiallyBatchedList_Collider.Count > 0) {
                            GL.End();
                        
                            for(int s = 0; s < buffer.partiallyBatchedList_Collider.Count; s++) {
                                batch_collider = buffer.partiallyBatchedList_Collider[s];

                                material = materialWhite;

                                if (batch_collider.collider2D.maskMode == LightingMaskMode.Invisible || (maskEffect && buffer.lightSource.transform.position.y > batch_collider.collider2D.transform.position.y)) {
                                    material = materialBlack;
                                }

                                LightingBufferSprite.MaskDepthWithoutAtlas(buffer, batch_collider.collider2D, material, offset, z);
                        
                            }
                            buffer.partiallyBatchedList_Collider.Clear();

                            manager.materials.GetAtlasMaterial().SetPass(0);
                            GL.Begin(GL.TRIANGLES);
                        }
                    }

                break;

                case ColliderDepth.Type.Tile:
                    if ((int)depth.tilemap.lightingCollisionLayer == layer && drawShadows) {	
                        GL.Color(Color.black);
                        LightingBufferTile.DrawShadow(buffer, depth.tile, depth.polyOffset, depth.tilemap, lightSizeSquared, z);
                    }

                    if ((int)depth.tilemap.lightingMaskLayer == layer && drawMask) {
                        LightingBufferTile.MaskSpriteDepthWithAtlas(buffer, depth.tile, layerSettings, depth.tilemap, depth.polyOffset, z);

                       // GL.Color(Color.white);
                       // depth.tile.MaskShapeDepthWithAtlas(buffer, order, depth.tilemap, depth.polyOffset, z);

                        // Partialy Batched (Depth Edition)
                        if (buffer.partiallyBatchedList_Tilemap.Count > 0) {
                            GL.End();

                            for(int s = 0; s < buffer.partiallyBatchedList_Tilemap.Count; s++) {
                                batch_tilemap = buffer.partiallyBatchedList_Tilemap[s];
                                LightingBufferTile.DrawMask(buffer, depth.tile, layerSettings.effect, materialWhite, materialBlack, new Vector2D(batch_tilemap.polyOffset), batch_tilemap.tilemap, lightSizeSquared, z);
                            }
                            buffer.partiallyBatchedList_Tilemap.Clear();

                            manager.materials.GetAtlasMaterial().SetPass(0);
                            GL.Begin(GL.TRIANGLES);
                        }
                    }   
                   
                break;

            }
        }
        
        GL.End();
    }

    static void DrawWithoutAtlas(LightingBuffer2D buffer, int layer) {
        Material materialWhite = manager.materials.GetWhiteSprite();
        Material materialBlack = manager.materials.GetBlackSprite();
        Material material;

        LayerSetting layerSetting = buffer.lightSource.layerSetting[layer];
        bool maskEffect = layerSetting.effect == LightingLayerEffect.InvisibleBellow;

        for(int i = 0; i < list.count; i ++) {
            depth = list.list[i];

             switch (depth.type) {
                case ColliderDepth.Type.Collider:
                    if ((int)depth.collider.lightingCollisionLayer == layer && drawShadows) {	
                        manager.materials.GetAtlasMaterial().SetPass(0);
                        
                        GL.Begin(GL.TRIANGLES);

                            LightingBufferShape.Shadow(buffer, depth.collider, lightSizeSquared, z, offset);

                        GL.End();
                    }

                   if ((int)depth.collider.lightingMaskLayer == layer && drawMask) {
                        // Masking
                        material = materialWhite;

                        if (depth.collider.maskMode == LightingMaskMode.Invisible || (maskEffect && buffer.lightSource.transform.position.y > depth.collider.transform.position.y)) {
                            material = materialBlack;
                        }
                        
                       LightingBufferSprite.MaskDepthWithoutAtlas(buffer, depth.collider, material, offset, z);
                    
                        materialWhite.SetPass(0);

                        GL.Begin(GL.TRIANGLES);

                            GL.Color(Color.white);
                            LightingBufferShape.Mask(buffer, depth.collider, layerSetting, offset, z);
                            
                        GL.End();
                    }

                break;

                case ColliderDepth.Type.Tile:
                    manager.materials.GetAtlasMaterial().SetPass(0);
                        
                    GL.Begin(GL.TRIANGLES);

                        if ((int)depth.tilemap.lightingCollisionLayer == layer && drawShadows) {	
                            LightingBufferTile.DrawShadow(buffer, depth.tile, depth.polyOffset, depth.tilemap, lightSizeSquared, z);
                        }

                    GL.End();  

                    if ((int)depth.tilemap.lightingMaskLayer == layer && drawMask) {
                        // Sprite, But What About Shape?
                        LightingBufferTile.DrawMask(buffer, depth.tile, layerSettings.effect, materialWhite, materialBlack, depth.polyOffset, depth.tilemap, lightSizeSquared, z);
                    } 
                   
                break;
             }
        }
    }

    static public void SortObjects(LightingBuffer2D buffer, int layer) {
        colliderList = LightingCollider2D.GetList();
        tilemapList = LightingTilemapCollider2D.GetList();

        list.Reset();

        for(int id = 0; id < colliderList.Count; id++) {
            // Check If It's In Light Area?
            collider = colliderList[id];

            if ((int)colliderList[id].lightingCollisionLayer != layer && (int)colliderList[id].lightingMaskLayer != layer) {
				continue;
			}

            if (layerSettings.renderingOrder == LightRenderingOrder.YAxis) {
                list.Add(collider, -collider.transform.position.y);
            } else {
                list.Add(collider, -Vector2.Distance(collider.transform.position, buffer.lightSource.transform.position));
            }
        }

        for(int id = 0; id < tilemapList.Count; id++) {
            SortTilemap(buffer, tilemapList[id]);
        }

        list.Sort();
    }

    static public void SortTilemap(LightingBuffer2D buffer, LightingTilemapCollider2D id) {
		//if (id.maskType != LightingTilemapCollider2D.MaskType.Sprite) {
		//	return;
		//}
		
		if (id.map == null) {
			return;
		}

		Vector3 rot = Math2D.GetPitchYawRollRad(id.transform.rotation);

		float rotationYScale = Mathf.Sin(rot.x + Mathf.PI / 2);
		float rotationXScale = Mathf.Sin(rot.y + Mathf.PI / 2);

		float scaleX = id.transform.lossyScale.x * rotationXScale * id.cellSize.x;
		float scaleY = id.transform.lossyScale.y * rotationYScale * id.cellSize.y;

		int sizeInt = LightingBufferTilemapRectangle.LightTilemapSize(id, buffer);

		LightingBufferTilemapRectangle.LightTilemapOffset(id, new Vector2(scaleX, scaleY), buffer);

        Vector2Int newPositionInt = LightingBufferTilemapRectangle.newPositionInt;

        Vector2D tilemapOffset = LightingBufferTilemapRectangle.tilemapOffset;
        Vector2D polyOffset = LightingBufferTilemapRectangle.polyOffset;
        Vector2D inverseOffset = LightingBufferTilemapRectangle.inverseOffset;

		tilemapOffset.x = id.area.position.x + id.cellAnchor.x + id.transform.position.x;
		tilemapOffset.y = id.area.position.y + id.cellAnchor.y + id.transform.position.y;

        offset.x = -buffer.lightSource.transform.position.x;
		offset.y = -buffer.lightSource.transform.position.y;

		VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();

		Vector2 tileSize = new Vector2(scaleX / id.cellSize.x, scaleY / id.cellSize.y);

        LightingTile tile;

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

				polyOffset.x *= scaleX;
				polyOffset.y *= scaleY;

				if (LightingManager2D.culling && Vector2.Distance(polyOffset.ToVector2(), buffer.lightSource.transform.position) > 2 + buffer.lightSource.lightSize) {
					LightingDebug.culled ++;
					continue;
				}
                
				polyOffset.x += offset.x;
				polyOffset.y += offset.y;
			
				//inverseOffset.x = -polyOffset.x;
				//inverseOffset.y = -polyOffset.y;

				//if (poly.PointInPoly (inverseOffset)) {
				//	continue;
				//}

                if (layerSettings.renderingOrder == LightRenderingOrder.YAxis) {
                   list.Add(id, tile, -(float)polyOffset.y - (float)offset.y, polyOffset);
                } else {
                    list.Add(id, tile,  -Vector2.Distance(polyOffset.ToVector2() - offset.ToVector2() , buffer.lightSource.transform.position), polyOffset);
                }
			}	
		}
    }
}