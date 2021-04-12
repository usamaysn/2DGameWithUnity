using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Move2D : MonoBehaviour
{


    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    CourutineHandler courutineHandler;
    
    

    Object bulletRef;

    bool isGrounded;

    [SerializeField]
    Transform bulletSpawnPos;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    Transform groundCheckL;

    [SerializeField]
    Transform groundCheckR;

    [SerializeField]
    private float runSpeed = 3f;

    [SerializeField]
    private float jumpSpeed = 5f;


    private bool isShooting;

    [SerializeField]
    private float shootDelay = .5f;

    bool isFacingLeft;

    private Material matWhite;
    private Material matDefault;
    private Object playerExplosionRef;
    public Vector3 initialPos;
    public bool isRespawning;
    public bool isDie;
    private bool isLevelCompleted;

    [SerializeField]
    private int playerLives = 3;

    public Image healthBarImg;

    [SerializeField]
    private GameObject livesImgParent;

    [Header("GaveOver")]
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject gameOverPanelfromGAP;
    [SerializeField]
    private GameObject levelCompletedPanel;

    public AudioClip audJump;
    public AudioClip audShoot;
    public AudioClip audDie;
    public AudioClip audCoin;

    public int totalEnemy, Health;
    public bool isObstacleColliderForLevelCompleted = false;



    public List<string> Inventory;
    

    void Start()
    {
        Health = 100;
        Debug.Log(Health);
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        courutineHandler = GameObject.FindObjectOfType<CourutineHandler>();

        bulletRef = Resources.Load("Bullet");
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;
        playerExplosionRef = Resources.Load("Explosion");
        initialPos = this.transform.position;

        totalEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Total Enemy = "+totalEnemy);

        Inventory = new List<string>();

    }

    
    void FixedUpdate()
    {
        if (isRespawning || isDie || isLevelCompleted)
            return;

        if (totalEnemy==0)
        {

            courutineHandler.LevelCompletedORGameOvr(levelCompletedPanel);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        if((Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"))) ||
                (Physics2D.Linecast(transform.position, groundCheckL.position, 1 << LayerMask.NameToLayer("Ground"))) ||
                (Physics2D.Linecast(transform.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Ground"))))

        {
            isGrounded = true;

        }
        else
        {
            isGrounded = false;
        }

        if(Input.GetKey("d") || Input.GetKey("right"))
        {

  
            rb2d.velocity = new Vector2(runSpeed, rb2d.velocity.y);

            if (isGrounded)
            
                animator.Play("Player_run");
            
            spriteRenderer.flipX = false;

            isFacingLeft = false;

        }
        else if(Input.GetKey("a") || Input.GetKey("left"))
        {


            rb2d.velocity = new Vector2(-runSpeed, rb2d.velocity.y);

            if (isGrounded)
            {

                animator.Play("Player_run");
            }
            isFacingLeft = true;
            spriteRenderer.flipX = true;
        }

        else 
        {
            if (isGrounded)
            {
                animator.Play("Player_idle");


            }

            rb2d.velocity = new Vector2(0, rb2d.velocity.y);


        }

        if (Input.GetKey("space") && isGrounded)
        {
            
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(audJump, 0.2F);
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            animator.Play("Player_jump");

        }

        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.LeftControl))
        {


            if (isShooting) return;

            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(audShoot, 0.1F);

            animator.Play("Player_shoot");
            isShooting = true;
            GameObject b = (GameObject)Instantiate(bulletRef);
            b.GetComponent<BulletScript>().StartShoot(isFacingLeft);
            b.transform.position = bulletSpawnPos.transform.position;

            Invoke("ResetShoot", shootDelay);



        }




    }

    void ResetShoot()
    {
        isShooting = false;
        animator.Play("Player_idle");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(audDie, 0.1F);
            healthBarImg.fillAmount -= (float)(collision.GetComponent<EnemyBullate>().damage / 20f);
            Health -= (collision.gameObject.GetComponent<EnemyBullate>().damage * 20);
            Destroy(collision.gameObject);
            CkeckDieOrRespawn(.5f);
        }
        else if(collision.CompareTag("GapCollider"))
        {

            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(audDie, 0.1F);

            isObstacleColliderForLevelCompleted = collision.gameObject.GetComponent<ObstacleScript>().isToCollideLevelCompleted;

            Debug.Log(isObstacleColliderForLevelCompleted);

            CkeckDieOrRespawn(.25f);


        }

        if (collision.CompareTag("Collectable"))
        {

            
            string itemType = collision.gameObject.GetComponent<CollectableScript>().itemType;
            print("we have collected a:" + itemType);
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(audCoin, 0.1F);
            Inventory.Add(itemType);
            print("Inventory Lenth:" + Inventory.Count);
            ScoreManager.coinAmount += 1;
            Destroy(collision.gameObject);

        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Obstacle"))
        {

            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(audDie, 0.1F);

            isObstacleColliderForLevelCompleted = collision.gameObject.GetComponent<ObstacleScript>().isToCollideLevelCompleted;

            Debug.Log(isObstacleColliderForLevelCompleted);

            healthBarImg.fillAmount -= (float)(collision.gameObject.GetComponent<ObstacleScript>().damage / 10f);
            Health -= (collision.gameObject.GetComponent<ObstacleScript>().damage * 10);
            CkeckDieOrRespawn(.25f);
        }


    }


    void CkeckDieOrRespawn(float resetMaterialTime)
    {
        spriteRenderer.material = matWhite;
        if (healthBarImg.fillAmount <= 0)
        {
            playerLives--;
            livesImgParent.transform.GetChild(playerLives).gameObject.SetActive(false);
            if (playerLives <= 0)
            {

                isDie = true;
                killSelf();
                courutineHandler.LevelCompletedORGameOvr(gameOverPanel);
                return;
            }
            else
            {
                isRespawning = true;
                killSelf();
            }
        }
        Invoke("ResetMaterial", resetMaterialTime);
    }

    void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }
    private void killSelf()
    {

 
        GameObject explosion = (GameObject)Instantiate(playerExplosionRef);

        explosion.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, -1f);


        gameObject.SetActive(false);

        if (isRespawning)
        {
            Invoke("EnemyRespawn", 1f);
        }
    }

    void EnemyRespawn()
    {
        this.transform.position = initialPos;

        isRespawning = false;
        healthBarImg.fillAmount = 1;
        Health = 100;
        gameObject.SetActive(true);

        
    }

    public void RestartLevel()
    {
        ScoreManager.coinAmount = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void PlayLevel1()
    {
        ScoreManager.coinAmount = 0;
        SceneManager.LoadScene("Level1");

    }
    public void PlayLevel2()
    {
        ScoreManager.coinAmount = 0;
        SceneManager.LoadScene("Level2");

    }
    public void PlayLevel3()
    {
        ScoreManager.coinAmount = 0;
        SceneManager.LoadScene("Level3");

    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");

    }




}
