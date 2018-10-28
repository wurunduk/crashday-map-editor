using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TT_Smooth : TT_General
{
	public float SmoothForce;


	public override void Initialize()
	{
		base.Initialize();
		ToolName = "Smooth";
	}

	public override void OnLMB(Vector3 point)
	{

		base.OnLMB(point);
	}
}
