/*
 * Author: Nicholas Kiv
 * Last Updated: 6/11/2018
 * 
 * enemy movement/attack/stats and hit detection and moves to the player aswell for the pink slime
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {



    public Collider2D colHit;

    //initialization

    private Rigidbody2D rb2d;
	[Range(0f, 3f)]
    public float speed;
	private SpriteRenderer enemy;
    Animator ani_Enemy;
	public LayerMask playerLayer;
    private Transform target;

    //aggro range

	[Range(0f, 3f)]
	public float attackRange;

	[Range(0f, 10f)]
	public float sightRange;

    //reaction to dmg
    
    public float knockBack;
    public float knockUp;

    public LayerMask sightLayer;
	public bool sighted;

    public float health;
    bool isAlive = true;
   


    void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		rb2d = GetComponent<Rigidbody2D>();
        //Targets the player specifically
		enemy = GetComponent<SpriteRenderer>();
        ani_Enemy = GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(11,11);
        Physics2D.IgnoreLayerCollision(9, 11);



    }
	
	void Update ()//follows the player
    {
        if (isAlive)
            if (rb2d.velocity.y == 0)
            {
                sighted = sight();

                if (sighted)
                {
                    enemyFollow();

                    raycastHitDetection();
                }

                if (!sighted)
                {
                    idle();
                }

            }
      



    }

    void enemyFollow()//follows the player
	{

		if ((transform.position.x < target.position.x ))
			rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
		else if (transform.position.x > target.position.x)
			rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
	}

	void raycastHitDetection()//detects the player using raycasting to determine if the player is with aggro range and attack range
	{
		Debug.DrawRay(transform.position, Vector2.right, Color.green);
		Debug.DrawRay(transform.position, Vector2.left, Color.red);

		//Drawing of raycast in Debug

		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, attackRange, playerLayer);
		RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.right, attackRange, playerLayer);

		//attack range Detection with RayCast


		if (hit || hit2) {
			ani_Enemy.SetBool ("attack", true);
			rb2d.velocity = new Vector2(0, rb2d.velocity.y);

		}
        //signal animator using attack parameter for animator
		else
			ani_Enemy.SetBool ("attack", false);
		//Else attack animation is false


		if (rb2d.velocity.x > 0)
			enemy.flipX = false;
		else if (rb2d.velocity.x < 0)
			enemy.flipX = true;

		

		
	}

	bool sight()
	{


		RaycastHit2D leftSight = Physics2D.Raycast(transform.position, Vector2.left, sightRange, playerLayer);
		RaycastHit2D rightSight = Physics2D.Raycast(transform.position, Vector2.right, sightRange, playerLayer);

		if (leftSight || rightSight) {	
			return true;
		} 
		else {
		
			return false;
		}

	}

    //determines the direction of the player
    int playerDirection()
    {
        if ((transform.position.x < target.position.x))
            return -1;
        else
            return 1;
    }

    //hit and react to player attack and lose health aswell as get knocked back and knocked up, upon dying, spin and destroy rigid body physics aswell as collider

    void IsHit(float dmg)
    {
        Debug.Log("recieve");
        rb2d.AddForce(transform.up * knockUp);
        rb2d.AddForce(transform.right * knockBack * playerDirection());

        health = health - dmg;

        if (health <= 0)//when player reaches zero or below health
        {
            Destroy(gameObject, 3);//3 seconds till is dies

            rb2d.freezeRotation = false; // unlock Z axis for rotaton
            
          

            Destroy(gameObject.GetComponent<BoxCollider2D>());
            rb2d.AddTorque(5 * -playerDirection()); // rotate va torque
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            Destroy(gameObject.GetComponent<CircleCollider2D>(), 2.0f);// destroy its collider so it fall through the ground

            Destroy(GetComponent<Animator>());

            Debug.Log(gameObject.name + "is dead");
            isAlive = false;


        }

    }
    

}
