﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour 
{
	void Start()
	{
		
	}

	void Update ()
	{
		if (Input.GetButton("Camera Move"))
		{
			if (Input.GetButton("Shift"))
			{
				transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y")*5, Space.Self);
				transform.Rotate(Vector3.up, Input.GetAxis("Mouse X")*5, Space.World);
			}
			else
			{
				Vector3 vec = new Vector3(Input.GetAxis("Mouse X")*-4, Input.GetAxis("Mouse Y")*-4, 0);
				transform.Translate(vec, Space.Self);
			}
		}
	}

	void LateUpdate()
	{
		//welcome to the ghetto
		//this is the size of the left GUI panel
		if(Input.mousePosition.x > 340 && Application.isFocused)
			transform.Translate(Vector3.forward*Input.GetAxis("Mouse ScrollWheel")*100, Space.Self);
	}
}
