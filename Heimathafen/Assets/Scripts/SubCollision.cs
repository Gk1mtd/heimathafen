using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCollision : MonoBehaviour
{
    GameObject gameManObj;
    GameManager gameMan;
    SubControl subCont;
    private float dmgModifier;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManObj = GameObject.Find("GameManager");
        gameMan = gameManObj.GetComponent<GameManager>();
        subCont = GetComponent<SubControl>();
        dmgModifier = gameMan.damageModifier;
    }

    //Bei Kollisionen mit Minen und Felsen
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 position = collision.contacts[0].point;
        if (collision.gameObject.CompareTag("Mine"))
        {
            Destroy(collision.gameObject);
            gameMan.ChangeHealth(-34 * dmgModifier);
        }
        else if (collision.gameObject.CompareTag("Felsen"))
        {
            gameMan.ChangeHealth((-subCont.forwardSpeed * 20 * dmgModifier));
        }
        gameManObj.GetComponent<Effects>().Effekt(position, Effects.Effekte.Explosion);
    }

    //Beim Erreichen von Triggern (Oberfläche, Ziel)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ziel"))
        {
            gameMan.YouWon();
        }
        if (other.CompareTag("OberflaecheWarnung"))
        {
            Debug.Log("Zu hoch - Abtauchen!");
        }
        if (other.CompareTag("Oberflaeche"))
        {
            Debug.Log("Zu hoch - Zerstört");
            gameMan.YouLost();
        }
    }
}
