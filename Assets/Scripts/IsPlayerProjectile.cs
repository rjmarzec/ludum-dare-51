using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerProjectile : MonoBehaviour
{
    public float damage;
    public int piercingCount;

    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;

        // if this bullet has been in the air for more than 30 seconds, destroy 
        // it to free up memory/processing power
        if(timer > 60)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if this bullet collided with an enemy, deal damage to them
        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.transform.gameObject.GetComponent<IsEnemy>().TakeDamage(damage);
        }

        // allow the bullet to pierce multiple things if it still has pierces remaining
        if (piercingCount > 1)
        {
            piercingCount--;
        }
        else
        {
			Destroy(gameObject);
		}

		
    }
}
