using UnityEngine;
using UnityEngine.SceneManagement;

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
        fadeDir = direction; // direction = 1 means fading out, otherwise, direction = -1
        return fadeSpeed; // return the fadeSpeed variable so it's easy to time the Application.LoadLevel()
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // alpha = 1; use this if the alpha ois not set to 1 by default
        BeginFade(-1);
    }
}
