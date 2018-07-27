using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_TileRemove : ToolGeneral
{
	private IntVector2 _gridPosition;

	private Material _mat;

	public int SelectedTileId;

	public override void Initialize()
	{
		_mat = Resources.Load<Material>("RedTransparent");
		ToolName = "Remove Tiles";
		_gridPosition = new IntVector2(0, 0);
	}

	public override void OnSelected()
	{
		SomePrefab.GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateCubeMesh(3, new Vector3(20, 10, 20));
		SomePrefab.GetComponent<MeshRenderer>().materials = new[]{_mat};

		SomePrefab.GetComponent<MeshRenderer>().enabled = true;
	}

	public override void OnDeselected()
	{
		SomePrefab.GetComponent<MeshRenderer>().enabled = false;
	}

	public override void OnRMBDown(Vector2 point)
	{
		
	}

	public override void OnLMBDown(Vector2 point)
	{
		TrackManager.SetTileByAtlasId(0, _gridPosition);
	}

	public override void OnMouseOverTile(IntVector2 point)
	{
		if (_gridPosition.x != point.x || _gridPosition.y != point.y)
		{
			_gridPosition = point;
		}

		SomePrefab.transform.position = new Vector3(point.x*TrackManager.TileSize, 5, -1*point.y*TrackManager.TileSize);
	}

	public override void Update()
	{

	}

	public override void UpdateGUI()
	{
		
	}
}
