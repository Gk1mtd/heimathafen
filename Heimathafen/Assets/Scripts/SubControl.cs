using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubControl : MonoBehaviour
{
    private Rigidbody body;
    public float horizontal;
    public float vertical;
    private float moveLimiter = 0.7f;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontal * speed, transform.rotation.z, 0);
        body.AddForce(movement * speed);

        Vector3 rotation = new Vector3(0, 0, vertical * speed);
        transform.Rotate(rotation);
    }

    /*
     *     void FixedUpdate()
    {
        Vector3 movement;

        if (horizontal != 0 && vertical != 0)
            movement = new Vector3((horizontal * speed) * moveLimiter, (vertical * speed) * moveLimiter, 0);
        else
            movement = new Vector3(horizontal * speed, vertical * speed, 0);

        body.AddForce(movement * speed);

        transform.Rotate(movement);
    }*/
}
 