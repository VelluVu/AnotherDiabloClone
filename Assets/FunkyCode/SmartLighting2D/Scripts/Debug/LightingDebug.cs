using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingDebug {
    static public int LightBufferUpdates = 0;
    static private int ShowLightBufferUpdates = 0;

    static public int LightMainBufferUpdates = 0;
    static private int ShowLightMainBufferUpdates = 0;

    static public int LightMainCameraUpdates = 0;
    static private int ShowLightMainCameraUpdates = 0;

    static public int SpriteRenderersDrawn = 0;
    static private int ShowSpriteRenderersDrawn = 0;

    static public int ConvexHullGenerations = 0;
    static private int ShowConvexHullGenerations = 0;

    static public int maskGenerations = 0;
    static private int show_maskGenerations = 0;

    static public int shadowGenerations = 0;
    static private int show_shadowGenerations = 0;

    static public int culled = 0;
    static private int show_culled = 0;

    static public int totalLightUpdates = 0;

    static public int totalObjectMaskShapeGenerations = 0;
    static public int totalObjectMaskMeshGenerations = 0;

    static public int ShadowColliderTotalGenerationsLocal_shape = 0;
    static public int ShadowColliderTotalGenerationsWorld_shape = 0;
    static public int ShadowColliderTotalGenerationsWorld_shape_re = 0;
    static public int ShadowColliderTotalGenerationsWorld_shape_pair = 0;


    static public int ShadowColliderTotalGenerationsLocal_collider = 0;

    static public int ShadowColliderTotalGenerationsWorld_collider = 0;
    static public int ShadowColliderTotalGenerationsWorld_collider_re = 0;
    static public int ShadowColliderTotalGenerationsWorld_collider_pair = 0;

    static public int NewRenderTextures = 0;
   

    
    static public TimerHelper timer;

    static Object[] lights = null;
    static Object[] colliders = null;
    static Object[] sprites = null;
    static Object[] tilemaps = null;

    static public void OnGUI() {
        if (lights == null) {
            lights = Object.FindObjectsOfType(typeof(LightingSource2D));
            colliders = Object.FindObjectsOfType(typeof(LightingCollider2D));
            sprites = Object.FindObjectsOfType(typeof(LightingSpriteRenderer2D));
            tilemaps = Object.FindObjectsOfType(typeof(LightingTilemapCollider2D));
        }

        if (timer == null) {
            LightingDebug.timer = TimerHelper.Create();
        }

        if (timer.GetMillisecs() > 1000) {
            SecondUpdate();
        }

        ShowUpdate();

        int count = 0;
        foreach(LightingSource2D light in LightingSource2D.GetList()) {
            if (light.InCamera() == false) {
                continue;
            }
            count ++;
        }

        LightingManager2D manager2D = LightingManager2D.Get();
        
        int y = 10;

        GUI.Label(new Rect(10, y, 500, 20), "Camera Size: " + manager2D.mainBuffer.cameraSize);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "New Render Textures: " + NewRenderTextures);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Lights in Camera: " + count + "/" + LightingSource2D.GetList().Count);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Free Buffers: " + FBOManager.GetFreeCount() + "/" + FBOManager.GetList().Count);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Light Buffer Updates: " + ShowLightBufferUpdates); // In Frame

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Total Light Updates: " + totalLightUpdates);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "=========================");

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Mask Generations: " + show_maskGenerations);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Shadow Generations: " + show_shadowGenerations);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Objects Culled: " + show_culled);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "=========================");

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Shadow Collider World Generations C: " + ShadowColliderTotalGenerationsWorld_collider + " (re: " + ShadowColliderTotalGenerationsWorld_collider_re +") Pair: " + LightingDebug.ShadowColliderTotalGenerationsWorld_collider_pair);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Shadow Collider Local Generations C: " + ShadowColliderTotalGenerationsLocal_collider);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Shadow Collider World Generations S: " + ShadowColliderTotalGenerationsWorld_shape + " (re: " + ShadowColliderTotalGenerationsWorld_shape_re +") Pair: " + LightingDebug.ShadowColliderTotalGenerationsWorld_shape_pair);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Shadow Collider Local Generations S: " + ShadowColliderTotalGenerationsLocal_shape);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Total Object SHAPE Generations: " + totalObjectMaskShapeGenerations);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Total Object MESH Generations : " + totalObjectMaskMeshGenerations);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "=========================");

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Convex Hull Generations: " + ShowConvexHullGenerations);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "=========================");

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Sprite Renderers Drawn: " + ShowSpriteRenderersDrawn);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "=========================");

        y += 10;
        
        GUI.Label(new Rect(10, y, 500, 20), "Light Main Buffer Updates: " + ShowLightMainBufferUpdates);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Light Main Camera Updates: " + ShowLightMainCameraUpdates);

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "=========================");

        y += 10;

        Texture texture = LightingMainBuffer2D.Get().bufferCamera.activeTexture;
        if (texture != null) {
            GUI.Label(new Rect(10, y, 500, 20), "Main Buffer Resolution: " + texture.width + "x" + texture.height);
        } else {
            GUI.Label(new Rect(10, y, 500, 20), "Main Buffer Resolution: NULL");
        }

        y += 10;

        GUI.Label(new Rect(10, y, 500, 20), "Glow Particles Generated: " + BlurManager.dictionary.Count);  

        RightBottomPanel() ;    
    }

    public static void RightBottomPanel() {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.LowerRight;
        style.normal.textColor = Color.white;
        style.fontSize = 13;

        #if UNITY_2018_1_OR_NEWER
            GUI.Label(new Rect(0, -10, Screen.width - 10, Screen.height), "Tilemap Collider Count: " + tilemaps.Length, style);
        #endif

        GUI.Label(new Rect(0, -30, Screen.width - 10, Screen.height), "Lights Count: " + lights.Length, style);
        GUI.Label(new Rect(0, -50, Screen.width - 10, Screen.height), "Colliders Count: " + colliders.Length, style);
        GUI.Label(new Rect(0, -70, Screen.width - 10, Screen.height), "Sprite Renderers Count: " + sprites.Length, style);
    }

    
   static public void SecondUpdate() {
         ShowLightMainBufferUpdates = LightMainBufferUpdates;

        LightMainBufferUpdates = 0;

        ShowLightMainCameraUpdates = LightMainCameraUpdates;

        LightMainCameraUpdates = 0;

        ShowConvexHullGenerations = 0;

        ShowSpriteRenderersDrawn = 0;

         ShowLightBufferUpdates = 0;

        timer = TimerHelper.Create();

        lights = Object.FindObjectsOfType(typeof(LightingSource2D));
        colliders = Object.FindObjectsOfType(typeof(LightingCollider2D));
        sprites = Object.FindObjectsOfType(typeof(LightingSpriteRenderer2D));
   }

   static public void ShowUpdate() {
        if (LightBufferUpdates > 0) {
            ShowLightBufferUpdates = LightBufferUpdates;
            LightBufferUpdates = 0;
        }
                   
        if (SpriteRenderersDrawn > 0) {
            ShowSpriteRenderersDrawn = SpriteRenderersDrawn;
            SpriteRenderersDrawn = 0;
        }

        if (ConvexHullGenerations > 0) {
            ShowConvexHullGenerations = ConvexHullGenerations;
            ConvexHullGenerations = 0;
        }

        if (maskGenerations > 0) {
            show_maskGenerations = maskGenerations;
            maskGenerations = 0;
        }

        if (shadowGenerations > 0) {
            show_shadowGenerations = shadowGenerations;
            shadowGenerations = 0;
        }

        if (culled > 0) {
            show_culled = culled;
            culled = 0;
        }
   }
}
