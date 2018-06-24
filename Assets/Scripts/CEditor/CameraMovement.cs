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
			//rotationPoint.Translate (new Vector3 (-speed*Time.deltaTime, 0, 0), Space.Self);
		}
		else if(Input.GetKey(KeyCode.D))
		{
			transform.Translate (new Vector3 (speed*Time.deltaTime, 0, 0), Space.Self);
			//rotationPoint.Translate (new Vector3 (speed*Time.deltaTime, 0, 0), Space.Self);
		}

		if(Input.GetKey(KeyCode.W))
		{
			transform.Translate (new Vector3 (0, 0, speed*Time.deltaTime), Space.Self);
			//rotationPoint.Translate (new Vector3 (0, 0, speed*Time.deltaTime), Space.Self);
		}
		else if(Input.GetKey(KeyCode.S))
		{
			transform.Translate (new Vector3 (0, 0, -speed*Time.deltaTime), Space.Self);
			//rotationPoint.Translate (new Vector3 (0, 0, -speed*Time.deltaTime), Space.Self);
		}

		if(Input.GetKey(KeyCode.LeftShift))
		{
			transform.Translate (new Vector3 (0, speed*Time.deltaTime, 0), Space.Self);
			//rotationPoint.Translate (new Vector3 (0, speed*Time.deltaTime, 0), Space.Self);
		}
		else if(Input.GetKey(KeyCode.LeftControl))
		{
			transform.Translate (new Vector3 (0, -speed*Time.deltaTime, 0), Space.Self);
			//rotationPoint.Translate (new Vector3 (0, -speed*Time.deltaTime, 0), Space.Self);
		}


		if(Input.GetKey(KeyCode.LeftArrow))
		{
			//rotationPoint.rotation = Quaternion.Euler(Vector3.RotateTowards (rotationPoint.rotation.eulerAngles, transform.position, float.MaxValue, float.MaxValue));
			//transform.RotateAround (transform.position, Vector3.right, -rotateSpeed*Time.deltaTime);
			transform.Rotate(new Vector3(0, -rotateSpeed*Time.deltaTime, 0), Space.World);
		}
		else if(Input.GetKey(KeyCode.RightArrow))
		{
			//rotationPoint.rotation = Quaternion.Euler(Vector3.RotateTowards (rotationPoint.rotation.eulerAngles, transform.position, float.MaxValue, float.MaxValue));
			//transform.RotateAround (transform.position, Vector3.right, rotateSpeed*Time.deltaTime);
			transform.Rotate(new Vector3(0, rotateSpeed*Time.deltaTime, 0), Space.World);
		}

		if(Input.GetKey(KeyCode.UpArrow))
		{
			//rotationPoint.rotation = Quaternion.Euler(Vector3.RotateTowards (rotationPoint.rotation.eulerAngles, transform.position, float.MaxValue, float.MaxValue));
			//transform.RotateAround (transform.position, Vector3.up, rotateSpeed*Time.deltaTime);
			transform.Rotate(new Vector3(-rotateSpeed*Time.deltaTime, 0, 0), Space.Self);
		}
		else if(Input.GetKey(KeyCode.DownArrow))
		{
			//rotationPoint.rotation = Quaternion.Euler(Vector3.RotateTowards (rotationPoint.rotation.eulerAngles, transform.position, float.MaxValue, float.MaxValue));
			//transform.RotateAround (transform.position, Vector3.up, -rotateSpeed*Time.deltaTime);
			transform.Rotate(new Vector3(rotateSpeed*Time.deltaTime, 0, 0), Space.Self);
		}
	}
}
