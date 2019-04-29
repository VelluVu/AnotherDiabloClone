using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Light Source
public enum LightingSourceTextureSize {px2048, px1024, px512, px256, px128};

// Light Layer
public enum LightRenderingOrder {Default, DistanceToLight, YAxis};
public enum LightingLayer {Layer1, Layer2, Layer3, Layer4, Layer5, Layer6};
public enum LightingLayerType {Default, ShadowOnly, MaskOnly}
public enum LightingLayerEffect {Default, InvisibleBellow};

[System.Serializable]
public class LayerSetting {
	public LightingLayer layerID = LightingLayer.Layer1;
	public LightingLayerType type = LightingLayerType.Default;
	public LightRenderingOrder renderingOrder = LightRenderingOrder.Default;
	public LightingLayerEffect effect = LightingLayerEffect.Default;
	public float maskEffectDistance = 1;
}

[ExecuteInEditMode]
public class LightingSource2D : MonoBehaviour {
	public enum LightSprite {Default, Custom};
	public enum WhenInsideCollider {DrawAbove, DoNotDraw};

	// Settings
	public Color lightColor = new Color(.5f,.5f, .5f, 1);
	public float lightAlpha = 1f;
	public float lightSize = 5f;
	public float lightCoreSize = 0.5f;
	public LightingSourceTextureSize textureSize = LightingSourceTextureSize.px1024;
	public bool enableCollisions = true;
	public bool rotationEnabled = false;
	public bool eventHandling = false;

	public bool additive = false;
	public float additive_alpha = 0.25f;

	public WhenInsideCollider whenInsideCollider = WhenInsideCollider.DrawAbove;

	public bool disableWhenInvisible = false;

	public LightSprite lightSprite = LightSprite.Default;
	public Sprite sprite;

	public int layerCount = 1;
	public LayerSetting[] layerSetting = new LayerSetting[8];

	private bool inScreen = false;

	public LightingBuffer2D buffer = null;
	private Material material;

	public int sortingOrder;
	public string sortingLayer;

	///// Movemnt ////
	public LightingSource2DMovement movement = new LightingSource2DMovement();

	//public bool staticUpdated = false; // Not Necessary

	public float occlusionSize = 15;

	public static Sprite defaultSprite = null;

	public static List<LightingSource2D> list = new List<LightingSource2D>();

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

	static public Sprite GetDefaultSprite() {
		if (defaultSprite == null) {
			defaultSprite = Resources.Load <Sprite> ("Sprites/gfx_light");
		}
		return(defaultSprite);
	}

	public void OnEnable() {
		list.Add(this);
	}

	public void OnDisable() {
		list.Remove(this);

		///// Free Buffer!
		FBOManager.FreeBuffer(buffer);

		buffer = null;
		inScreen = false;
	}

	static public List<LightingSource2D> GetList() {
		return(list);
	}

	public bool InCamera() {
		Camera camera = LightingManager2D.Get().GetCamera();
		float cameraSize = LightingMainBuffer2D.Get().cameraSize;

		if (camera == null) {
			return(false);
		} else {
			return(Vector2.Distance(transform.position, camera.transform.position) < Mathf.Sqrt((cameraSize * 2f) * (cameraSize * 2f)) + lightSize );
		}	
	}

	void Start () {
		SetMaterial ();

		for(int i = 0; i < layerCount; i++) {
			if (layerSetting[i] == null) {
				layerSetting[i] = new LayerSetting();
				layerSetting[i].layerID = LightingLayer.Layer1;
				layerSetting[i].type = LightingLayerType.Default;
				layerSetting[i].renderingOrder = LightRenderingOrder.Default;
			}
		}
	}

	public void SetMaterial() {
		GetMaterial();

		movement.ForceUpdate();
	}

	public Sprite GetSprite() {
		if (sprite == null) {
			sprite = GetDefaultSprite();
		}
		return(sprite);
	}
		
	public Material GetMaterial() {
		if (material == null) {
			material = new Material (Shader.Find (Max2D.shaderPath + "Particles/Multiply"));
			material.mainTexture = GetSprite().texture;
		}
		return(material);
	}

	public LightingBuffer2D GetBuffer() {
		buffer = FBOManager.PullBuffer (LightingManager2D.GetTextureSize(textureSize), this);
		return(buffer);
	}

	void Update() {
		movement.Update(this);

		LightingManager2D manager = LightingManager2D.Get();
		bool disabled = manager.disableEngine;

		if (InCamera()) {
			if (movement.update == true) {
				if (inScreen == false) {
					GetBuffer();

					movement.update = false;
					if (buffer != null) {
						if (disabled == false) {
							buffer.bufferCamera.enabled = true; // //UpdateLightBuffer(True)
							buffer.bufferCamera.orthographicSize = lightSize;
						}
					}

					inScreen = true;
				} else {
					movement.update = false;
					if (buffer != null) {
						if (disabled == false) {
							buffer.bufferCamera.enabled = true; // //UpdateLightBuffer(True)
							buffer.bufferCamera.orthographicSize = lightSize;
						}
					}
				}
			} else {
				if (buffer == null) {
					GetBuffer();

					movement.update = false;
					if (buffer != null) {
						if (disabled == false) {
							buffer.bufferCamera.enabled = true; // //UpdateLightBuffer(True)
							buffer.bufferCamera.orthographicSize = lightSize;
						}
					}
				
					inScreen = true;
				}
			}
		} else {
			///// Free Buffer!
			if (buffer != null) {
				FBOManager.FreeBuffer(buffer);
				buffer = null;
			}
			inScreen = false;
		}
		
		if (eventHandling) {
			Vector2D zero = Vector2D.Zero();	
			float lightSizeSquared = Mathf.Sqrt(lightSize * lightSize + lightSize * lightSize);
	
			List<LightCollision2D> collisions = new List<LightCollision2D>();

			foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
				if (id.shape.colliderType == LightingCollider2D.ColliderType.None) {
					continue;
				}
				//if (LightingManager2D.culling && Vector2.Distance(id.transform.position, transform.position) > id.GetCullingDistance() + lightSize) {
				//	continue;
				//}
				LightCollision2D collision = new LightCollision2D();
				collision.lightSource = this;
				collision.collider = id;
				collision.pointsColliding = id.shape.GetPolygons_Local_ColliderType(id.transform)[0].ToWorldSpace(id.transform).ToOffset (new Vector2D (-transform.position)).pointsList;
				collisions.Add(collision);
			}
			
			foreach (LightingCollider2D id in LightingCollider2D.GetList()) {
				if (LightingManager2D.culling && Vector2.Distance(id.transform.position, transform.position) > id.shape.GetFrustumDistance(id.transform) + lightSize) {
					continue;
				}

				if (id.shape.colliderType == LightingCollider2D.ColliderType.None) {
					continue;
				}

				List<Polygon2D> polygons = id.shape.GetPolygons_Local_ColliderType(id.transform);

				if (polygons.Count < 1) {
					continue;
				}

				foreach(Polygon2D polygon in polygons) {
					Polygon2D poly = polygon.ToWorldSpace (id.gameObject.transform);
					poly.ToOffsetItself (new Vector2D (-transform.position));

					if (poly.PointInPoly (zero)) {
						continue;
					}

					Vector2D vA, pA, vB, pB;
					float angleA, angleB;
					foreach (Pair2D p in Pair2D.GetList(poly.pointsList, false)) {
						vA = p.A.Copy();
						pA = p.A.Copy();

						vB = p.B.Copy();
						pB = p.B.Copy();

						angleA = (float)Vector2D.Atan2 (vA, zero);
						angleB = (float)Vector2D.Atan2 (vB, zero);

						vA.Push (angleA, lightSizeSquared);
						pA.Push (angleA - Mathf.Deg2Rad * occlusionSize, lightSizeSquared);

						vB.Push (angleB, lightSizeSquared);
						pB.Push (angleB + Mathf.Deg2Rad * occlusionSize, lightSizeSquared);

						if (eventHandling) {
							Polygon2D triPoly = new Polygon2D();
							triPoly.AddPoint(p.A);
							triPoly.AddPoint(p.B);
							triPoly.AddPoint(pB);
							triPoly.AddPoint(pA);

							foreach(LightCollision2D col in new List<LightCollision2D>(collisions)) {
								if (col.collider == id) {
									continue;
								}
								foreach(Vector2D point in new List<Vector2D>(col.pointsColliding)) {
									if (triPoly.PointInPoly(point)) {
										col.pointsColliding.Remove(point);
									}
								}
								if (col.pointsColliding.Count < 1) {
									collisions.Remove(col);
								}
							}
						}
					}
					
					//events ++;	
				}
			}

			if (collisions.Count > 0) {
				foreach(LightCollision2D collision in collisions) {
					collision.collider.CollisionEvent(collision);
				}
			}
		}
	}
}
