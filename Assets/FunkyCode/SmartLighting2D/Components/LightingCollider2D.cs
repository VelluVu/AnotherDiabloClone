using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum LightingMaskMode {Visible, Invisible};


[ExecuteInEditMode]
public class LightingCollider2D : MonoBehaviour {
	public enum MaskType {None, Sprite, Collider, SpriteCustomPhysicsShape, Mesh};

	public enum ColliderType {None, Collider, SpriteCustomPhysicsShape, Mesh};
	
	public delegate void LightCollision2DEvent(LightCollision2D collision);

	public LightingLayer lightingMaskLayer = LightingLayer.Layer1;
	public LightingLayer lightingCollisionLayer = LightingLayer.Layer1;

	public LightingMaskMode maskMode = LightingMaskMode.Visible;

	public event LightCollision2DEvent collisionEvents;

	public LightingShadowCollider shape = new LightingShadowCollider();

	DayLightingShadowCollider dayLightingShadow;

	private LightingColliderMovement movement = new LightingColliderMovement();

	public bool edgeCollider2D = false;

	///// Day Lighting /////
	public bool generateDayMask = false;
	
	public bool dayHeight = false;
	public float height = 1f;

	public bool ambientOcclusion = false;
	public bool smoothOcclusionEdges = false;
	public float occlusionSize = 1f;
	/////////////////////////

	public bool disableWhenInvisible = false;

	public static List<LightingCollider2D> list = new List<LightingCollider2D>();

	public SpriteRenderer spriteRenderer;
	public MeshFilter meshFilter;
	
	public void AddEvent(LightCollision2DEvent collisionEvent) {
		collisionEvents += collisionEvent;
	}

	public void CollisionEvent(LightCollision2D collision) {
		if (collisionEvents != null) {
			collisionEvents (collision);
		}
	}

	public bool isVisibleForLight(LightingBuffer2D buffer) {
		// 1.5f??
		
		if (LightingManager2D.culling && Vector2.Distance(transform.position, buffer.lightSource.transform.position) > shape.GetFrustumDistance(transform) + buffer.lightSource.lightSize * 1.5f) {
			LightingDebug.culled ++;
			return(false);
		}

		return(true);
	}

	public bool InCamera() {
		float cameraSize = LightingMainBuffer2D.Get().cameraSize;
		Camera camera = LightingManager2D.Get().GetCamera();
        return (Vector2.Distance(transform.position, camera.transform.position) < Mathf.Sqrt((cameraSize * 2f) * (cameraSize * 2f)) + shape.GetFrustumDistance(transform));
    }

	public void OnEnable() {
		list.Add(this);

		Initialize();
	}

	public void OnDisable() {
		list.Remove(this);

		float distance = shape.GetFrustumDistance(transform);
		foreach (LightingSource2D id in LightingSource2D.GetList()) {
			if (Vector2.Distance (id.transform.position, transform.position) < distance + id.lightSize) {
				id.movement.ForceUpdate();
			}
		}
	}

	public void OnBecameVisible() {
		if (disableWhenInvisible) {
			if (this.enabled == false) {
				this.enabled = true;
			}
		}	
	}

	public void OnBecameInvisible() {
		if (disableWhenInvisible) {
			if (this.enabled == true) {
				this.enabled = false;
			}
		}
	}

	static public List<LightingCollider2D> GetList() {
		return(list);
	}

	public void Initialize() {
		movement.Reset();

		// Initialize Local Collider

		edgeCollider2D = (GetComponent<EdgeCollider2D>() != null);
		
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer != null) {
			shape.SetOriginalSprite(spriteRenderer.sprite);
		}

		meshFilter = GetComponent<MeshFilter>();
		// lightMesh?
	}

	public bool DrawOrNot(LightingSource2D id) {
		for(int i = 0; i < id.layerCount; i++) {
			switch(id.layerSetting[i].type) {
				case LightingLayerType.Default:
					if (i == (int)lightingCollisionLayer || i == (int)lightingMaskLayer) {
						return(true);
					}
				break;

				case LightingLayerType.MaskOnly:
					if (i == (int)lightingMaskLayer) {
						return(true);
					}
				break;

				case LightingLayerType.ShadowOnly:
					if (i == (int)lightingCollisionLayer) {
						return(true);
					}
				break;
			}
		}
		return(false);
	}

	public void Update() {
		movement.Update(this);

		if (movement.moved) {
			shape.ResetWorld();
			
			float distance = shape.GetFrustumDistance(transform);
		
			foreach (LightingSource2D id in LightingSource2D.GetList()) {
				bool draw = DrawOrNot(id);

				if (draw == false) {
					continue;
				}
				
				if (Vector2.Distance (id.transform.position, transform.position) < distance + id.lightSize) {
					id.movement.ForceUpdate();
				}
			}
		}
	}	
	
	public DayLightingShadowCollider GetDayLightingShadow(float sunDirection) {
		if (dayLightingShadow == null) {
			dayLightingShadow = new DayLightingShadowCollider();
		} else {
			if ((int)sunDirection == (int)dayLightingShadow.sunDirection) {
				return(dayLightingShadow);
			}
		}

		dayLightingShadow.sunDirection = sunDirection;
		dayLightingShadow.polygons.Clear();
		dayLightingShadow.meshes.Clear();

		sunDirection *= Mathf.Deg2Rad;

		Polygon2D poly;
		Mesh mesh;

		List<Polygon2D> polygons = shape.GetPolygons_Local_ColliderType(transform);

		foreach(Polygon2D polygon in polygons) {
			poly = polygon.ToWorldSpace (gameObject.transform);
			poly = Polygon2D.GenerateShadow(poly, sunDirection, height);

			poly.ToOffsetItself(new Vector2D(-gameObject.transform.position));

			dayLightingShadow.polygons.Add(poly.Copy());
			
			mesh = poly.CreateMesh(Vector2.zero, Vector2.zero);

			dayLightingShadow.meshes.Add(mesh);

			LightingDebug.ConvexHullGenerations ++;
		}

		return(dayLightingShadow);
	}
}