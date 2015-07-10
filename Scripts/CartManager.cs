using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Manages the carts speed, respawns, and rotation

[RequireComponent(typeof(ForcesManager))]
[RequireComponent(typeof(CartRaycastManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TireManager))]

public class CartManager : MonoBehaviour {
	
	Rigidbody body;
	ForcesManager forces;
	CartRaycastManager rayman;
	TireManager tires;

	public bool showCenterOfMassSphere;
	public Vector3 centerOfMass;

	[HideInInspector]
	public float speed;

	[Tooltip(" Set between 0-1 - 0.9 is recommended")]
	public float brakePercentage = .9f;
	public bool unrollable = true;
	public float groundCorrectionTime = .1f;

	Vector3 respawnPos;
	float respawnTimer = 0;
	float respawnTimerMax = 1.5f;

	Vector3 curFwd;
	Vector3 curNormal;

	// Use this for initialization
	void Start () {
		forces = GetComponent<ForcesManager>();
		body = GetComponent<Rigidbody>();
		rayman = GetComponent<CartRaycastManager>();
		tires = GetComponent<TireManager>();

		curFwd = transform.forward;
		curNormal = transform.up;

		body.constraints = RigidbodyConstraints.FreezeRotation;
	}

	//visual representation of center of mass
	void OnDrawGizmosSelected()
	{
		if(showCenterOfMassSphere)
		{
			Gizmos.DrawSphere(transform.position + centerOfMass, .5f);
		}
	}

	void Update()
	{
		speed = body.velocity.magnitude; //update speed
		respawnPos = transform.position; //save respawn pos (This should be a temperary solution)

		//update the corners for the raycast

		rayman.UpdateRayPositions();


		Vector3 newUp = tires.GetOrientationToGround();
		CorrectAngleToGround(newUp);
	}

	void respawn()
	{
		transform.position = respawnPos;

		transform.Translate(new Vector3(0,.3f, 0), Space.World);
		print ("Respawn!");
	}

	public void RespawnCheck()
	{
		respawnTimer += Time.deltaTime;
		print ("Respawn?");
		if(respawnTimer >= respawnTimerMax)
		{
			respawnTimer = 0;
			respawn();
		}
	}

	public bool Accelerate(float vel, Vector3 dir)
	{
		if(speed < forces.maxSpeed)
		{
			body.AddForce(dir * vel, ForceMode.Acceleration);
			return true;
		}
		return false;		
	}

	public void ApplyBrakes()
	{
		body.velocity *= brakePercentage;
	}

	public void CorrectAngleToGround(Vector3 newFloorNormal)
	{
		curFwd = transform.forward;

		curNormal = Vector3.Lerp(curNormal, newFloorNormal, 10*Time.deltaTime);
		transform.up = curNormal;

		//transform.forward = curFwd;
	}

}