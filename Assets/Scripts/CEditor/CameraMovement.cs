using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private TerrainManager _terrainManager;

    void Awake()
    {
        _terrainManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TerrainManager>();
    }

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
		if (Input.mousePosition.x > 340 && Application.isFocused)
		{
			if (!_terrainManager) return;
			
			Vector3 pos = _terrainManager.GetCameraPointOnTerrain() - transform.position;

			float speed = Input.GetAxis("Mouse ScrollWheel") * 100;


			if (pos.sqrMagnitude <= speed * speed)
			{
				if (pos.sqrMagnitude < 0.15f && Vector3.Angle(pos, transform.forward*speed) < 3.1415f/3.0f)
					speed = 0;
				else
					speed = Mathf.Sign(speed)*pos.magnitude/2.0f;
			}
				

			transform.Translate(Vector3.forward*speed, Space.Self);
		}
			
	}
}
