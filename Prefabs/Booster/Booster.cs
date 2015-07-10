using UnityEngine;
using System.Collections;

public class Booster : MonoBehaviour {

	public float boost = 100;
	[HideInInspector]
	public Vector3 direction;

	[Range(.1f, 5f)]
	public float scrollSpeed = 1F;
	public bool scrollReverse = true;
	Renderer rend;

	void Start()
	{
		rend = GetComponent<Renderer>();
		direction = -transform.forward;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawLine(transform.position, transform.position + -transform.forward * 8);
	}

	void LateUpdate()
	{
		float offset = Time.time * scrollSpeed;
		if(scrollReverse)
		{
			rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
		}
		else
		{
			rend.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
		}
	}
}
