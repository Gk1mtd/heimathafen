using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvirMovement : MonoBehaviour
{
    //Bewegt Minen und Deko-Objekte (Fische...)
    public float speedX;
    public float speedY;
    public float speedZ;
    Vector3 movement;

    public bool loop;
    public float loopMax;
    private float loopDist;

    // Start is called before the first frame update
    void Start()
    {
        movement = new Vector3(speedX, speedY, speedZ);
        loopDist = 0.0f;
        loopMax *= 100;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movement * Time.deltaTime);
        if (loop)
        {
            loopDist += movement.magnitude;
            if (loopDist >= loopMax)
            {
                loopDist = 0.0f;
                movement = -movement;
            }
        }
    }
}
