using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInBehaviour : MonoBehaviour
{
    public static FadeInBehaviour instance;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else Destroy(this);
    }

    public Texture2D fadeTex;
    public float fadeSpeed;

    private int drawDepth = 1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    private void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b,alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTex);

    }

    public float BeginFade(int dir)
    {
        fadeDir = dir;
        return (fadeSpeed);
    }
}
