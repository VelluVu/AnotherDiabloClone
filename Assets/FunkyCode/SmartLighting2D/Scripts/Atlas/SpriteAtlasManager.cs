using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AtlasTexture {
    public Texture2D texture;
    public int currentX = 0;
    public int currentY = 0;
    public int currentHeight = 0;
    // Y Offset?
}

public class SpriteRequest {
    // Normal = Black Alpha
    public enum Type {BlackAlpha, WhiteMask, Normal};
    public Sprite sprite;
    public Type type;

    public SpriteRequest (Sprite s, Type t) {
        sprite = s;
        type = t;
    }
}

[ExecuteInEditMode]
public class SpriteAtlasManager : MonoBehaviour {
    public static SpriteAtlasManager instance;

    public AtlasTexture atlasTexture = new AtlasTexture();

    // Normal / White Mask / Black Mask
    public List<Sprite> normal_spriteList = new List<Sprite>();
    public List<Sprite> whiteMask_spriteList = new List<Sprite>();
    public List<Sprite> blackMask_spriteList = new List<Sprite>();
    
    public List<SpriteRequest> requestList = new List<SpriteRequest>();

    public Dictionary<Sprite, Sprite> normal_dictionary_Sprite = new Dictionary<Sprite, Sprite>();
    public Dictionary<Sprite, Sprite> whiteMask_dictionary_Sprite = new Dictionary<Sprite, Sprite>();
    public Dictionary<Sprite, Sprite> blackMask_dictionary_Sprite = new Dictionary<Sprite, Sprite>();
  
    public Dictionary<Sprite, CustomPhysicsShape> dictionary_CustomShape = new Dictionary<Sprite, CustomPhysicsShape>();

    //////////////////// CUSTOM PHYSICS SHAPE ////////////////////////////////

    static public CustomPhysicsShape RequesCustomShape(Sprite originalSprite) {
        instance = Get();

		CustomPhysicsShape shape = null;

		bool exist = instance.dictionary_CustomShape.TryGetValue(originalSprite, out shape);

		if (exist) {
			if (shape == null || shape.sprite.texture == null) {
                shape = RequestCustomShapeAccess(originalSprite);
			} 
			return(shape);
		} else {
            shape = RequestCustomShapeAccess(originalSprite);
			return(shape);
		}
    }

    static public CustomPhysicsShape RequestCustomShapeAccess(Sprite originalSprite) {
		CustomPhysicsShape shape = null;

		bool exist = instance.dictionary_CustomShape.TryGetValue(originalSprite, out shape);

		if (exist) {
			if (shape == null || shape.sprite.texture == null) {
				instance.dictionary_CustomShape.Remove(originalSprite);

				shape = AddShape(originalSprite);

				instance.dictionary_CustomShape.Add(originalSprite, shape);
			} 
			return(shape);
		} else {		
			shape = AddShape(originalSprite);

			instance.dictionary_CustomShape.Add(originalSprite, shape);

			return(shape);
		}
    }

   static private CustomPhysicsShape AddShape(Sprite sprite) {
        if (sprite == null || sprite.texture == null) {
            return(null);
        }

        CustomPhysicsShape shape = new CustomPhysicsShape();
        shape.sprite = sprite;

        return(shape);
    }

    ///////////////////////////////////////////////////////////////////////////////////

    public static SpriteAtlasManager Get() {
        if (instance != null) {
			return(instance);
		}

		foreach(SpriteAtlasManager meshModeObject in Object.FindObjectsOfType(typeof(SpriteAtlasManager))) {
			instance = meshModeObject;
			return(instance);
		}

        if (instance == null) {
            GameObject spriteAtlas = new GameObject("Sprite Atlas");
            instance = spriteAtlas.AddComponent<SpriteAtlasManager>();
          
            LightingManager2D manager = LightingManager2D.Get();
            spriteAtlas.transform.parent = manager.transform;
        }

        return(instance);
    }


    void Awake() {
        atlasTexture.texture = new Texture2D(2048, 2048);

         for(int x = 0; x < atlasTexture.texture.width; x++) {
            for(int y = 0; y < atlasTexture.texture.height; y++) {
               atlasTexture.texture.SetPixel(x, y, new Color(0, 0, 0 ,0));
            }
        }
         
        atlasTexture.texture.Apply();

        normal_dictionary_Sprite = new Dictionary<Sprite, Sprite>();
        whiteMask_dictionary_Sprite = new Dictionary<Sprite, Sprite>();
        blackMask_dictionary_Sprite = new Dictionary<Sprite, Sprite>();

        normal_spriteList = new List<Sprite>();
        whiteMask_spriteList = new List<Sprite>();
        blackMask_spriteList = new List<Sprite>();

        instance = this;

        atlasTexture.currentX = 0;
        atlasTexture.currentY = 0;
        atlasTexture.currentHeight = 0;

   //	#if UNITY_EDITOR
    //        Debug.Log(2);
      //  #else
       //     Debug.Log(1);
       // #endif
             LightingManager2D manager = LightingManager2D.Get();
            
            for(int i = 1; i <= manager.spriteAtlasPreloadFoldersCount; i++) {
                string folder = manager.spriteAtlasPreloadFolders[i - 1];

                object[] sprites = Resources.LoadAll(folder, typeof(Sprite));
                foreach(object obj in sprites) {
                    Sprite sprite = (Sprite)obj;
                    RequestAccess(sprite, SpriteRequest.Type.WhiteMask);
                }
            }
     //   #endif  
    }

    void Update() {
        foreach(SpriteRequest req in requestList) {
            RequestAccess(req.sprite, req.type);
        }

        requestList.Clear();
    }

    public Sprite RequestAccess(Sprite originalSprite, SpriteRequest.Type type) {
		Sprite spriteObject = null;

        Dictionary<Sprite, Sprite> dictionary = null;

        switch(type) {
            case SpriteRequest.Type.BlackAlpha:
                dictionary = normal_dictionary_Sprite;
            break;

            case SpriteRequest.Type.WhiteMask:
                dictionary = whiteMask_dictionary_Sprite;
            break;

            case SpriteRequest.Type.Normal:
                dictionary = blackMask_dictionary_Sprite;
            break;
        }

		bool exist = dictionary.TryGetValue(originalSprite, out spriteObject);

		if (exist) {
			if (spriteObject == null || spriteObject.texture == null) {
				dictionary.Remove(originalSprite);

				spriteObject = AddSprite(originalSprite, type);

				dictionary.Add(originalSprite, spriteObject);
			} 
			return(spriteObject);
		} else {		
			spriteObject = AddSprite(originalSprite, type);

			dictionary.Add(originalSprite, spriteObject);

			return(spriteObject);
		}
    }

    static public Sprite RequestSprite(Sprite originalSprite, SpriteRequest.Type type) {
        if (originalSprite == null) {
            return(null);
        }

        instance = Get();

		Sprite spriteObject = null;
        Dictionary<Sprite, Sprite> dictionary = null;

        switch(type) {
            case SpriteRequest.Type.BlackAlpha:
                dictionary = instance.normal_dictionary_Sprite;
            break;

            case SpriteRequest.Type.WhiteMask:
                dictionary = instance.whiteMask_dictionary_Sprite;
            break;

            case SpriteRequest.Type.Normal:
                dictionary = instance.blackMask_dictionary_Sprite;
            break;
        }

		bool exist = dictionary.TryGetValue(originalSprite, out spriteObject);

		if (exist) {
			if (spriteObject == null || spriteObject.texture == null) {
                instance.requestList.Add(new SpriteRequest(originalSprite, type));
				return(null);
			} 
			return(spriteObject);
		} else {
            instance.requestList.Add(new SpriteRequest(originalSprite, type));
			return(null);
		}
    }

    private Sprite AddSprite(Sprite sprite, SpriteRequest.Type type) {
        if (sprite == null || sprite.texture == null) {
            return(null);
        }

        LightingManager2D manager = LightingManager2D.Get();

        switch(manager.spriteAtlasScale) {
            case SpriteAtlasScale.None:
                return(GenerateSpriteDefault(sprite, type));

            case SpriteAtlasScale.X2:
                return(GenerateSpriteScaled(sprite, type));
        }

        return(null);
    }

    public Sprite GenerateSpriteDefault(Sprite sprite, SpriteRequest.Type type) {
       // Create a temporary RenderTexture of the same size as the texture
        RenderTexture tmp = RenderTexture.GetTemporary(sprite.texture.width, sprite.texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

        // Blit the pixels on texture to the RenderTexture
        Graphics.Blit(sprite.texture, tmp);

         // Backup the currently set RenderTexture
        RenderTexture previous = RenderTexture.active;

        // Set the current RenderTexture to the temporary one we created
        RenderTexture.active = tmp;

        // Create a new readable Texture2D to copy the pixels to it
       // Texture2D myTexture2D = new Texture2D(sprite.texture.width, sprite.texture.height);
        Texture2D myTexture2D = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

        // Copy the pixels from the RenderTexture to the new Texture
        Rect tempRect = sprite.rect;
        tempRect.y = sprite.texture.height - tempRect.y - sprite.rect.height;

        myTexture2D.ReadPixels(tempRect, 0, 0);
        myTexture2D.Apply();

        // Reset the active RenderTexture
        RenderTexture.active = previous;

        // Release the temporary RenderTexture
        RenderTexture.ReleaseTemporary(tmp);

        if (atlasTexture.currentX + sprite.rect.width >= 2048) {
            atlasTexture.currentX = 0;
            atlasTexture.currentY += atlasTexture.currentHeight;
            atlasTexture.currentHeight = 0;
        }

        Color color;
     
       switch(type) {
            case SpriteRequest.Type.BlackAlpha:
                for(int x = 0; x < (int)sprite.rect.width; x++) {
                    for(int y = 0; y < (int)sprite.rect.height; y++) {
                        color = myTexture2D.GetPixel(x + (int)sprite.rect.x, y + (int)sprite.rect.y);
                       
                        color.a = ((1 - color.r) + (1 - color.g) + (1 - color.b)) / 3;
                        color.r = 0;
                        color.g = 0;
                        color.b = 0;
                        atlasTexture.texture.SetPixel(atlasTexture.currentX + x, atlasTexture.currentY + y, color);
                    }
                }
                break;

             case SpriteRequest.Type.WhiteMask:
                for(int x = 0; x < (int)sprite.rect.width; x++) {
                    for(int y = 0; y < (int)sprite.rect.height; y++) {
                        color = myTexture2D.GetPixel(x + (int)sprite.rect.x, y + (int)sprite.rect.y);
                        color.r = 1;
                        color.g = 1;
                        color.b = 1;
                        atlasTexture.texture.SetPixel(atlasTexture.currentX + x, atlasTexture.currentY + y, color);
                    }
                }
                break;

             case SpriteRequest.Type.Normal:
                for(int x = 0; x < (int)sprite.rect.width; x++) {
                    for(int y = 0; y < (int)sprite.rect.height; y++) {
                        color = myTexture2D.GetPixel(x + (int)sprite.rect.x, y + (int)sprite.rect.y);
                        color.a = 1;
                        atlasTexture.texture.SetPixel(atlasTexture.currentX + x, atlasTexture.currentY + y, color);
                    }
                }
                break;
        }

      
        atlasTexture.texture.Apply();

        Vector2 pivot = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
        
        Sprite output = Sprite.Create(atlasTexture.texture, new Rect(atlasTexture.currentX, atlasTexture.currentY, myTexture2D.width, myTexture2D.height), pivot, sprite.pixelsPerUnit);
       
        switch(type) {
            case SpriteRequest.Type.BlackAlpha:
                normal_spriteList.Add(output);
                break;

             case SpriteRequest.Type.WhiteMask:
                whiteMask_spriteList.Add(output);
                break;

             case SpriteRequest.Type.Normal:
                blackMask_spriteList.Add(output);
                break;
        }

        atlasTexture.currentX += (int)sprite.rect.width;
        atlasTexture.currentHeight = Mathf.Max(atlasTexture.currentHeight, (int)sprite.rect.height);
        return(output);
    }

    public Sprite GenerateSpriteScaled(Sprite sprite, SpriteRequest.Type type) {
        RenderTexture tmp = RenderTexture.GetTemporary(sprite.texture.width, sprite.texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(sprite.texture, tmp);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmp;
        Texture2D myTexture2D = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Rect tempRect = sprite.rect;
        tempRect.y = sprite.texture.height - tempRect.y - sprite.rect.height;

        myTexture2D.ReadPixels(tempRect, 0, 0);
        myTexture2D.Apply();

        RenderTexture.active = previous;

        RenderTexture.ReleaseTemporary(tmp);

        int image_x = (int)(sprite.rect.x * 0.5f);
        int image_y = (int)(sprite.rect.y * 0.5f);
        int image_width = (int)(sprite.rect.width * 0.5f);
        int image_height = (int)(sprite.rect.height * 0.5f);

        if (atlasTexture.currentX + image_width >= 2048) {
            atlasTexture.currentX = 0;
            atlasTexture.currentY += atlasTexture.currentHeight;
             atlasTexture.currentHeight = 0;
        }

        Color color;

         switch(type) {
            case SpriteRequest.Type.BlackAlpha:
               for(int x = 0; x < image_width; x++) {
                    for(int y = 0; y < image_height; y++) {
                        color = myTexture2D.GetPixel(x * 2 , y * 2);
                        atlasTexture.texture.SetPixel(atlasTexture.currentX + x, atlasTexture.currentY + y, color);
                    }
                }
                break;

             case SpriteRequest.Type.WhiteMask:
                for(int x = 0; x < image_width; x++) {
                    for(int y = 0; y < image_height; y++) {
                        color = myTexture2D.GetPixel(x * 2 , y * 2);
                        color.r = 1;
                        color.g = 1;
                        color.b = 1;
                        atlasTexture.texture.SetPixel(atlasTexture.currentX + x, atlasTexture.currentY + y, color);
                    }
                }
                break;

             case SpriteRequest.Type.Normal:
                for(int x = 0; x < image_width; x++) {
                    for(int y = 0; y < image_height; y++) {
                        color = myTexture2D.GetPixel(x * 2 , y * 2); 
                        color.r = 0;
                        color.g = 0;
                        color.b = 0;
                        atlasTexture.texture.SetPixel(atlasTexture.currentX + x, atlasTexture.currentY + y, color);
                    }
                }
                break;
        }

      
       atlasTexture.texture.Apply();

        Vector2 pivot = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
        Sprite output = Sprite.Create(atlasTexture.texture, new Rect(atlasTexture.currentX, atlasTexture.currentY, myTexture2D.width / 2, myTexture2D.height / 2), pivot, sprite.pixelsPerUnit * 0.5f);
       
        switch(type) {
            case SpriteRequest.Type.BlackAlpha:
                normal_spriteList.Add(output);
                break;

             case SpriteRequest.Type.WhiteMask:
                whiteMask_spriteList.Add(output);
                break;

             case SpriteRequest.Type.Normal:
                blackMask_spriteList.Add(output);
                break;
        }

        atlasTexture.currentX += (int)image_width;
        atlasTexture.currentHeight = Mathf.Max(atlasTexture.currentHeight, image_height);

         return(output);
    }
}