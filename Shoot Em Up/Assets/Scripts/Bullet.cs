using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    private int direction = 1;
    private int bulletDamage = 400;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        rigidBody.velocity = new Vector2(0, 12 * direction);

        if (gameObject.GetComponent<Renderer>().isVisible == false)
            gameObject.SetActive(false);
    }
    public void ChangedDirection()
    {
        direction *= -1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (direction == 1)
        //{
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<Enemy>().Damage(bulletDamage);
                PlayerController.OnSpecialBulletHit.Invoke(bulletDamage);
                gameObject.SetActive(false);
            }
        //}
        //else
        //{
        //    if (collision.gameObject.tag == "Player")
        //    {
        //        collision.gameObject.GetComponent<PlayerController>().Damage(1000);
        //        gameObject.SetActive(false);
        //    }
        //}
    }

}
