using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]
public class LightingManager2DMaterials {
	private Material occlusionEdge;
	private Material occlusionBlur;
	private Material shadowBlur;
	private Material additive;

	private Material whiteSprite;
	private Material blackSprite;

	private Sprite penumbraSprite;
	private Sprite atlasPenumbraSprite;

	private Sprite whiteMaskSprite;
	private Sprite atlasWhiteMaskSprite;

	private Material atlasMaterial;

	public Sprite GetPenumbraSprite() {
		if (penumbraSprite == null) {
			penumbraSprite = Resources.Load<Sprite>("textures/penumbra"); 
		}
		return(penumbraSprite);
	}

	public Sprite GetAtlasPenumbraSprite() {
		if (atlasPenumbraSprite == null) {
			atlasPenumbraSprite = SpriteAtlasManager.RequestSprite(GetPenumbraSprite(), SpriteRequest.Type.BlackAlpha);
		}
		return(atlasPenumbraSprite);
	}

	public Sprite GetWhiteMaskSprite() {
		if (whiteMaskSprite == null) {
			whiteMaskSprite = Resources.Load<Sprite>("textures/white"); 
		}
		return(whiteMaskSprite);
	}

	public Sprite GetAtlasWhiteMaskSprite() {
		if (atlasWhiteMaskSprite == null) {
			atlasWhiteMaskSprite = SpriteAtlasManager.RequestSprite(GetWhiteMaskSprite(), SpriteRequest.Type.WhiteMask);
		}
		return(atlasWhiteMaskSprite);
	}

	public void Initialize() {
		GetPenumbraSprite();
		GetAtlasPenumbraSprite();

		GetWhiteMaskSprite();

		GetAtlasWhiteMaskSprite();

		GetAdditive();
		GetOcclusionBlur();
		GetOcclusionEdge();
		GetShadowBlur();
		GetWhiteSprite();
		GetBlackSprite();
		GetAtlasMaterial();
	}

	public Material GetAtlasMaterial() {
		if (atlasMaterial == null) {
			atlasMaterial = new Material (Shader.Find (Max2D.shaderPath + "Particles/Alpha Blended"));
		}
		
		atlasMaterial.mainTexture = SpriteAtlasManager.Get().atlasTexture.texture;

		return(atlasMaterial);
	}
	
	public Material GetAdditive() {
		if (additive == null) {
			additive = new Material (Shader.Find (Max2D.shaderPath + "Particles/Additive"));
		}
		return(additive);
	}

	public Material GetOcclusionEdge() {
		if (occlusionEdge == null) {
			occlusionEdge = new Material (Shader.Find (Max2D.shaderPath + "Particles/Multiply"));
			occlusionEdge.mainTexture = Resources.Load ("textures/occlusionedge") as Texture;
		}
		return(occlusionEdge);
	}

	public Material GetShadowBlur() {
		if (shadowBlur == null) {
			shadowBlur = new Material (Shader.Find (Max2D.shaderPath + "Particles/Multiply"));
			shadowBlur.mainTexture = Resources.Load ("textures/shadowblur") as Texture;
		}
		return(shadowBlur);
	}

	public Material GetOcclusionBlur() {
		if (occlusionBlur == null) {
			occlusionBlur = new Material (Shader.Find (Max2D.shaderPath + "Particles/Multiply"));
			occlusionBlur.mainTexture = Resources.Load ("textures/occlussionblur") as Texture;
		}
		return(occlusionBlur);
	}

	public Material GetWhiteSprite() {
		if (whiteSprite == null) {
			whiteSprite = new Material (Shader.Find ("SmartLighting2D/SpriteWhite"));
		}
		return(whiteSprite);
	}

	public Material GetBlackSprite() {
		if (blackSprite == null) {
			blackSprite = new Material (Shader.Find ("SmartLighting2D/SpriteBlack"));
		}
		return(blackSprite);
	}
}