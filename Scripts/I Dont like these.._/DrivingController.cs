using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//based from http://docs.unity3d.com/Manual/WheelColliderTutorial.html

// I Dont like this one...

public class DrivingController : MonoBehaviour {

	[System.Serializable]
	public class AxleInfo
	{
		public WheelCollider leftWheel;
		public WheelCollider rightWheel;
		public bool motor; //is this a mototrized wheel
		public bool steering; //does this wheel apply steer angle
	}

	private Animator chasisAnimator;

	public List<AxleInfo> axleInfos; //info on each indiv axle
	public float maxMotorTorque; //maximum torque the motor applys to wheels
	public float maxSteerAngle; //max steer angle of a wheel

	public Vector3 CenterOfMass;

	public float speed;

	void Start()
	{
		GetComponent<Rigidbody>().centerOfMass = CenterOfMass;
		chasisAnimator = GetComponent<Animator>();
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawSphere(transform.position + CenterOfMass, .3f);
	}

	void FixedUpdate () 
	{
		float motor = maxMotorTorque * Input.GetAxis("Accelerate");
		float steering = maxSteerAngle * Input.GetAxis("Turn");

		foreach(AxleInfo axleInfo in axleInfos)
		{
			if(axleInfo.steering)
			{
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if(axleInfo.motor)
			{
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
			}
		}
		ApplyWheelVisuals();

		Rigidbody body = GetComponent<Rigidbody>();
		speed = body.velocity.magnitude;		
	}

	void ApplyWheelVisuals()
	{
		if(Input.GetAxis("Turn") < 0)
		{
			chasisAnimator.SetBool("LeftTurn", true);
			chasisAnimator.SetBool("RightTurn", false);
		}
		else if(Input.GetAxis("Turn") > 0)
		{
			chasisAnimator.SetBool("LeftTurn", false);
			chasisAnimator.SetBool("RightTurn", true);
		}
		else
		{
			chasisAnimator.SetBool("LeftTurn", false);
			chasisAnimator.SetBool("RightTurn", false);
		}

	}
}
