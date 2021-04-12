using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escript : MonoBehaviour
{

    [SerializeField]
    Transform castPos;

    [SerializeField]
    float baseCastDist;
    const string LEFT = "left";
    const string RIGHT = "Right";

    string facingDirection;

    Vector4 baseScale;

    private int health = 5;
    private Material matWhite;
    private Material matDefault;
    private UnityEngine.Object explosionRef;
    private UnityEngine.Object enemyRef;
    

    SpriteRenderer sr;

    [SerializeField]
    float delayBeforeDestroy = 10;

    Animator animator;

    [SerializeField]
    Transform player;

    [SerializeField]
    float agroRange;

    [SerializeField]
    float moveSpeed;

    Rigidbody2D rb2d;

    Object enemyBulletRef;
    private bool isEnemyShooting;

    [SerializeField]
    private float enemyShootDelay = .5f;

    [SerializeField]
    Transform EnemyBulletSpawnPos;

    bool isEnemyFacingLeft;

    Move2D move2D;

 
    void Start()
    {
        facingDirection = RIGHT;
        baseScale = transform.localScale;

        animator = GetComponent<Animator>();
        enemyRef = Resources.Load("Enemy");

        sr = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = sr.material;
        explosionRef = Resources.Load("Explosion");

        rb2d = GetComponent<Rigidbody2D>();

        enemyBulletRef = Resources.Load("EnemyBullet");
    }


    void Update()
    {
    



        if (player.GetComponent<Move2D>().isRespawning || player.GetComponent<Move2D>().isDie)
        {
            animator.Play("EnemyIdle1");
            return;
        }

        if (IsHittingWall() || IsNearEdge())
        {


            if (facingDirection == LEFT)
            {
                ChangeFacingDirection(RIGHT);

            }
            else if (facingDirection == RIGHT)
            {
                ChangeFacingDirection(LEFT);

            }
           
        
        }


        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer < agroRange)
        {

            ChasePlayer();
        }
        else
        {
            StopChasingPlayer();
        }
    }


    void ChangeFacingDirection(string newDirection)
    { 
        Vector4 newScale = baseScale;
        if (newDirection == LEFT)
        {
            newScale.x = baseScale.x;

        }
        else if (newDirection == RIGHT)
        {

            newScale.x = baseScale.x;
            
        }

        transform.localScale = newScale;

        facingDirection = newDirection;
    
    
    }

    bool IsHittingWall()
    {
        bool val = false;

        float castDist = baseCastDist;

        if (facingDirection == LEFT)
        {
            castDist = baseCastDist;
        }
        else
        {
            castDist = baseCastDist;
        
        }

        Vector4 targetPos = castPos.position;
        targetPos.x += castDist;


        Debug.DrawLine(castPos.position, targetPos, Color.blue);


        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Terrain")))
        {

            val = true;

        }
        else
        {

            val = false;
        
        }

        return val;
    }

    


    void ChasePlayer()
    {

        if (transform.position.x < player.position.x)
        {
            
            rb2d.velocity = new Vector2(moveSpeed, 0);

            

            transform.localScale = new Vector2(7, 7);

            isEnemyFacingLeft = false;
        }
        else
        {

            rb2d.velocity = new Vector2(-moveSpeed, 0);

            

            transform.localScale = new Vector2(-7, 7);

            isEnemyFacingLeft = true;
        }

        animator.Play("EnemyRun");

        if (isEnemyShooting)
            return;

        isEnemyShooting = true;
        GameObject b = (GameObject)Instantiate(enemyBulletRef);
        b.GetComponent<EnemyBullate>().EnemyStartShoot(isEnemyFacingLeft);
        b.transform.position = EnemyBulletSpawnPos.transform.position;

        Invoke("EnemyResetShoot", enemyShootDelay);


   

    }
    void StopChasingPlayer()
    {
        rb2d.velocity = new Vector2(0, 0);

        animator.Play("EnemyIdle1");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {

            Destroy(collision.gameObject);
            health--;
            sr.material = matWhite;
            if (health <= 0)
            {
                player.GetComponent<Move2D>().totalEnemy--;
                killSelf();
            }
            else
            {

                Invoke("ResetMaterial", .5f);
            }

        }
    }

    void ResetMaterial()
    {

        sr.material = matDefault;
    
    }

    private void killSelf()
    {
        GameObject explosion = (GameObject)Instantiate(explosionRef);
        explosion.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, -1f);
        
        gameObject.SetActive(false);

       
    }

    void EnemyResetShoot()
    {
        isEnemyShooting = false;
    }



    bool IsNearEdge()
    {
        bool val = true;

        float castDist = baseCastDist;



        Vector4 targetPos = castPos.position;
        targetPos.y -= castDist;


        Debug.DrawLine(castPos.position, targetPos);


        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Terrain")))
        {

            val = false;

        }
        else
        {

            val = true;

        }

        return val;
    }


}
