using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour {
    public Texture2D bloodTexture;

    private float fadeSpeed = 0.1f;
    private int drawDepth = -1000;
    private float alpha = 0f;
    private float ALPHA_MAX = 0.5f;

    void OnGUI()
    {
        float alphaReduction = fadeSpeed * Time.deltaTime;
        if (alpha - alphaReduction < 0)
        {
            alpha = 0;
        }
        else
        {
            alpha -= alphaReduction;
        }
        // force (clamp) the number between 0 and 1 because GUI.color uses alpha values between 0 and 1
        alpha = Mathf.Clamp01(alpha);

        // set color of our GUI. 
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha); // set alpha
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bloodTexture);
    }

    public void TakeDamage(int damage)
    {
        float plusAlpha;
        if (damage < 10) {
            plusAlpha = 0.1f;
        } else if (damage < 100)
        {
            plusAlpha = 0.2f;
        } else
        {
            plusAlpha = 0.3f;
        }
        
        if (alpha + plusAlpha > ALPHA_MAX)
        {
            alpha = ALPHA_MAX;
            return;
        }
        alpha += plusAlpha;
    }
}
