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
}