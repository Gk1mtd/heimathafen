using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public ParticleSystem explosion;
    public ParticleSystem sonar;
    public ParticleSystem stoerkoerper;

    public enum Effekte
    {
        Explosion,
        Sonar,
        Stoerkoerper
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Effekt(Vector3 position, Effekte effekt)
    {
        ParticleSystem objekt;
        switch (effekt)
        {
            case Effekte.Explosion:
                objekt = Instantiate(explosion, position, Quaternion.identity);
                break;
            case Effekte.Sonar:
                objekt = Instantiate(sonar, position, Quaternion.identity);
                break;
            case Effekte.Stoerkoerper:
                objekt = Instantiate(stoerkoerper, position, Quaternion.Euler(-195.0f, 90.0f, -90.0f));
                break;
            default:
                objekt = new ParticleSystem();
                break;
        }

        StartCoroutine(DestroyEffect(objekt.gameObject));
    }

    IEnumerator DestroyEffect(GameObject objekt)
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(objekt);
    }
}
