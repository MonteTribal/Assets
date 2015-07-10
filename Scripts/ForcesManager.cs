using UnityEngine;
using System.Collections;

public class ForcesManager : MonoBehaviour{

	[Range(10, 50)]
	public float accelIncrement = 100;
	[Range(10, 1000)]
	public float maxSpeed = 120;

	[Range(1, 100)]
	public float turnSpeed = 10;

	[Range(1, 10)]
	public float bump = 120;
}
