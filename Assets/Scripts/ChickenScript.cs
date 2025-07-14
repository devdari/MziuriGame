using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class ChickenScript : MonoBehaviour
{

    [Header("Chicken Movement")]
    [Space(5)]
    public float speed;
    public float jumpForce;
    bool isGrounded;
    float move;
    float jumpCount = 1;

    [Header("Chicken Components")]
    [Space(5)]
    Rigidbody2D rb;
    Animator anim;

    [Header("Chicken Shoot")]
    [Space(5)]
    public Text eggBulletText;    
    public float eggBulletForce;
    public GameObject eggBulletPrefab;

    [Header("Chicken Health")]
    [Space(5)]
    public GameObject gameOver;

    void ChickenShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {           
            anim.SetTrigger("peck");                        
            GameObject eggBulletClone = Instantiate(eggBulletPrefab, transform.position, transform.rotation);
            
            if(transform.localScale.x > 0)
            {
                eggBulletClone.GetComponent<Rigidbody2D>().velocity = transform.right * eggBulletForce;
            }
            else
            {
                eggBulletClone.GetComponent<Rigidbody2D>().velocity = -transform.right * eggBulletForce;
            }

            Destroy(eggBulletClone, 2);
        }
    }

    void ChickenMove()
    {
        move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(speed * move, rb.velocity.y);

        if (move > 0)
        {
            anim.SetBool("run", true);
            transform.localScale = new Vector2(0.7f, 0.7f);
        }
        else if (move < 0)
        {
            anim.SetBool("run", true);           
            transform.localScale = new Vector2(-0.7f, 0.7f);
        }
        else
        {
            anim.SetBool("run", false);           
        }


        if (Input.GetKeyDown(KeyCode.Space) & jumpCount > 0)
        {
            anim.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount -= 1;
        }

        
    }

    void ChickenStart()
    {
        eggBulletText.text = "∞";
        GetComponent<BoxCollider2D>().isTrigger = false;
        FindObjectOfType<CinemachineVirtualCamera>().GetComponent<CinemachineVirtualCamera>().Follow = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("ChickenStart", 1);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerScript.isAlive)
        {
            ChickenMove();
            ChickenShoot();
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {                       
            jumpCount = 1;
        }

        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            gameOver.SetActive(true);
        }

        if (collision.gameObject.name == "snowball(Clone)")
        {            
            Destroy(gameObject);
            gameOver.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "death")
        {
            Destroy(gameObject);
            gameOver.SetActive(true);
        }
    }

}
