using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public float snowForce;
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
        if(collision.gameObject.name == "snowball(Clone)")
        {
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            collision.GetComponent<Rigidbody2D>().AddForce(Vector2.right * snowForce, ForceMode2D.Impulse);
        }
    }
}
