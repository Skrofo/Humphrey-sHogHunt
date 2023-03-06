using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints; //we use game object because the waypoints are set as empty game objects
                                                     //the [] create an array so we can store as many waypoints as we want (works just like C so is a 1D array)
    private int currentWaypointIndex = 0;


    [SerializeField] private float speed = 4f;

    private void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f) //this tells the script the distance between 2 vectors (the waypoint and platform), when the current waypoint is <0.1 from the platform we know its touching
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed); //Time.deltatime helps to set the correct framerate for different devices, making the framerate independent
    }
}
