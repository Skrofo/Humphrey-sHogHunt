using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPosition;

    private int currentEnemyPosition = 0;

    [SerializeField] private float speed = 3f;


    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(enemyPosition[currentEnemyPosition].transform.position, transform.position) < .1f) //this tells the script the distance between 2 vectors (the waypoint and platform), when the current waypoint is <0.1 from the platform we know its touching
        {
            currentEnemyPosition++;
            if (currentEnemyPosition >= enemyPosition.Length)
            {
                currentEnemyPosition = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, enemyPosition[currentEnemyPosition].transform.position, Time.deltaTime * speed); //Time.deltatime helps to set the correct framerate for different devices, making the framerate independent
    }
}
