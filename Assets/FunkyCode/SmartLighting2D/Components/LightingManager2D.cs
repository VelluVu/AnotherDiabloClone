using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
	using UnityEditor;
#endif

public enum SpriteAtlasScale {
	None,
	X2
}

[ExecuteInEditMode] // Only 1 Lighting Manager Allowed
public class LightingManager2D : MonoBehaviour {
	public enum RenderingMode {OnRender = 2, OnPreRender = 0, OnPostRender = 1}
	public enum DayLightingPreset {Default, TopDown, SideScroller, Isometric};
	public enum CameraType {MainCameraTag, Custom};


	private static LightingManager2D instance;

	public CameraType cameraType = CameraType.MainCameraTag;
	public Camera customCamera = null;

	public RenderingMode renderingMode = RenderingMode.OnRender;

	// Mesh Render Settings

	public MeshRendererMode meshRendererMode = null;
	public int sortingLayerID;
	public string sortingLayerName;
	public int sortingLayerOrder;

	public DayLightingPreset preset = DayLightingPreset.Default;

	public Color darknessColor = Color.black;
	public float sunDirection = - 90; // Should't it represent degrees???
	public float shadowDarkness = 1;

	public bool drawDayShadows = true;
	public bool drawRooms = true;
	public bool drawOcclusion = true;
	public bool drawPenumbra = true;
	public bool drawAdditiveLights = true;
	public bool darknessBuffer = true; // Draw Main Buffer

	public float lightingResolution = 1f;

	public bool fixedLightBufferSize = true;
	public LightingSourceTextureSize fixedLightTextureSize = LightingSourceTextureSize.px512;

	//public bool batchColliderMask = false;

	public bool debug = false;
	public bool drawSceneView = false;
	public bool disableEngine = false;

	public bool lightingBufferPreload = false;
	public int lightingBufferPreloadCount = 1;

	// Buffer Instance
	public LightingMainBuffer2D mainBuffer;

	// FBO Instance
	public FBOManager fboManager;

	public MeshRendererManager meshRendererManager;
	
	public bool lightingSpriteAtlas = false;
	public SpriteAtlasManager spriteAtlas;

	public SpriteAtlasScale spriteAtlasScale = SpriteAtlasScale.None;

	public int spriteAtlasPreloadFoldersCount = 0;
	public string[] spriteAtlasPreloadFolders = new string[1];

	public LightingManager2DMaterials materials = new LightingManager2DMaterials();

	public const int lightingLayer = 8;
	public const bool culling = true;

	public int version = 0;
	public const int VERSION = 106;

	public RenderTextureFormat textureFormat = RenderTextureFormat.ARGB32;

	public static Mesh preRenderMesh = null;

	public Camera GetCamera() {
		Camera currentCamera = null;

		switch(cameraType) {
			case CameraType.MainCameraTag:
				return(Camera.main);

			case CameraType.Custom:
				return(customCamera);
		}

		return(currentCamera);
	}
	
	static public LightingManager2D Get() {
		if (instance != null) {
			return(instance);
		}

		foreach(LightingManager2D manager in Object.FindObjectsOfType(typeof(LightingManager2D))) {
			instance = manager;
			return(instance);
		}

		// Create New Light Manager
		GameObject gameObject = new GameObject();
		gameObject.name = "Lighting Manager 2D";

		instance = gameObject.AddComponent<LightingManager2D>();
		instance.Initialize();

		return(instance);
	}

	public static Mesh GetRenderMesh() {
		if (preRenderMesh == null) {
			Polygon2D tilePoly = Polygon2D.CreateFromRect(new Vector2(1f, 1f));
			Mesh staticTileMesh = tilePoly.CreateMesh(new Vector2(2f, 2f), Vector2.zero);
			preRenderMesh = staticTileMesh;
		}
		return(preRenderMesh);
	}

	static public int GetTextureSize(LightingSourceTextureSize textureSize) {
		switch(textureSize) {
			case LightingSourceTextureSize.px2048:
				return(2048);

			case LightingSourceTextureSize.px1024:
				return(1024);

			case LightingSourceTextureSize.px512:
				return(512);

			case LightingSourceTextureSize.px256:
				return(256);
			
			default:
				return(128);
		}
	}

	public void Initialize () {
		instance = this;

		transform.position = Vector3.zero;

		mainBuffer = LightingMainBuffer2D.Get(); 

		fboManager = FBOManager.Get();

		spriteAtlas = SpriteAtlasManager.Get();

		meshRendererManager = MeshRendererManager.Get();

		version = VERSION;
	}

	void DrawAdditiveLights() {
		foreach (LightingSource2D id in LightingSource2D.GetList()) {
			if (id.buffer == null) {
				continue;
			}

			if (id.isActiveAndEnabled == false) {
				continue;
			}

			if (id.buffer.bufferCamera == null) {
				continue;
			}

			if (id.InCamera() == false) {
				continue;
			}

			if (id.additive == false) {
				continue;
			}

			if (id.buffer.GetAdditiveMaterial()) {
				LightingMeshRenderer lightingMesh = MeshRendererManager.Pull(id);

				lightingMesh.UpdateAsLightSource(id);
			}
		}
	}

	public void Start() {
		materials.Initialize();
		
		if (instance != null && instance != this) {
			DestroyImmediate(gameObject);
			Debug.LogWarning("Smartlighting2D: Lighting Manager duplicate was found and destroyed.", gameObject);
		}
	}

	void Update() {
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.L)) {
			debug = !debug;
		}

		if (disableEngine) {
			mainBuffer.enabled = false;
			mainBuffer.bufferCamera.enabled = false;
			meshRendererMode.meshRenderer.enabled = false;
			return;
		}
		
		if (darknessBuffer == false) {
			meshRendererMode.meshRenderer.enabled = false;
		}

		if (drawAdditiveLights) {
			DrawAdditiveLights();
		}

		meshRendererMode = MeshRendererMode.Get();
		if (renderingMode != RenderingMode.OnRender) {
			meshRendererMode.meshRenderer.enabled = false;
		}

		if (mainBuffer != null) {
			if (darknessBuffer) {
				mainBuffer.enabled = true;
				mainBuffer.bufferCamera.enabled = true;
			} else {
				mainBuffer.enabled = false;
				mainBuffer.bufferCamera.enabled = false;
			}
		}


		if (Render_Check() == false) {
			return;
		}

		Render_MeshRenderMode();
	}

	// Post-Rendering Mode Drawing	
	public void OnRenderObject() {
		if (disableEngine) {
			return;
		}
		Render_PostRenderMode();
	}

	public bool Render_Check() {
		if (darknessBuffer == false) {
			return(false);
		}

		if (mainBuffer == null) {
			mainBuffer = LightingMainBuffer2D.Get();
			return(false);
		}	

		if (mainBuffer.bufferCamera == null) {
			return(false);
		}

		return(true);
	}

	public static Vector3 Render_Size() {
		LightingManager2D  manager = Get();
		float sizeY = manager.mainBuffer.bufferCamera.orthographicSize;
		Vector3 size = new Vector2(sizeY, sizeY);
		
		size.x *= ((float)Screen.width / (float)Screen.height);

		size.x *= (((float)Screen.width + 1f) / Screen.width);

		// Necessary?
		size.y *= (((float)Screen.height + 1f) / Screen.height);
		
		size.z = 1;

		return(size);
	}

	public static Vector3 Render_Position() {
		Camera camera = LightingManager2D.Get().GetCamera();
		Vector3 pos = camera.transform.position;
		pos.z += camera.nearClipPlane + 0.1f;
		return(pos);
	}

	public void Render_MeshRenderMode() {
		// Mesh-Render Mode Drawing
		if (Render_Check() == false) {
			return;
		}

		if (renderingMode != RenderingMode.OnRender) {
			return;
		}

		if (meshRendererMode == null) {
			return;
		}

		if (meshRendererMode.meshRenderer != null) {	
			meshRendererMode.meshRenderer.sortingLayerID = sortingLayerID;
			meshRendererMode.meshRenderer.sortingLayerName= sortingLayerName;
			meshRendererMode.meshRenderer.sortingOrder = sortingLayerOrder;
		}

		meshRendererMode.meshRenderer.enabled = true;
		if (meshRendererMode.meshRenderer.sharedMaterial != mainBuffer.material) {
			meshRendererMode.meshRenderer.sharedMaterial = mainBuffer.material;
		}
	}

	void Render_PostRenderMode() {
		// Post-Render Mode Drawing
		if (Render_Check() == false) {
			return;
		}

		if (renderingMode != RenderingMode.OnPostRender) {
			return;
		}

		Camera camera = GetCamera();

		if (Camera.current != camera) {
			return;
		}

		LightingDebug.LightMainCameraUpdates +=1;

		Max2D.DrawImage(mainBuffer.material, Render_Position(), Render_Size(), camera.transform.eulerAngles.z, Render_Position().z);
	}

	public void Render_PreRenderMode() {
		if (Render_Check() == false) {
			return;
		}

		if (renderingMode != RenderingMode.OnPreRender) {
			return;
		}

		LightingDebug.LightMainCameraUpdates +=1;

		Camera camera = GetCamera();

		Quaternion rotation = camera.transform.rotation;

		Graphics.DrawMesh(GetRenderMesh(), Matrix4x4.TRS(Render_Position(), rotation, Render_Size()), mainBuffer.material, 0);
	}

	static public float GetSunDirection() {
		return(Get().sunDirection * Mathf.Deg2Rad);
	}

	void OnGUI() {
		if (debug) {
			LightingDebug.OnGUI();
		}
	}

	#if UNITY_EDITOR
		
		public void OnEnable() {
			SceneView.onSceneGUIDelegate += OnSceneGUI;
		}

		public void OnDisable() {
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
		}

		private void OnSceneGUI(SceneView view) {
			if (drawSceneView == false) {
				return;
			}
			
			foreach (LightingSource2D id in LightingSource2D.GetList()) {
				if (id.buffer == null) {
					continue;
				}

				if (id.isActiveAndEnabled == false) {
					continue;
				}

				if (id.buffer.bufferCamera == null) {
					continue;
				}

				//if (id.buffer.material) {
				//	Color lightColor = id.lightColor;
				//	lightColor.a = id.lightAlpha / 4;
				//	id.buffer.material.SetColor ("_TintColor", lightColor);

				//	Graphics.DrawMesh(GetRenderMesh(), Matrix4x4.TRS(id.transform.position, Quaternion.Euler(0, 0, 0),  id.transform.lossyScale * id.lightSize), id.buffer.material, 0);
				//}
			}
		}
	
	#endif 
}

/* 
	void DrawLightingBuffers(float z) {
		
	}*/

			/* Draw Lights In Scene View
		Polygon2D rect = Polygon2D.CreateFromRect(new Vector2(1, 1));
		Mesh mesh = rect.CreateMesh(new Vector2(2, 2), Vector2.zero);

		
		foreach (LightingBuffer2D id in FBOManager.GetList()) {
			if (id.lightSource == null) {
				continue;
			}

			if (id.lightSource.isActiveAndEnabled == false) {
				continue;
			}

       		Draw(id, mesh);
		} */	
	/*
    private void Draw(LightingBuffer2D id, Mesh mesh) {
        if (mesh && id.material) {
			Color lightColor = id.lightSource.lightColor;
			lightColor.a = id.lightSource.lightAlpha / 4;
			id.material.SetColor ("_TintColor", lightColor);
			
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(id.lightSource.transform.position, id.lightSource.transform.rotation,  id.lightSource.transform.lossyScale * id.lightSource.lightSize), id.material, 0);
        }
    } */
