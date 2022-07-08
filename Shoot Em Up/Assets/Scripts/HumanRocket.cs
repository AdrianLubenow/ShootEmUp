using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRocket : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    private int direction = 1;
    private int minimumDamage = 3000;

    private int rocketDamage = 12000;
    private int splashRange = 3;
    

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

    }


    private void Update()
    {
        rigidBody.velocity = new Vector2(0, 6 * direction);

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

                    if (distance < 0.8)
                    {
                        hitCollider.gameObject.GetComponent<Enemy>().Damage(rocketDamage);
                        Destroy(gameObject);
                    }
                    else
                    {
                        var damagePercent = Mathf.InverseLerp(splashRange, 0, distance);

                        if (damagePercent * rocketDamage < minimumDamage)
                            hitCollider.GetComponent<Enemy>()?.Damage(minimumDamage);
                        else
                            hitCollider.GetComponent<Enemy>()?.Damage(rocketDamage * damagePercent);;

                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}