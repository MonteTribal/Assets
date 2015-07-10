using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TireManager : MonoBehaviour {

	[SerializeField]
	List<Tire> tires; //order in inspector: BL, BR, FL, FR

	CartManager cart;

	void Start()
	{
		cart = GetComponent<CartManager>();
		if(tires.Count == 0)
		{
			FindTires();
		}
	}

	void FixedUpdate()
	{
		CheckForModification();
	}

	//reads any modifications that a tire is detecting and sends accelerates the car to.
	//Potential Change
	//	have this detect the greatest acceleration on the tires, then ship it to cart manager
	//	have the cart manager do the actual acceleration
	void CheckForModification()
	{
		foreach(Tire tire in tires)
		{
			if(tire.modifyAmount != 0)
			{
				cart.Accelerate(tire.modifyAmount, tire.modifyDirection);
			}
		}
	}

	void FindTires()
	{
		tires = new List<Tire>();
		foreach(Tire child in transform.root.GetComponentsInChildren<Tire>())
		{				
			tires.Add(child);
		}
	}

	public bool getGrounded()
	{
		//returns true if ANY tire is on the ground
		foreach(Tire tire in tires)
		{
			if(tire.grounded)
			{
				return true;
			}
		}
		return false;
	}

	public Vector3 GetOrientationToGround(bool debug = true)
	{		
		Vector3 upDir = (Vector3.Cross(tires[1].downHit.point - Vector3.up, tires[0].downHit.point - Vector3.up) +
		         		 Vector3.Cross(tires[0].downHit.point - Vector3.up, tires[3].downHit.point - Vector3.up) +
		         		 Vector3.Cross(tires[3].downHit.point - Vector3.up, tires[2].downHit.point - Vector3.up) +
		         		 Vector3.Cross(tires[2].downHit.point - Vector3.up, tires[1].downHit.point - Vector3.up)
		         		).normalized;
		if(debug)
		{
			Debug.DrawRay(tires[1].transform.position+ tires[1].downHit.point, Vector3.up, Color.magenta);
			Debug.DrawRay(tires[0].downHit.point, Vector3.up, Color.magenta);
			Debug.DrawRay(tires[3].downHit.point, Vector3.up, Color.magenta);
			Debug.DrawRay(tires[2].downHit.point, Vector3.up, Color.magenta);
			Debug.DrawLine(transform.position, transform.position + upDir * 5, Color.magenta);
		}

		return upDir;
	}
}
