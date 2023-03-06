using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb; //private means only this script can use this variable
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField]private float moveSpeed = 7f; //using [SerializeField] allows value to be changed in the editor
                                                    //(same thing can be done if we use public instead of private but then other scripts can access the variable)
    [SerializeField]private float jumpForce = 14f;


    private enum MovementState { idle, running, jumping, falling }

                                                                                    //int wholeNumber = 16; //used for whole numbers
                                                                                    //float decimalNumber = 4.54f; //used for decimal numbers, should always put f after number
                                                                                    //string text = "blabla"; //prints text
                                                                                    // Start is called before the first frame update
                                                                                    //bool boolen = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //GetComponenet gets a component directly from unity (e.g. physics like rigid body or an animation)
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame (something we want to happen throughout the game, like movement)
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y); //We use dirX * the x value so that code is more concise and allows joysitck support 
                                                             //using rb.velocity.y means that we can move while we jump

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); //using rb.velocity.x means that we can move while we jump
        }

        UpdateAnimationState(); //animation update is called throughout the game so must be in void update 
    }


    private void UpdateAnimationState() //used to update animation 
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running; 
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded() //this stops the player from being about to jump without touching the ground
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
