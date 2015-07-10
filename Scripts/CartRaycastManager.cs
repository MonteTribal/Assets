using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CartRaycastManager : MonoBehaviour {

	public GameObject cartHitbox;

	[Range(2,10)]
	public int rayCount = 4; //level of detail for amount of rays cast in checkForHitInFrontOfColliderPoints()

	[HideInInspector]
	public float X_axisSpacing; //spacing for the rays along certain axes
	[HideInInspector]
	public float Y_axisSpacing;
	[HideInInspector]
	public float Z_axisSpacing;

	[Range(.001f, .5f)]
	public float skinWidth = .01f;

	public RayOrigins rayOrigins;

	public enum raycastOriginOption // an easy way to associate the struct variables in a function parameter
	{
		frontRightBotton, frontLeftBottom, 
		backRightBotton, backLeftBottom, 
		frontRightTop, frontLeftTop, 
		backRightTop, backLeftTop
	};

	public struct RayOrigins //the cornners of the Cart Hitbox
	{
		public Vector3 frontRightBotton;
		public Vector3 frontLeftBottom;
		public Vector3 backRightBotton;
		public Vector3 backLeftBottom;

		public Vector3 frontRightTop;
		public Vector3 frontLeftTop;
		public Vector3 backRightTop;
		public Vector3 backLeftTop;

	}

	void Start()
	{
		UpdateRayPositions(); // calculaterayspacing will not work unthis has happened at least once
		CalculateRaySpacing();
	}

	public KeyValuePair<bool, RaycastHit> checkForHitInFrontOfColliderPoints(raycastOriginOption a, raycastOriginOption b, Vector3 direction, float distanceForward, string mask="Ground", bool debug=true)
	{
		Vector3 va, vb;
		va = convertEnumToRayOrigin(a);
		vb = convertEnumToRayOrigin(b);
		float spacingToUse = discoverSpacing(a, b);
		return hitBetweenPoints(va, vb, direction, distanceForward, spacingToUse, mask, debug);
	}

	public KeyValuePair<bool, RaycastHit> hitBetweenPoints(Vector3 a, Vector3 b, Vector3 direction, float distance, float spacing, string mask = "Ground", bool debug=true)
	{
		Vector3 correctingVector = (b-a).normalized;
		for(int i=0; i<rayCount; i++)
		{
			Vector3 pos = a + correctingVector*spacing*i;

			if(debug)
			{
				Debug.DrawLine(pos, pos+direction*distance, Color.yellow);
			}

			RaycastHit hit;
			if(Physics.Raycast(pos, direction, out hit, distance, LayerMask.NameToLayer(mask)))
			{
				return new KeyValuePair<bool, RaycastHit>(true, hit); 
			}
		}
		return new KeyValuePair<bool, RaycastHit>(false, new RaycastHit());
	}

	// discovers the spacing to use for the raycasts between the known corners
	float discoverSpacing(raycastOriginOption a, raycastOriginOption b)
	{
		if((a == raycastOriginOption.backLeftBottom && b == raycastOriginOption.backLeftTop) ||
		   (a == raycastOriginOption.backLeftTop && b == raycastOriginOption.backLeftBottom) ||
		   (a == raycastOriginOption.backRightBotton && b == raycastOriginOption.backRightTop) || 
		   (a == raycastOriginOption.backRightTop && b == raycastOriginOption.backRightBotton) ||
		   (a == raycastOriginOption.frontLeftBottom && b == raycastOriginOption.frontLeftTop) ||
		   (a == raycastOriginOption.frontLeftTop && b == raycastOriginOption.frontLeftBottom) ||
		   (a == raycastOriginOption.frontRightBotton && b == raycastOriginOption.frontRightTop) || 
		   (a == raycastOriginOption.frontRightTop && b == raycastOriginOption.frontRightBotton))
		{
			return Y_axisSpacing;
		}
		else if((a == raycastOriginOption.backLeftBottom && b == raycastOriginOption.backRightBotton) ||
		        (a == raycastOriginOption.backRightBotton && b == raycastOriginOption.backLeftBottom) ||
		        (a == raycastOriginOption.backLeftTop && b == raycastOriginOption.backRightTop) || 
		        (a == raycastOriginOption.backRightTop && b == raycastOriginOption.backLeftTop) ||
		        (a == raycastOriginOption.frontLeftBottom && b == raycastOriginOption.frontRightBotton) ||
		        (a == raycastOriginOption.frontRightBotton && b == raycastOriginOption.frontLeftBottom) ||
		        (a == raycastOriginOption.frontLeftTop && b == raycastOriginOption.frontRightTop) || 
		        (a == raycastOriginOption.frontRightTop && b == raycastOriginOption.frontLeftTop))
		{
			return X_axisSpacing;
		}
		return Z_axisSpacing;
	}

	//converts the enum into the struct
	//va is a biproduct of me being lazy
	Vector3 convertEnumToRayOrigin(raycastOriginOption a)
	{
		Vector3 va;
		switch(a)
		{
		case raycastOriginOption.backLeftBottom:
			va = rayOrigins.backLeftBottom;
			return va;
		case raycastOriginOption.backLeftTop:
			va = rayOrigins.backLeftTop;
			return va;;
		case raycastOriginOption.backRightBotton:
			va = rayOrigins.backRightBotton;
			return va;;
		case raycastOriginOption.backRightTop:
			va = rayOrigins.backRightTop;
			return va;;
		case raycastOriginOption.frontLeftBottom:
			va = rayOrigins.frontLeftBottom;
			return va;;
		case raycastOriginOption.frontLeftTop:
			va = rayOrigins.frontLeftTop;
			return va;;
		case raycastOriginOption.frontRightBotton:
			va = rayOrigins.frontRightBotton;
			return va;;
		case raycastOriginOption.frontRightTop:
			va = rayOrigins.frontRightTop;
			return va;;
		}
		Debug.LogError("?? convertEnumToRayOrigin messed up somehow");
		return Vector3.zero;
	}

	//spaces between the raycasts for for collisions
	void CalculateRaySpacing()
	{
		Bounds bounds = cartHitbox.GetComponent<Collider>().bounds;
		bounds.Expand(skinWidth * -2);

		// assumes the hitbox is a square
		X_axisSpacing = Vector3.Distance(rayOrigins.frontLeftBottom, rayOrigins.frontRightBotton) / (rayCount-1);
		Y_axisSpacing = Vector3.Distance(rayOrigins.frontLeftBottom, rayOrigins.frontLeftTop) / (rayCount-1);
		Z_axisSpacing = Vector3.Distance(rayOrigins.frontLeftBottom, rayOrigins.backLeftBottom) / (rayCount-1);
	}

	void UpdateRayPositionsOld()
	{
		//*************************************
		//DOES NOT ACCURATELY FOLLOW HITBOX CORNERS
		//*************************************
		Bounds bounds = cartHitbox.GetComponent<Collider>().bounds;
		bounds.Expand(skinWidth * -2);

		rayOrigins.frontLeftBottom = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
		rayOrigins.frontRightBotton = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
		rayOrigins.backLeftBottom = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
		rayOrigins.backRightBotton = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);

		rayOrigins.frontLeftTop = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
		rayOrigins.frontRightTop = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
		rayOrigins.backLeftTop = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
		rayOrigins.backRightTop = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
	}

	public void UpdateRayPositions()
	{

		BoxCollider b = cartHitbox.GetComponent<BoxCollider>();

		rayOrigins.frontLeftBottom =  transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, b.size.z)*0.5f);
		rayOrigins.frontRightBotton = transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, b.size.z)*0.5f);
		rayOrigins.backLeftBottom =   transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z)*0.5f);
		rayOrigins.backRightBotton =  transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, -b.size.z)*0.5f);

		rayOrigins.frontLeftTop =  transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, b.size.z)*0.5f);
		rayOrigins.frontRightTop = transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, b.size.z)*0.5f);
		rayOrigins.backLeftTop =   transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, -b.size.z)*0.5f);
		rayOrigins.backRightTop =  transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, -b.size.z)*0.5f);
	}

	//proves that the corners of the hitbox are followed

	[Range(0,11)]
	public uint DebugShowCornerSpheres = 0;
	void OnDrawGizmosSelected()
	{
		if(DebugShowCornerSpheres == 1 || DebugShowCornerSpheres ==  5 || DebugShowCornerSpheres ==  11)
			Gizmos.DrawSphere(rayOrigins.frontLeftBottom, .2f);
		if(DebugShowCornerSpheres == 2 || DebugShowCornerSpheres ==  5 || DebugShowCornerSpheres ==  11)
			Gizmos.DrawSphere(rayOrigins.frontRightBotton, .2f);
		if(DebugShowCornerSpheres == 3 || DebugShowCornerSpheres ==  5 || DebugShowCornerSpheres ==  11)
			Gizmos.DrawSphere(rayOrigins.backLeftBottom, .2f);
		if(DebugShowCornerSpheres == 4 || DebugShowCornerSpheres ==  5 || DebugShowCornerSpheres ==  11)
			Gizmos.DrawSphere(rayOrigins.backRightBotton, .2f);

		if(DebugShowCornerSpheres == 6 || DebugShowCornerSpheres ==  10 || DebugShowCornerSpheres ==  11)
			Gizmos.DrawSphere(rayOrigins.frontLeftTop, .2f);
		if(DebugShowCornerSpheres == 7 || DebugShowCornerSpheres ==  10 || DebugShowCornerSpheres ==  11)
			Gizmos.DrawSphere(rayOrigins.frontRightTop, .2f);
		if(DebugShowCornerSpheres == 8 || DebugShowCornerSpheres ==  10 || DebugShowCornerSpheres ==  11)
			Gizmos.DrawSphere(rayOrigins.backLeftTop, .2f);
		if(DebugShowCornerSpheres == 9 || DebugShowCornerSpheres ==  10 || DebugShowCornerSpheres ==  11)
			Gizmos.DrawSphere(rayOrigins.backRightTop, .2f);
	}
}
