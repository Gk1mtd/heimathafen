using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class FogEffect : MonoBehaviour
{
    public Material _mat;
    public Color fogColor;
    public float depthStart;
    public float depthDistance;

    void Start()
    {
        GetComponent<UnityEngine.Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    void Update()
    {
        _mat.SetColor("_FogColor", fogColor);
        _mat.SetFloat("_DepthStart", depthStart);
        _mat.SetFloat("_DepthDistance", depthDistance);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _mat);
    }
}
