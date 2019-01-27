using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_EDITOR_OSX

public class SubCollision : MonoBehaviour
{
    GameObject gameManObj;
    GameManager gameMan;
    SubControl subCont;
    private float dmgModifier;
    
    // Start is called before the first frame update
    void Start()
    {
        gameMan = GameManager.instance;
        subCont = GetComponent<SubControl>();
        dmgModifier = gameMan.damageModifier;
    }

    //Bei Kollisionen mit Minen und Felsen
    private void OnCollisionEnter(Collision collision)
    {
        ControllerManager.instance.maxRumble(0);

        Vector3 position = collision.contacts[0].point;
        if (collision.gameObject.CompareTag("Mine"))
        {
            Destroy(collision.gameObject);
            gameMan.ChangeHealth(-34 * dmgModifier);
            gameMan.effectScript.Effekt(position, Effects.Effekte.Explosion);
        }
        else if (collision.gameObject.CompareTag("Felsen"))
        {
            gameMan.ChangeHealth((-subCont.forwardSpeed * 20 * dmgModifier));
            gameMan.effectScript.Effekt(position, Effects.Effekte.Funken);
        }
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
            gameMan.effectScript.Effekt(transform.position, Effects.Effekte.ZuHoch);
        }
        if (other.CompareTag("Oberflaeche"))
        {
            gameMan.effectScript.Effekt(transform.position, Effects.Effekte.Explosion);
            gameMan.YouLost();
        }
        if (other.CompareTag("Text"))
        {
            SubtitleTrigger triggerBox = other.GetComponent<SubtitleTrigger>();
            gameMan.GetComponent<GameUI>().ChangeSubtitles(triggerBox.subtitleText);
            if (triggerBox.image1 != null)
                gameMan.GetComponent<GameUI>().ChangeImage1(triggerBox.image1);
            if (triggerBox.image2 != null)
                gameMan.GetComponent<GameUI>().ChangeImage2(triggerBox.image2);
            if (triggerBox.launchEnemyTorpedo)
                gameMan.StartSonarTorpedo();
        }
    }
}
#endif