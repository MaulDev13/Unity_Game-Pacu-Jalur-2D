using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
	[SerializeField] private float rotSpeed;
	[SerializeField] private Vector3 rotDirection;

	private void Update()
	{
		transform.Rotate(rotDirection * rotSpeed * Time.deltaTime, Space.World);
	}

}
