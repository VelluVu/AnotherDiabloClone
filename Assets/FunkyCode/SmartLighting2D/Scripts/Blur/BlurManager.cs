using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlurManager {
	static public Dictionary<Sprite, BlurObject> dictionary = new Dictionary<Sprite, BlurObject>();

	static public Sprite RequestSprite(Sprite originalSprite, int blurSize, int blurIterations) {
		BlurObject blurObject = null;

		bool exist = dictionary.TryGetValue(originalSprite, out blurObject);

		if (exist) {
			if (blurObject.sprite == null || blurObject.sprite.texture == null) {
				dictionary.Remove(originalSprite);

				blurObject.sprite = LinearBlur.Blur(originalSprite, blurSize, blurIterations, Color.white);
				blurObject.blurSize = blurSize;
				blurObject.blurIterations = blurIterations;

				dictionary.Add(originalSprite, blurObject);
			} else if (blurObject.blurSize != blurSize || blurObject.blurIterations != blurIterations){
				blurObject.sprite = LinearBlur.Blur(originalSprite, blurSize, blurIterations, Color.white);
				blurObject.blurSize = blurSize;
				blurObject.blurIterations = blurIterations;
			}
			return(blurObject.sprite);
		} else {		
			Sprite sprite = LinearBlur.Blur(originalSprite, blurSize, blurIterations, Color.white);

			blurObject = new BlurObject(sprite, blurSize, blurIterations);

			dictionary.Add(originalSprite, blurObject);

			return(blurObject.sprite);
		}
	}
}

[System.Serializable]
public class BlurObject {
	public Sprite sprite;
	public int blurSize;
	public int blurIterations;

	public BlurObject(Sprite image, int size, int iterations) {
		sprite = image;
		blurSize = size;
		blurIterations = iterations;
	}
}