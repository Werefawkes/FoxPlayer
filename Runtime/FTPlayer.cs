using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foxthorne.FoxScreens;
using UnityEngine.InputSystem;
using CustomInspector;

namespace Foxthorne.FoxPlayer
{
	public class FTPlayer : MonoBehaviour
	{
		[HorizontalLine("Movement")]
		public float moveSpeed = 1;
		public float jumpStrength = 1;
		public float groundedDistance = 0.1f;
		public float groundedSpeedLimit = 10;

		[HorizontalLine("Camera")]
		public float lookSpeed = 1;
		public float minLookAngle = -80;
		public float maxLookAngle = 80;

		[HorizontalLine("References")]
		[ForceFill]
		public Camera playerCam;
		[SelfFill(true)]
		public Rigidbody rb;


		[HorizontalLine("Info")]
		[ReadOnly]
		public Vector3 gravity = Vector3.down;
		[ReadOnly]
		public Vector3 moveInput;
		[ReadOnly]
		public Vector2 lookInput;
		[ReadOnly]
		public bool isGrounded = false;


		public virtual void Update()
		{
			if (!UIManager.IsUIClear)
			{
				moveInput = Vector2.zero;
				lookInput = Vector2.zero;
			}

			isGrounded = Physics.Raycast(new(transform.position.x, transform.position.y + 0.01f, transform.position.z), -transform.up, groundedDistance);

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
			if (!isGrounded)
			{
				velocity.y = rb.velocity.y + gravity.y;
			}

			rb.velocity = velocity;
		}

		public virtual void DoLook()
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
		public void OnMove(InputValue value)
		{
			moveInput = value.Get<Vector2>();
		}

		public void OnLook(InputValue value)
		{
			lookInput = value.Get<Vector2>();
		}

		public virtual void OnJump()
		{
			if (isGrounded)
			{
				rb.AddForce(transform.up * jumpStrength, ForceMode.Impulse);
			}
		}
		#endregion Movement
	}
}