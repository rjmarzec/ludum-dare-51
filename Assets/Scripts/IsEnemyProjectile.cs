using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemyProjectile : MonoBehaviour
{
	public float damage;

	private float timer = 0;

	private void Update()
	{
		timer += Time.deltaTime;

		// if this bullet has been in the air for more than 30 seconds, destroy 
		// it to free up memory/processing power
		if (timer > 30)
		{
			Destroy(gameObject);
		}

		// set the rotation of the projectile to point in the direction of motion
		float movementAngle = Vector2.Angle(Vector2.up, GetComponent<Rigidbody2D>().velocity);
		transform.rotation = Quaternion.Euler(0, 0, movementAngle);
		GetComponent<Rigidbody2D>().angularVelocity = 0;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// if this bullet collided with the player, it's game over!
		if (collision.transform.gameObject.layer == LayerMask.NameToLayer("PlayerHealth"))
		{
			FindObjectOfType<EndsGame>().EndGame();
		}
		else if (collision.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			return;
		}

		// destroy this projectile on contact with a planet
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.gameObject.layer == LayerMask.NameToLayer("PlayerHealth"))
		{
			FindObjectOfType<EndsGame>().EndGame();
		}
	}
}
