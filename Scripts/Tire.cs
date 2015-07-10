using UnityEngine;
using System.Collections;


public class Tire : MonoBehaviour {

	public LayerMask groundCheck;

	//booster detector
	[HideInInspector]
	public Vector3 modifyDirection;
	[HideInInspector]
	public float modifyAmount = 0;

	//ground detections
	//[HideInInspector]
	public bool grounded = true;

	public RaycastHit downHit;
	public float rayLength = 2;

	void FixedUpdate()
	{
		if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out downHit, rayLength, groundCheck))
		{
			//print (transform.name + " hit ground");
		}

		//Debug.DrawLine(transform.position, transform.position + Vector3.down*rayLength, Color.yellow);
	}

	void OnTriggerStay(Collider other)
	{
		if(other.GetComponent<Booster>())
		{
			//print ("BOOST");
			modifyAmount = other.GetComponent<Booster>().boost;
			modifyDirection = other.GetComponent<Booster>().direction;
		}
		else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			grounded = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<Booster>())
		{
			modifyAmount = 0;
			modifyDirection = Vector3.zero;
		}
		else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			grounded = false;
		}
	}
	
}
