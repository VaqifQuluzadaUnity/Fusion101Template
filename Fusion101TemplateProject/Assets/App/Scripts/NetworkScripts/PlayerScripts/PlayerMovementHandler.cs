using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : NetworkBehaviour
{
	[Header("Player Movement Parameters")]
	[SerializeField] private float playerWalkSpeed = 200;

	[SerializeField] private float playerRunSpeed = 400;

	private float playerCurrentSpeed;

	[Header("Player Rotation Parameters")]
	[SerializeField] private float sensitivity=50;

	[Header("Player Component References")]
	[SerializeField] private Rigidbody playerRb;


	private void Start()
	{
		playerCurrentSpeed = playerWalkSpeed;
	}


	public override void FixedUpdateNetwork()
	{
		if(Runner.TryGetInputForPlayer<NetworkInputData>(Object.InputAuthority, out NetworkInputData inputData))
		{
			MovePlayer(inputData.movementInput);
			RotatePlayer(inputData.mouseInput);
		}
	}

	private void MovePlayer(Vector3 movementInput)
	{
		
		playerRb.velocity = (playerRb.transform.forward * movementInput.z + playerRb.transform.right * movementInput.x) * Runner.DeltaTime * playerCurrentSpeed;
	}

	private void RotatePlayer(Vector2 mouseInput)
	{
		Vector3 currentRotation = transform.eulerAngles;

		currentRotation.y += mouseInput.x * Runner.DeltaTime * sensitivity;

		transform.rotation = Quaternion.Euler(currentRotation);
	}
}
