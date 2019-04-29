using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_2018_1_OR_NEWER
using UnityEngine.Tilemaps;
#endif

using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(LightingManager2D))]
public class LightingManager2DEditor : Editor {

	static bool foldout_daylighting = false;
	static bool foldout_lightingAtlas = false;
	static bool foldout_sortingLayer = false;
	static bool foldout_settings = false;
	static bool foldout_lightingBuffers = false;

	[MenuItem("GameObject/2D Light/Light Source", false, 4)]
    static void CreateLightSource(){
		Ray worldRay = SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1.0f));
		
		GameObject newGameObject = new GameObject("2D Light Source");

		Vector3 pos = worldRay.origin;
		pos.z = 0;

		newGameObject.AddComponent<LightingSource2D>();

		newGameObject.transform.position = pos;
	}

	[MenuItem("GameObject/2D Light/Light Collider", false, 4)]
    static void CreateLightCollider(){
		Ray worldRay = SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1.0f));
		
		GameObject newGameObject = new GameObject("2D Light Collider");

		Vector3 pos = worldRay.origin;
		pos.z = 0;

		newGameObject.AddComponent<PolygonCollider2D>();

		newGameObject.AddComponent<LightingCollider2D>();

		newGameObject.transform.position = pos;
    }

	#if UNITY_2018_1_OR_NEWER

	[MenuItem("GameObject/2D Light/Light Tilemap Collider", false, 4)]
    static void CreateLightTilemapCollider(){
		GameObject newGrid = new GameObject("2D Light Grid");
		newGrid.AddComponent<Grid>();

		GameObject newGameObject = new GameObject("2D Light Tilemap");
		newGameObject.transform.parent = newGrid.transform;

		newGameObject.AddComponent<Tilemap>();
		newGameObject.AddComponent<LightingTilemapCollider2D>();
    }

	#endif

	[MenuItem("GameObject/2D Light/Light Sprite Renderer", false, 4)]
    static void CreateLightSpriteRenderer(){
		Ray worldRay = SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1.0f));
		
		GameObject newGameObject = new GameObject("2D Light Sprite Renderer");

		Vector3 pos = worldRay.origin;
		pos.z = 0;

		newGameObject.AddComponent<LightingSpriteRenderer2D>();

		newGameObject.transform.position = pos;
    }

	[MenuItem("GameObject/2D Light/Light Manager", false, 4)]
    static void CreateLightManager(){
		LightingManager2D.Get();
    }

	override public void OnInspectorGUI() {
		LightingManager2D script = target as LightingManager2D;

		script.cameraType = (LightingManager2D.CameraType)EditorGUILayout.EnumPopup("Camera Type", script.cameraType);

		if (script.cameraType == LightingManager2D.CameraType.Custom) {
			script.customCamera = (Camera)EditorGUILayout.ObjectField(script.customCamera, typeof(Camera), true);
		}

		script.renderingMode = (LightingManager2D.RenderingMode)EditorGUILayout.EnumPopup("Rendering Mode", script.renderingMode);

		if (script.renderingMode == LightingManager2D.RenderingMode.OnRender) {
			foldout_sortingLayer = EditorGUILayout.Foldout(foldout_sortingLayer, "Sorting Layer" );
			if (foldout_sortingLayer) {
				EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
				script.sortingLayerID = (int)EditorGUILayout.Slider("Sorting Layer ID", script.sortingLayerID, 0, 31);
				script.sortingLayerOrder = EditorGUILayout.IntField("Sorting Layer Order", script.sortingLayerOrder);
				script.sortingLayerName = EditorGUILayout.TextField("Sorting Layer Name", script.sortingLayerName);
				EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
			}
		}

		foldout_settings = EditorGUILayout.Foldout(foldout_settings, "Settings" );
		if (foldout_settings) {
			EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
			Color newDarknessColor = EditorGUILayout.ColorField("Darkness Color", script.darknessColor);
			if (script.darknessColor.Equals(newDarknessColor) == false) {
				script.darknessColor = newDarknessColor;

				LightingMainBuffer2D.ForceUpdate();	
			}

			script.drawAdditiveLights = EditorGUILayout.Toggle("Draw Additive Lights", script.drawAdditiveLights);

			script.drawPenumbra = EditorGUILayout.Toggle("Draw Penumbra", script.drawPenumbra);

			script.drawRooms = EditorGUILayout.Toggle("Draw Rooms", script.drawRooms);

			script.drawOcclusion = EditorGUILayout.Toggle("Draw Occlusion", script.drawOcclusion);

			script.darknessBuffer = EditorGUILayout.Toggle("Draw Darkness Buffer", script.darknessBuffer);

			script.drawSceneView = EditorGUILayout.Toggle("Draw Scene View", script.drawSceneView);

			EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
		}

		foldout_lightingBuffers = EditorGUILayout.Foldout(foldout_lightingBuffers, "Lighting Buffers" );
		if (foldout_lightingBuffers) {
			script.lightingResolution = EditorGUILayout.Slider("Lighting Resolution", script.lightingResolution, 0.125f, 1.0f);
			
			script.textureFormat = (RenderTextureFormat)EditorGUILayout.EnumPopup("Light Texture Format", script.textureFormat);

			script.fixedLightBufferSize = EditorGUILayout.Toggle("Fixed Light Buffer", script.fixedLightBufferSize);

			if (script.fixedLightBufferSize) {
				script.fixedLightTextureSize = (LightingSourceTextureSize)EditorGUILayout.EnumPopup("Fixed Light Buffer Size", script.fixedLightTextureSize);
				script.lightingBufferPreload = EditorGUILayout.Toggle("Pre Load Lights", script.lightingBufferPreload);
				script.lightingBufferPreloadCount = (int)EditorGUILayout.Slider("Pre Load Count", script.lightingBufferPreloadCount, 1, 50);
			} else {
				EditorGUI.BeginDisabledGroup(true);
				EditorGUILayout.EnumPopup("Fixed Light Buffer Size", script.fixedLightTextureSize);
				EditorGUILayout.Toggle("Pre Load Lights", script.lightingBufferPreload);
				EditorGUILayout.Slider("Pre Load Count", script.lightingBufferPreloadCount, 1, 50);
				EditorGUI.EndDisabledGroup();
			}
		
			

		}

		foldout_daylighting = EditorGUILayout.Foldout(foldout_daylighting, "Day Lighting" );
		if (foldout_daylighting) {
			EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

			// Day Lighting Settings
			EditorGUI.BeginDisabledGroup(true);
			script.preset = LightingManager2D.DayLightingPreset.Default;
			EditorGUILayout.EnumPopup("Preset", LightingManager2D.DayLightingPreset.Default);
			EditorGUI.EndDisabledGroup();

			float newSunDirection = EditorGUILayout.FloatField("Sun Rotation", script.sunDirection);
			if (newSunDirection != script.sunDirection) {
				script.sunDirection = newSunDirection;

				LightingMainBuffer2D.ForceUpdate();
			}

			script.drawDayShadows = EditorGUILayout.Toggle("Cast Shadows", script.drawDayShadows);

			float newShadowDarkness = EditorGUILayout.Slider("Shadow Darkness", script.shadowDarkness, 0, 1);
			if (newShadowDarkness != script.shadowDarkness) {
				script.shadowDarkness = newShadowDarkness;

				LightingMainBuffer2D.ForceUpdate();
			}

			EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
		}

		foldout_lightingAtlas = EditorGUILayout.Foldout(foldout_lightingAtlas, "Lighting Atlas" );
		if (foldout_lightingAtlas) {
			EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
			script.lightingSpriteAtlas = EditorGUILayout.Toggle("Enable Atlas", script.lightingSpriteAtlas);
			script.spriteAtlasScale = (SpriteAtlasScale)EditorGUILayout.EnumPopup("Sprites Scaling", script.spriteAtlasScale);
			
			
			
			
			script.spriteAtlasPreloadFoldersCount = EditorGUILayout.IntField("Folder Count", script.spriteAtlasPreloadFoldersCount);

			EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
			for(int i = 0; i < script.spriteAtlasPreloadFoldersCount; i++) {
				if (script.spriteAtlasPreloadFolders.Length <= i) {
					System.Array.Resize(ref script.spriteAtlasPreloadFolders, i + 1);
				}
			//	if (script.spriteAtlasPreloadFolders[i] == null) {
				//	script.spriteAtlasPreloadFolders[i] = "";
				//}
				script.spriteAtlasPreloadFolders[i] = EditorGUILayout.TextField("Folder (" + (i + 1) + ")", script.spriteAtlasPreloadFolders[i]);
			}
			EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
	
			
			
			
			
			
			
			
			EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
		}
		
		script.debug = EditorGUILayout.Toggle("Debug", script.debug);
		script.disableEngine = EditorGUILayout.Toggle("Disable Engine", script.disableEngine);

		string buttonName = "Re-Initialize";
		if (script.version < LightingManager2D.VERSION) {
			buttonName += " (Outdated)";
			GUI.backgroundColor = Color.red;
		}
		
		if (GUILayout.Button(buttonName)) {
			if (script.mainBuffer != null && script.fboManager != null && script.meshRendererMode != null) {
				DestroyImmediate(script.mainBuffer.gameObject);
				DestroyImmediate(script.fboManager.gameObject);
				DestroyImmediate(script.meshRendererMode.gameObject);

				script.Initialize();
			} else {
				DestroyImmediate(script.gameObject);
				LightingManager2D.Get();
			}
			
			LightingMainBuffer2D.ForceUpdate();
		}

		if (GUI.changed && EditorApplication.isPlaying == false) {
			if (target != null) {
				EditorUtility.SetDirty(target);
			}
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}
	}
}