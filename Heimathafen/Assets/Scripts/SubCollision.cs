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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 position = collision.gameObject.transform.position;
        if (collision.gameObject.CompareTag("Mine"))
        {
            Destroy(collision.gameObject);
            gameMan.ChangeHealth(-34 * dmgModifier);
        }
        else if (collision.gameObject.CompareTag("Felsen"))
        {
            gameMan.ChangeHealth((-subCont.forwardSpeed * 20 * dmgModifier));
        }
        gameManObj.GetComponent<Effects>().Explosion(position);
    }
}
