using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class CameraFX : MonoBehaviour
{


    [Header("Underwater Shader")]
    public Material underwaterMat;
    [Range(0.001f,0.1f)]
    public float pixelOffset;
    [Range(0.1f, 20f)]
    public float noiseScale;
    [Range(0.1f, 20f)]
    public float noiseFrequency;
    [Range(0.1f, 20f)]
    public float noiseSpeed;

    public float depthStart;
    public float depthDistance;

    void Update()
    {
        transform.rotation = Quaternion.identity;
        underwaterMat.SetFloat("_PixelOffset", pixelOffset);
        underwaterMat.SetFloat("_NoiseScale", noiseScale);
        underwaterMat.SetFloat("_NoiseFrequency", noiseFrequency);
        underwaterMat.SetFloat("_NoiseSpeed", noiseSpeed);
        underwaterMat.SetFloat("_DepthStart", depthStart);
        underwaterMat.SetFloat("_DepthDistance", depthDistance);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, underwaterMat);
    }
}
