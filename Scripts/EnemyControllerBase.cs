using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerBase : MonoBehaviour
{

    //set what each enemy can do
    public int healthPoints;
    public float enemySpeed = 5f;
    public bool shuffleOnly;
    public bool followAttack;
    public bool waitTillPlayer = false;
    public float moveTimer = 2f;
    public bool OHK;//one hit kill
    public float visionRangeX = 20f;
    public float visionRangeY = 5f;
    public float attackRangeX = 1f;
    public float attackRangeY = 1f;
    public float attackDelay = 2f;

    //private componets
    private GameObject player;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer spriteInfo;
    private PlayerStats myPlayerStats;
    private EnemyStats myEnemyStats;
    private AnimatorStateInfo animInfo;
    private Animator anim;
    private float inputSpeed = 1f;//speed mutiplyer
    private float horizontalSpeed;
    private float PVTmoveTimer;
    private bool pauseMovement = false;
    private bool deathTrigger = false;
    private float deathTimer = 5f;
    private float attackDelayPVT;
    public bool playerFound = false;


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteInfo = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        myPlayerStats = FindObjectOfType<PlayerStats>();
        player = GameObject.FindGameObjectWithTag("Player");
        myEnemyStats = GetComponentInChildren<EnemyStats>();

        PVTmoveTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        animInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!animInfo.IsName("attack"))
        {
            horizontalSpeed = inputSpeed * enemySpeed;
        }
        else
        {
            horizontalSpeed = 0;
        }
        
    }
    //limited frame call
    private void FixedUpdate()
    {
        //cal that have to go first

        //if death then linger then destroy
        if (deathTrigger)
        {
            //take over timer to count down till fade
            deathTimer -= Time.deltaTime;
            if(deathTimer <= 0)
            {
                Destroy(gameObject);
            }
            

        }
        //if idle and run then shuffle back and forth
        else if (shuffleOnly && !followAttack)
        {
            PVTmoveTimer += Time.deltaTime;
            //once time has been reached flip
            if ((PVTmoveTimer >= moveTimer) && !pauseMovement)
            {
                myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                anim.SetFloat("GetX", 0f);
                myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);

                flipSpeed();
                flipSprite();
                PVTmoveTimer = 0f;
                pauseMovement = !pauseMovement;
            }
            else if ((PVTmoveTimer >= moveTimer) && pauseMovement)
            {
                PVTmoveTimer = 0f;
                pauseMovement = !pauseMovement;
            }
            //if not paused then move
            if (!pauseMovement)
            {
                anim.SetFloat("GetX", horizontalSpeed);
                myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                myRigidbody.velocity = new Vector2(horizontalSpeed, myRigidbody.velocity.y);
            }
        }
        else if(followAttack)
        {
            //player found algo
            isPlayerFound();




            if (!playerFound)
            {
                PVTmoveTimer += Time.deltaTime;
                //once time has been reached flip
                if ((PVTmoveTimer >= moveTimer) && !pauseMovement)
                {
                    myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    anim.SetFloat("GetX", 0f);
                    myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);

                    flipSpeed();
                    flipSprite();
                    PVTmoveTimer = 0f;
                    pauseMovement = !pauseMovement;
                }
                else if ((PVTmoveTimer >= moveTimer) && pauseMovement)
                {
                    PVTmoveTimer = 0f;
                    pauseMovement = !pauseMovement;
                }
                //if not paused then move
                if (!pauseMovement)
                {
                    anim.SetFloat("GetX", horizontalSpeed);
                    myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    myRigidbody.velocity = new Vector2(horizontalSpeed, myRigidbody.velocity.y);
                }
            }
            // if the player is in range of the enemy then follow and attack
            else if (playerFound)
            {
                //first find the direction to face starting with the left then right
                //use input speed a the enemys facing direction
                if(player.transform.position.x <= gameObject.transform.position.x)
                {
                    //check enemys facing dir
                    if(inputSpeed == -1f)
                    {

                    }
                    else
                    {
                        flipSpeed();
                        flipSprite();
                    }
                }
                //player is to the right
                else
                {
                    //check enemys facing dir
                    if (inputSpeed == -1f)
                    {
                        flipSpeed();
                        flipSprite();
                    }
                    else
                    {
                        
                    }
                }

                attackDelayPVT -= Time.deltaTime;

                if(attackDelayPVT <= 0)
                {
                    //now that we have dir we need movement
                    anim.SetFloat("GetX", horizontalSpeed);
                    myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    myRigidbody.velocity = new Vector2(horizontalSpeed, myRigidbody.velocity.y);
                    attackDelayPVT = 0;
                    isPlayerAttack();
                }
                else
                {
                    anim.SetFloat("GetX", 0);
                    myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
                }
               

                //check if player is within attack range
                
                

            }
        }

        //work on attacking the player if close
        
            
        
    }
    //flip the enemies direction
    private void flipSpeed()
    {
        if(inputSpeed == 1f)
        {
            inputSpeed = -1f;
        }
        else if(inputSpeed == -1f)
        {
            inputSpeed = 1f;
        }
    }

    private void flipSprite()
    {
        spriteInfo.flipX = !spriteInfo.flipX;
        //vision.gameObject.transform.localScale = new Vector3(vision.gameObject.transform.localScale.x * -1, vision.gameObject.transform.localScale.y, vision.gameObject.transform.localScale.z);
    }

    //if the player is found then change playerfound
    private void isPlayerFound()
    {
        //first grab the diff of the two axis
        float tempX = Mathf.Abs(player.transform.position.x - gameObject.transform.position.x);
        float tempY = Mathf.Abs(player.transform.position.y - gameObject.transform.position.y);

        //now if within the X range chack for Y range
        if(tempX <= visionRangeX && tempY <= visionRangeY)
        {
            playerFound = true;
        }
        else
        {
            playerFound = false;
        }


    }

    //if the player reachs the attack range
    private void isPlayerAttack()
    {
        float tempX = Mathf.Abs(player.transform.position.x - gameObject.transform.position.x);
        float tempY = Mathf.Abs(player.transform.position.y - gameObject.transform.position.y);

        if (tempX <= attackRangeX && tempY <= attackRangeY)
        {
            anim.SetTrigger("Attack");
            attackDelayPVT = attackDelay;
        }
        
    }


    // the enemy comes in contact with the players attack
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if player enters vision cone then follow and attack
        if(collision.gameObject.tag == "Player")
        {
            //myPlayerStats.addHealthPoints(-1);

            

        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //first check if hit player
        if (collision.gameObject.tag == "PlayerAttack")
        {
            //if player then get hit or die animations
            if (OHK)
            {
                myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                anim.SetTrigger("Hit");
                anim.SetBool("Dead", true);
                deathTrigger = true;
            }
            else
            {
                myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                anim.SetTrigger("Hit");
                myEnemyStats.addEnemyHealthPoints(-(collision.gameObject.GetComponentInChildren<PlayerStats>().getPlayerAttackDamage()));
                if(myEnemyStats.getEnemyHealthPoints() <= 0)
                {
                    anim.SetBool("Dead", true);
                    deathTrigger = true;
                }
            }


        }
    }
    
    

}
