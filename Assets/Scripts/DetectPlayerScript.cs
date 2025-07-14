using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" & PlayerScript.isAlive)
        {
            if (transform.parent.name == "bee")
            {
                transform.parent.GetComponent<BeeScript>().InvokeRepeating("Attack", 0, 3);
            }
            else if (transform.parent.name == "snowman")
            {              
                transform.parent.GetComponent<SnowManScript>().InvokeRepeating("Shoot", 0, 2f);
            }
        }

        if(collision.gameObject.name == "chicken")
        {
            if (transform.parent.name == "snowman")
            {              
                transform.parent.GetComponent<SnowManScript>().CancelInvoke("Shoot");
                transform.parent.GetComponent<SnowManScript>().InvokeRepeating("Shoot", 1, 2f);
            }
        }

        if (collision.gameObject.tag == "Player" & PlayerScript.isAlive)
        {
            if (transform.parent.name == "goblin_rider")
            {
                transform.parent.GetComponent<GoblinRiderScript>().moveRight = false;
                transform.parent.GetComponent<GoblinRiderScript>().moveLeft = false;
                transform.parent.GetComponent<Animator>().Play("goblin_rider_run");
                transform.parent.GetComponent<GoblinRiderScript>().canRun = true;
            }

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (transform.parent.name == "bee")
            {
                transform.parent.GetComponent<BeeScript>().LookAtPlayer();
            }
            else if (transform.parent.name == "goblin_rider")
            {
                transform.parent.GetComponent<GoblinRiderScript>().LookAtPlayer();
            }

        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (transform.parent.name == "bee")
            {
                transform.parent.GetComponent<BeeScript>().CancelInvoke("Attack");
            }
            else if (transform.parent.name == "snowman" & PlayerScript.isAlive)
            {
                transform.parent.GetComponent<SnowManScript>().CancelInvoke("Shoot");
            }
            else if (transform.parent.name == "goblin_rider" & PlayerScript.isAlive)
            {
                transform.parent.GetComponent<GoblinRiderScript>().canRun = false;
                transform.parent.GetComponent<Animator>().Play("goblin_rider_idle");
                GetComponentInParent<GoblinRiderScript>().InvokeRepeating("ContinuePatrol", 2, 0.01f);
            }


        }

        if(collision.gameObject.name == "chicken")
        {
            transform.parent.GetComponent<SnowManScript>().CancelInvoke("Shoot");
        }
    }
}
