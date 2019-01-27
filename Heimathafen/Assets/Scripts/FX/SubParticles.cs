using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_EDITOR_OSX

public class SubParticles : MonoBehaviour
{
    public SubControl control;
    public Rigidbody rb;
    public ParticleSystem ps;
    public float particleRate;

    void Update()
    {
        var em = ps.emission;
        em.rateOverTime = Mathf.Abs(control.forwardSpeed * particleRate);
    }
}
#endif