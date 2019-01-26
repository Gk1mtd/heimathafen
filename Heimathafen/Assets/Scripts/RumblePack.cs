﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class RumblePack : MonoBehaviour
{
    public bool debug;

    //Player1
    private bool player1IndexSet = false;
    private PlayerIndex player1Index;

    private GamePadState statePlayer1;
    private GamePadState prevStatePlayer1;

    [Range(0, 1)]
    public float[] player1Rumble = { 0.0f, 0.0f }; //0 = links 1= rechts

    //Player2
    private bool player2IndexSet = false;
    private PlayerIndex player2Index;

    private GamePadState statePlayer2;
    private GamePadState prevStatePlayer2;

    [Range (0,1)]
    public float[] player2Rumble = { 0.0f,0.0f}; //0 = links 1= rechts


    // Use this for initialization
    void Start()
    {

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

        //decrease over time 
        decreaseRumble();

    }

    private void decreaseRumble()
    {
        if (player1Rumble[0] >= 0)
            player1Rumble[0]--;
        if (player1Rumble[1] >= 0)
            player1Rumble[1]--;

        if (player2Rumble[0] >= 0)
            player2Rumble[0]--;
        if (player2Rumble[1] >= 0)
            player2Rumble[1]--;
    }

    // Update is called once per frame
    void Update()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
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
                        player1Index = testPlayerIndex;
                        player1IndexSet = true;

                    }

                    if (!player1IndexSet || !prevStatePlayer1.IsConnected)
                    {
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
