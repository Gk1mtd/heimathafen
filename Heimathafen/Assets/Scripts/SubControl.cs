using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubControl : MonoBehaviour
{
    private Rigidbody body;
    private float horizontal;       //Controller X-Achse
    private float vertical;         //Controller Y-Achse
    public float maxSpeed;          //Begrenzt die Geschwindigkeit des U-Boots
    public float rotationSpeed;     //Rotationsgeschwindigkeit
    public float acceleration;      //Beschleunigen / Abbremsen
    private float forwardSpeed;     //Die aktuelle Geschwindigkeit

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        forwardSpeed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector3 rotation = new Vector3(0, 0, vertical * rotationSpeed);
        transform.Rotate(rotation);

        forwardSpeed += horizontal * acceleration;
        forwardSpeed = Mathf.Max(-maxSpeed, forwardSpeed);
        forwardSpeed = Mathf.Min(maxSpeed, forwardSpeed);
        transform.Translate(Vector3.right * Time.deltaTime * forwardSpeed);
    }
}
 