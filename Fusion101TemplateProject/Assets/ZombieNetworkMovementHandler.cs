using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieNetworkMovementHandler : NetworkBehaviour
{
  [SerializeField] private NavMeshAgent zombieRb;

  [SerializeField] private Transform target;

	public override void FixedUpdateNetwork()
	{
		if (!Object.HasStateAuthority)
		{
			return;
		}

		if (target == null)
		{
			target = FindNewTarget();
		}

		if (target != null)
		{
			zombieRb.SetDestination(target.position);
		}
	}

	public Transform FindNewTarget()
	{
		GameObject[] targetPlayersList = GameObject.FindGameObjectsWithTag("Player");

		float distance = Mathf.Infinity;

		Transform currentTarget = null;
		foreach(GameObject player in targetPlayersList)
		{
			float currentDistance = Vector3.Distance(player.transform.position,transform.position);

			if (currentDistance < distance)
			{
				currentTarget = player.transform;
				distance = currentDistance;
			}
		}

		return currentTarget;
		
	}

}
