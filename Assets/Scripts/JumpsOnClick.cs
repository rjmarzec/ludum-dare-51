using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpsOnClick : MonoBehaviour
{
    public float movementSpeed = 20;

    private Rigidbody2D rb;
    private RotatesAround ra;

    private Transform currentTarget = null;

    private Transform currentParent;

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
            // get the camera we want to reference for jumps
            Camera targetCamera = null;
            foreach(Camera currentCamera in Camera.allCameras)
            {
                if(currentCamera.orthographic)
                {
                    targetCamera = currentCamera;
                    break;
                }
            }

			// get the direction from the player to the mouse. we'll use this as the launch direction
			Vector2 cameraPosition = targetCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 characterPosition = transform.position;
            Vector2 launchDirection = (cameraPosition - characterPosition).normalized;

            // check to see if the player would hit a planet going this direction
            RaycastHit2D[] raycastHits = Physics2D.CircleCastAll(characterPosition, 1.0f/2, launchDirection, Mathf.Infinity, LayerMask.GetMask("Planet"));

            foreach(RaycastHit2D raycastHit in raycastHits)
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
                }
            }

		    Debug.DrawRay(characterPosition, launchDirection * 20, Color.magenta, 3, false);
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
		}
    }
}
