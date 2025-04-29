using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public GameObject impact;

    Transform elevator;

    Animator leverAnim;

    public GameObject[] lever;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
        elevator = GameObject.FindGameObjectWithTag("elevator").transform;
        lever = GameObject.FindGameObjectsWithTag("lever");
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "spiderden")
        {
            collision.GetComponent<Animator>().Play("spider den");
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.transform.GetChild(0).gameObject.SetActive(true);
            collision.transform.GetChild(1).gameObject.SetActive(true);
            collision.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (collision.gameObject.layer == 3)
        {            
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Instantiate(impact, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "barrel")
        {
            Instantiate(impact, transform.position, transform.rotation);
            collision.gameObject.GetComponent<Animator>().Play("barrel_break");
            collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == "lever")
        {
            leverAnim = collision.GetComponent<Animator>();
            if (leverAnim.GetCurrentAnimatorStateInfo(0).IsName("New State") & LeverScript.canAnimate)
            {

                leverAnim.GetComponent<LeverScript>().SetCanAnimate();
                leverAnim.GetComponent<LeverScript>().Invoke("ResetCanAnimate", 3);
                leverAnim.Play("lever right");
                elevator.GetComponent<Animator>().Play("elevator_down");
                lever[0].GetComponent<Animator>().Play("lever right");
                lever[1].GetComponent<Animator>().Play("lever right");
            }
            else if(collision.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("lever right") & LeverScript.canAnimate)
            {
                leverAnim.GetComponent<LeverScript>().SetCanAnimate();
                leverAnim.GetComponent<LeverScript>().Invoke("ResetCanAnimate", 3);
                leverAnim.Play("lever left");
                elevator.GetComponent<Animator>().Play("elevator_up");
                lever[0].GetComponent<Animator>().Play("lever left");
                lever[1].GetComponent<Animator>().Play("lever left");
            }
            else if (collision.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("lever left") & LeverScript.canAnimate)
            {
                leverAnim.GetComponent<LeverScript>().SetCanAnimate();
                leverAnim.GetComponent<LeverScript>().Invoke("ResetCanAnimate", 3);
                leverAnim.Play("lever right");
                elevator.GetComponent<Animator>().Play("elevator_down");
                lever[0].GetComponent<Animator>().Play("lever right");
                lever[1].GetComponent<Animator>().Play("lever right");
            }
            Destroy(gameObject);
        }
    }

   
}
