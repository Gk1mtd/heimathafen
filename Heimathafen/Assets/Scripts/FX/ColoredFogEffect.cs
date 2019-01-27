using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class ColoredFogEffect : MonoBehaviour
{
    public Material _mat;
    public Texture2D fogColorRamp;
    public float fogIntensity;
    public float fogAmount;

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    void Update()
    {
        _mat.SetTexture("_ColorRamp", fogColorRamp);
        _mat.SetFloat("_FogIntensity", fogIntensity);
        _mat.SetFloat("_FogAmount", fogAmount);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _mat);
    }
}
