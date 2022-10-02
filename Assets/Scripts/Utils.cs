using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector2 GetMouseDirectionVector(Transform character)
    {
		// get the camera we want to reference for jumps
		Camera targetCamera = null;
		foreach (Camera currentCamera in Camera.allCameras)
		{
			if (currentCamera.orthographic)
			{
				targetCamera = currentCamera;
				break;
			}
		}

		// get the direction from the player to the mouse. we'll use this as the launch direction
		Vector2 cameraPosition = targetCamera.ScreenToWorldPoint(Input.mousePosition);
		Vector2 characterPosition = character.position;
		return (cameraPosition - characterPosition).normalized;
	}
}
