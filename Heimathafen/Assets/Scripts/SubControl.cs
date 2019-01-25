using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubControl : MonoBehaviour
{
    private Rigidbody body;
    private float schub;            //Controller Rechter Trigger
    private float rueckschub;       //Controller Linker Trigger
    private float vertical;         //Controller Y-Achse
    public float maxSpeed;          //Begrenzt die Geschwindigkeit des U-Boots
    public float rotationSpeed;     //Rotationsgeschwindigkeit
    public float acceleration;      //Beschleunigen / Abbremsen
    public float maxAngle;          //Begrenzt die Rotation des U-Boots
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
        schub = Input.GetAxisRaw("Schub");
        rueckschub = Input.GetAxisRaw("Rueckschub");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        //Rotation
        if (RotationErlaubt(vertical))
        {
            Vector3 rotation = new Vector3(0, 0, vertical * rotationSpeed);
            transform.Rotate(rotation);
            Debug.Log(transform.rotation.eulerAngles.z);
        }
        //Bewegung
        forwardSpeed += schub * acceleration;       //Schub addieren
        forwardSpeed -= rueckschub * acceleration;  //Rückschub subtrahieren
        forwardSpeed = Mathf.Max(-maxSpeed, forwardSpeed);
        forwardSpeed = Mathf.Min(maxSpeed, forwardSpeed);
        transform.Translate(Vector3.right * Time.deltaTime * forwardSpeed);
    }

    private bool RotationErlaubt(float vert)
    {
        if (vertical > 0)
        {
            if (transform.rotation.eulerAngles.z < maxAngle) //0-ma
                return true;
            if (transform.rotation.eulerAngles.z > 360-maxAngle-2) //360-ma - 360
                return true;
            return false;
        }
        if (vertical < 0)
        {
            if (transform.rotation.eulerAngles.z < maxAngle+2) //0-ma
                return true;
            if (transform.rotation.eulerAngles.z > 360 - maxAngle) //360-ma - 360
                return true;
            return false;
        }
        return false;
    }
}
 