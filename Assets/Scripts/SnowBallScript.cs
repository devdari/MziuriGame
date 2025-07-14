using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallScript : MonoBehaviour
{
    public GameObject impact;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject impactClone = Instantiate(impact, transform.position, transform.rotation);            
        }

        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Shield")
        {
            GameObject impactClone = Instantiate(impact, transform.position, transform.rotation);
        }

        if (collision.gameObject.tag == "snowman")
        {
            GameObject impactClone = Instantiate(impact, transform.position, transform.rotation);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
