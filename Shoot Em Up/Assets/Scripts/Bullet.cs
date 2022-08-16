using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public GameObject explosion;

    private int direction = 1;

    public float bulletDamage = 1000f;
    public float bulletSpeed;
    public bool friendly;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        if (friendly == false)
            ChangeDirection();
    }

    void Update()
    {
        rigidBody.velocity = new Vector2(0, bulletSpeed * direction);

        if (gameObject.GetComponent<Renderer>().isVisible == false)
            gameObject.SetActive(false);
    }
    public void ChangeDirection()
    {
        direction *= -1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().Damage(bulletDamage);
            PlayerController.OnBulletHit.Invoke(bulletDamage);
            Instantiate(explosion, transform.position, transform.rotation = Quaternion.Euler(0, 0, 90));
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Miniboss")
        {
            collision.gameObject.GetComponent<AceMiniboss>().Damage(bulletDamage);
            PlayerController.OnBulletHit.Invoke(bulletDamage);
            Instantiate(explosion, transform.position, transform.rotation = Quaternion.Euler(0, 0, 90));
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().HasShield())
                collision.gameObject.GetComponent<PlayerController>().DeactivateShield();
            else
                collision.gameObject.GetComponent<PlayerHealth>().Damage(bulletDamage);

            if (gameObject.layer == 9)
            {
                Instantiate(explosion, transform.position, transform.rotation = Quaternion.identity);
                gameObject.SetActive(false);
            }
            else
            {
                Instantiate(explosion, transform.position, transform.rotation = Quaternion.Euler(0, 0, 270));
                gameObject.SetActive(false);
            }

        }
        else if (collision.gameObject.tag == "PHIDI")
        {
            collision.gameObject.GetComponent<Phidi>().Damage(bulletDamage);
            PlayerController.OnBulletHit.Invoke(bulletDamage);
            Instantiate(explosion, transform.position, transform.rotation = Quaternion.Euler(0, 0, 90));
            gameObject.SetActive(false);
        }
    }

}
