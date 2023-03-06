using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private CircleCollider2D circle;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState {idle, running, jumping, falling }

    //[SerializeField] private AudioSource jumpSoundEffect;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            //jumpsoundeffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState() {
        MovementState state;

        if (dirX > 0f) {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f) {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f) { 
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f) {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);

    }

    private bool IsGrounded() {
        return Physics2D.BoxCast(circle.bounds.center, circle.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class PlayerMovement : MonoBehaviour {
//Rigidbody2D rb;
//    Animator animator;
//    public Collider2D standingCollider, crouchingCollider;
//    public Transform groundCheckCollider;
//    public Transform overheadCheckCollider;
//    public LayerMask groundLayer;
//    public Transform wallCheckCollider;
//    public LayerMask wallLayer;

//    const float groundCheckRadius = 0.2f;
//    const float overheadCheckRadius = 0.2f;
//    const float wallCheckRadius = 0.2f;
//    [SerializeField] float speed = 2;
//    [SerializeField] float jumpPower = 500;
//    [SerializeField] float slideFactor = 0.2f;
//    public int totalJumps;
//    int availableJumps;
//    float horizontalValue;
//    float runSpeedModifier = 2f;
//    float crouchSpeedModifier = 0.5f;

//    bool isGrounded = true;
//    bool isRunning;
//    bool facingRight = true;
//    bool crouchPressed;
//    bool multipleJump;
//    bool coyoteJump;
//    bool isSliding;
//    bool isDead = false;

//    void Awake() {
//        availableJumps = totalJumps;

//        rb = GetComponent<Rigidbody2D>();
//        animator = GetComponent<Animator>();
//    }

//    void Update() {
//        if (CanMoveOrInteract() == false)
//            return;

//        //Store the horizontal value
//        horizontalValue = Input.GetAxisRaw("Horizontal");

//        //If LShift is clicked enable isRunning
//        if (Input.GetKeyDown(KeyCode.LeftShift))
//            isRunning = true;
//        //If LShift is released disable isRunning
//        if (Input.GetKeyUp(KeyCode.LeftShift))
//            isRunning = false;

//        //If we press Jump button enable jump 
//        if (Input.GetButtonDown("Jump"))
//            Jump();

//        //If we press Crouch button enable crouch 
//        if (Input.GetButtonDown("Crouch"))
//            crouchPressed = true;
//        //Otherwise disable it
//        else if (Input.GetButtonUp("Crouch"))
//            crouchPressed = false;

//        //Set the yVelocity Value
//        animator.SetFloat("yVelocity", rb.velocity.y);

//        //Check if we are touching a wall to slide on it
//        WallCheck();
//    }

//    void FixedUpdate() {
//        GroundCheck();
//        Move(horizontalValue, crouchPressed);
//    }

//    private void OnDrawGizmosSelected() {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);
//        Gizmos.color = Color.red;
//        Gizmos.DrawSphere(overheadCheckCollider.position, overheadCheckRadius);
//    }

//    bool CanMoveOrInteract() {
//        bool can = true;

//        //if (FindObjectOfType<InteractionSystem>().isExamining)
//        //    can = false;
//        //if (FindObjectOfType<InventorySystem>().isOpen)
//        //    can = false;
//        if (isDead)
//            can = false;

//        return can;
//    }

//    void GroundCheck() {
//        bool wasGrounded = isGrounded;
//        isGrounded = false;
//        //Check if the GroundCheckObject is colliding with other
//        //2D Colliders that are in the "Ground" Layer
//        //If yes (isGrounded true) else (isGrounded false)
//        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
//        if (colliders.Length > 0) {
//            isGrounded = true;
//            if (!wasGrounded) {
//                availableJumps = totalJumps;
//                multipleJump = false;

//                //AudioManager.instance.PlaySFX("landing");
//            }

//            //Check if any of the colliders is moving platform
//            //Parent it to this transform
//            foreach (var c in colliders) {
//                if (c.tag == "MovingPlatform")
//                    transform.parent = c.transform;
//            }
//        }
//        else {
//            //Un-parent the transform
//            transform.parent = null;

//            if (wasGrounded)
//                StartCoroutine(CoyoteJumpDelay());
//        }

//        //As long as we are grounded the "Jump" bool
//        //in the animator is disabled
//        animator.SetBool("Jump", !isGrounded);
//    }

//    void WallCheck() {
//        //If we are touching a wall
//        //and we are moving towards the wall
//        //and we are falling
//        //and we are not grounded
//        //Slide on the wall
//        if (Physics2D.OverlapCircle(wallCheckCollider.position, wallCheckRadius, wallLayer)
//            && Mathf.Abs(horizontalValue) > 0
//            && rb.velocity.y < 0
//            && !isGrounded) {
//            if (!isSliding) {
//                availableJumps = totalJumps;
//                multipleJump = false;
//            }

//            Vector2 v = rb.velocity;
//            v.y = -slideFactor;
//            rb.velocity = v;
//            isSliding = true;

//            if (Input.GetButtonDown("Jump")) {
//                availableJumps--;

//                rb.velocity = Vector2.up * jumpPower;
//                animator.SetBool("Jump", true);
//            }
//        }
//        else {
//            isSliding = false;
//        }
//    }

//    #region Jump
//    IEnumerator CoyoteJumpDelay() {
//        coyoteJump = true;
//        yield return new WaitForSeconds(0.2f);
//        coyoteJump = false;
//    }

//    void Jump() {
//        if (isGrounded) {
//            multipleJump = true;
//            availableJumps--;

//            rb.velocity = Vector2.up * jumpPower;
//            animator.SetBool("Jump", true);
//        }
//        else {
//            if (coyoteJump) {
//                multipleJump = true;
//                availableJumps--;

//                rb.velocity = Vector2.up * jumpPower;
//                animator.SetBool("Jump", true);
//            }

//            if (multipleJump && availableJumps > 0) {
//                availableJumps--;

//                rb.velocity = Vector2.up * jumpPower;
//                animator.SetBool("Jump", true);
//            }
//        }
//    }
//    #endregion

//    void Move(float dir, bool crouchFlag) {
//        #region Crouch

//        //If we are crouching and disabled crouching
//        //Check overhead for collision with Ground items
//        //If there are any, remain crouched, otherwise un-crouch
//        if (!crouchFlag) {
//            if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
//                crouchFlag = true;
//        }

//        animator.SetBool("Crouch", crouchFlag);
//        standingCollider.enabled = !crouchFlag;
//        crouchingCollider.enabled = crouchFlag;

//        #endregion

//        #region Move & Run
//        //Set value of x using dir and speed
//        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
//        //If we are running mulitply with the running modifier (higher)
//        if (isRunning)
//            xVal *= runSpeedModifier;
//        //If we are running mulitply with the running modifier (higher)
//        if (crouchFlag)
//            xVal *= crouchSpeedModifier;
//        //Create Vec2 for the velocity
//        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
//        //Set the player's velocity
//        rb.velocity = targetVelocity;

//        //If looking right and clicked left (flip to the left)
//        if (facingRight && dir < 0) {
//            transform.localScale = new Vector3(-1, 1, 1);
//            facingRight = false;
//        }
//        //If looking left and clicked right (flip to rhe right)
//        else if (!facingRight && dir > 0) {
//            transform.localScale = new Vector3(1, 1, 1);
//            facingRight = true;
//        }

//        //(0 idle , 4 walking , 8 running)
//        //Set the float xVelocity according to the x value 
//        //of the RigidBody2D velocity 
//       // animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
//        #endregion
//    }

//    //public void Die() {
//    //    isDead = true;
//    //    FindObjectOfType<LevelManager>().Restart();
//    //}

//    //public void ResetPlayer() {
//    //    isDead = false;
//    //}

//}



//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class PlayerMovement : MonoBehaviour {
//    Animator animator;
//    private Rigidbody2D rb; //private means only this script can use this variable
//    //private BoxCollider2D coll;
//    private CircleCollider2D circle;
//    private SpriteRenderer sprite;
//    private Animator anim;

//    [SerializeField] private Collider2D standingCollider, crouchingCollider;
//    private Transform overheadCheckCollider;

//    public LayerMask jumpableGround;
//    //public LayerMask groundLayer;

//    private float dirX = 0f;
//    bool crouchPressed;
//    [SerializeField] private float moveSpeed = 7f; //using [SerializeField] allows value to be changed in the editor
//                                                   //(same thing can be done if we use public instead of private but then other scripts can access the variable)
//    [SerializeField] private float crouchSpeed = 4f;
//    [SerializeField] private float jumpForce = 14f;

//    const float overheadCheckRadius = 0.2f;

//    private enum MovementState { idle, running, jumping, falling, crouching }

//    //int wholeNumber = 16; //used for whole numbers
//    //float decimalNumber = 4.54f; //used for decimal numbers, should always put f after number
//    //string text = "blabla"; //prints text
//    // Start is called before the first frame update
//    //bool boolen = false;
//    private void Start() {
//        rb = GetComponent<Rigidbody2D>(); //GetComponenet gets a component directly from unity (e.g. physics like rigid body or an animation)
//        //coll = GetComponent<BoxCollider2D>();
//        circle = GetComponent<CircleCollider2D>();
//        sprite = GetComponent<SpriteRenderer>();
//        anim = GetComponent<Animator>();
//    }

//    // Update is called once per frame (something we want to happen throughout the game, like movement)
//    private void Update() {
//        dirX = Input.GetAxisRaw("Horizontal");
//        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y); //We use dirX * the x value so that code is more concise and allows joysitck support 
//                                                                    //using rb.velocity.y means that we can move while we jump

//        if (Input.GetButtonDown("Jump") && IsGrounded()) {
//            rb.velocity = new Vector2(rb.velocity.x, jumpForce); //using rb.velocity.x means that we can move while we jump
//        }

//        if (Input.GetButtonDown("Crouch"))
//            crouchPressed = true;
//        else if (Input.GetButtonUp("Crouch"))
//            crouchPressed = false;

//        UpdateAnimationState(); //animation update is called throughout the game so must be in void update 
//    }

//    void FixedUpdate() {
//        IsGrounded();
//        //    Move(dirX, crouchPressed);
//    }



//    private void UpdateAnimationState() //used to update animation 
//    {
//        MovementState state;

//        if (dirX > 0f) {
//            state = MovementState.running;
//            sprite.flipX = false;
//        }
//        else if (dirX < 0f) {
//            state = MovementState.running;
//            sprite.flipX = true;
//        }
//        else {
//            state = MovementState.idle;
//        }

//        if (rb.velocity.y > .1f) {
//            state = MovementState.jumping;
//        }
//        else if (rb.velocity.y < -.1f) {
//            state = MovementState.falling;
//        }

//        void Move(float dir, bool crouchFlag) {
//            #region Crouch

//            if (!crouchFlag) {
//                if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, jumpableGround))
//                    crouchFlag = true;
//            }
//            animator.SetBool("Crouch", crouchFlag);
//            standingCollider.enabled = !crouchFlag;
//            crouchingCollider.enabled = crouchFlag;

//            #endregion
//        }


//        anim.SetInteger("state", (int)state);
//    }



//    private bool IsGrounded() //this stops the player from being about to jump without touching the ground
//    {
//        return Physics2D.BoxCast(circle.bounds.center, circle.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
//    }
//}
