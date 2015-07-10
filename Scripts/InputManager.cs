using UnityEngine;
using System.Collections;

//handles the input given to a player controlled cart

[RequireComponent(typeof(CartManager))]

public class InputManager : MonoBehaviour { 

	ForcesManager forces;
	CartManager cart;
	TireManager tires;

	void Start () {	
		forces = GetComponent<ForcesManager>();
		cart = GetComponent<CartManager>();
		tires = GetComponent<TireManager>();
	}
	
	void Update () {
		if(tires.getGrounded())
		{    
			//only accelerate (with button), and hop if 
			if(Input.GetAxis("Accelerate") != 0)
			{
				cart.Accelerate((forces.accelIncrement * 100) * Input.GetAxis("Accelerate") * Time.deltaTime, transform.forward);
			}
			if(Input.GetAxis("Brake") != 0)
			{
				cart.ApplyBrakes();
			}
			if(Input.GetButtonDown("HopLeft") || Input.GetButtonDown("HopRight"))
			{
				cart.Accelerate((forces.bump * 100), transform.up);
			}
		}
		else if(!tires.getGrounded())
		{
			// add additional gravity?

			if(cart.speed == 0)
			{
				cart.RespawnCheck();
			}
			//safe zones for respawn?
		}

		//always turn
		if(Input.GetAxis("Turn") != 0)
		{
			transform.RotateAround(transform.position, transform.up, forces.turnSpeed * cart.speed * Input.GetAxis("Turn") * Time.deltaTime);
		}		


	}
}
