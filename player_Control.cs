/*
 * Author: Nicholas Kiv
 * Last Updated: 6/18/2018
 * 
 * player controls:
 * includes: attack,jump, movement, and rigid body physics as well as animator calls
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_Control : MonoBehaviour
{




    private SpriteRenderer player;


    // parameters for Animator
    Animator ani_Player;

    public float Xspeed;
    public float Yspeed;

    //public variables to change running, walking, and sprint speeds
    public float runSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public Vector2 offset;

    //boolean for motion/movement
    private bool run = false;
    private bool sprint = false;
    private bool walk = false;
  


    public LayerMask groundLayer;

    //private float speedM;
    //private float Speedfinal;
    private float changeInY;

    private bool jumpFlag;
    public float speed;
    public float jumpheight;
    private Rigidbody2D rb2d;
    private GameObject target;
    public bool grounded;

   


    // ATTACK VARIABLES
    //----------------------------------------------------

    public Collider2D AttackTrigger1;
    public float attackTimer1;
    public float attackCd1;
    private bool punch1;
    public bool attackFlag;
    public float attackLag1;
    public float attackLagReset1;
    //----------------------------------------------------

    public GameObject source;

    // intialiazing physics rigid body, sprite renderer, and animator 
    void Start()
    {
        ani_Player = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        player = GetComponent<SpriteRenderer>();
        //nextlevel = false;

        AttackTrigger1.enabled = false;
    }

    /// <summary>
    /// player controls such as jumping and running/walking/spring/attacking etc
    /// </summary>
    void FixedUpdate()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        float speedM = runSpeed;

        grounded = groundcheck();

        if (Input.GetKeyDown("space") && grounded)
            if (grounded && rb2d.velocity.y == 0)
                rb2d.AddForce(transform.up * jumpheight);

        if (grounded)
        {

            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
            {
                sprint = true;
                walk = false;
                sprint = true;
                run = false;
                speedM = sprintSpeed;
            }
            else if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
            {
                walk = true;
                sprint = false;
                run = false;
                speedM = walkSpeed;
            }
            else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                run = true;
            else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                walk = false;
                sprint = false;
                run = true;
                speedM = 1;
            }

            //++++++++++++++++++++++++++++++++++++++++++++++

            speedM = attack(speedM);

            //+++++++++++++++++++++++++++++++++++++++++++++++
            float Speedfinal = speedM * speed;

            float moveHorizontal = Input.GetAxis("Horizontal");
            rb2d.velocity = new Vector2(moveHorizontal * Speedfinal, rb2d.velocity.y);
        }

        if (!grounded)
        {


            if (sprint)
                speedM = sprintSpeed;

            if (walk)
                speedM = walkSpeed;

            if (run)
                speedM = 1;

            punch1 = false;

            float Speedfinal = speedM * speed;
            float moveHorizontal = Input.GetAxis("Horizontal");
            rb2d.velocity = new Vector2(moveHorizontal * Speedfinal, rb2d.velocity.y);
        }

        if (punch1)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }




        if (Input.GetKeyUp(KeyCode.R))
        {
            DestroyObject(target);
            Instantiate(source, target.transform.position, source.transform.rotation);
            target = GameObject.FindGameObjectWithTag("Player");
        }
        //Debug.DrawRay(transform.position, Vector2.down, Color.red, 1f, true);




    }

    IEnumerator nextFrame()
    {
        yield return new WaitForFixedUpdate();
    }

    
    //raycasting for ground check
    bool groundcheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, .55f, groundLayer);
        if (hit.collider == null)
            return false;
        else
            return true;
    }

    //parameters for the animator
    void Update()
    {
        // Debug.DrawRay(transform.position, Vector2.down, Color.green);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, .51f, groundLayer);

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //Vector2 directioono = new Vector2(mousePosition.x, mousePosition.y);

        grounded = groundcheck();

        if (Input.GetKeyDown("space") && grounded)
        {
            if (grounded && rb2d.velocity.y == 0)
            {

                rb2d.AddForce(transform.up * jumpheight);

            }
        }

        //=========================================================

        //ATTACK MECHANICS


        attack_Mechanic(mousePosition);

        //================================================================

        if (rb2d.velocity.x > 0)
            player.flipX = false;
        else if (rb2d.velocity.x < 0)
            player.flipX = true;


        if (PauseMenu.GameIsPaused)
            player.flipX = false;

        /*
        if (punch1 && rb2d.position.x < Input.mousePosition.x)
            player.flipX = false;
        else if (punch1 && rb2d.position.x > Input.mousePosition.x)
            player.flipX = true;



        */


        ///ANIMATOR PARAMETERS

        //================================================================================================
        float velocity = rb2d.velocity.x;

        if (rb2d.velocity.y == 0)
            ani_Player.SetFloat("SpeedX", rb2d.velocity.x);
        if (rb2d.velocity.y == 0)
            ani_Player.SetBool("is_In_Air", false);
        else
            ani_Player.SetBool("is_In_Air", true);
        ani_Player.SetBool("isRunning", false);
        ani_Player.SetBool("isWalking", false);
        ani_Player.SetBool("isSprinting", false);

        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
        {
            ani_Player.SetBool("isSprinting", true);
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
        {
            ani_Player.SetBool("isWalking", true);
        }
        else if (rb2d.velocity.x != 0)
            ani_Player.SetBool("isRunning", true);
        if (rb2d.velocity.x == 0)
            ani_Player.SetBool("isIdle", true);
        else
            ani_Player.SetBool("isIdle", false);
        ani_Player.SetFloat("SpeedX", Mathf.Abs(rb2d.velocity.x));
        ani_Player.SetFloat("SpeedY", rb2d.velocity.y);

        ani_Player.SetBool("punch1", punch1);
        //================================================================================================

    }

    float attack(float speedM) //attacking attack trigger
    {

       // Vector3 mousePosition = Input.mousePosition;
       // mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (Input.GetKey(KeyCode.Mouse0) && !punch1)
        {
           
            punch1 = true;
            attackTimer1 = attackCd1;
            attackLag1 = attackLagReset1;
            //AttackTrigger1.enabled = true;

        }
    

        if (punch1)
        {
            if (attackTimer1 > 0)//attack cooldown
            {
                attackTimer1 -= Time.deltaTime;
                attackLag1 -= Time.deltaTime;

                if (attackLag1 < 0)//attack delay
                {
                    AttackTrigger1.enabled = true;
                }
                
            }
            else//attack reset
            {
                punch1 = false;
                AttackTrigger1.enabled = false;
            }
            
        }



        attackFlag = AttackTrigger1.enabled; // enable attack trigger

        if (attackFlag)
            speedM = 0;

        return speedM;
    }
    
    void attack_Mechanic(Vector3 mousePosition) //directs the punch to the direction of the mosue
    {

        
        
        if (rb2d.position.x < mousePosition.x)
        {
            player.flipX = false;
            AttackTrigger1.offset = offset;
            

        }

        else if (rb2d.position.x > mousePosition.x)
        {
            player.flipX = true;
            AttackTrigger1.offset = -offset;
           

        }


        

    }

        


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            //DestroyObject(coll.gameObject);
            //nextlevel = true;
        }
        /*
		if ((coll.gameObject.tag == "Door") //&& (nextlevel == true))
		{
			//DestroyObject(coll.gameObject);
			string scene = SceneManager.GetActiveScene().name;
			if ( scene == "MTP1") 
			{
				Application.LoadLevel("MTP2");
			}
			if (scene == "MTP2") 
			{
				Application.LoadLevel("MTP3");
			}
		*/
    }




}
