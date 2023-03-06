using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;

    void Update ()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
    }

    void FixedUpdate ()
    {
        //controller.Move(horizontalMove * Time.fixedDeltaTime, false, false); 
    }
}
