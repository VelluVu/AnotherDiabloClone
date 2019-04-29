using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DemoTabs : MonoBehaviour {
	public GameObject[] tabObjects;
	public GameObject[] buttons;
	int tabid = 0;

	public void SetTab(int id) {
		tabid = id;
		for(int i = 0; i < tabObjects.Length; i++) {
			if (i == id) {
				tabObjects[i].SetActive(true);
			} else {
				tabObjects[i].SetActive(false);
			}
		}
		ColorUpdate ();
	}

	void Start() {
		 ColorUpdate ();
	}
	
	void ColorUpdate () {
		for(int i = 0; i < tabObjects.Length; i++) {
			Button button = buttons[i].GetComponent<Button>();
			ColorBlock colors = button.colors;
			if (i == tabid) {
				colors.normalColor = new Color(1, 142f / 255, 0, 1);
				colors.highlightedColor = new Color(1, 142f / 255, 0, 1);
				
			} else {
				colors.normalColor = new Color(47f / 255, 47f / 255, 47f / 255, 76f / 255);
				colors.highlightedColor = new Color(255f / 255, 144f / 255, 47f / 255, 206f / 255);
			}
			
			button.colors = colors;
		}
	}
}
