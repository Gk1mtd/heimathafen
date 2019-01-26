using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameIsRunning;
    public int health { get; private set; }
    public float damageModifier;
    private float sonarTorpedoTimer;
    public float sonarTorpedoTime;

    
    // Start is called before the first frame update
    void Start()
    {
        gameIsRunning = true;
        health = 100;
        sonarTorpedoTimer = sonarTorpedoTime;
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
        }
    }

    public void ChangeHealth(float mod)
    {
        health += (int)mod;
        if (health <= 0)
            YouLost();
        Debug.Log(health);
    }

    private void YouLost()
    {
        Debug.Log("You lost");
    }

    //Unsichtbares Torpedo starten
    private void StartSonarTorpedo()
    {
        Debug.Log("Starte Torpedo");

    }
}
