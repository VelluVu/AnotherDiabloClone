using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightingMeshRenderer : MonoBehaviour {
 	public static List<LightingMeshRenderer> list = new List<LightingMeshRenderer>();

	public bool free = true;

	public Object owner = null;

	public MeshRenderer meshRenderer = null;
	public MeshFilter meshFilter = null;

	static public int GetCount() {
		return(list.Count);
	}

	public void OnEnable() {
		list.Add(this);
	}

	public void OnDisable() {
		list.Remove(this);
	}

	static public List<LightingMeshRenderer> GetList() {
		return(list);
	}

	public void Initialize() {
		meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.mesh = LightingManager2D.GetRenderMesh();	
	}

	public void LateUpdate() {
		if (owner == null) {
			return;
		}

		string type = owner.GetType().ToString();

		switch(type) {
			case "LightingSource2D":
				LightingSource2D source = (LightingSource2D)owner;
				if (source) {
					if (source.additive == false) {
						meshRenderer.enabled = false;
						owner = null;
						free = true;
						return;
					}
					if (source.enabled == false || source.gameObject.active == false) {
						owner = null;
						free = true;

						meshRenderer.enabled = false;
					} else {
						meshRenderer.enabled = true;
					}
				}
			break;

			case "LightingSpriteRenderer2D":
				LightingSpriteRenderer2D source2 = (LightingSpriteRenderer2D)owner;
				if (source2) {
					if (source2.applyAdditive == false) {
						meshRenderer.enabled = false;
						owner = null;
						free = true;
						return;
					}
					if (source2.enabled == false || source2.gameObject.active == false) {
						owner = null;
						free = true;

						meshRenderer.enabled = false;
					} else {
						meshRenderer.enabled = true;
					}
				}

			break;
		}
	}

	public void UpdateAsLightSource(LightingSource2D id) {
		transform.position = id.transform.position;
       	// transform.rotation = id.transform.rotation; // only if rotation enabled
		transform.localScale = new Vector3(id.lightSize, id.lightSize, 1);

		transform.rotation = Quaternion.Euler(0, 0, 0);
	
		if (id.buffer != null && meshRenderer != null) {
			Color lightColor = id.lightColor;
			lightColor.a = id.additive_alpha;

			id.buffer.GetAdditiveMaterial().SetColor ("_TintColor", lightColor);

			meshRenderer.sortingOrder = id.sortingOrder;
			meshRenderer.sortingLayerName = id.sortingLayer;

			meshRenderer.material = id.buffer.GetAdditiveMaterial();

			meshRenderer.enabled = true;
		}
	}

	public void UpdateAsLightSprite(LightingSpriteRenderer2D id) {

		float rotation = id.offsetRotation;
		if (id.applyTransformRotation) {
			rotation += id.transform.rotation.eulerAngles.z;
		}

		Vector2 position = id.transform.position;
		position.x += id.offsetPosition.x;
		position.y += id.offsetPosition.y;

		Vector2 size = id.offsetScale;

		if (id.applyBlur) {
			size.x *= 2;
			size.y *= 2;
		}

		float spriteSheetUV_X = (float)(id.sprite.texture.width) / id.sprite.rect.width;
		float spriteSheetUV_Y = (float)(id.sprite.texture.height) / id.sprite.rect.height;

		Rect rect = id.sprite.rect;
		//Rect uvRect = new Rect((float)rect.x / sprite.texture.width, (float)rect.y / sprite.texture.height, (float)rect.width / sprite.texture.width , (float)rect.height / sprite.texture.height);

		Vector2 scale = new Vector2(spriteSheetUV_X * rect.width / id.sprite.pixelsPerUnit, spriteSheetUV_Y * rect.height / id.sprite.pixelsPerUnit);

		scale.x = (float)id.sprite.texture.width / id.sprite.rect.width;
		scale.y = (float)id.sprite.texture.height / id.sprite.rect.height;

		size.x /= scale.x;
		size.y /= scale.y;

		size.x *= (float)id.sprite.texture.width / (id.sprite.pixelsPerUnit * 2);
		size.y *= (float)id.sprite.texture.height / (id.sprite.pixelsPerUnit * 2);
		
		if (id.spriteRenderer.flipX) {
			size.x = -size.x;
		}

		if (id.spriteRenderer.flipY) {
			size.y = -size.y;
		}

		Vector2 pivot = id.sprite.pivot;
		pivot.x /= id.sprite.rect.width;
		pivot.y /= id.sprite.rect.height;
		pivot.x -= 0.5f;
		pivot.y -= 0.5f;

		float pivotDist = Mathf.Sqrt(pivot.x * pivot.x + pivot.y * pivot.y);
		float pivotAngle = Mathf.Atan2(pivot.y, pivot.x);

		//float rectAngle = Mathf.Atan2(size.y, size.x);
		//float dist = Mathf.Sqrt(size.x * size.x + size.y * size.y);
		float rot = rotation * Mathf.Deg2Rad + Mathf.PI;

		Vector3 scale2 = id.transform.lossyScale;
		
		// Pivot Pushes Position
		position.x += Mathf.Cos(pivotAngle + rot) * pivotDist * scale2.x;
		position.y += Mathf.Sin(pivotAngle + rot) * pivotDist * scale2.y;

		// Scale
	
		scale2.x *= size.x;
		scale2.y *= size.y;
		scale2.z = 1;



		transform.position = id.transform.position;
       	// transform.rotation = id.transform.rotation; // only if rotation enabled
		transform.localScale = scale2;
		transform.rotation = Quaternion.Euler(0, 0, rotation);
	
		if (meshRenderer != null) {
			Color lightColor = id.color;
			lightColor.a = id.alpha;

			id.GetMaterial().SetColor ("_TintColor", lightColor);

			meshRenderer.sortingOrder = id.sortingOrder;
			meshRenderer.sortingLayerName = id.sortingLayer;

			meshRenderer.material = id.GetMaterial();

			meshRenderer.enabled = true;
		}
	}
}
