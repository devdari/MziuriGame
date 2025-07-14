using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    [Space(5)]
    public float speed;
    public float jumpForce;
    bool canDoubleJump = true;
    bool isGrounded;
    float move;
    float jumpCount = 2;

    [Header("Player Health")]
    [Space(5)]
    public float health;
    public Image[] hearts;
    public Sprite heartFill;
    public Sprite heartEmpty;
    public Text healthText;
    public static bool isAlive = true;
    public Slider staminaBar;
    public float staminaSpeed;
    public GameObject shield;
    bool shieldActive;


    [Header("Player Components")]
    [Space(5)]
    public Rigidbody2D rb;
    public Animator anim;
    public AudioSource audioSource;

    [Header("Player Shoot")]
    [Space(5)]
    public Text eggBulletText;
    public float eggBullet;
    public float eggBulletForce;
    public GameObject eggBulletPrefab;
    bool chickenMode;    
    bool canShoot = true;

    [Header("Player Bow")]
    [Space(5)]
    public GameObject arrowPrefab;
    public Transform arrowSpot;
    public float arrowForce;
    public Text arrowText;
    float arrow;
    bool bowMode;
    public RuntimeAnimatorController player;
    public RuntimeAnimatorController playerBow;


    [Header("Player Climb")]
    [Space(5)]
    public float ladderSpeed;
    bool isLadder;
    bool isClimbing;

    [Header("Player Dash")]
    [Space(5)]   
    public float dashTime;
    public float dashPower;
    public float dashCoolDown;
    float dashDirection = 1;
    bool canDash = true;
    bool isDashing;

    [Header("Player Sounds")]
    [Space(5)]
    public AudioClip jumpSound;

    [Header("Player Shop")]
    [Space(5)]
    public GameObject shop;
    public Button buyBowButton;
    public Text coinText;
    public GameObject noMoney;
    float coinCount;
    GameObject merchantText;
    bool merchantNeverSeen = true;
    bool merchantRange;
    bool haveBow;


    [Header("Player Checkpoints")]
    [Space(5)]
    public Transform checkpoint1;
    public Transform checkpoint2;




    IEnumerator ResetDoubleJump()
    {
        canDoubleJump = false;
        yield return new WaitForSeconds(2);
        canDoubleJump = true;
    }

    IEnumerator Dash()
    {
        anim.SetBool("jump", false);
        anim.SetTrigger("roll");
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;        
        rb.gravityScale = 0;        
        rb.velocity = new Vector2(dashPower * dashDirection, rb.velocity.y);
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        rb.gravityScale = originalGravity;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    IEnumerator ShootRate()
    {
        canShoot = false;
        yield return new WaitForSeconds(1);
        canShoot = true;
    }

    public void Buy()
    {
        if(coinCount > 0 & EventSystem.current.currentSelectedGameObject.name == "BuyEgg")
        {
            eggBullet++;
            eggBulletText.text = eggBullet.ToString();
            coinCount -= 1;
            coinText.text = coinCount.ToString();
            PlayerPrefs.SetFloat("Coins", coinCount);
            PlayerPrefs.SetFloat("Eggs", eggBullet);
        }
        else if(coinCount >= 50 & EventSystem.current.currentSelectedGameObject.name == "BuyBow")
        {
            coinCount -= 50;
            coinText.text = coinCount.ToString();
            PlayerPrefs.SetFloat("Coins", coinCount);
            haveBow = true;
            PlayerPrefs.SetString("HaveBow", haveBow.ToString());
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
           
        }
        else if (coinCount >= 3 & EventSystem.current.currentSelectedGameObject.name == "BuyArrow")
        {
            coinCount -= 3;
            coinText.text = coinCount.ToString();
            arrow++;
            arrowText.text = arrow.ToString();
            PlayerPrefs.SetFloat("Arrows", arrow);
            PlayerPrefs.SetFloat("Coins", coinCount);


        }
        else
        {
            GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
            noMoney.transform.position = selectedButton.transform.position + new Vector3(1, 0, 0);           
            noMoney.SetActive(true);
            Invoke("NoMoneyDisable", 2);
        }
    }

    void NoMoneyDisable()
    {
        noMoney.SetActive(false);
    }

    void PlayerClimb()
    {
        float moveVertical = Input.GetAxisRaw("Vertical");

        if (moveVertical > 0 & isLadder)
        {
            isClimbing = true;
        }

        if (isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, moveVertical * ladderSpeed);
            rb.gravityScale = 0;

            if(moveVertical != 0)
            {
                anim.SetBool("climb", true);
            }
            else
            {
                anim.PlayInFixedTime("player_climb", 0, 1);
            }

        }
        

    }

    void PlayerStamina()
    {
        if(Input.GetKey(KeyCode.LeftShift) & move != 0)
        {           
            if(staminaBar.value <= 5)
            {
                speed = 1;
            }
            else
            {
                speed = 10;
                staminaBar.value -= staminaSpeed * Time.deltaTime;
            }
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5;
        }
        else
        {
            staminaBar.value += staminaSpeed * Time.deltaTime;
        }
    }

    void PlayerMove()
    {
        //პერსონაჟის ჰორიზონტალური მოძრაობა(მარჯვნივ და მარცხნივ)
        move = Input.GetAxisRaw("Horizontal");        
        if(!isDashing)
        {
            rb.velocity = new Vector2(speed * move, rb.velocity.y);
        }

        //ამოწმებს პერსონაჟის მოძრაობის მიმართულებას
        if (move > 0) //ამოწმებს მოძრაობს თუ არა მარჯვნივ
        {
            dashDirection = 1;
            anim.SetBool("run", true); //პერსონაჟი გადადის სირბილის ანიმაციაზე
            if (chickenMode)
            {
                transform.GetChild(0).GetComponent<Animator>().SetBool("run", true); //ქათამი გადადის სირბილის ანიმაციაზე
            }
            transform.rotation = Quaternion.Euler(0, 0, 0); //ატრიალებს პერსონაჟს მარჯვნივ(y-ზე უწერს 0-ს)
        }
        else if (move < 0) //ამოწმებს მოძრაობს თუ არა მარცხნივ
        {
            dashDirection = -1;
            anim.SetBool("run", true); 
            if (chickenMode)
            {
                transform.GetChild(0).GetComponent<Animator>().SetBool("run", true);
            }
            transform.rotation = Quaternion.Euler(0, 180, 0); //ატრიალებს პერსონაჟს მარცხნივ(y-ზე უწერს 180-ს)
        }
        else //მუშაობს მაშინ როცა ზევით if და else if-ის შემოწმებული მდგომარეობები მცდარია.
        {
            anim.SetBool("run", false); //პერსონაჟი გადადის დგომის ანიმაციაზე(სირბილის ანიმაცია ითიშება)
            if (chickenMode)
            {
                transform.GetChild(0).GetComponent<Animator>().SetBool("run", false); //ქათამი გადადის დგომის ანიმაციაზე
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) & rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }


        //პერსონაჟის ახტომა
        if (Input.GetKeyDown(KeyCode.Space) & jumpCount == 2)
        {
            audioSource.PlayOneShot(jumpSound);
            anim.SetBool("jump", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); //პერსონაჟს ახტუნებს ზევით
            isGrounded = false;
            jumpCount -= 1;
        }
        else if(Input.GetKeyDown(KeyCode.Space) & jumpCount == 1 & canDoubleJump)
        {
            StartCoroutine(ResetDoubleJump());
            audioSource.PlayOneShot(jumpSound);
            anim.SetBool("jump", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); //პერსონაჟს ახტუნებს ზევით
            isGrounded = false;
            jumpCount -= 1;
        }

        //პერსონაჟის კოტრიალი და დეში
        if (Input.GetKeyDown(KeyCode.LeftControl) & canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void PlayerShoot()
    {
        //სროლა(ამოწმებს მაუსის მარცხენა ღილაკზე დაჭერას, კვერცხის რაოდენობას, ქათამი უჭირავს თუ არა და შეუძლია თუ არა სროლა)
        if (Input.GetMouseButtonDown(0) & eggBullet > 0 & chickenMode & canShoot & isAlive & !shop.activeSelf)
        {
            StartCoroutine(ShootRate()); //ნუმერატორის გამოძახება
            StartCoroutine(ChickenRotate());
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("peck"); //სროლის ანიმაციაზე გადასვლა
            eggBullet -= 1; //კვერცხის რაოდენობის დაცლა
            eggBulletText.text = eggBullet.ToString(); //კვერცხის რაოდენობის ტექსტში ვიზუალურად ასახვა
            PlayerPrefs.SetFloat("Eggs", eggBullet);
            GameObject eggBulletClone = Instantiate(eggBulletPrefab, transform.GetChild(0).GetChild(0).position, transform.rotation); //გაჩენის ფუნქცია(აჩენს პრეფაბის კლონს, კონკრეტულ პოზიციაზე და კონკრეტული rotation-ით)
            eggBulletClone.GetComponent<Rigidbody2D>().velocity = transform.right * eggBulletForce; //უკავშირდება გაჩენილი კვერცხის კლონს და ამოძრავებს
            Destroy(eggBulletClone, 2); //შლის და ანადგურებს კვერცხის კლონებს გაჩენიდან 2 წამში
        }
    }

    void PlayerBowShoot()
    {
        if (Input.GetMouseButtonDown(0) & bowMode & canShoot & arrow > 0 & !shop.activeSelf)
        {
            anim.SetBool("jump", false);
            StartCoroutine(ShootRate());
            anim.SetTrigger("attack");
            
        }
    }

    void PlayerInput()
    {
        //ამოწმებს ვაჭერთ თუ არა 1-იანს და გვიჭირავს თუ არა ქათამი
        if(Input.GetKeyDown(KeyCode.Alpha1) & !chickenMode)
        {
            transform.GetChild(0).gameObject.SetActive(true); //გამოაჩენს ქათამს
            chickenMode = true;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1) & chickenMode)
        {
            transform.GetChild(0).gameObject.SetActive(false); //დამალავს ქათამს
            chickenMode = false;
        }

        if(Input.GetKeyDown(KeyCode.E) & merchantRange)
        {
            shop.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) & !bowMode & haveBow)
        {
            anim.runtimeAnimatorController = playerBow;
            bowMode = true;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) & bowMode & haveBow)
        {
            anim.runtimeAnimatorController = player;
            bowMode = false;
        }
    }


    void HeartsUpdate()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = heartFill;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }
        }
    }

    public void PlayerDamage(float damageAmount)
    {
        if(health > 0 & !shieldActive & isAlive)
        {
            health -= damageAmount; //პერსონაჟს აკლებს სიცოცხლეს
            healthText.text = health.ToString(); //პერსონაჟის სიცოცხლე აისახება ტექსტში
            HeartsUpdate();
            if (health <= 0 & isAlive) //ამოწმებს სიცოცხლის რაოდენობას
            {
                
                isAlive = false; //პერსონაჟი ცოცხალი აღარ არის
                anim.SetBool("jump", false);
                anim.SetBool("run", false);
                anim.SetTrigger("die"); //სიკვდილი
                ChickenPlay(); //ქათამი იწყებს მოღვაწეობას
                //this.enabled = false;
            }
        }
        
    }

    void ChickenPlay()
    {
        transform.GetChild(0).gameObject.SetActive(true); //ქათამი ჩნდება
        transform.GetChild(0).gameObject.AddComponent<Rigidbody2D>(); //ქათამს ენიჭება ფიზიკის კომპონენტი
        transform.GetChild(0).GetComponent<Rigidbody2D>().velocity = transform.up * 15; //ვარდება ზევით
        transform.GetChild(0).GetComponent<Rigidbody2D>().gravityScale = 3; //გრავიტაცია უხდება 3-ის ტოლი
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true; //კოლაიდერი(შეჯახება) აქტიურდება
        transform.GetChild(0).GetComponent<BoxCollider2D>().isTrigger = true; //ერთვება ტრიგერი და ობიექტებს აღარ ეჯახება
        transform.GetChild(0).GetComponent<ChickenScript>().enabled = true; //აქტიურდება სკრიპტი
        transform.GetChild(0).parent = null; //მშობელი აღარ ჰყავს(ქათამი დამოუკიდებელი ხდება)
    }

    IEnumerator ChickenRotate()
    {
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 180, 0); //ტრიალდება 180 გრადუსით
        yield return new WaitForSeconds(2); //2 წამით დაყოვნება
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0); //rotationი უბრუნდება საწყის მდგომარეობას
    }


    public void ArrowSpawn()
    {
        arrow -= 1;
        arrowText.text = arrow.ToString();
        PlayerPrefs.SetFloat("Arrows", arrow);
        GameObject arrowClone = Instantiate(arrowPrefab, arrowSpot.position, transform.rotation);
        arrowClone.GetComponent<Rigidbody2D>().velocity = transform.right * arrowForce;
        Destroy(arrowClone, 3);
    }


    // Start is called before the first frame update
    void Start()
    {
        HeartsUpdate();
        arrow = 100;
        arrowText.text = arrow.ToString();
        if(PlayerPrefs.GetString("HaveBow") == "True")
        {
            haveBow = true;
            buyBowButton.interactable = false;           
        }

        if(PlayerPrefs.GetFloat("Arrows") > 0)
        {
            arrow = PlayerPrefs.GetFloat("Arrows");
            arrowText.text = arrow.ToString();
        }

        if (PlayerPrefs.GetFloat("Eggs") > 0)
        {
            eggBullet = PlayerPrefs.GetFloat("Eggs");
            eggBulletText.text = eggBullet.ToString();
        }

        if (PlayerPrefs.GetFloat("Coins") > 0)
        {
            coinCount = PlayerPrefs.GetFloat("Coins");
            coinText.text = coinCount.ToString();
        }

        MapCheckpointsScript checkpointsScript = FindObjectOfType<MapCheckpointsScript>();


        if(PlayerPrefs.GetFloat("checkpoint") == 1)
        {
            transform.position = checkpoint1.position;
        }
        else if (PlayerPrefs.GetFloat("checkpoint") == 2)
        {
            transform.position = checkpoint2.position;
        }

        if(checkpointsScript != null)
        {
            if (checkpointsScript.checkpoint == 1 & ChangeSceneScript.sceneName == "Menu")
            {
                transform.position = checkpoint1.position;
            }
            else if (checkpointsScript.checkpoint == 2 & ChangeSceneScript.sceneName == "Menu")
            {
                transform.position = checkpoint2.position;
            }
        }

      

        isAlive = true;
        healthText.text = health.ToString();   //პერსონაჟის სიცოცხლე აისახება ტექსტში     
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive) //ამოწმებს არის თუ არა ცოცხალი
        {
            PlayerMove();
            PlayerShoot();
            PlayerBowShoot();
            PlayerInput();
            PlayerStamina();
            PlayerClimb();
        }
        else if(!isAlive & jumpCount == 2)
        {          
            rb.bodyType = RigidbodyType2D.Static;
            GetComponent<BoxCollider2D>().enabled = false;
        }
       
        
    }

    IEnumerator MerchantDialogue()
    {
        yield return new WaitForSeconds(2);
        merchantText.GetComponent<TMP_Text>().text = "Welcome..";
        yield return new WaitForSeconds(2);
        merchantText.GetComponent<TMP_Text>().text = "You are Welcome to Shop here..";
        yield return new WaitForSeconds(2);
        merchantText.GetComponent<TMP_Text>().text = "Just Press E..";
        merchantRange = true;
    }

    IEnumerator ShieldShow() //ფარის გაჩენა-გაქრობა
    {
        shieldActive = true;
        shield.SetActive(true);
        yield return new WaitForSeconds(5);
        shieldActive = false;
        shield.SetActive(false);
    }


    //ხვდება ობიექტებთან დაჯახებას
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3) //ამოწმებს დაჯახებულ ობიექტს აქვს თუ არა Ground-ის ლეიერი
        {
            anim.SetBool("jump", false);
            isGrounded = true;
            jumpCount = 2;
        }

        if(collision.gameObject.tag == "Egg") //ამოწმებს დაჯახებულ ობიექტის თეგი არის თუ არა Egg
        {
            eggBullet++; //ზრდის კვერცხის რაოდენობას 1-ით
            eggBulletText.text = eggBullet.ToString(); //ასახავს კვერცხის რაოდენობას ტექსტში
            Destroy(collision.gameObject); //შლის დაჯახებულ ობიექტს(კვერცხს)
            PlayerPrefs.SetFloat("Eggs", eggBullet);
        }

        if(collision.gameObject.name == "bee")
        {
            PlayerDamage(1); //პერსონაჯს აკლებს სიცოცხლეს 1-ით.
        }

        if (collision.gameObject.tag == "coin")
        {
            float randomCoin = Random.Range(1, 21); //შემთხვევით არჩევს რიცხვს
            coinCount += randomCoin;  //იზრდება მონეტების რაოდენობა
            coinText.text = coinCount.ToString(); //მონეტების რაოდენობა აისახება ტექსტში
            Destroy(collision.gameObject); //აქრობს მონეტას
            PlayerPrefs.SetFloat("Coins", coinCount);
        }

        if (collision.gameObject.tag == "elevator")
        {
            transform.parent = collision.transform;
        }

        if (collision.gameObject.name == "snowball(Clone)")
        {
            Destroy(collision.gameObject);
            PlayerDamage(1);
        }

        if (collision.gameObject.tag == "shieldpotion")
        {
            Destroy(collision.gameObject);
            StartCoroutine(ShieldShow());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "elevator")
        {
            transform.parent = null;
        }
    }


    //ხვდება ობიექტების კოლაიდერებში შესვლას(გავლას)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ladder") 
        {
            isLadder = true; //ეხება კიბეს
        }

        if (collision.gameObject.tag == "merchant")
        {
            if(merchantNeverSeen)
            {
                merchantNeverSeen = false;
                collision.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                merchantText = collision.transform.GetChild(0).GetChild(0).gameObject;
                StartCoroutine(MerchantDialogue());
            }
            else
            {
                merchantRange = true;
            }
            
        }

        if (collision.gameObject.name == "checkpoint1")
        {
            collision.GetComponent<Animator>().Play("checkpoint");
            PlayerPrefs.SetFloat("checkpoint", 1);
        }

        if (collision.gameObject.name == "checkpoint2")
        {
            collision.GetComponent<Animator>().Play("checkpoint");
            PlayerPrefs.SetFloat("checkpoint", 2);
        }

        if (collision.gameObject.name == "death")
        {           
            PlayerDamage(health);
        }
    }

    //ხვდება ობიექტების კოლაიდერებიდან გამოსვლას
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ladder")
        {
            isLadder = false; //არ ეხება კიბეს
            isClimbing = false; //არ დაცოცავს(ცოცვის დასრულება)
            rb.gravityScale = 3;
            anim.SetBool("climb", false);
        }

        if (collision.gameObject.tag == "merchant")
        {
            collision.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            shop.SetActive(false);
            merchantRange = false;
        }
    }

}
