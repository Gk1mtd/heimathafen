using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_EDITOR_OSX
public class Effects : MonoBehaviour
{
    private AudioSource playerAudioSource;

    public ParticleSystem explosion;
    public ParticleSystem sonar;
    public AudioClip sonarAudioStart;
    public ParticleSystem stoerkoerper;
    public AudioClip stoerkoerperAudio;
    public ParticleSystem funkenKollision;
    public AudioClip torpedoLaunch;
    //sprache
    public List<AudioClip> huellenbruch;
    public List<AudioClip> sonarBereit;
    public List<AudioClip> stoerkoerperBereit;
    public List<AudioClip> feindlichesTorpedo;
    public List<AudioClip> torpedoBereit;
    public List<AudioClip> zuHoch;

    private System.Random rnd = new System.Random();

    public enum Effekte
    {
        Explosion,
        Sonar,
        Stoerkoerper,
        Funken,
        TorpedoStart,
        Huellenbruch,
        SonarBereit,
        StoerkoerperBereit,
        FeindlTorpedo,
        TorpedoBereit,
        ZuHoch
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
            case Effekte.Huellenbruch:
                playerAudioSource.PlayOneShot(huellenbruch[rnd.Next(0, huellenbruch.Count)]);
                break;
            case Effekte.SonarBereit:
                playerAudioSource.PlayOneShot(sonarBereit[rnd.Next(0, sonarBereit.Count)]);
                break;
            case Effekte.StoerkoerperBereit:
                playerAudioSource.PlayOneShot(stoerkoerperBereit[rnd.Next(0, stoerkoerperBereit.Count)]);
                break;
            case Effekte.FeindlTorpedo:
                playerAudioSource.PlayOneShot(feindlichesTorpedo[rnd.Next(0, feindlichesTorpedo.Count)]);
                break;
            case Effekte.TorpedoBereit:
                playerAudioSource.PlayOneShot(torpedoBereit[rnd.Next(0, torpedoBereit.Count)]);
                break;
            case Effekte.ZuHoch:
                playerAudioSource.PlayOneShot(zuHoch[rnd.Next(0, zuHoch.Count)]);
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
#endif