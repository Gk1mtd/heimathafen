using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int health { get; private set; }
    public float damageModifier;

    
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
