using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Update is called once per frame
    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z); //using player.position.x/y tells the script to follow the x,y of player
                                                                                                        //using transform.position.z tells the script to keep the original z position of the camera
    }
}
