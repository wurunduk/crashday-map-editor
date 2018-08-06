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
		TerrainManager.ApplyTerrainToMesh(SomePrefab.GetComponent<MeshFilter>().mesh, _gridPosition, 0, new IntVector2(1,1), false);
		SomePrefab.GetComponent<MeshRenderer>().materials = new[]{_mat};

		SomePrefab.GetComponent<MeshRenderer>().enabled = true;
	}

	public override void OnDeselected()
	{
		SomePrefab.GetComponent<MeshRenderer>().enabled = false;
	}

	public override void OnLMBDown(Vector3 point)
	{
		TrackManager.RemoveTileAt(_gridPosition);
	}

	public override void OnMouseOverTile(IntVector2 point)
	{
		if (_gridPosition.x != point.x || _gridPosition.y != point.y)
		{
			_gridPosition = point;
			SomePrefab.transform.position = new Vector3(point.x*TrackManager.TileSize, 5, -1*point.y*TrackManager.TileSize);
			SomePrefab.GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateCubeMesh(3, new Vector3(20, 10, 20));
			TerrainManager.ApplyTerrainToMesh(SomePrefab.GetComponent<MeshFilter>().mesh, _gridPosition, 0, new IntVector2(1,1), false);
		}
	}
}
