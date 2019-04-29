using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class FBOManager : MonoBehaviour
{
    static private FBOManager instance;

	static public List<LightingBuffer2D> GetList() {
		return(LightingBuffer2D.list);
	}

    static public int GetFreeCount() {
        int count = 0;
        foreach(LightingBuffer2D buffer in Object.FindObjectsOfType(typeof(LightingBuffer2D))) {
			if (buffer.free) {
                count ++;
            }
		}
        return(count);
    }

    public void Awake() {
        foreach(LightingBuffer2D buffer in Object.FindObjectsOfType(typeof(LightingBuffer2D))) {
			DestroyImmediate(buffer.gameObject);
		}

		LightingManager2D manager = LightingManager2D.Get();
		if (manager.fixedLightBufferSize && manager.lightingBufferPreload) {
			for(int i = 1; i <= manager.lightingBufferPreloadCount; i++) {
				AddBuffer(LightingManager2D.GetTextureSize(manager.fixedLightTextureSize), null);
			}
		}
    }

	static public int GetCount() {
		return(LightingBuffer2D.list.Count);
	}

    static public FBOManager Get() {
		if (instance != null) {
			return(instance);
		}

		foreach(FBOManager mainBuffer in Object.FindObjectsOfType(typeof(FBOManager))) {
			instance = mainBuffer;
			return(instance);
		}

		GameObject setFBOManager = new GameObject ();
		setFBOManager.transform.parent = LightingManager2D.Get().transform;
		setFBOManager.name = "FBO Manager";
		setFBOManager.layer = LightingManager2D.lightingLayer;

		instance = setFBOManager.AddComponent<FBOManager> ();

		return(instance);
	}

    // Management
	static public LightingBuffer2D AddBuffer(int textureSize, LightingSource2D light) {
        LightingManager2D manager = LightingManager2D.Get();

        if (manager.fixedLightBufferSize) {
            textureSize = LightingManager2D.GetTextureSize(manager.fixedLightTextureSize);
        }

		FBOManager fboManager = manager.fboManager;

		if (fboManager == null) {
			Debug.LogError("Lighting Manager Instance is Out-Dated.");
			Debug.LogError("Try Re-Initializing 'Lighting Manager 2D' Component");
			return(null);
		}

		GameObject buffer = new GameObject ();
		buffer.name = "Buffer " + GetCount();
		buffer.transform.parent = fboManager.transform;
		buffer.layer = LightingManager2D.lightingLayer;

		LightingBuffer2D lightingBuffer = buffer.AddComponent<LightingBuffer2D> ();
		lightingBuffer.Initiate (textureSize);
		lightingBuffer.lightSource = light; // Unnecessary?
        
		if (light != null) {
			lightingBuffer.free = false;
			lightingBuffer.bufferCamera.orthographicSize = light.lightSize;
		} else {
			lightingBuffer.free = true;
		}

		return(lightingBuffer);
	}

	static public LightingBuffer2D PullBuffer(int textureSize, LightingSource2D lightSource) {
		LightingManager2D manager = LightingManager2D.Get();
        if (manager.fixedLightBufferSize) {
            textureSize = LightingManager2D.GetTextureSize(manager.fixedLightTextureSize);
        }

		foreach (LightingBuffer2D id in LightingBuffer2D.GetList()) {
			if (id.free == true && id.textureSize == textureSize) {
				id.bufferCamera.orthographicSize = lightSource.lightSize;
				lightSource.movement.ForceUpdate();
				id.lightSource = lightSource; // Unnecessary?
				id.free = false;
				//id.gameObject.SetActive (true);
				return(id);
			}
		}
			
		return(AddBuffer(textureSize, lightSource));		
	}

    static public void FreeBuffer(LightingBuffer2D buffer) {
        if (buffer == null) {
            return;
        }

        buffer.free = true;
        buffer.lightSource = null;
        buffer.bufferCamera.enabled = false;
	}
}
