using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    
    // Start is called before the first frame update
    void Start()
    {
        gameIsRunning = true;
        playerObj = GameObject.Find("U-Boot-Prefab");
        health = 100;
        sonarTorpedoTimer = sonarTorpedoTime;
        torpedoLaunched = false;
        rnd = new System.Random();
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
    }

    private void YouLost()
    {
        Debug.Log("You lost");
    }

    //Unsichtbares Torpedo starten
    private void StartSonarTorpedo()
    {
        Debug.Log("Starte Torpedo");
        int max = Mathf.Min((int)sonarTorpedoTimer - 1, torpedoMaxDist);
        int min = torpedoMinDist;
        if (max < min)
            Debug.Log("Torpedo-Zeit zu kurz");
        torpedoDist = rnd.Next(min, max);
        torpedoLaunched = true;
    }

    //Torpedo abgefangen
    public void TorpedoStopped()
    {
        torpedoLaunched = false;
    }
}
