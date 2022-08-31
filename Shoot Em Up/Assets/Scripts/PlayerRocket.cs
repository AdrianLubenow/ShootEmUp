using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public GameObject explosion;

    private int direction = 1;
    private readonly int minimumDamage = 3000;
    private readonly int splashRange = 3;

    public float rocketSpeed;
    public int rocketDamage;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rigidBody.velocity = new Vector2(0, rocketSpeed * direction);

        if (gameObject.GetComponent<Renderer>().isVisible == false)
            Destroy(gameObject);
    }

    public void ChangedDirection()
    {
        direction *= -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (splashRange > 0)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "Enemy")
                {
                    var closestPoint = hitCollider.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    if (distance < 50)
                    {
                        hitCollider.gameObject.GetComponent<Enemy>().Damage(rocketDamage);
                        Instantiate(explosion, transform.position, transform.rotation = Quaternion.identity);
                        FindObjectOfType<AudioManager>().Play("RocketExplosion");
                        Destroy(gameObject);
                    }
                    else
                    {
                        var damagePercent = Mathf.InverseLerp(splashRange, 0, distance);

                        if (damagePercent * rocketDamage < minimumDamage)
                        {
                            hitCollider.GetComponent<Enemy>()?.Damage(minimumDamage);
                            hitCollider.GetComponent<AceMiniboss>()?.Damage(minimumDamage);
                            hitCollider.GetComponent<Phidi>()?.Damage(minimumDamage);
                        }
                        else
                        {
                            hitCollider.GetComponent<Enemy>()?.Damage(rocketDamage * damagePercent);
                            hitCollider.GetComponent<AceMiniboss>()?.Damage(rocketDamage * damagePercent);
                            hitCollider.GetComponent<Phidi>()?.Damage(rocketDamage * damagePercent);
                        }

                        Instantiate(explosion, transform.position, transform.rotation = Quaternion.identity);
                        FindObjectOfType<AudioManager>().Play("RocketExplosion");
                        Destroy(gameObject);
                    }
                }
                else if (hitCollider.gameObject.tag == "Miniboss")
                {
                    var closestPoint = hitCollider.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    if (distance < 50)
                    {
                        hitCollider.gameObject.GetComponent<AceMiniboss>().Damage(rocketDamage);
                        Instantiate(explosion, transform.position, transform.rotation = Quaternion.identity);
                        FindObjectOfType<AudioManager>().Play("RocketExplosion");
                        Destroy(gameObject);
                    }
                    else
                    {
                        var damagePercent = Mathf.InverseLerp(splashRange, 0, distance);

                        if (damagePercent * rocketDamage < minimumDamage)
                        {
                            hitCollider.GetComponent<Enemy>()?.Damage(minimumDamage);
                            hitCollider.GetComponent<AceMiniboss>()?.Damage(minimumDamage);
                            hitCollider.GetComponent<Phidi>()?.Damage(minimumDamage);
                        }
                        else
                        {
                            hitCollider.GetComponent<Enemy>()?.Damage(rocketDamage * damagePercent);
                            hitCollider.GetComponent<AceMiniboss>()?.Damage(rocketDamage * damagePercent);
                            hitCollider.GetComponent<Phidi>()?.Damage(rocketDamage * damagePercent);
                        }

                        Instantiate(explosion, transform.position, transform.rotation = Quaternion.identity);
                        FindObjectOfType<AudioManager>().Play("RocketExplosion");
                        Destroy(gameObject);
                    }
                }
                else if (hitCollider.gameObject.tag == "PHIDI")
                {
                    var closestPoint = hitCollider.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    if (distance < 50)
                    {
                        hitCollider.gameObject.GetComponent<Phidi>().Damage(rocketDamage);
                        Instantiate(explosion, transform.position, transform.rotation = Quaternion.identity);
                        FindObjectOfType<AudioManager>().Play("RocketExplosion");
                        Destroy(gameObject);
                    }
                    else
                    {
                        var damagePercent = Mathf.InverseLerp(splashRange, 0, distance);

                        if (damagePercent * rocketDamage < minimumDamage)
                        {
                            hitCollider.GetComponent<Enemy>()?.Damage(minimumDamage);
                            hitCollider.GetComponent<AceMiniboss>()?.Damage(minimumDamage);
                            hitCollider.GetComponent<Phidi>()?.Damage(minimumDamage);
                        }
                        else
                        {
                            hitCollider.GetComponent<Enemy>()?.Damage(rocketDamage * damagePercent);
                            hitCollider.GetComponent<AceMiniboss>()?.Damage(rocketDamage * damagePercent);
                            hitCollider.GetComponent<Phidi>()?.Damage(rocketDamage * damagePercent);
                        }

                        Instantiate(explosion, transform.position, transform.rotation = Quaternion.identity);
                        FindObjectOfType<AudioManager>().Play("RocketExplosion");
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}