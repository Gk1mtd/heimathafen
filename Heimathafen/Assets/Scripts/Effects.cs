using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    private AudioSource playerAudioSource;

    public ParticleSystem explosion;
    public ParticleSystem sonar;
    public AudioClip sonarAudioStart;
    public List<AudioClip> sonarPitch;
    public ParticleSystem stoerkoerper;
    public AudioClip stoerkoerperAudio;
    public ParticleSystem funkenKollision;
    public AudioClip funkenKollisionAudio;
    public AudioClip torpedoLaunch;

    public enum Effekte
    {
        Explosion,
        Sonar,
        Stoerkoerper,
        Funken,
        TorpedoStart
    }

    void Start()
    {
        playerAudioSource = GameObject.Find("U-Boot-Prefab").GetComponent<AudioSource>();
    }
    
    public void Effekt(Vector3 position, Effekte effekt)
    {
        ParticleSystem objekt;
        switch (effekt)
        {
            case Effekte.Explosion:
                objekt = Instantiate(explosion, position, Quaternion.identity);
                StartCoroutine(DestroyEffect(objekt.gameObject));
                break;
            case Effekte.Sonar:
                objekt = Instantiate(sonar, position, Quaternion.identity);
                StartCoroutine(DestroyEffect(objekt.gameObject));
                playerAudioSource.PlayOneShot(sonarAudioStart);
                if (GetComponent<GameManager>().torpedoLaunched)
                    StartCoroutine(ReturnPing());
                break;
            case Effekte.Stoerkoerper:
                playerAudioSource.PlayOneShot(stoerkoerperAudio);
                objekt = Instantiate(stoerkoerper, position, Quaternion.Euler(-195.0f, 90.0f, -90.0f));
                StartCoroutine(DestroyEffect(objekt.gameObject));
                break;
            case Effekte.Funken:
                objekt = Instantiate(funkenKollision, position, Quaternion.identity);
                StartCoroutine(DestroyEffect(objekt.gameObject));
                break;
            case Effekte.TorpedoStart:
                playerAudioSource.PlayOneShot(torpedoLaunch);
                break;
            default:
                Debug.Log("Fehler in Effects");
                break;
        }
    }

    //Zerstört die Partikeleffekte wieder
    IEnumerator DestroyEffect(GameObject objekt)
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(objekt);
    }

    //Gibt den Sonarping zurück
    IEnumerator ReturnPing()
    {
        float wait;
        float dist = GetComponent<GameManager>().torpedoDist;
        if (dist < 3.0f)
            wait = 0.5f;
        else if (dist < 5.0f)
            wait = 1.0f;
        else if (dist < 7.0f)
            wait = 1.5f;
        else if (dist < 9.0f)
            wait = 2.0f;
        else
            wait = 2.5f;
        yield return new WaitForSeconds(wait);
        playerAudioSource.PlayOneShot(sonarAudioStart);
    }
}
