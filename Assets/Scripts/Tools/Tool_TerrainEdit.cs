using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_TerrainEdit : ToolGeneral
{

	public override void Initialize()
	{
		ToolName = "Edit Terrain";
	}

	public override void OnSelected()
	{
		TerrainManager.Terrain.GetComponent<MeshRenderer>().enabled = true;
	}

	public override void OnDeselected()
	{
		TerrainManager.Terrain.GetComponent<MeshRenderer>().enabled = false;
	}

	public override void OnRMBDown(Vector2 point)
	{
		
	}

	public override void OnLMBDown(Vector2 point)
	{
		
	}

	public override void OnMouseOverTile(IntVector2 point)
	{
	
	}

	public override void Update()
	{
		
	}

	public override void UpdateGUI()
	{
		
	}
}