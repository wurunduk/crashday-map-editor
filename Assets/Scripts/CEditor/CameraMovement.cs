using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour 
{
	public int speed;

	public int rotateSpeed;

	void Start()
	{
		
	}

	void Update ()
	{
		if(Input.GetKey(KeyCode.A))
		{
			transform.Translate (new Vector3 (-speed*Time.deltaTime, 0, 0), Space.Self);
		}
		else if(Input.GetKey(KeyCode.D))
		{
			transform.Translate (new Vector3 (speed*Time.deltaTime, 0, 0), Space.Self);
		}

		if(Input.GetKey(KeyCode.W))
		{
			transform.Translate (new Vector3 (0, 0, speed*Time.deltaTime), Space.Self);
		}
		else if(Input.GetKey(KeyCode.S))
		{
			transform.Translate (new Vector3 (0, 0, -speed*Time.deltaTime), Space.Self);
		}

		if(Input.GetKey(KeyCode.LeftShift))
		{
			transform.Translate (new Vector3 (0, speed*Time.deltaTime, 0), Space.Self);
		}
		else if(Input.GetKey(KeyCode.LeftControl))
		{
			transform.Translate (new Vector3 (0, -speed*Time.deltaTime, 0), Space.Self);
		}


		if(Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Rotate(new Vector3(0, -rotateSpeed*Time.deltaTime, 0), Space.World);
		}
		else if(Input.GetKey(KeyCode.RightArrow))
		{
			transform.Rotate(new Vector3(0, rotateSpeed*Time.deltaTime, 0), Space.World);
		}

		if(Input.GetKey(KeyCode.UpArrow))
		{
			transform.Rotate(new Vector3(-rotateSpeed*Time.deltaTime, 0, 0), Space.Self);
		}
		else if(Input.GetKey(KeyCode.DownArrow))
		{
			transform.Rotate(new Vector3(rotateSpeed*Time.deltaTime, 0, 0), Space.Self);
		}
	}
}
