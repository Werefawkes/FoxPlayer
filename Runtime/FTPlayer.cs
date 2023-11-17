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
		public float lookSpeed = 1;
		public float jumpStrength = 1;
		public Vector2 moveInput;
		public Vector3 gravity = Vector3.down;
		public bool isGrounded = false;
		public float groundedDistance = 0.1f;
		public float groundedSpeedLimit = 10;

		public float minLookAngle = -80;
		public float maxLookAngle = 80;
		public Vector2 lookInput;
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

			isGrounded = Physics.Raycast(transform.position, -transform.up, groundedDistance);

			DoMovement();
			DoLook();
		}

		#region Movement
		public void DoMovement()
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

		public void DoLook()
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

		void OnJump()
		{
			if (isGrounded)
			{
				rb.AddForce(transform.up * jumpStrength, ForceMode.Impulse);
			}
		}
		#endregion Movement
	}
}