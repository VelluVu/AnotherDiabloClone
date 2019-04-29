using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(LightingRoom2D))]
public class LightingRoom2DEditor :Editor {
    	override public void OnInspectorGUI() {
		    LightingRoom2D script = target as LightingRoom2D;

            script.roomType = (LightingRoom2D.RoomType)EditorGUILayout.EnumPopup("Room Type", script.roomType);

            script.color = EditorGUILayout.ColorField("Color", script.color);
        }
}
