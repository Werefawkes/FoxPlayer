using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foxthorne.FoxScreens;
using UnityEngine.InputSystem;

namespace Foxthorne.FoxPlayer
{
	public class FTPlayer : MonoBehaviour
	{
		[Header("Movement")]
		public float moveSpeed = 1;
		public Vector2 moveInput;

		public float lookSpeed = 1;
		public float minLookAngle = -80;
		public float maxLookAngle = 80;
		Vector2 lookInput;
		public Camera playerCam;

		[Header("References")]
		public Rigidbody rb;

		public virtual void Update()
		{
			if (!UIManager.IsUIClear)
			{
				moveInput = Vector2.zero;
				lookInput = Vector2.zero;
			}

			DoMovement();
			DoLook();
		}

		#region Movement
		void DoMovement()
		{
			Vector3 camForward = playerCam.transform.forward;
			camForward.y = 0;
			camForward.Normalize();
			Vector3 camRight = playerCam.transform.right;
			camRight.y = 0;
			camRight.Normalize();

			Vector3 velocity = moveInput.y * moveSpeed * camForward + moveInput.x * moveSpeed * camRight;
			rb.velocity = velocity;
		}

		void DoLook()
		{
			Vector3 angles = playerCam.transform.localEulerAngles;
			angles.x += -lookInput.y * lookSpeed;
			if (angles.x > 270)
			{
				angles.x -= 360;
			}
			angles.x = Mathf.Clamp(angles.x, minLookAngle, maxLookAngle);

			angles.y += lookInput.x * lookSpeed;
			playerCam.transform.localEulerAngles = angles;
		}

		// Input methods
		void OnMove(InputValue value)
		{
			moveInput = value.Get<Vector2>();
		}

		void OnLook(InputValue value)
		{
			lookInput = value.Get<Vector2>();
		}
		#endregion Movement
	}
}