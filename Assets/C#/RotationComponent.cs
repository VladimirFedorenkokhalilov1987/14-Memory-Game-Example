using UnityEngine;
using System.Collections;

public class RotationComponent : MonoBehaviour
{

	[SerializeField, Range(0.1f, 300f)]
	private float _speed;

	private Vector3 finalCond;

	public void StartRotation (Vector3 finalCond) 
	{
		this.finalCond = finalCond;
	}
	
	void Update () 
	{
		if(Vector3.Distance(transform.eulerAngles, finalCond) < (Time.deltaTime * _speed * 2))
		{
			transform.eulerAngles= finalCond;
			return;
		}

		transform.Rotate (Vector3.up * Time.deltaTime * _speed);
	}
}
