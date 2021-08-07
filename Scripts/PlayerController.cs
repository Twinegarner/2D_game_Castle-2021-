using System.Collections;
using System.Collections.Generic;
using UnityEngine;







//need to find a locking system






public class PlayerController : MonoBehaviour
{
    //public vars
    public float moveSpeed = 10f;//basic movement speed
    public float jumpForce = 5f;
    public bool jumping = false;//used to check if jumping
    public LayerMask groundLayer;//to hold what the raycast is looking at
    public float attackForce = 1f;
 

    //private vars
    private Rigidbody2D myRigidbody2D;//get the rigidbody information and change it
    private Animator anim;//the animation var
    private SpriteRenderer spriteInfo;//edit sprite to flip animation
    private AnimatorStateInfo animInfo;
    private BoxCollider2D myAttackBox;
    private CapsuleCollider2D myHitBox;

    private float HorizontalMove;
    private float moveingY;
    private bool isAttacking = false;
    private bool lastFace;//true right false left
    
    
 

    // Start is called before the first frame update
    //find and set the objects rigidbody to myRigid
    //find the animator and spriteinfo
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteInfo = GetComponent<SpriteRenderer>();
        myHitBox = GetComponent<CapsuleCollider2D>();
        myAttackBox = GetComponentInChildren<BoxCollider2D>();
       

    }

    // Update is called once per frame
    //capture movement
    //cpature jump
    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);
        //horizontal move is the primary movement var
        if (animInfo.IsName("mage_idle") || animInfo.IsName("mage_run") || animInfo.IsName("mage_jump"))
        {
            HorizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        }
        //checks the ground if in contact and displays in editor
        IsGrounded();
        //if jump is hit and in contact to the ground
        if (Input.GetButtonDown("Jump") && IsGrounded() && !isAttacking)
        {
            jumping = true;
            moveingY = 1f;
        }
        //the player attack 
        if (Input.GetKeyDown(KeyCode.F) && IsGrounded())
        {
            playerAttack();
        }

    }
    //called limited times
    private void FixedUpdate()
    {
        //player movement
        //left right movement if lock isnt active
        myRigidbody2D.velocity = new Vector2(HorizontalMove, myRigidbody2D.velocity.y);


        //player lock in during attack by grabbing the animator state infomation 
        animInfo = anim.GetCurrentAnimatorStateInfo(0);
        //if first attack stop player and bump forward
        if((animInfo.IsName("mage_att_1") || animInfo.IsName("mage_att_2")|| animInfo.IsName("mage_att_3")) && animInfo.normalizedTime < .1)
        {
            
            HorizontalMove = 0;
            //bump forward during attack using impluse
            myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (lastFace)//go right
            {
                myRigidbody2D.AddForce(new Vector2(-attackForce, myRigidbody2D.velocity.y), ForceMode2D.Impulse);
            }
            else if (!lastFace)
            {
                myRigidbody2D.AddForce(new Vector2(attackForce, myRigidbody2D.velocity.y), ForceMode2D.Impulse);
            }
            
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
            
        }


        //check if grounded and stop horizontal movement if on ledges or stairs
        if (HorizontalMove == 0 && IsGrounded() && !isAttacking)
        {
            myRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
           
        }
        else if(HorizontalMove != 0)
        {
            myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        
        
        

        //the player hits jump button
        //add force to the jump
        if (jumping)
        {
            myRigidbody2D.AddForce(new Vector2(myRigidbody2D.velocity.x, jumpForce), ForceMode2D.Impulse);
            jumping = false;
            moveingY = 0f;
        }

        //postive movment or to the right and checks the players facing movment
        
        checkFaceDir(HorizontalMove);

        //aniamtion sets
        anim.SetFloat("GetX", HorizontalMove);
        anim.SetFloat("GetY", moveingY);
        anim.SetBool("isJumping", jumping);

    }

    

    //command the player to attack by animation trigger "attack"
    private void playerAttack()
    {
        anim.SetTrigger("Attack");
    }

    private void checkFaceDir(float moving)
    {
        //facing left
        if (moving > 0 && lastFace)
        {
            flip(false);
            lastFace = false;
            myAttackBox.offset = new Vector2(myAttackBox.offset.x * -1,myAttackBox.offset.y);
            myHitBox.offset = new Vector2(myHitBox.offset.x * -1,myHitBox.offset.y);
        }
        //facing right
        else if (moving < 0 && !lastFace)
        {
            flip(true);
            lastFace = true;
            myAttackBox.offset = new Vector2(myAttackBox.offset.x * -1, myAttackBox.offset.y);
            myHitBox.offset = new Vector2(myHitBox.offset.x * -1, myHitBox.offset.y);
        }
        
    }

    //flips sprite
    private void flip(bool toggle)
    {
        spriteInfo.flipX = toggle;

    }

    //checks if the player is grounded useing raycasting
    private bool IsGrounded()
    {
        Vector2 position = myRigidbody2D.transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.5f;
        Debug.DrawRay(position, direction, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        //gounded check
        if(hit.collider == null)
        {
            moveingY = 1f;
        }
        else
        {
            moveingY = 0f;
        }

        //Debug.Log(hit.collider);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

   
}
