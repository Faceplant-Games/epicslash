using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {
    public Texture2D bloodTexture;

    private float fadeSpeed = 0.8f;
    private int drawDepth = -1000;
    private float alpha = 0f;

    void OnGUI()
    {
        alpha = alpha * fadeSpeed * Time.deltaTime;
        // force (clamp) the number between 0 and 1 because GUI.color uses alpha values between 0 and 1
        alpha = Mathf.Clamp01(alpha);

        // set color of our GUI. 
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha); // set alpha
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bloodTexture);
    }

    public void takeDamage(int damage)
    {
        alpha += 0.1f;
    }
}
