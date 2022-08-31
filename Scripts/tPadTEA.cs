
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;
using System.Collections;

public class tPadTEA : UdonSharpBehaviour
{
    public tPad tPad = null;

    //public Text onPadCount;
    public Text warningState;
    public Text timerCount;
    public Text padNames;

    bool masterButtonPressed = false;
    bool isTimerFinished;
    bool isTimerRunning;
    float remainingTime;
    public void Start()
    {
        masterButtonPressed = false;
        isTimerRunning = false;
    }
    public void Update()
    {
        padNames.text = tPad.ListPlayers();
        if (isTimerRunning && masterButtonPressed)
        {
            timerCount.text = remainingTime + "";
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0.0f)
            {
                masterButtonPressed = false;
                isTimerRunning = false;
                isTimerFinished = true;
                remainingTime = 0f;
                timerCount.text = remainingTime + "";
                tPad.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TeleportArrayPlayer");
            }
            else
            {
                isTimerFinished = false;
            }
        }
        else
        {
            return;
        }
    }
    public override void Interact()
    {
        TimerStart();
    }
    public void TimerStart()
    {
        remainingTime = 10f;
        masterButtonPressed = true;
        isTimerRunning = true;
        warningState.text = "Timer Started.";
    }
}
