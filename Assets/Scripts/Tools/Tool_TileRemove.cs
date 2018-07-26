using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_TileRemove : ToolGeneral
{
	private IntVector2 _gridPosition;

	private Material mat;

	public int SelectedTileId;

	public override void Initialize()
	{
		mat = Resources.Load<Material>("RedTransparent");
		ToolName = "Remove Tiles";
		_gridPosition = new IntVector2(0, 0);
	}

	public override void OnSelected()
	{
		SomePrefab.GetComponent<MeshFilter>().mesh = GenerateCubeMesh();
		SomePrefab.GetComponent<MeshRenderer>().materials = new[]{mat};

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

		SomePrefab.transform.position = new Vector3(point.x*TrackManager.TileSize, 0, -1*point.y*TrackManager.TileSize);
	}

	public override void Update()
	{

	}

	public override void UpdateGUI()
	{
		
	}

	private Mesh GenerateCubeMesh () 
	{
		//todo 
		//make divided by 4 cube so it can be bent to the terrain

		Vector3[] vertices = 
		{
			//10 is the half of the TrackManager.TileSize
			new Vector3 (-10, -10, -10),
			new Vector3 (10, -10, -10),
			new Vector3 (10, 10, -10),
			new Vector3 (-10, 10, -10),
			new Vector3 (-10, 10, 10),
			new Vector3 (10, 10,10),
			new Vector3 (10, -10, 10),
			new Vector3 (-10, -10, 10),
		};

		int[] triangles = 
		{
			0, 2, 1, //face front
			0, 3, 2,
			2, 3, 4, //face top
			2, 4, 5,
			1, 2, 5, //face right
			1, 5, 6,
			0, 7, 4, //face left
			0, 4, 3,
			5, 4, 7, //face back
			5, 7, 6,
			0, 6, 7, //face bottom
			0, 1, 6
		};

		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		return mesh;
	}
}
