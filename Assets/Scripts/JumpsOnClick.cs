using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpsOnClick : MonoBehaviour
{
    public float movementSpeed = 40.0f;
	public SpriteRenderer sr;

    private Rigidbody2D rb;
    private RotatesAround ra;

    private Transform currentTarget = null;

    private Transform currentParent;

	public bool isOnPlanet = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		ra = GetComponent<RotatesAround>();

        currentParent = GameObject.Find("Sun").transform;
    }

    void Update()
    {
        // if the player is flying towards a target, softly pull them into it
        if(currentTarget != null)
        {
            Vector2 intendedDirection = currentTarget.position - transform.position;
            rb.velocity += intendedDirection * movementSpeed * Time.deltaTime * 0.2f;

            // normalize the speed back to the normal movement speed so players can't move faster
            // by utilizing the pull
            rb.velocity = rb.velocity.normalized * movementSpeed;
        }

		// when the player presses the right mouse button down, launch their character
		if (Input.GetMouseButtonDown(1))
		{
			// only let the player jump if they are attached to a planet
			if(currentParent != null)
            {
                TryLaunchCharacter();
			}
		}

		// set the rotation of the player to point in the direction of motion
		if(rb.velocity.magnitude > 5.0f)
		{
			float movementAngle = Vector2.Angle(Vector2.up, rb.velocity);
			sr.transform.rotation = Quaternion.Euler(0, 0, movementAngle);
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Planet"))
        {
			// stop all movement
			rb.velocity = Vector2.zero;

			// make the player rotate around the collided planet
			transform.parent = collision.transform;

			// make the player copy the RotatesAround that the planet uses so that rotation stays consistent
			ra.SetNewTarget(collision.transform.GetComponent<RotatesAround>());

            // set this planet as the current parent, so we can ignore them on the next jump
            currentParent = collision.transform;

            // reset the target so that the player doesn't get pulled towards the target more
            currentTarget = null;

			isOnPlanet = true;
		}
    }

	private void TryLaunchCharacter()
	{
		// get the direction we'll be launching in
		Vector2 launchDirection = Utils.GetMouseDirectionVector(transform);

		// check to see if the player would hit a planet going this direction
		RaycastHit2D[] raycastHits = Physics2D.CircleCastAll(transform.position, 1.0f / 2, launchDirection, Mathf.Infinity, LayerMask.GetMask("Planet"));

		foreach (RaycastHit2D raycastHit in raycastHits)
		{
			// ignore the current raycastHit if it's null or equal to our current parent
			if (raycastHit.collider != null && raycastHit.transform != currentParent)
			{
				// reset the object we're rotating around, since want to stop rotating around our current target
				transform.parent = null;
				currentTarget = raycastHit.collider.transform;
				ra.Reset();

				// launch the player in the direction of the new planet
				rb.velocity = launchDirection * movementSpeed;

				isOnPlanet = false;
			}
		}

		Debug.DrawRay(transform.position, launchDirection * 20, Color.magenta, 3, false);
	}
}
