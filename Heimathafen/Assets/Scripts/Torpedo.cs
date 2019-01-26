using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Vector3 position = collision.gameObject.transform.position;
        Vector3 position = collision.contacts[0].point;
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (collision.gameObject.CompareTag("Mine"))
        {
            Destroy(collision.gameObject);
        }
        GameObject.Find("GameManager").GetComponent<Effects>().Effekt(position, Effects.Effekte.Explosion);
        Destroy(this.gameObject);
    }
}
