using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PartiallyBatched_Tilemap {
	public VirtualSpriteRenderer virtualSpriteRenderer;
	public Vector2 polyOffset; 
	public Vector2 tileSize;
	public LightingTilemapCollider2D tilemap;
}

public class PartiallyBatched_Collider {
	public LightingCollider2D collider2D;
}

[ExecuteInEditMode]
public class LightingBuffer2D : MonoBehaviour {
	public RenderTexture renderTexture;
	public int textureSize = 0;

	private Material material;
	private Material additiveMaterial;

	public LightingSource2D lightSource;
	public Camera bufferCamera;

	public List<PartiallyBatched_Collider> partiallyBatchedList_Collider = new List<PartiallyBatched_Collider>();
	public List<PartiallyBatched_Tilemap> partiallyBatchedList_Tilemap = new List<PartiallyBatched_Tilemap>();

	public bool free = true;

	public static List<LightingBuffer2D> list = new List<LightingBuffer2D>();

	public void OnEnable() {
		list.Add(this);
	}

	public void OnDisable() {
		list.Remove(this);
	}

	static public List<LightingBuffer2D> GetList() {
		return(list);
	}

	static public int GetCount() {
		return(list.Count);
	}

	public void CalculateCoords() {
		Sprite fillSprite = LightingManager2D.Get().materials.GetAtlasWhiteMaskSprite();

		if (fillSprite != null) {
			Rect uvRect = new Rect((float)fillSprite.rect.x / fillSprite.texture.width, (float)fillSprite.rect.y / fillSprite.texture.height, (float)fillSprite.rect.width / fillSprite.texture.width , (float)fillSprite.rect.height / fillSprite.texture.height);
			uvRect.x += uvRect.width / 2;
			uvRect.y += uvRect.height / 2;
			
			Max2DMatrix.c_x = uvRect.x;
			Max2DMatrix.c_y = uvRect.y;
		}
	}

	public void Initiate (int textureSize) {
		SetUpRenderTexture (textureSize);
		SetUpCamera ();
	}

	void SetUpRenderTexture(int _textureSize) {
		textureSize = _textureSize;

		LightingDebug.NewRenderTextures ++;
		
		renderTexture = new RenderTexture(textureSize, textureSize, 16, LightingManager2D.Get().textureFormat);

		name = "Buffer " + GetCount() + " (size: " + textureSize + ")";
	}

	public Material GetMaterial() {
		if (material == null) {
			material = new Material (Shader.Find (Max2D.shaderPath + "Particles/Additive"));
			material.mainTexture = renderTexture;
		}
		return(material);
	}

	public Material GetAdditiveMaterial() {
		if (additiveMaterial == null) {
			additiveMaterial = new Material (Shader.Find (Max2D.shaderPath + "Particles/Additive"));
			additiveMaterial.mainTexture = renderTexture;
		}
		return(additiveMaterial);
	}

	void SetUpCamera() {
		bufferCamera = gameObject.AddComponent<Camera> ();
		bufferCamera.clearFlags = CameraClearFlags.Color;
		bufferCamera.backgroundColor = Color.white;
		bufferCamera.cameraType = CameraType.Game;
		bufferCamera.orthographic = true;
		bufferCamera.targetTexture = renderTexture;
		bufferCamera.farClipPlane = 0.5f;
		bufferCamera.nearClipPlane = 0f;
		bufferCamera.allowHDR = false;
		bufferCamera.allowMSAA = false;
		bufferCamera.enabled = false;
	}

	void LateUpdate() {
		float cameraZ = -1000f;

		Camera camera = LightingManager2D.Get().GetCamera();

		if (camera != null) {
			cameraZ = camera.transform.position.z - 10 - GetCount();
		}

		bufferCamera.transform.position = new Vector3(0, 0, cameraZ);

		transform.rotation = Quaternion.Euler(0, 0, 0);
	}
	
	public void OnRenderObject() {
		if(Camera.current != bufferCamera) {
			return;
		}

		Max2D.Check(); // Is This Needed Now?

		LateUpdate ();

		for (int layer = 0; layer < lightSource.layerCount; layer++) {
			if (lightSource.layerSetting == null || lightSource.layerSetting.Length < layer) {
				continue;
			}

			if (lightSource.layerSetting[layer] == null) {
				continue;
			}

			int layerID = (int)lightSource.layerSetting[layer].layerID;
			if (layerID < 0) {
				continue;
			}
			
			if (lightSource.enableCollisions) {	
				switch(lightSource.layerSetting[layer].renderingOrder) {
					case LightRenderingOrder.Default:
						DrawLighting_Default(layerID);
						break;

					case LightRenderingOrder.DistanceToLight:
					case LightRenderingOrder.YAxis:
						DrawLighting_Depth(layerID);
						break;
				}
			}
		}
	
		LightingSourceTexture.Draw(this);

		LightingDebug.LightBufferUpdates ++;
		LightingDebug.totalLightUpdates ++;

		bufferCamera.enabled = false;
	}

	void DrawLighting_Default(int layer) {
		LightingBufferDefault.DrawShadowsAndMask(this, layer);
	}

	void DrawLighting_Depth(int layer) {
		LightingBufferSorted.DrawShadowsAndMask(this, layer);
	}
}