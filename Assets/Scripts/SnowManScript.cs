using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowManScript : MonoBehaviour
{
    public float health;
    public GameObject snowBall;
    public float snowForce;
    public void Shoot()
    {
        GameObject snowBallClone = Instantiate(snowBall, transform.position - new Vector3(2,0), Quaternion.identity);
        snowBallClone.GetComponent<Rigidbody2D>().AddForce(Vector2.left * snowForce, ForceMode2D.Impulse);
        Destroy(snowBallClone, 3);
    }



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
        if (collision.gameObject.name == "snowball(Clone)")
        {
            health -= 1;
            transform.localScale = new Vector3(transform.localScale.x + 0.4f, transform.localScale.y + 0.4f);
            Destroy(collision.gameObject);
            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
