using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeshVertices {
	public List<MeshVertice> list = new List<MeshVertice>();
}

[System.Serializable]
public class MeshVertice {
	public Vector2D a;
	public Vector2D b;
	public Vector2D c;
}

[System.Serializable]
public class LightingShadowCollider {
	public LightingCollider2D.ColliderType colliderType = LightingCollider2D.ColliderType.Collider;
	public LightingCollider2D.MaskType maskType = LightingCollider2D.MaskType.Sprite;

	private List<List<Pair2D>> polygons_collider_world_pair = null;
	private List<List<Pair2D>> polygons_collider_world_pair_cache = null;
	
	private List<Polygon2D> polygons_collider_world = null;
	private List<Polygon2D> polygons_collider_world_cache = null;

	private List<Polygon2D> polygons_collider_local = null;
	private float meshDistance_collider = -1f;
	private Mesh mesh_collider = null;
	private MeshVertices mesh_collider_vertices = null;
	
	private List<List<Pair2D>> polygons_shape_world_pair = null;
	private List<List<Pair2D>> polygons_shape_world_pair_cache = null;

	private List<Polygon2D> polygons_shape_world = null;
	private List<Polygon2D> polygons_shape_world_cache = null;

	private List<Polygon2D> polygons_shape_local = null;
	private float meshDistance_shape = -1f;
	private Mesh mesh_shape = null;
	private MeshVertices mesh_shape_vertices = null;
	
	private CustomPhysicsShape customPhysicsShape = null;

	public CustomPhysicsShape GetPhysicsShape() {
		if (customPhysicsShape == null) {
			customPhysicsShape = SpriteAtlasManager.RequesCustomShape(originalSprite);
		}
		return(customPhysicsShape);
	}

	private Sprite originalSprite;
	private Sprite atlasSprite;

	public Sprite GetOriginalSprite() {
		return(originalSprite);
	}

	public Sprite GetAtlasSprite() {
		return(atlasSprite);
	}

	public void SetAtlasSprite(Sprite sprite) {
		atlasSprite = sprite;
	}

	public void SetOriginalSprite(Sprite sprite) {
		originalSprite = sprite;
	}

	public void ResetLocal() {
		polygons_collider_local = null;
		meshDistance_collider = -1f;
		mesh_collider = null;

		polygons_shape_local = null;
		meshDistance_shape = -1f;
		mesh_shape = null;

		customPhysicsShape = null;

		ResetWorld();
	}

	public void ResetWorld() {
		// Only If Shape Changed

		polygons_collider_world_pair = null;

		polygons_collider_world = null;

		mesh_collider_vertices = null;

		/////////////////////////////////////////////
	
		polygons_shape_world_pair = null;

		polygons_shape_world = null;

		mesh_shape_vertices = null;
	}

	//////////////////////////////////////////////////////////////////////////////
	
	public float GetFrustumDistance(Transform transform) {
		switch(colliderType) {
			case LightingCollider2D.ColliderType.Collider:
				return(GetFrustumDistance_Collider(transform));
			case LightingCollider2D.ColliderType.SpriteCustomPhysicsShape:
				return(GetFrustumDistance_Shape());
		}
		return(1000f);
	}
	
	public float GetFrustumDistance_Collider(Transform transform) {
		if (meshDistance_collider < 0) {
			meshDistance_collider = 0;
			if (GetPolygons_Collider_Local(transform).Count > 0) {
				foreach (Vector2D id in GetPolygons_Collider_Local(transform)[0].pointsList) {
					meshDistance_collider = Mathf.Max(meshDistance_collider, Vector2.Distance(id.ToVector2(), Vector2.zero));
				}
			}
		}
		return(meshDistance_collider);
	}

	public float GetFrustumDistance_Shape() {
		if (meshDistance_shape < 0) {
			meshDistance_shape = 0;
			if (GetPolygons_Shape_Local().Count > 0) {
				foreach (Vector2D id in GetPolygons_Shape_Local()[0].pointsList) {
					meshDistance_shape = Mathf.Max(meshDistance_shape, Vector2.Distance(id.ToVector2(), Vector2.zero));
				}
			}
		}
		return(meshDistance_shape);
	}

	//////////////////////////////////////////////////////////////////////////////// Pair List 
	public List<List<Pair2D>> GetPolygons_Pair_World_ColliderType(Transform transform, VirtualSpriteRenderer spriteRenderer) {
		switch(colliderType) {
			case LightingCollider2D.ColliderType.Collider:
				return(GetPolygons_Pair_Collider_World(transform));

			case LightingCollider2D.ColliderType.SpriteCustomPhysicsShape:
				return(GetPolygons_Pair_Shape_World(transform, spriteRenderer));
		}
		return(null);
	}

	public List<List<Pair2D>> GetPolygons_Pair_Collider_World(Transform transform) {
		if (polygons_collider_world_pair == null) {
			if (polygons_collider_world_pair_cache != null) {
				polygons_collider_world_pair = polygons_collider_world_pair_cache;
				

				// Recalculate Cache !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

			} else {
				LightingDebug.ShadowColliderTotalGenerationsWorld_collider_pair ++;

				polygons_collider_world_pair = new List<List<Pair2D>>();
				foreach(Polygon2D poly in GetPolygons_Collider_World(transform)) {
					polygons_collider_world_pair.Add(Pair2D.GetList(poly.pointsList));
				}

				polygons_collider_world_pair_cache = polygons_collider_world_pair;
			}
		}

		return(polygons_collider_world_pair);
	}

	public List<List<Pair2D>> GetPolygons_Pair_Shape_World(Transform transform, VirtualSpriteRenderer spriteRenderer) {
		if (polygons_shape_world_pair == null) {
			
			if (polygons_shape_world_pair_cache != null) {
				GetPolygons_Shape_World(transform, spriteRenderer);					
			}

			if (polygons_shape_world_pair_cache != null) {
				polygons_shape_world_pair = polygons_shape_world_pair_cache;
		
				
			} else {
				polygons_shape_world_pair = new List<List<Pair2D>>();

				foreach(Polygon2D poly in GetPolygons_Shape_World(transform, spriteRenderer)) {
					polygons_shape_world_pair.Add(Pair2D.GetList(poly.pointsList));
				}

				polygons_shape_world_pair_cache = polygons_shape_world_pair;
			}
		}

		return(polygons_shape_world_pair);
	}

	/////////////////////////////////////////// Mesh Object
	public Mesh GetMesh_MaskType(Transform transform) {
		switch(maskType) {
			case LightingCollider2D.MaskType.Collider:
				return(GetMesh_Collider(transform));

			case LightingCollider2D.MaskType.SpriteCustomPhysicsShape:
				return(GetMesh_Shape());
		}
		return(null);
	}
	
	public Mesh GetMesh_Collider(Transform transform) {
       if (mesh_collider == null) {
			if (GetPolygons_Collider_Local(transform).Count > 0) {
				if (GetPolygons_Collider_Local(transform)[0].pointsList.Count > 2) {
					// Triangulate Polygon List?
					mesh_collider = PolygonTriangulator2D.Triangulate (GetPolygons_Collider_Local(transform)[0], Vector2.zero, Vector2.zero, PolygonTriangulator2D.Triangulation.Advanced);
				}
			}
		}
		return(mesh_collider);
    }

	public Mesh GetMesh_Shape() {
        if (mesh_shape == null) {
            if (GetPolygons_Shape_Local().Count > 0) {
                if (GetPolygons_Shape_Local()[0].pointsList.Count > 2) {
                    // Triangulate Polygon List?
                    mesh_shape = GetPhysicsShape().GetShapeMesh();
                }
            }
           LightingDebug.ConvexHullGenerations ++;
        }
        return(mesh_shape);
    }

	//////////////////////////////////////////// Mesh Vertices
	public MeshVertices GetMesh_Vertices_MaskType(Transform transform) {
		switch(maskType) {
			case LightingCollider2D.MaskType.Collider:
				return(GetMesh_Vertices_Collider(transform));

			case LightingCollider2D.MaskType.SpriteCustomPhysicsShape:
				return(GetMesh_Vertices_Shape(transform));
		}
		return(null);
	}
	
	public MeshVertices GetMesh_Vertices_Collider(Transform transform) {
       if (mesh_collider_vertices == null) {
			Mesh mesh = GetMesh_Collider(transform);

			mesh_collider_vertices = new MeshVertices();

			if (mesh != null) {
				MeshVertice vertice;

				int triangleCount = mesh.triangles.GetLength (0);

				for (int i = 0; i < triangleCount; i = i + 3) {
					vertice = new MeshVertice();
					vertice.a = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i]]));
					vertice.b = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i + 1]]));
					vertice.c = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i + 2]]));

					mesh_collider_vertices.list.Add(vertice);
				}
			}	
		}
		return(mesh_collider_vertices);
    }

	public MeshVertices GetMesh_Vertices_Shape(Transform transform) {
        if (mesh_shape_vertices == null) {
          	Mesh mesh = GetMesh_Shape();

			mesh_shape_vertices = new MeshVertices();

			if (mesh != null) {
				MeshVertice vertice;

				int triangleCount = mesh.triangles.GetLength (0);

				for (int i = 0; i < triangleCount; i = i + 3) {
					vertice = new MeshVertice();
					vertice.a = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i]]));
					vertice.b = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i + 1]]));
					vertice.c = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i + 2]]));

					mesh_shape_vertices.list.Add(vertice);
				}
			}
        }
        return(mesh_shape_vertices);
    }

	//////////////////////////////////////////// Polygon Objects
	public List<Polygon2D> GetPolygons_World_ColliderType(Transform transform, VirtualSpriteRenderer virtualSpriteRenderer) {
		switch(colliderType) {
			case LightingCollider2D.ColliderType.Collider:
				return(GetPolygons_Collider_World(transform));

			case LightingCollider2D.ColliderType.SpriteCustomPhysicsShape:
				return(GetPolygons_Shape_World(transform, virtualSpriteRenderer));
		}
		return(null);
	}

	public List<Polygon2D> GetPolygons_Local_ColliderType(Transform transform) {
		switch(colliderType) {
			case LightingCollider2D.ColliderType.Collider:
				return(GetPolygons_Collider_Local(transform));

			case LightingCollider2D.ColliderType.SpriteCustomPhysicsShape:
				return(GetPolygons_Shape_Local());
		}
		return(null);
	}

	public List<Polygon2D> GetPolygons_Collider_World(Transform transform) {
		if (polygons_collider_world == null) {
			if (polygons_collider_world_cache != null) {
				LightingDebug.ShadowColliderTotalGenerationsWorld_collider_re ++;
				
				polygons_collider_world = polygons_collider_world_cache;

				Polygon2D poly;
				Vector2D point;
				List<Polygon2D> list = GetPolygons_Collider_Local(transform);
				for(int i = 0; i < list.Count; i++) {
					poly = list[i];
					for(int p = 0; p < poly.pointsList.Count; p++) {
						point = poly.pointsList[p];
						
						polygons_collider_world[i].pointsList[p].x = point.x;
						polygons_collider_world[i].pointsList[p].y = point.y;
					}
					polygons_collider_world[i].ToWorldSpaceItself(transform);
				}

			} else {
				LightingDebug.ShadowColliderTotalGenerationsWorld_collider ++;

				polygons_collider_world = new List<Polygon2D>();
				
				foreach(Polygon2D poly in GetPolygons_Collider_Local(transform)) {
					polygons_collider_world.Add(poly.ToWorldSpace(transform));
				}

				 polygons_collider_world_cache = polygons_collider_world;
			}
		}

		return(polygons_collider_world);
	}

	public List<Polygon2D> GetPolygons_Shape_World(Transform transform, VirtualSpriteRenderer virtualSpriteRenderer) {
		if (polygons_shape_world == null) {

			Vector2 scale = new Vector2();
			List<Polygon2D> list = GetPolygons_Shape_Local();

			if (polygons_shape_world_cache != null) {				
				if (list.Count != polygons_shape_world_cache.Count) {
					polygons_shape_world_cache = null;
					polygons_shape_world_pair_cache = null;

				} else {
					for(int i = 0; i < list.Count; i++) {
						if (list[i].pointsList.Count != polygons_shape_world_cache[i].pointsList.Count) {
							polygons_shape_world_cache = null;
							polygons_shape_world_pair_cache = null;

							break;
						}
					}
				}
			}
		
			if (polygons_shape_world_cache != null) {
				LightingDebug.ShadowColliderTotalGenerationsWorld_shape_re ++;

				polygons_shape_world = polygons_shape_world_cache;

				Polygon2D poly;
				Vector2D point;

				for(int i = 0; i < list.Count; i++) {
					poly = list[i];
					for(int p = 0; p < poly.pointsList.Count; p++) {
						point = poly.pointsList[p];
						
						polygons_shape_world[i].pointsList[p].x = point.x;
						polygons_shape_world[i].pointsList[p].y = point.y;
					}

					if (virtualSpriteRenderer != null) {
						scale.x = 1;
						scale.y = 1;

						if (virtualSpriteRenderer.flipX == true) {
							scale.x = -1;
						}

						if (virtualSpriteRenderer.flipY == true) {
							scale.y = -1;
						}
						
						if (virtualSpriteRenderer.flipX != false || virtualSpriteRenderer.flipY != false) {
							polygons_shape_world[i].ToScaleItself(scale);
						}
					}

					polygons_shape_world[i].ToWorldSpaceItself(transform);
				}
			} else {
				LightingDebug.ShadowColliderTotalGenerationsWorld_shape ++;

				Polygon2D polygon;

				polygons_shape_world = new List<Polygon2D>();

				foreach(Polygon2D poly in list) {
					polygon = poly.Copy();

					if (virtualSpriteRenderer != null) {
						scale.x = 1;
						scale.y = 1;

						if (virtualSpriteRenderer.flipX == true) {
							scale.x = -1;
						}

						if (virtualSpriteRenderer.flipY == true) {
							scale.y = -1;
						}
						
						if (virtualSpriteRenderer.flipX != false || virtualSpriteRenderer.flipY != false) {
							polygon.ToScaleItself(scale);
						}
					}
					
					polygon.ToWorldSpaceItself(transform);

					polygons_shape_world.Add(polygon);

					polygons_shape_world_cache = polygons_shape_world;
				}
			}
		}

		return(polygons_shape_world);
	}

	public List<Polygon2D> GetPolygons_Collider_Local(Transform transform) {
		if (polygons_collider_local == null) {
			LightingDebug.ShadowColliderTotalGenerationsLocal_collider ++;
			
			polygons_collider_local = Polygon2DList.CreateFromGameObject (transform.gameObject);
			if (polygons_collider_local.Count > 0) {

			} else {
				Debug.LogWarning("SmartLighting2D: LightingCollider2D object is missing Collider2D Component", transform.gameObject);
			}
		}
		return(polygons_collider_local);
	}

    public List<Polygon2D> GetPolygons_Shape_Local() {
		if (polygons_shape_local == null) {
			LightingDebug.ShadowColliderTotalGenerationsLocal_shape ++;

			polygons_shape_local = new List<Polygon2D>();

			#if UNITY_2018_1_OR_NEWER

				if (customPhysicsShape == null) {
					if (originalSprite == null) {
						return(polygons_shape_local);
					}

					customPhysicsShape = GetPhysicsShape();

					polygons_shape_local = customPhysicsShape.GetShape();
				}

			#endif
	
		}
		return(polygons_shape_local);
	}
}