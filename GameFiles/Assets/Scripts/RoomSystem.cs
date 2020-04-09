﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomSystem : MonoBehaviour
{
    // List of generated rooms
    public List<GameObject> rooms = new List<GameObject>();

    // Array of room spawners
    public GameObject[] roomSpawners;

    // Rooms with South opening
    public GameObject[] southRooms;

    // Rooms with North opening
    public GameObject[] northRooms;

    // Rooms with East opening
    public GameObject[] eastRooms;

    // Rooms with West opening
    public GameObject[] westRooms;

    // Level exit object
    public GameObject levelExiter;

    // Empty room for plugging up holes
    public GameObject emptyRoom;

    // Flag to check if the level layout is finished generating
    public bool layoutFinished;

    // Flag to check if enitre level generation is finished
    public bool genFinished;

    private void LateUpdate()
    {
        // Check if level generation is finished, if it is then we dont need to do the rest of the update
        if(genFinished)
        {
            return;
        }

        // If level layout done, we want to put the exit in the last room
        if(layoutFinished)
        {
            Instantiate(levelExiter, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
            genFinished = true;
            Debug.Log("GameLevel: " + Variables.GameLevel);
            Debug.Log("Level Size: " + rooms.Count);
            ValidateSize();
            InvokeRepeating("CalculateDPS", 1F, 1F);
        }

        roomSpawners = GameObject.FindGameObjectsWithTag("roomSpawner"); // Update array with all room spawners

        // Go through each spawner, checking to see if it has spawned a room yet
        foreach(GameObject spawner in roomSpawners)
        {
            if(!spawner.GetComponent<RoomSpawner>().spawnedFlag) // If not spawned yet, level is still generating, check again next frame
            {
                return;
            }
        }

        // If program makes it here, level is done generating
        layoutFinished = true;
    }

    // Record the damage done by the player the previous second
    private void CalculateDPS()
    {
        if(Variables.CurrentDPS > Variables.HighestDPS)
        {
            Variables.HighestDPS = Variables.CurrentDPS;
            Debug.Log("HighestDPS: " + Variables.HighestDPS);
        }

        Variables.CurrentDPS = 0;
    }

    // Checks to make sure the level generated is an apporpriate size for the current level
    private void ValidateSize()
    {
        if(Variables.GameLevel < 5)
        {
            // If too small or too big, load new level
            if(rooms.Count < 7 || rooms.Count > 15)
            {
                SceneManager.LoadScene("SampleScene");  // Load new level
            }
        }

        else if(Variables.GameLevel < 20)
        {
            // If too small or too big, load new level
            if (rooms.Count < 11 || rooms.Count > 28)
            {
                SceneManager.LoadScene("SampleScene");  // Load new level
            }
        }

        else
        {
            // If too small or too big, load new level
            if (rooms.Count < 17 || rooms.Count > 100)
            {
                SceneManager.LoadScene("SampleScene");  // Load new level
            }
        }
    }
}
