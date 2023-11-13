using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foxthorne.FoxInventory;
using UnityEngine.InputSystem;

namespace Foxthorne.PlayerController
{
	public class Player : MonoBehaviour
	{
		[Header("Movement")]
		public float moveSpeed = 1;
		public Vector2 moveInput;

		public float lookSpeed = 1;
		public float minLookAngle = -80;
		public float maxLookAngle = 80;
		Vector2 lookInput;
		public Camera playerCam;

		[Header("Interaction")]
		public SO_ItemData selectedItemData;

		[Header("Input")]
		PlayerInput PlayerInput;

		[Header("References")]
		public Rigidbody rb;

		void Start()
		{

		}

		void Update()
		{
			DoMovement();
			DoLook();
		}

		#region Movement
		void DoMovement()
		{
			Vector3 velocity = Vector3.zero;
			velocity.x = moveInput.x * moveSpeed;
			velocity.z = moveInput.y * moveSpeed;
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