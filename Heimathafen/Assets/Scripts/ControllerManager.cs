using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager instance = null;

    public bool debug;
    [Range(0,1)]
    public float rumbleReduce;

    [Range(0.00f,1.0f)]
    public float maxNaturalRumble;

    [Range(0.00f, 1.0f)]
    public float minNaturalRumble;

    //Player1
    private bool player1IndexSet = false;
    private PlayerIndex player1Index;

    public GamePadState statePlayer1;
    public GamePadState prevStatePlayer1;

    [Range(0, 1)]
    public float[] player1Rumble = { 0.0f, 0.0f }; //0 = links 1= rechts

    //Player2
    private bool player2IndexSet = false;
    private PlayerIndex player2Index;

    public GamePadState statePlayer2;
    public GamePadState prevStatePlayer2;

    [Range (0,1)]
    public float[] player2Rumble = { 0.0f,0.0f}; //0 = links 1= rechts


    // Use this for initialization
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

    void FixedUpdate()
    {
        
        // RumblePack controller!!!!!!!!!
        if (debug)           
        {
            player1Rumble[0] = statePlayer1.Triggers.Left;
            player1Rumble[1] = statePlayer1.Triggers.Right;

            player2Rumble[0] = statePlayer2.Triggers.Left;
            player2Rumble[1] = statePlayer2.Triggers.Right;
        }

        GamePad.SetVibration(player1Index, player1Rumble[0], player1Rumble[1]);
        GamePad.SetVibration(player2Index, player2Rumble[0], player2Rumble[1]);

        
    }

    public void maxRumble (int player)
    {
        if (player == 0)
        {
            player1Rumble[0] = 1.0f;
            player1Rumble[1] = 1.0f;
        }
        if (player == 1)
        {
            player2Rumble[0] = 1.0f;
            player2Rumble[1] = 1.0f;
        }
    }

    //accesible from everywhere to increase rumble
    public void increaseRumble(float increase,int player, int motor)
    {
        if (player == 0)
        {
            player1Rumble[0] = Mathf.Clamp(player1Rumble[0] + increase, minNaturalRumble, maxNaturalRumble);
        }
        if (player == 1)
        {
            player2Rumble[0] = Mathf.Clamp(player2Rumble[0] + increase, minNaturalRumble, maxNaturalRumble);
        }

    }

    //private to handle decreasing internally
        private void decreaseRumble()
    {
        if (player1Rumble[0] > 0.0f)
            player1Rumble[0] -= Time.deltaTime * rumbleReduce;
        if (player1Rumble[1] > 0.0f)
            player1Rumble[1] -= Time.deltaTime * rumbleReduce;

        player1Rumble[0] = Mathf.Clamp(player1Rumble[0], minNaturalRumble, 1.0f);
        player1Rumble[1] = Mathf.Clamp(player1Rumble[1], minNaturalRumble, 1.0f);

        if (player2Rumble[0] > 0.0f)
            player2Rumble[0] -= Time.deltaTime * rumbleReduce;
        if (player2Rumble[1] > 0.0f)
            player2Rumble[1] -= Time.deltaTime*rumbleReduce;

        player2Rumble[0] = Mathf.Clamp(player2Rumble[0], minNaturalRumble, maxNaturalRumble);
        player2Rumble[1] = Mathf.Clamp(player2Rumble[1], minNaturalRumble, maxNaturalRumble);

    }

    // Update is called once per frame
    void Update()
    {

        if (!player1IndexSet || !prevStatePlayer1.IsConnected || !player2IndexSet || !prevStatePlayer2.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    //inverted order so first player will be set first etc. :) 
                    if (!player2IndexSet || !prevStatePlayer2.IsConnected)
                    {
                        Debug.Log(string.Format("GamePad {0}", testPlayerIndex));
                        player2Index = testPlayerIndex;
                        player2IndexSet = true;
                    }

                    else if (!player1IndexSet || !prevStatePlayer1.IsConnected)
                    {
                        Debug.Log(string.Format("GamePad {0}", testPlayerIndex));
                        player1Index = testPlayerIndex;
                        player1IndexSet = true;
                    }
                    
                }
            }
        }

            prevStatePlayer1 = statePlayer1;
            statePlayer1 = GamePad.GetState(player1Index);

            prevStatePlayer2 = statePlayer2;
            statePlayer2 = GamePad.GetState(player2Index);

            //decrease over time 
            decreaseRumble();
    }

    void OnGUI()
    {
        if (debug)
        {
            string text = "Debug Controller States\n";
            text += string.Format("IsConnected {0} Packet #{1}\n", statePlayer1.IsConnected, statePlayer1.PacketNumber);
            text += string.Format("\tTriggers {0} {1}\n", statePlayer1.Triggers.Left, statePlayer1.Triggers.Right);
            text += string.Format("\tD-Pad {0} {1} {2} {3}\n", statePlayer1.DPad.Up, statePlayer1.DPad.Right, statePlayer1.DPad.Down, statePlayer1.DPad.Left);
            text += string.Format("\tButtons Start {0} Back {1} Guide {2}\n", statePlayer1.Buttons.Start, statePlayer1.Buttons.Back, statePlayer1.Buttons.Guide);
            text += string.Format("\tButtons LeftStick {0} RightStick {1} LeftShoulder {2} RightShoulder {3}\n", statePlayer1.Buttons.LeftStick, statePlayer1.Buttons.RightStick, statePlayer1.Buttons.LeftShoulder, statePlayer1.Buttons.RightShoulder);
            text += string.Format("\tButtons A {0} B {1} X {2} Y {3}\n", statePlayer1.Buttons.A, statePlayer1.Buttons.B, statePlayer1.Buttons.X, statePlayer1.Buttons.Y);
            text += string.Format("\tSticks Left {0} {1} Right {2} {3}\n", statePlayer1.ThumbSticks.Left.X, statePlayer1.ThumbSticks.Left.Y, statePlayer1.ThumbSticks.Right.X, statePlayer1.ThumbSticks.Right.Y);

            text += "\n\n";

            text += string.Format("IsConnected {0} Packet #{1}\n", statePlayer2.IsConnected, statePlayer2.PacketNumber);
            text += string.Format("\tTriggers {0} {1}\n", statePlayer2.Triggers.Left, statePlayer2.Triggers.Right);
            text += string.Format("\tD-Pad {0} {1} {2} {3}\n", statePlayer2.DPad.Up, statePlayer2.DPad.Right, statePlayer2.DPad.Down, statePlayer2.DPad.Left);
            text += string.Format("\tButtons Start {0} Back {1} Guide {2}\n", statePlayer2.Buttons.Start, statePlayer2.Buttons.Back, statePlayer2.Buttons.Guide);
            text += string.Format("\tButtons LeftStick {0} RightStick {1} LeftShoulder {2} RightShoulder {3}\n", statePlayer2.Buttons.LeftStick, statePlayer2.Buttons.RightStick, statePlayer2.Buttons.LeftShoulder, statePlayer2.Buttons.RightShoulder);
            text += string.Format("\tButtons A {0} B {1} X {2} Y {3}\n", statePlayer2.Buttons.A, statePlayer2.Buttons.B, statePlayer2.Buttons.X, statePlayer2.Buttons.Y);
            text += string.Format("\tSticks Left {0} {1} Right {2} {3}\n", statePlayer2.ThumbSticks.Left.X, statePlayer2.ThumbSticks.Left.Y, statePlayer2.ThumbSticks.Right.X, statePlayer2.ThumbSticks.Right.Y);

            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), text);
        }
    }
}
