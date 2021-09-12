using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyboardControll2 : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;

    private int availableJumps;
    private float horizontalValue;
    private float runSpeedModifier = 2f;
    private float crouchSpeedModifier = 0.5f;
    const float groundCheckRadius = 0.2f;
    const float overheadCheckRadius = 0.2f;
   
    [SerializeField] float speed;
    [SerializeField] float speedLadder;

    public float jumpPower;
    public int totalJumps;

    private float vertical;
 
    private bool isLadder;
    private bool isClimbing;




    [SerializeField] private Transform groundCheckCollider;
    [SerializeField] private Transform overheadCheckCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D standingCollider, crouchingCollider;
    [SerializeField] private bool isGrounded;

    [Header("WallJump")]
   
    public Transform wallCheckCollider;
    public LayerMask wallLayer;
    const float wallCheckRadius = 0.2f;
    public float slideFactor =0.3f;
    public float WallJumpPower;
    bool isSliding;




    private bool facingRight = true;
    private bool isrunning;
    private bool crouchPressed;
    private bool multipleJumps;
    private bool coyoteJump;
    private bool isDead = false;
   

    private void Awake()
    {
        availableJumps = totalJumps;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
       
    }

    private void Update()
    {
        if (CanMoveOrInteracit() == false)
            return;

        
        //move
        horizontalValue = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift))
          isrunning = true;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            isrunning = false;

        //jump
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }    
       
           
        
        //crouch
        if (Input.GetButtonDown("Crouch"))
            crouchPressed = true;
        else if (Input.GetButtonUp("Crouch"))
            crouchPressed = false;

        animator.SetFloat("yVelocity", rb.velocity.y);
        //Ladder
        vertical = Input.GetAxisRaw("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
          
        }


        WallCheck();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, crouchPressed);
        //Ladder
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speedLadder);
            

        }
        else
        {
            rb.gravityScale = 4f;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(overheadCheckCollider.position, overheadCheckRadius);
    }
     
    private bool CanMoveOrInteracit()
    {
        bool can = true;

        if (FindObjectOfType<InteractionSystem>().isExamining)
            can = false;
        if (FindObjectOfType<InventorySystem>().isOpen)
            can = false;
        if (isDead)
            can = false;

        return can;
    }    

    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll( groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJumps = false;

                AudioManager.instance.PlaySFX("landing");
            }

            foreach(var c in colliders)
            {
                if (c.CompareTag("MovingPlatform"))
                    transform.parent = c.transform;
            }
              
        }
        else
        {
            transform.parent = null;

            if (wasGrounded)
                StartCoroutine(CoyoteJumpDelay());
        }
            
        animator.SetBool("Jump", isGrounded);

    }

    void WallCheck()
    {
       
        if (Physics2D.OverlapCircle(wallCheckCollider.position, wallCheckRadius, wallLayer)
            && Mathf.Abs(horizontalValue) > 0
            && rb.velocity.y < 0
            && !isGrounded)
        {
            if (!isSliding)
            {
                availableJumps = totalJumps;
                multipleJumps = false;
            }

            Vector2 v = rb.velocity;
            v.y = -slideFactor;
            rb.velocity = v;
            isSliding = true;
            if (Input.GetButtonDown("Jump"))
            {
                availableJumps--;
                rb.velocity = Vector2.up * WallJumpPower;
                animator.SetBool("Jump", true);
            }


        }
        else if(isSliding)
        {
            isSliding = false;
        }
    }

    #region Jump
    IEnumerator CoyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.2f);
        coyoteJump = false;
    }
    

    private void Jump()
    {
        if (isGrounded)
        {
            multipleJumps = true;
            availableJumps--;

            rb.velocity = Vector2.up * jumpPower;
            animator.SetBool("Jump", true);
            animator.SetBool("Climb", false);
        }


        else
        {
            if (coyoteJump)
            {
                multipleJumps = true;
                availableJumps--;
                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
              
            }
            if (multipleJumps && availableJumps > 0)
            {

                availableJumps--;
                rb.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
               
            }
        }    
    }
    #endregion

    private void Move(float dir, bool crouchFlag)
    {
        #region Crouch

        if(!crouchFlag)
        {
            if(Physics2D.OverlapCircle(overheadCheckCollider.position,overheadCheckRadius,groundLayer))
            
                crouchFlag = true;
        }

       
        animator.SetBool("Crouch",crouchFlag);
        standingCollider.enabled = !crouchFlag;
        crouchingCollider.enabled = crouchFlag;

        #endregion

        #region Move & Run

        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        if (isrunning)
            xVal *= runSpeedModifier;
        if (crouchFlag)
            xVal *= crouchSpeedModifier;
  
        Vector2 targetvelocity = new Vector2(xVal, rb.velocity.y);

        rb.velocity = targetvelocity;

        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        else if (!facingRight && dir > 0) 
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
       
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        #endregion

    }

    public void Die()
    {
        isDead = true;
        FindObjectOfType<LevelManager>().Restart();
    }

    public void ResetPlayer()
    {
       
        isDead = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
            animator.SetBool("Climb", true);
            Debug.Log("climb");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            animator.SetBool("Climb", false);
            Debug.Log("Not climb");
        }

    }


}


