using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CanEditMultipleObjects]
[CustomEditor(typeof(LightingCollider2D))]
public class LightingCollider2DEditor : Editor {

	override public void OnInspectorGUI() {
		LightingCollider2D script = target as LightingCollider2D;

		script.shape.colliderType = (LightingCollider2D.ColliderType)EditorGUILayout.EnumPopup("Collision Type", script.shape.colliderType);
		
		if (script.shape.colliderType != LightingCollider2D.ColliderType.None) {
			script.lightingCollisionLayer = (LightingLayer)EditorGUILayout.EnumPopup("Collision Layer", script.lightingCollisionLayer);
		} else {
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.EnumPopup("Collision Layer", script.lightingCollisionLayer);
			EditorGUI.EndDisabledGroup();
		}

		script.shape.maskType = (LightingCollider2D.MaskType)EditorGUILayout.EnumPopup("Mask Type", script.shape.maskType);

		if (script.shape.maskType != LightingCollider2D.MaskType.None) {
			script.maskMode = (LightingMaskMode)EditorGUILayout.EnumPopup("Mask Mode", script.maskMode);
			script.lightingMaskLayer = (LightingLayer)EditorGUILayout.EnumPopup("Mask Layer", script.lightingMaskLayer);
		} else {
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.EnumPopup("Mask Mode", script.maskMode);
			EditorGUILayout.EnumPopup("Mask Layer", script.lightingMaskLayer);
			EditorGUI.EndDisabledGroup();
		}

		script.dayHeight = EditorGUILayout.Toggle("Apply Day Shadow", script.dayHeight);
		if (script.dayHeight)  {
			script.height = EditorGUILayout.FloatField("Height", script.height);
			if (script.height < 0) {
				script.height = 0;
			}
		}

		script.generateDayMask = EditorGUILayout.Toggle("Apply Day Mask", script.generateDayMask);

		script.ambientOcclusion = EditorGUILayout.Toggle("Apply Ambient Occlusion", script.ambientOcclusion);
		if (script.ambientOcclusion)  {
			script.smoothOcclusionEdges = EditorGUILayout.Toggle("Smooth Edges", script.smoothOcclusionEdges);
			script.occlusionSize = EditorGUILayout.FloatField("Occlussion Size", script.occlusionSize);
			if (script.occlusionSize < 0) {
				script.occlusionSize = 0;
			}
		}

		script.disableWhenInvisible = EditorGUILayout.Toggle("Disable When Invisible", script.disableWhenInvisible);

		if (GUILayout.Button("Update Collisions")) {
			script.Initialize();
			LightingMainBuffer2D.ForceUpdate();
		}

		if (targets.Length > 1) {
			if (GUILayout.Button("Apply to All")) {
				foreach(Object obj in targets) {
					LightingCollider2D copy = obj as LightingCollider2D;
					if (copy == script) {
						continue;
					}

					copy.shape.colliderType = script.shape.colliderType;
					copy.lightingCollisionLayer = script.lightingCollisionLayer;

					copy.shape.maskType = script.shape.maskType;
					copy.lightingMaskLayer = script.lightingMaskLayer;

					copy.dayHeight = script.dayHeight;
					copy.height = script.height;

					copy.ambientOcclusion = script.ambientOcclusion;
					copy.occlusionSize = script.occlusionSize;

					copy.generateDayMask = script.generateDayMask;

					copy.Initialize();
				}

				LightingMainBuffer2D.ForceUpdate();
			}
		}

		if (GUI.changed && EditorApplication.isPlaying == false) {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}
		
		//script.lighten = EditorGUILayout.Toggle("Lighten", script.lighten);
		//script.darken = EditorGUILayout.Toggle("Darken", script.darken);
	}
}
