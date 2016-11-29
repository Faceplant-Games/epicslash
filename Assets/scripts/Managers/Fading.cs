using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

	void OnGUI()
    {
        // fade out/in the alpha value using a direction, speed, Time.deltatime to convert the operation to seconds
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        // force (clamp) the number between 0 and 1 because GUI.color uses alpha values between 0 and 1
        alpha = Mathf.Clamp01(alpha);

        // set color of our GUI. 
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha); // set alpha
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), fadeOutTexture);
    }

    // sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
    public float BeginFade(int direction)
    {
        if (direction == 1)
        {
            print("fading out");
        }
        fadeDir = direction;
        return fadeSpeed; // return the fadeSpeed variable so it's easy to time the Application.LoadLevel()
    }

    // OnLevelWasLoaded is called when a level is loaded. It takes loaded level index (int) as a parameter so you can limit the fade in to certain scenes
    void OnLevelWasLoaded()
    {
        // alpha = 1; use this if the alpha ois not set to 1 by default
        BeginFade(-1);
    }
}
