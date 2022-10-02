using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiresOnClick : MonoBehaviour
{
    private JumpsOnClick jumpsOnClick;

    public GameObject bulletPrefab;
    public float fireCooldown = 0.5f;
    public float timer = 0;

    public int shotsToFire = 1;
    public int piercingCounter = 1;
    public float bulletAngleVariance = 15.0f;
    public float bulletDamage = 5.0f;
    public float bulletSizeScalar = 1.0f;
    public float bulletSpeed = 60.0f;

    void Start()
    {
        jumpsOnClick = GetComponent<JumpsOnClick>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // only fire when the player presses their mouse
		if (Input.GetMouseButton(0))
        {
            // make sure the player is attached to a planet when firing
            if(jumpsOnClick.isOnPlanet)
            {
                // make sure the projectile isn't on cooldown
                if(timer <= 0)
                {
                    // reset the cooldowna on firing
                    timer = fireCooldown;
					FireProjectile();
				}
            }
        }
	}

    private void FireProjectile()
    {
		// get the direction we'll be firing in
		Vector3 fireDirection = Utils.GetMouseDirectionVector(transform);

        // spawn a projectile
        GameObject bullet = Instantiate(bulletPrefab, transform.position + fireDirection * 0.3f * bulletSizeScalar, Quaternion.identity);

		// set its damage, size, and speed 
		bullet.GetComponent<IsPlayerProjectile>().damage = bulletDamage;
		bullet.GetComponent<IsPlayerProjectile>().piercingCount = piercingCounter;
		bullet.transform.localScale *= bulletSizeScalar;
		bullet.GetComponent<Rigidbody2D>().velocity = fireDirection * bulletSpeed;

		// for each additional bullet, launch with some variance on the angle
		for (int i = 1; i < shotsToFire; i++)
        {
			Vector3 randomizedDirection = Quaternion.Euler(0, 0, Random.Range(-bulletAngleVariance, bulletAngleVariance)) * fireDirection;

			// spawn a projectile
			GameObject randomizedBullet = Instantiate(bulletPrefab, transform.position + randomizedDirection * 0.3f * bulletSizeScalar, Quaternion.identity);

			// set its damage, size, and speed 
			randomizedBullet.GetComponent<IsPlayerProjectile>().damage = bulletDamage;
			randomizedBullet.GetComponent<IsPlayerProjectile>().piercingCount = piercingCounter;
			randomizedBullet.transform.localScale *= bulletSizeScalar;
			randomizedBullet.GetComponent<Rigidbody2D>().velocity = randomizedDirection * bulletSpeed;
		}
	}
}
