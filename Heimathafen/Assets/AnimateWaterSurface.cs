using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateWaterSurface : MonoBehaviour
{
    Material mat;
    float offset = 0;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime * 0.1f;
        mat.SetVector("_NoiseOffset", new Vector4(offset, offset, 0, 0));
    }
}
