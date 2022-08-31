
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;
using System.Collections;

public class tPad : UdonSharpBehaviour
{
    //Group Teleport Pad
    //Log Players on Enter, remove Player on Leave.
    //Player presses button, timer counts down- Teleport Action to all Players remaining on PAD.
    private VRCPlayerApi players = null;
    public GameObject TeleportObject;

    [SerializeField] VRCPlayerApi[] pLog;

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        AddPlayerToArray(player);
    }
    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        return;
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        RemovePlayer(player);
    }
    public void TeleportArrayPlayer()
    {
        for (int i = 0; i < pLog.Length; i++)
        {
            if (pLog[i] == null) continue;
            if (Networking.LocalPlayer.displayName == pLog[i].displayName)
            {
                Networking.LocalPlayer.TeleportTo(TeleportObject.transform.position, TeleportObject.transform.rotation);
                return;
            }
        }
    }
    public void AddPlayerToArray(VRCPlayerApi player)
    {
        // Save a copy of the current array so anything that may modify it while this function is running doesnt get added.
        VRCPlayerApi[] bLog = pLog;

        // Create a new player array that is one larger than the current player array.
        VRCPlayerApi[] tLog = new VRCPlayerApi[bLog.Length + 1];

        // Boolean to check if the player already exists in the array.
        bool playerExists = false;

        // Loop through the current player array.
        for (int i = 0; i < bLog.Length; i++)
        {
            if (bLog[i] == null) continue;
            // Print the current index and player name to the console.
            Debug.Log("Current Index: " + i + " - " + bLog[i]);

            // Check if the current player name in the current loop is the same as the player name passed into the function (player).
            if (bLog[i].displayName == player.displayName)
            {
                // If it is, set the playerAlreadyExists boolean to true.
                playerExists = true;
            }

            // Set the current index in the new player array to the current index in the current player array.
            tLog.SetValue(bLog[i], i);
        }

        // If the player does not exist in the array, add the player to the array.
        if (!playerExists)
        {
            // Set the last index in the new player array to the player name passed into the function (player).
            tLog.SetValue(player, bLog.Length);

            // Set the new player array to the new player array.
            pLog = tLog;
        }
    }
    public void RemovePlayer(VRCPlayerApi player)
    {
        // Save a copy of the current array so anything that may modify it while this function is running doesnt get removed.
        VRCPlayerApi[] bLog = pLog;

        // Create a new player array that is one smaller than the current player array.
        VRCPlayerApi[] tLog = new VRCPlayerApi[bLog.Length - 1];

        // Boolean to check if the player exists in the array.
        bool playerExists = false;

        // Loop through the current player array.
        for (int i = 0; i < bLog.Length; i++)
        {
            if (bLog[i] == null) continue;
            // Print the current index and player name to the console.
            Debug.Log("Current Index: " + i + " - " + bLog[i]);

            // Check if the current player name in the current loop is the same as the player name passed into the function (player).
            if (bLog[i].displayName == player.displayName)
            {
                // If it is, set the playerExists boolean to true.
                playerExists = true;
            }
            else
            {
                // If the player name in the current loop is not the same as the player name passed into the function (player), set the current index in the new player array to the current index in the current player array.
                tLog.SetValue(bLog[i], playerExists ? i - 1 : i);
            }
        }

        // If the player exists in the array, set the playerLog array to the new player array.
        if (playerExists)
        {
            pLog = tLog;
        }
    }
    public string ListPlayers()
    {
        // Loop through the playerLog array.
        string playerList = "";

        for (int i = 0; i < pLog.Length; i++)
        {
            if(pLog[i] != null)
            {
                playerList = playerList + pLog[i].displayName + ", ";
            }
        }
        return playerList;
    }
}
