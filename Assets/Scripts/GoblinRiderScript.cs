using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinRiderScript : MonoBehaviour
{

    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator anim;
    public Transform player;
    Transform patrolPoint1, patrolPoint2;
    public bool moveRight, moveLeft, canRun;



    public Transform hitBox;
    public Vector2 hitBoxSize;
    public LayerMask playerLayer;
    public Collider2D playerToHit;


    public void AttackHitBox()
    {
        playerToHit = Physics2D.OverlapBox(hitBox.position, hitBoxSize, 0, playerLayer);
        if(playerToHit != null)
        {
            playerToHit.GetComponent<PlayerScript>().PlayerDamage(2);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(hitBox.position, hitBoxSize);
    }

    public void ContinuePatrol()
    {
        patrolPoint1.gameObject.SetActive(true);
        patrolPoint2.gameObject.SetActive(true);
        transform.position = Vector2.MoveTowards(transform.position, patrolPoint1.position, moveSpeed * Time.deltaTime);
        anim.Play("goblin_rider_walk");
    }


    public void LookAtPlayer()
    {
        if (transform.position.x > player.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "patrol point 1" & !canRun)
        {
            patrolPoint1 = collision.transform;
            moveLeft = false;
            moveRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            anim.Play("goblin_rider_walk");
            CancelInvoke("ContinuePatrol");
            print("point1");
        }

        if (collision.gameObject.name == "patrol point 2" & !canRun)
        {
            print("point2");

            patrolPoint2 = collision.transform;
            moveRight = false;
            moveLeft = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            anim.Play("goblin_rider_walk");
            CancelInvoke("ContinuePatrol");

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(moveRight)
        {           
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }

        if (moveLeft)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }

        if(canRun)
        {
            CancelInvoke("ContinuePatrol");
            patrolPoint1.gameObject.SetActive(false);
            patrolPoint2.gameObject.SetActive(false);

            float distance = Vector2.Distance(transform.position, player.position);
            if(distance < 1)
            {
                anim.Play("goblin_rider_attack");
            }
            else
            {
                moveSpeed = 4;
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
            
        }

    }
}
