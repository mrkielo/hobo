using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
	NavMeshAgent agent;

	[SerializeField] Vector3 destination;
	[SerializeField] LayerMask groundMask;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	public void Move(Vector3 destination)
	{
		agent.destination = destination;
	}

	void MoveToMouseClick() //test function
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit, 100f, groundMask))
		{
			agent.destination = hit.point;
		}
	}
}
