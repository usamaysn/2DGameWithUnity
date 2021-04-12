using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullate : MonoBehaviour
{
    [SerializeField]
    float speed;

   
    public int damage;

    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", .40f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Ground"))
        {

            DestroySelf();

        }
    }

    private void DestroySelf()
    {

        Destroy(gameObject);

    }


    public void EnemyStartShoot(bool isFacingLeft)
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();

        if (isFacingLeft)
        {

            rb2d.velocity = new Vector2(-speed, 0);

        }

        else
        {

            rb2d.velocity = new Vector2(speed, 0);

        }
    }
}
