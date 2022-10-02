using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemy : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
	private float timer = 0;

	public float health;
    public float movementSpeed;
    public GameObject enemyProjectilePrefab;

    private bool doesFire = false;
    private float damageTakenScalar = 1;
    private float movementSpeedScalar = 1;
    private float magnetSpeedScalar = 1;

    public Sprite meleeSprite;
    public Sprite rangedSprite;

    public AudioClip fireSound;
    public AudioClip deathSound;


	void Start()
    {
        player = FindObjectOfType<IsPlayer>().gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // get the vector between the player and this enemy
        Vector2 enemyToPlayer = player.transform.position - transform.position;
        Vector2 movementDirection = enemyToPlayer.normalized;

        // move towards the player with some fixed velocity plus variable speed
        // that increases the farther away the player is
        rb.velocity = movementDirection * movementSpeed * movementSpeedScalar + enemyToPlayer * 0.1f * magnetSpeedScalar;

		// incerement our timer
		timer += Time.deltaTime;

		// fire a projectile every 2 seconds if this enemy is ranged
        if(doesFire)
        {
			if (timer % 2 < (timer - Time.deltaTime) % 2)
			{
				FireProjectile();
			}
		}

		// make the enemy always point in their direction of motion and never spinning due to physics collisions
		float movementAngle = Vector2.Angle(Vector2.right, movementDirection);
		transform.rotation = Quaternion.Euler(0, 0, movementAngle);
		GetComponent<Rigidbody2D>().angularVelocity = 0;
	}

    private void FireProjectile()
    {
		// get the vector between the player and this enemy
		Vector2 enemyToPlayer = (player.transform.position - transform.position).normalized;

		// spawn the projectile
		float movementAngle = Vector2.Angle(Vector2.right, enemyToPlayer);
		GameObject enemyProjectile = Instantiate(enemyProjectilePrefab, transform.position, Quaternion.Euler(0, 0, movementAngle));

		// give the projectile velocity in some direction
		enemyProjectile.GetComponent<Rigidbody2D>().velocity = enemyToPlayer * 10.0f;

        // play a sound
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}

    public void TakeDamage(float damage)
    {
        health -= damage * damageTakenScalar;

        if(health <= 0)
        {
            Die();
        }
    }

    public void SetType(bool isMeleeIn)
    {
        if(isMeleeIn)
        {
            doesFire = false;
            damageTakenScalar = 1;
            movementSpeedScalar *= 1.2f;
            magnetSpeedScalar *= 0.8f;
            GetComponent<SpriteRenderer>().sprite = meleeSprite;
        }
        else
        {
            doesFire = true;
            damageTakenScalar = 1.5f;
			movementSpeedScalar *= 0.8f;
			magnetSpeedScalar *= 1.2f;
			GetComponent<SpriteRenderer>().sprite = rangedSprite;
		}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		// if this enemy collids with the player, it's game over!
		if (collision.transform.gameObject.layer == LayerMask.NameToLayer("PlayerHealth"))
		{
			FindObjectOfType<EndsGame>().EndGame();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.gameObject.layer == LayerMask.NameToLayer("PlayerHealth"))
		{
			FindObjectOfType<EndsGame>().EndGame();
		}
	}

    private void Die()
    {
		// give the player xp
		player.GetComponent<IsPlayer>().AddXP(1);

        // play a sound
        AudioSource.PlayClipAtPoint(deathSound, transform.position);

        // destroy this gameobject
        Destroy(gameObject);
    }
}
