using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubControl : MonoBehaviour
{
    public  bool debug = false;

    private Rigidbody body;
    private ControllerManager contManager;
    private GameManager gameMan;
    private ControllerManager controllerManager;

    private float schub;            //Controller Rechter Trigger
    private float rueckschub;       //Controller Linker Trigger
    private float vertical;         //Controller Y-Achse
    public float maxSpeed;          //Begrenzt die Geschwindigkeit des U-Boots
    public float rotationSpeed;     //Rotationsgeschwindigkeit
    public float acceleration;      //Beschleunigen / Abbremsen
    public float maxAngle;          //Begrenzt die Rotation des U-Boots
    public float forwardSpeed { get; private set; }     //Die aktuelle Geschwindigkeit

    public GameObject torpedoPrefab;
    public Transform torpedoRohr;   //Abfeuern des Torpedos von hier
    public float torpedoCooldown;   //Cooldownzeit Torpedo
    private bool torpedoReady;      //false, wenn Cooldown läuft

    public float sonarCooldown;     //Sonar Cooldownzeit
    private bool sonarReady;        //false, wenn Cooldown läuft

    public float stoerkoerperCooldown;  //Störkörper Cooldownzeit
    private bool stoerkoerperReady;     //false, wenn Cooldown läuft


    // Start is called before the first frame update
    void Start()
    {
        gameMan = GameManager.instance;
        contManager = ControllerManager.instance;
        controllerManager = gameMan.GetComponent<ControllerManager>();
        body = GetComponent<Rigidbody>();
        forwardSpeed = 0.0f;
        torpedoReady = true;
        sonarReady = true;
        stoerkoerperReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMan.gameIsRunning)
        {
            // ######################  Controller 1 = Steuermann #########################
            schub = contManager.statePlayer1.Triggers.Right;
            rueckschub = contManager.statePlayer1.Triggers.Left;
            vertical = contManager.statePlayer1.ThumbSticks.Left.Y;

            if (debug)
            {
                if (contManager.statePlayer1.Buttons.A == XInputDotNetPure.ButtonState.Pressed )
                {
                    Sonar();
                }

                // ######################  Controller 2 = Ausguck #########################

                if (contManager.statePlayer1.Buttons.B == XInputDotNetPure.ButtonState.Pressed )
                {
                    Torpedo();
                }


                if (contManager.statePlayer1.Buttons.X == XInputDotNetPure.ButtonState.Pressed )
                {
                    Stoerkoerper();
                }
            }

            else
            {
                if (contManager.statePlayer1.Buttons.A == XInputDotNetPure.ButtonState.Pressed && sonarReady)
                {
                    Sonar();
                }

                // ######################  Controller 2 = Ausguck #########################

                if (contManager.statePlayer1.Buttons.B == XInputDotNetPure.ButtonState.Pressed && torpedoReady)
                {
                    Torpedo();
                }


                if (contManager.statePlayer2.Buttons.A == XInputDotNetPure.ButtonState.Pressed && stoerkoerperReady)
                {
                    Stoerkoerper();
                }
            }
            
        }
    }

    void FixedUpdate()
    {
        //Rotation
        if (RotationErlaubt(vertical))
        {
            Vector3 rotation = new Vector3(0, 0, vertical * rotationSpeed);
            transform.Rotate(rotation);
        }
        //Bewegung
        forwardSpeed += schub * acceleration;       //Schub addieren
        forwardSpeed -= rueckschub * acceleration;  //Rückschub subtrahieren
        forwardSpeed = Mathf.Max(-maxSpeed, forwardSpeed);
        forwardSpeed = Mathf.Min(maxSpeed, forwardSpeed);
        transform.Translate(Vector3.right * Time.deltaTime * forwardSpeed);

        if (transform.rotation.eulerAngles.z > maxAngle && transform.rotation.eulerAngles.z < maxAngle + 90)
            transform.eulerAngles = new Vector3(0, 0, maxAngle - 0.5f);
        if (transform.rotation.eulerAngles.z > 360 - maxAngle - 90 && transform.rotation.eulerAngles.z < 360 - maxAngle)
            transform.eulerAngles = new Vector3(0, 0, -maxAngle + 0.5f);


        contManager.increaseRumble(schub*Time.deltaTime, 0, 0);
        contManager.increaseRumble(rueckschub * Time.deltaTime, 0, 0);


    }

    //Begrenzt die Rotation des U-Boots
    private bool RotationErlaubt(float vert)
    {
        if (vertical > 0)
        {
            if (transform.rotation.eulerAngles.z < maxAngle) //0-ma
                return true;
            if (transform.rotation.eulerAngles.z > 360-maxAngle-2) //360-ma - 360
                return true;
            return false;
        }
        if (vertical < 0)
        {
            if (transform.rotation.eulerAngles.z < maxAngle+2) //0-ma
                return true;
            if (transform.rotation.eulerAngles.z > 360 - maxAngle) //360-ma - 360
                return true;
            return false;
        }
        return false;
    }

    //Stoppt das U-Boot, wenn das Spiel endet
    public void StoppeUBoot()
    {
        forwardSpeed = 0.0f;
    }

    //Torpedo abfeuern
    private void Torpedo()
    {
        GameObject torpedo = Instantiate(torpedoPrefab, torpedoRohr.transform.position, transform.rotation);
        torpedoReady = false;
        StartCoroutine(Cooldown("Torpedo"));
        gameMan.GetComponent<Effects>().Effekt(transform.position, Effects.Effekte.TorpedoStart);
    }

    //Sonar starten
    private void Sonar()
    {
        Vector3 sonarPos = new Vector3(transform.position.x + 1.39f, transform.position.y, transform.position.z + 1.0f);
        gameMan.effectScript.Effekt(sonarPos, Effects.Effekte.Sonar);
        sonarReady = false;
        StartCoroutine(Cooldown("Sonar"));
        if (gameMan.torpedoLaunched)
        {
            Debug.Log(gameMan.torpedoDist);
        }
    }

    //Störpörper abfeuern
    private void Stoerkoerper()
    {
        Vector3 stoerPos = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
        gameMan.effectScript.Effekt(stoerPos, Effects.Effekte.Stoerkoerper);
        stoerkoerperReady = false;
        StartCoroutine(Cooldown("Stoerkoerper"));
        if (gameMan.torpedoLaunched && (gameMan.torpedoDist < 2.0f))
        {
            Debug.Log("Feindl. Torpedo zerstört");
            Vector3 torpPos = new Vector3(transform.position.x - 2.0f, transform.position.y, transform.position.z);
            gameMan.effectScript.Effekt(torpPos, Effects.Effekte.Explosion);
            gameMan.TorpedoStopped();
        }
    }

    //Cooldown abwarten
    IEnumerator Cooldown(string typ)
    {
        switch (typ)
        {
            case "Torpedo":
                yield return new WaitForSeconds(torpedoCooldown);
                torpedoReady = true;
                break;
            case "Sonar":
                yield return new WaitForSeconds(sonarCooldown);
                sonarReady = true;
                gameMan.effectScript.Effekt(transform.position, Effects.Effekte.SonarBereit);
                break;
            case "Stoerkoerper":
                yield return new WaitForSeconds(stoerkoerperCooldown);
                stoerkoerperReady = true;
                break;
            default:
                Debug.Log("Fehler in SubControl-Cooldown");
                break;
        }

    }
}
 