using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 10);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "barrel")
        {
            collision.gameObject.GetComponent<Animator>().Play("barrel_break");
            collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            Destroy(gameObject);
        }
    }

}
