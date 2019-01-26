using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public ParticleSystem explosion;
    public ParticleSystem sonar;

    public enum Effekte
    {
        Explosion,
        Sonar
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
