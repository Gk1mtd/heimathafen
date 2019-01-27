using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_EDITOR_OSX

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool gameIsRunning;

    GameObject playerObj;
    public int health { get; private set; } //Zustan des U-Boots
    public float damageModifier;            //erhöht/reduziert den Schaden am U-Boot - 1.0 ist keine Änderung
    public MeshRenderer subRenderer;       //Renderer-Komponente des U-Boots

    private float sonarTorpedoTimer;        //Countdown bis zum Start eines unsichtbaren Torpedos
    public float sonarTorpedoTime;          //Startzeit des Countdowns
    public bool torpedoLaunched { get; private set; }           //ist ein Torpedo unterwegs?
    public float torpedoDist { get; private set; }              //aktuelle Entfernung des Torpedos zum U-Boot
    public int torpedoMaxDist;              //Startentfernung des Torpedos
    public int torpedoMinDist;              //Startentfernung des Torpedos
    private System.Random rnd;              //Für Zufallszahlen

    public Effects effectScript;


    // Start is called before the first frame update
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameIsRunning = true;
        playerObj = GameObject.Find("U-Boot-Prefab");
        health = 100;
        sonarTorpedoTimer = sonarTorpedoTime;
        torpedoLaunched = false;
        rnd = new System.Random();

        effectScript = GetComponent<Effects>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsRunning)
        {
            if (sonarTorpedoTimer > 0)
            {
                sonarTorpedoTimer -= Time.deltaTime;
                if (sonarTorpedoTimer <= 0)
                {
                    sonarTorpedoTimer = sonarTorpedoTime;
                    StartSonarTorpedo();
                }
            }

            if (torpedoDist > 0 && torpedoLaunched)
            {
                torpedoDist -= Time.deltaTime;
                if (torpedoDist <= 0)
                {
                    GetComponent<Effects>().Effekt(playerObj.transform.position, Effects.Effekte.Explosion);
                    ChangeHealth(-34 * damageModifier);
                    torpedoLaunched = false;
                }
            }
        }
        else
        {
            ControllerManager.instance.noRumble();
        }
    }

    public void ChangeHealth(float mod)
    {
        health += (int)mod;
        if (health <= 0)
            YouLost();
        Debug.Log(health);
        float normalVal = subRenderer.material.GetFloat("_DetailNormalMapScale");
        normalVal += -mod * 0.12f;
        subRenderer.material.SetFloat("_DetailNormalMapScale", normalVal);
        GetComponent<GameUI>().ChangeMessages("We're taking damage!");
    }

    public void YouWon()
    {
        Debug.Log("Gewonnen");
        gameIsRunning = false;
        playerObj.GetComponent<SubControl>().StoppeUBoot();
        GetComponent<GameUI>().ChangeSubtitles("You won");
    }

    public void YouLost()
    {
        Debug.Log("You lost");
        gameIsRunning = false;
        playerObj.GetComponent<SubControl>().StoppeUBoot();
        GetComponent<GameUI>().ChangeSubtitles("You lost");
    }

    //Unsichtbares Torpedo starten
    public void StartSonarTorpedo()
    {
        int max = sonarTorpedoTimer - 1 > 1 ? Mathf.Min((int)sonarTorpedoTimer - 1, torpedoMaxDist) : torpedoMaxDist;
        //int max = Mathf.Min((int)sonarTorpedoTimer - 1, torpedoMaxDist);
        int min = torpedoMinDist;
        if (max < min)
            Debug.Log("Torpedo-Zeit zu kurz");
        torpedoDist = rnd.Next(min, max);
        torpedoLaunched = true;
        effectScript.Effekt(playerObj.transform.position, Effects.Effekte.FeindlTorpedo);
        GetComponent<GameUI>().ChangeMessages("Enemy torpedo detected!");
    }

    //Torpedo abgefangen
    public void TorpedoStopped()
    {
        torpedoLaunched = false;
        GetComponent<GameUI>().ChangeMessages("Torpedo intercepted!");
    }
}
#endif