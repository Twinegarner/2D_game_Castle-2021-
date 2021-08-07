using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float movementSpeed;
    public float pauseTime;
    public float runTime;
    public bool altMovement;//moving left and then right 
    public bool spriteFaceingLeft;

    private Rigidbody2D myRigidbody;
    private float horizontalMove;
    private float timeLeftPause;
    private float timeLeftRun;
    private bool fliped = true;
    private float deathTime = 1f;
    private bool triggerDeath = false;
    private SpriteRenderer spriteInfo;//edit sprite to flip animation
    private CapsuleCollider2D capsuleCollider;


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteInfo = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        timeLeftRun = runTime;
        timeLeftPause = pauseTime;
        if (spriteFaceingLeft)
        {
            horizontalMove = -movementSpeed; //left faceing sprite
        }
        else
        {
            horizontalMove = movementSpeed; //left faceing sprite
        }
        
    }



    private void FixedUpdate()
    {
        

        if (altMovement)//if alt left and right movment is seleted
        {
            timeLeftRun -= Time.deltaTime;//count down time
            if(timeLeftRun > 0f)
            {
                myRigidbody.velocity = new Vector2(horizontalMove, myRigidbody.velocity.y);//left right movement
                timeLeftPause = pauseTime;
            }
            else if(timeLeftRun <= 0f && timeLeftPause > 0f)
            {
                timeLeftPause -= Time.deltaTime;
                myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);//stop moving
            }else if (timeLeftPause <= 0f && timeLeftRun <= 0f)
            {
                if (fliped)
                {
                    flip(true);
                    fliped = false;
                    timeLeftRun = runTime;
                    timeLeftPause = pauseTime;
                    horizontalMove = movementSpeed;
                }
                else
                {
                    flip(false);
                    fliped = true;
                    timeLeftRun = runTime;
                    timeLeftPause = pauseTime;
                    horizontalMove = -movementSpeed;
                }
            }
            
        }

        if (triggerDeath)//death trigger
        {
            altMovement = false;
            deathTime -= Time.deltaTime;
            if(deathTime < 0f)
            {
                Destroy(gameObject);
            }
        }

    }

    private void flip(bool toggle)//flips sprite
    {
        spriteInfo.flipX = toggle;
    }

    private void OnCollisionEnter2D(Collision2D collision)//handle kill or be killed
    {
        if (collision.gameObject.tag == "Player")//if in contact with player
        {
            if(collision.transform.position.y > transform.position.y + .5f)//player is higher then enemy kill enemy
            {
                spriteInfo.flipY = true;
                capsuleCollider.enabled = false;
                myRigidbody.AddForce(new Vector2(myRigidbody.velocity.x, 10f), ForceMode2D.Impulse);//add force to the jump
                triggerDeath = true;
                
            }
        }
    }
}
