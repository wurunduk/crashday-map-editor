using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
	public Transform ObjectToFollow;
	
	
	// Update is called once per frame
	void Update ()
	{
		float h = Mathf.Abs(ObjectToFollow.position.y);
		if (h > 900) h = 900; 
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, h + 100);

		Vector3 newPos = new Vector3(ObjectToFollow.position.x, ObjectToFollow.position.y, ObjectToFollow.position.z - h/2);
		transform.position = newPos;
	}
}
