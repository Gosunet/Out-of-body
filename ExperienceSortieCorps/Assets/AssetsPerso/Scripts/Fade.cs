using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {

	public Texture fadeOutTexture;
	public float fadeSpeed = 0.8f;

	int drawDepth = -1000;
	float alpha = 1.0f;
	int fadeDir = -1;

	void OnGUI(){
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

	public float BeginFade(int direction){
		fadeDir = direction;
	//	OnGUI ();
		return fadeSpeed;
	}

	void OnLevelWasLoaded(){
		BeginFade (-1);
	}
}
